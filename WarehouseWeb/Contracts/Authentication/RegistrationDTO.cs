using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Model.Enums;

namespace WarehouseWeb.Contracts
{
    public class RegistrationDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string  PhoneNumber { get; set; }
        public string Password { get; set; }
        public CustomerType CustomerType { get; set; }
        //public long? companyId { get; set; }
        public string RoleName { get; set; }
        public string CompanyName { get; set; }
        public string Pib { get; set; }

        // [Required]
        //[Compare("Password")]
        //public string PasswordConfirm { get; set; }

    }
}
