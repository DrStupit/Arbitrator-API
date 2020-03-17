using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace couchbase_rest_api.Models
{
    public class User
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CellNo { get; set; }
        public DateTime DateCreated { get; set; }
        public string Password { get; set; }
        public string  Token { get; set; }
        public string Type { get; set; }
    }
}
