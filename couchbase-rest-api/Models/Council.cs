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
        public string PhysicalAddress { get; set; }
        public DateTime DateCreated { get; set; }
        public string Type { get; set; }
    }
}
