using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WarehouseWeb.Authentication
{
    public  sealed class HasPermissionAttribute:AuthorizeAttribute
    {

        public HasPermissionAttribute(Permission permission):base(policy:permission.ToString())
        { 

        }
    }
}
