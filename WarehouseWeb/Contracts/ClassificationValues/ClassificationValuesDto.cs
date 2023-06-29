using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Contracts.ClassificationValues
{
    public class ClassificationValuesDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public long ClassificationSpecificationId { get; set; }

    }
}
