using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Contracts.CustomerDto
{
    public class GetCustomerByIdResponse
    {
        public GetCustomerByIdResponse(long id, string firstName, string lastName)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;

        }

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
