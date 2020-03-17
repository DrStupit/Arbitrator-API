using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Core;
using couchbase_rest_api.Models;
using couchbase_rest_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace couchbase_rest_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private IBookingService _bookingService;
        private IBucket _bucket;

        public BookingController(IBookingService bookingService)
        {
            _bucket = ClusterHelper.GetBucket("lawyermanagementdb");
            _bookingService = bookingService;

        }
        
        [HttpGet]
        [Route("GetAllBookings")]
        public IActionResult GetAll()
        {
            var bookings = _bookingService.GetAll();
            return Ok(bookings);
        }
        [HttpGet]
        [Route("GetBookingByUser")]
        public IActionResult GetBookingByUser(Guid userId)
        {
            var bookings = _bookingService.GetByUser(userId);
            return Ok(bookings);
        }

        [HttpPost]
        [Route("AddNewBooking")]
        public IActionResult AddNewCouncil([FromBody] Booking booking)
        {
            if (!booking.Id.HasValue && !booking.Cases.Id.HasValue)
            {
                booking.Id = Guid.NewGuid();
                booking.Cases.Id = Guid.NewGuid();

                _bucket.Upsert(booking.Id.ToString(), new
                {
                    booking.UserId,
                    booking.CouncilName,
                    booking.DateOfBooking,
                    booking.DateCreated,
                    booking.Cases,
                    booking.IsTravel,
                    V = booking.Cases.Type = "Case",
                    Type = "Booking"
                });
            }
            return Ok(booking);
        }
    }
}