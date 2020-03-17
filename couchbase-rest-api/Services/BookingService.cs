using Couchbase;
using Couchbase.Core;
using Couchbase.N1QL;
using couchbase_rest_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace couchbase_rest_api.Services
{
    public interface IBookingService
    {
        IEnumerable<Booking> GetAll();
        IEnumerable<Booking> GetByUser(Guid userId);

    }
    public class BookingService : IBookingService
    {
        private List<Booking> _bookings = new List<Booking>();
        private IBucket _bucket;

        public BookingService()
        {
            _bucket = ClusterHelper.GetBucket("lawyermanagementdb");
        }

        public void GetAllBookings()
        {
            var n1ql = @"SELECT b.*, META(b).id
                FROM lawyermanagementdb b
                WHERE b.type = 'Booking';";
            var query = QueryRequest.Create(n1ql);
            query.ScanConsistency(ScanConsistency.RequestPlus);
            var result = _bucket.Query<Booking>(query);

            foreach (Booking booking in result)
            {
                _bookings.Add(booking);
            }
        }
        public void GetBookingByUser(Guid userId)
        {
            var n1ql = @$"SELECT b.*, META(b).id
                FROM lawyermanagementdb b
                WHERE b.type = 'Booking' AND b.userId = '{userId}';";
            var query = QueryRequest.Create(n1ql);
            query.ScanConsistency(ScanConsistency.RequestPlus);
            var result = _bucket.Query<Booking>(query);

            foreach (Booking booking in result)
            {
                _bookings.Add(booking);
            }

        }


        public IEnumerable<Booking> GetAll()
        {
            GetAllBookings();
            return _bookings;
        }

        public IEnumerable<Booking> GetByUser(Guid userId)
        {
            GetBookingByUser(userId);
            return _bookings;
        }
    }
}
