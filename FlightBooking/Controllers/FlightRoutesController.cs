using FlightBooking.Application.Commands.FlightRouteCommands;
using FlightBooking.Application.DTOs;
using FlightBooking.Application.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Helper;

namespace FlightBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = AppRole.ADMIN)]
    public class FlightRoutesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FlightRoutesController> _logger;

        public FlightRoutesController(IMediator mediator, ILogger<FlightRoutesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("get-all-flight-route")]
        public async Task<IActionResult> GetAll(int? page, int? pageSize)
        {
            int pageIndex = page ?? 1;
            int pSize = pageSize ?? 3;

            _logger.LogInformation("reques lấy ds tuyến bay từ client !");
            var listFRoute = await _mediator.Send(new GetAllFlightRouteQuery(pageIndex, pSize));
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Lấy dữ liệu thành công !",
                Data = listFRoute
            });
        }

        [HttpPost("create-flight-route")]
        public async Task<IActionResult> CreateFlightRoute(CreateFlightRouteCommand command)
        {
            var createFRoute = await _mediator.Send(command);
            if (createFRoute)
            {
                return Conflict(new ApiResponse
                {
                    Success = false,
                    Message = "Tuyến bay đã tồn tại !",
                });
            }
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Tạo mới tuyến bay thành công !",
                Data = command
            });
        }

        [HttpDelete("delete-flight-route")]
        public async Task<IActionResult> DeleteFlightRoute(DeleteFlightRouteCommand command)
        {
            if (command == null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Đầu vào không hợp lệ !",
                });
            }
            var createFRoute = await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("update-flight-route")]
        public async Task<IActionResult> UpdateFlightRoute(UpdateFlightRouteCommand command)
        {
            var updateFRoute = await _mediator.Send(command);
            if (!updateFRoute)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Đầu vào không hợp lệ !",
                });
            }
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Cập nhật tuyến bay thành công !",
                Data = command
            });
        }
    }
}
