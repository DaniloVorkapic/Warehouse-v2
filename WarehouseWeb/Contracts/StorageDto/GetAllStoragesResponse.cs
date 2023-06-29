using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Contracts.StorageDto
{
    public class GetAllStoragesResponse
    {
        public GetAllStoragesResponse(long id, string name, string city)
        {
            this.Id = id;
            this.Name = name;
            this.City = city;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }

    }
}
