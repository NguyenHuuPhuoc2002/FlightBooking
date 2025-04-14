using FlightBooking.Application.DTOs;
using FlightBooking.Entities.Entities;
using FlightBooking.Infrastructure.Repositories;
using FlightBooking.Infrastructure.Repositories.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Application.Commands
{
    public class RegisterUserCommand : IRequest<IdentityResult>
    {
        public SignUpDTO SignUpDTO { get; set; }

        public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, IdentityResult>
        {
            private readonly IAccountRepository _accountRepository;

            public RegisterUserCommandHandler(IAccountRepository accountRepository)
            {
                _accountRepository = accountRepository;
            }
            public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
            {
                var user = new ApplicationUser
                {
                    LastName = request.SignUpDTO.LastName,
                    FirstName = request.SignUpDTO.FirstName,
                    Email = request.SignUpDTO.Email,
                    PasswordHash = request.SignUpDTO.Password,
                    DateOfBirth = request.SignUpDTO.DayOfBirth,
                    Gender = request.SignUpDTO.Gender,
                    CCCD = request.SignUpDTO.CCCD,
                    Image = null,
                    UserName = request.SignUpDTO.Email
                };

                var registerUser = await _accountRepository.SignUpAsync(user);
                return registerUser;
            }
        }
    }
}
