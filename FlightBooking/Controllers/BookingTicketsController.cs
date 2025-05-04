using FlightBooking.Application.Commands.BookingTicketCommands;
using FlightBooking.Application.DTOs;
using FlightBooking.Application.Services;
using FlightBooking.Application.Services.IServices;
using FlightBooking.Infrastructure.Repositories;
using FlightBooking.Infrastructure.Repositories.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Exceptions;
using System.Security.Claims;

namespace FlightBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingTicketsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BookingTicketsController> _logger;
        private readonly IFlightRepository _flightRepository;
        private readonly IVnPayService _vnPayService;
        private readonly IResponseCacheService _responseCacheService;

        public BookingTicketsController(IMediator mediator, ILogger<BookingTicketsController> logger,
                                        IVnPayService vnPayService, IResponseCacheService responseCacheService,
                                        IFlightRepository flightRepository)
        {
            _mediator = mediator;
            _logger = logger;
            _flightRepository = flightRepository;
            _vnPayService = vnPayService;
            _responseCacheService = responseCacheService;
        }

        [HttpGet("PaymentCallBack")]
        public async Task<IActionResult> PaymentCallBack([FromQuery] string email, [FromQuery] string orderId)
        {
            var keyCache = $"{email}:{orderId}";
            var cachedData = await _responseCacheService.GetCacheResponseAsync(keyCache);

            if (string.IsNullOrEmpty(cachedData))
            {
                return NotFound("Phiên thanh toán đã hết hạn !");
            }

            cachedData = JsonConvert.DeserializeObject<string>(cachedData);
            // Chuyển chuỗi JSON thành object
            var _model = JsonConvert.DeserializeObject<BookingTichketDTO>(cachedData);

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new ApiResponse { Success = false, Message = "Không tìm thấy email!" });
            }

            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response == null)
            {
                throw new KeyNotFoundException();
            }
            else if (response.VnPayResponseCode != "00")
            {
                throw new AppException("Thanh toán không thành công");
            }

            var maThanhVien = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _model.MaThanhVien = maThanhVien;
            var result = await _mediator.Send(_model);
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Đặt vé thành công !",
                Data = _model
            });
        }

        [HttpPost("booking-ticket")]
        [Authorize]
        public async Task<IActionResult> BookingTicket(BookingCommand model)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                var flight = await _flightRepository.GetById(model.BookingTichketDTO.MaChuyenBay);
                //payment model
                var vnPayModel = new VnPaymentRequestMode
                {
                    CreatedDate = DateTime.Now,
                    Amount = flight.GiaVe,
                    Description = $"{model.BookingTichketDTO.HoTen} {model.BookingTichketDTO.SDT}",
                    FullName = model.BookingTichketDTO.HoTen,
                    OrderId = new Random().Next(1000, 10000),
                    Email = email,
                };

                var cacheDataString = JsonConvert.SerializeObject(model);
                var keyCache = $"{email}:{vnPayModel.OrderId}";

                await _responseCacheService.SetCacheReponseAsync(keyCache, cacheDataString, TimeSpan.FromMinutes(15));

                var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, vnPayModel);
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Ok",
                    Data = paymentUrl
                });
                
            }
            catch (Exception ex)
            {
                throw new AppException($"Lỗi server {ex.Message}", 500);
            }
        }
        [Authorize]

        [HttpPost("cancel-booking-ticket")]
        public async Task<IActionResult> CancelBookingTicket([FromQuery] CancelBookingTicketCommand model)
        {
            try
            {
                var result = await _mediator.Send(model);
                if(!result)

                    return BadRequest(new ApiResponse
                    {
                        Success = true,
                        Message = "Hủy vé không thành công !"
                    });
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Hủy vé thành công !"
                });
            }
            catch (Exception ex)
            {
                throw new AppException($"Lỗi server {ex.Message}", 500);
            }
        }
    }
}
