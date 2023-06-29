using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Model;

namespace WarehouseWeb.Contracts
{
    public class LoginActivityDto
    {
        public long CustomerId { get; set; }
        public string Username { get; set; }
        public List<string> ClaimsList { get; set; }
        public string Token { get; set; }
    }
}
