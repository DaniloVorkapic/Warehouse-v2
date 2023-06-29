using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;

namespace WarehouseWeb.Middlewares
{
    public class GlobalExceptionHandler:IMiddleware
    {

        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);

            }
            catch (Exception ex)
            {
                await HandleException(context, ex);

            }
        }



        public static  Task HandleException(HttpContext context, Exception ex)
        {

            
            int statusCode = StatusCodes.Status500InternalServerError;
            context.Response.StatusCode = statusCode;

            var result = new Result
                {
                    StatusCode = context.Response.StatusCode,
                    ErrorMessage = ex.Message,
                    Value = null
                };

            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(result.ToString());
            
        }

        

    }
    public static class GlobalExceptionHandlerExtension
    {
        public static void ConfigureExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionHandler>(); 
        }
    }
}
