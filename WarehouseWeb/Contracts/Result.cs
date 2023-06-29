using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace WarehouseWeb.Contracts
{
    public class Result 
    {
        public Object? Value { get; set; }
        public int? StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public int totalCount { get; set; }

        public static Result Create(Object? value, int? statusCode,string errorMessage,int totalCount)
        {
            var result = new Result();
            result.Value = value;
            result.StatusCode = statusCode;
            result.ErrorMessage = errorMessage;
            result.totalCount = totalCount;
            return result;
        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

    }
    
    

    public class Result<T> : Result
    {
      
    }
}
