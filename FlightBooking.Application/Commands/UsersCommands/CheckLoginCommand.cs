using FlightBooking.Application.DTOs;
using FlightBooking.Entities.Entities;
using FlightBooking.Infrastructure.Repositories;
using FlightBooking.Infrastructure.Repositories.Interfaces;
using FlightBooking.Infrastructure.Repositories.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Application.Commands.UsersCommand
{
    public class CheckLoginCommand : IRequest<IdentityUser>
    {
        public SignInDTO SignInDTO { get; set; }
        public class CheckLoginCommandHandler : IRequestHandler<CheckLoginCommand, IdentityUser>
        {
            private readonly IAccountRepository _accountRepository;
            private readonly ILogger<CheckLoginCommandHandler> _logger;

            public CheckLoginCommandHandler(IAccountRepository accountRepository, ILogger<CheckLoginCommandHandler> logger)
            {
                _accountRepository = accountRepository;
                _logger = logger;
            }
            public async Task<IdentityUser> Handle(CheckLoginCommand request, CancellationToken cancellationToken)
            {
                var checkLogin = await _accountRepository.SignInAsync(request.SignInDTO.Email, request.SignInDTO.Password);
                if (checkLogin == null)
                {
                    return null;
                }
                return checkLogin;
            }
        }
    }
}
