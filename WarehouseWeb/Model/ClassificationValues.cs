using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Model
{
    public class ClassificationValues:CoreObject
    { 
        public string Name { get; set; }
        public string Caption { get; set; }
        public long ClassifictionSpecificationId { get; set; }
        public ClassificationSpecification ClassificationSpecification { get; set; }
    }
}
