using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace couchbase_rest_api.Models
{
    public class Council
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public string Name { get; set; }
        public Address PhysicalAddress { get; set; }
        public DateTime DateCreated { get; set; }
        public string Type { get; set; }
    }

    public class Address
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string AreaCode { get; set; }
        public string Province { get; set; }
    }
}
