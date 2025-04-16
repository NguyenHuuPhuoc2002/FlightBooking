using FlightBooking.Application.DTOs;
using FlightBooking.Application.Services.IServices;
using FlightBooking.Entities.Entities;
using FlightBooking.Infrastructure.Repositories;
using FlightBooking.Infrastructure.Repositories.Interfaces;
using FlightBooking.Infrastructure.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static FlightBooking.Application.Commands.UsersCommand.CheckLoginCommand;

namespace FlightBooking.Application.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly AppSetting _appSettings;
        private readonly ILogger<CheckLoginCommandHandler> _logger;
        private readonly IRefreshTokenRepository _refreshToken;
        private SecurityToken validatedToken;

        public JwtTokenService(IAccountRepository accountRepository, IOptionsMonitor<AppSetting> optionsMonitor,
                               ILogger<CheckLoginCommandHandler> logger, IRefreshTokenRepository refreshToken)
        {
            _accountRepository = accountRepository;
            _appSettings = optionsMonitor.CurrentValue;
            _logger = logger;
            _refreshToken = refreshToken;
        }
        public async Task<TokenDTO> GenerateTokenAsync(IdentityUser user)
        {
            var authClaim = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            };

            var _user = await _accountRepository.FindByEmailAsync(user.Email);
            //lay usseRole
            var useRole = await _accountRepository.GetRolesAsync(_user);
            foreach (var role in useRole)
            {
                authClaim.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));

            //tao moi token
            var token = new JwtSecurityToken(
                issuer: _appSettings.ValidIssuer,
                audience: _appSettings.ValidAudience,
                expires: DateTime.UtcNow.AddMinutes(1),
                claims: authClaim,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)

                );
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var accessToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                UserId = user.Id,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddMinutes(20),
            };

            await _refreshToken.AddAsync(refreshTokenEntity);
            _logger.LogInformation("Tạo thành công AccessToken và RefreshToken");
            return new TokenDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public async Task<TokenDTO> RenewTokenAsync(TokenDTO tokenDTO, string userId)
        {
            _logger.LogInformation("Tạo RefreshToken");
            //check xem token gửi lên nó còn hợp lệ không trước khi cấp phát một access token mới.
            var jwtTokenHandler = new JwtSecurityTokenHandler(); //sử dụng để tạo và viết token JWT.
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.Secret); //Chuyển đổi khóa bí mật thành mảng byte.

            //Cấu hình
            var tokenValidateParam = new TokenValidationParameters
            {
                //tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,
                //ký vào token
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                ValidateIssuerSigningKey = true,

                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false// ko kiem tra token het hang  
            };
            try
            {
                _logger.LogDebug("Kiểm tra định dạng token");
                //check 1: AccessToken valid format
                var tokenInverification = jwtTokenHandler.ValidateToken(tokenDTO.AccessToken, tokenValidateParam, out validatedToken);
                //check 2: check alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)
                    {
                        throw new AppException("Invalid token", 401);
                    }
                }
                var storedToken = await _refreshToken.GetTokenAsync(tokenDTO.RefreshToken);
                // New Check: Compare userId in storedToken with current userId
                if (storedToken.UserId != userId)
                {
                    _logger.LogWarning("Refresh token không khớp với userId hiện tại");
                    throw new AppException("Refresh token does not match current user", 400);
                }
                _logger.LogDebug("Kiểm tra token đã hết hạn chưa");
                //check 3: Check accessToken expire?
                var utcExpireDate = long.Parse(tokenInverification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConvertUnixTimeToDaateTime(utcExpireDate);

                //if (expireDate > DateTime.UtcNow)
                //{
                //    _logger.LogWarning("Token vẫn còn hạn sử dụng");
                //    throw new AppException("Access token has not yet expired", 400);
                //}

                _logger.LogDebug("Kiểm tra token đã tồn tại trong csdl hay chưa");
                //check 4: check refreshtoken exist in DB

                if (storedToken is null)
                {
                    _logger.LogWarning("Token không tồn tại trong csdl");
                    throw new AppException("Refresh token doesn't exist", 404);
                }

                _logger.LogDebug("Kiểm tra token đã được sử dụng chưa");
                //check 5: check refresh is used/ revoked ?
                if (storedToken.IsUsed)
                {
                    _logger.LogWarning("Token đã được sử dụng");
                    throw new AppException("Refresh token has been exist", 400);
                }
                _logger.LogDebug("Kiểm tra có bị hủy không");
                if (storedToken.IsRevoked)
                {
                    _logger.LogWarning("Token đã bị hủy");
                    throw new AppException("Refresh token has been Revoked", 400);
                }

                _logger.LogDebug("Dịch ngược để lấy JwtId từ chuỗi token và so sánh");
                //check 6: AccessToken ID = JwID in RefreshToken // dịch ngược lại để lấy JwtId từ chuỗi token
                var jti = tokenInverification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != jti)
                {
                    _logger.LogWarning("Token không khớp");
                    throw new AppException("Token doesn't match", 400);
                }

                _logger.LogInformation("Cập nhật token");
                //check 7: update token is used
                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                await _refreshToken.UpdateAsync(storedToken, tokenDTO.RefreshToken);

                //create new token
                var user = await _accountRepository.FindByIdAsync(storedToken.UserId);
                var token = await GenerateTokenAsync(user);
                _logger.LogInformation("Tạo refresh token thành công");
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Xảy ra lỗi khi tạo refresh token");
                throw;
            }
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            //sinh số ngẫu nhiên  
            using (var rng = RandomNumberGenerator.Create())
            {
                //lưu vào mảng ramdom
                rng.GetBytes(random);
                //chuyển mảng byte thành chuỗi Base64
                return Convert.ToBase64String(random);
            }
        }
        private DateTime ConvertUnixTimeToDaateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateTimeInterval.AddSeconds(utcExpireDate);
        }
    }
}
