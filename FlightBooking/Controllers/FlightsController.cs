using FlightBooking.Application.Commands.FlightCommands;
using FlightBooking.Application.DTOs;
using FlightBooking.Application.Query;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FlightRoutesController> _logger;

        public FlightsController(IMediator mediator, ILogger<FlightRoutesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("get-all-flight")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllFlightQuery query)
        {
            _logger.LogInformation("reques lấy ds chuyến bay");
            var flights = await _mediator.Send(query);
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Lấy ds chuyến bay thành công !",
                Data = flights
            });
        }

        [HttpGet("search-flights")]
        public async Task<IActionResult> Search([FromQuery] SearchFlightsQuery query)
        {
            _logger.LogInformation("reques tìm kiếm chuyến bay");
            var flights = await _mediator.Send(query);
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Tìm kiếm chuyến bay thành công !",
                Data = flights
            });
        }

        [HttpPost("create-flight")]
        public async Task<IActionResult> Create(CreateFlightCommand command)
        {
            _logger.LogInformation("request tạo chuyến bay !");
            var createFlight = await _mediator.Send(command);
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Tạo chuyến bay thành công !",
                Data = command
            });
        }

        [HttpDelete("delete-flight")]
        public async Task<IActionResult> Delete(DeleteFlightCommand command)
        {
            _logger.LogInformation("request xóa chuyến bay !");
            var result = await _mediator.Send(command);
            if (!result)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Xóa chuyến bay thất bại !",
                });
            }
            return NoContent();
        }

        [HttpPut("update-flight")]
        public async Task<IActionResult> Update(UpdateFlightCommand command)
        {
            _logger.LogInformation("request cập nhật chuyến bay !");
            var result = await _mediator.Send(command);
            if (!result)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Xóa chuyến bay thất bại !",
                });
            }
            return NoContent();
        }
    }
}
