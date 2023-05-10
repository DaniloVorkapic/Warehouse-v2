using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Model
{
    public class Role:CoreObject //IdentityRole
    {
        public string Name { get; set; }
        public string Desctirption { get; set; }
        public List<RoleClaims> RoleClaimsList { get; set; }
        public List<CustomerRole> CustomerRoleList { get; set; }
    }
}
