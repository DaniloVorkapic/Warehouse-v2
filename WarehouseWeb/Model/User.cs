using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Model.Enums;

namespace WarehouseWeb.Model
{
    public class User:IdentityUser
    {
       // public long UserId { get; set; }
        // public string Email { get; set; }
       // public string Password { get; set; }
      //  public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
