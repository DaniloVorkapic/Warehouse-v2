    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Model.Enums;

namespace WarehouseWeb.Model
{
    public class Customer:CoreObject
    {
        public Customer()
        {
            CustomerRoleList = new List<CustomerRole>();
        }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<CustomerRole> CustomerRoleList { get; set; }
        public CustomerType CustomerType { get; set; }
        public long? CompanyId { get; set; }
       
        public Company Company { get; set; }
    }
}
