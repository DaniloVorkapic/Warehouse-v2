using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Model
{
    public class RoleClaims:CoreObject
    {
        public long RoleId { get; set; }
        public Role Role { get; set; }
        public long ClaimsId { get; set; }
        public Claims Claims { get; set; }
    }
}
