using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Model
{
    public class Claims:CoreObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<RoleClaims> RoleClaimsList { get; set; }
    }
}
