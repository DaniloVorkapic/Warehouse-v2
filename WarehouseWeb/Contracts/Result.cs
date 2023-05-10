using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Contracts
{
    public class Result 
    {
        public Object? Value { get; set; }
        public int? StatusCode { get; set; }
        public static Result Create(Object? value, int? statusCode)
        {
            var result = new Result();
            result.Value = value;
            result.StatusCode = statusCode;
            return result;
        }
      
    }

    public class Result<T> : Result
    {
      
    }
}
