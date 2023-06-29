using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Contracts.SupplierDto
{
    public class GetSupplierByIdResponse
    {
        public GetSupplierByIdResponse(long id, string name, string City)
        {
            this.Id = id;
            this.City = City;
            this.Name = name;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
    }
}
