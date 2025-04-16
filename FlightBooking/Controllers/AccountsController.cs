using FlightBooking.Application.Commands.UsersCommand;
using FlightBooking.Application.DTOs;
using FlightBooking.Application.Services;
using FlightBooking.Application.Services.IServices;
using FlightBooking.Entities.Entities;
using FlightBooking.Infrastructure.Repositories.Interfaces;
using FlightBooking.Infrastructure.Repositories.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace FlightBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountsController> _logger;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IJwtTokenService _jwtService;

        public AccountsController(IMediator mediator, ILogger<AccountsController> logger, IJwtTokenService jwtTokenService,
                                IJwtTokenService jwtService) 
        {
            _mediator = mediator;
            _logger = logger;
            _jwtTokenService = jwtTokenService;
            _jwtService = jwtService;
        }
        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser([FromForm] RegisterUserCommand command)
        {
            _logger.LogInformation("Client gửi request đăng kí tài khoản!");
            var registerUser = await _mediator.Send(command);

            if (registerUser.Succeeded)
            {
                _logger.LogInformation("Đăng kí tài khoản thành công!");
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Đăng kí thành công !",
                    Data = command.SignUpDTO
                });
            }
            _logger.LogInformation("Đăng kí tài khoản không thành công!");
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Đăng kí thất bại",
                Data = registerUser.Errors
            });
        }

        [HttpPost("login-user")]
        public async Task<IActionResult> Login([FromBody] CheckLoginCommand command)
        {
            _logger.LogInformation("Client gửi request đăng kí tài khoản!");
            var checkLogin = await _mediator.Send(command);

            if (checkLogin == null)
            {
                _logger.LogInformation("Đăng nhập không thành công!");
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Đăng nhập thất bại"
                });
            }

            var token = await _jwtTokenService.GenerateTokenAsync(checkLogin);
            _logger.LogInformation("Đăng nhập thành công!");
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Đăng nhập thành công !",
                Data = token
            });
        }

        [Authorize]
        [HttpPost("renew-access-token")]
        public async Task<IActionResult> RenewToken(TokenDTO tokenDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var token = await _jwtTokenService.RenewTokenAsync(tokenDTO, userId);
            if (token == null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Renew token không thành công !"
                });
            }
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Renew token thành công !",
                Data = token
            });
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetProduct()
        {
            return Ok("OK roi !");
        }

        
    }
    
}
