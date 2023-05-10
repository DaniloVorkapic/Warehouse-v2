using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Model
{
    public class ClassificationSpecification:CoreObject
    { 
        public string Name { get; set; }
        public List<ClassificationValues> ClassificationValuesList { get; set; }
    }
}
