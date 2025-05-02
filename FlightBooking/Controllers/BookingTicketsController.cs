using FlightBooking.Application.Commands.BookingTicketCommands;
using FlightBooking.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Exceptions;

namespace FlightBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingTicketsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BookingTicketsController> _logger;

        public BookingTicketsController(IMediator mediator, ILogger<BookingTicketsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("booking-ticket")]
        public async Task<IActionResult> BookingTicket(BookingCommand model)
        {
            try
            {
                var result = await _mediator.Send(model);
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Đặt vé thành công !",
                    Data = model
                });
            }
            catch (Exception ex)
            {
                throw new AppException($"Lỗi server {ex.Message}", 500);
            }
        }
    }
}
