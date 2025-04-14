using FlightBooking.Application.Commands;
using FlightBooking.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(IMediator mediator, ILogger<AccountsController> logger) 
        {
            _mediator = mediator;
            _logger = logger;
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
    }
    
}
