using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace couchbase_rest_api.Models
{
    public class Booking
    {
        public Guid? Id { get; set; }
        public string UserId { get; set; }
        public string CouncilName { get; set; }
        public DateTime DateOfBooking { get; set; }
        public DateTime DateCreated { get; set; }
        public Case Cases { get; set; }
        public bool IsTravel { get; set; }
        public string Type { get; set; }
    }

    public class Case
    {
        public Guid? Id { get; set; }
        public string ApplicantName { get; set; }
        public string DefendentName { get; set; }
        public string CaseNo { get; set; }
        public DateTime TimeOfCase { get; set; }
        public string Venue { get; set; }
        public string Type { get; set; }
    }
}
