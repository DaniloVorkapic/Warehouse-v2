using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;

namespace WarehouseWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase

    {
        [HttpGet("[action]/{r}")]

        public ActionResult GetReturnResultByStatusCode(Result r) 
        {
            switch (r.StatusCode)
            {
                case StatusCodes.Status400BadRequest:
                   return BadRequest();
                    
                case StatusCodes.Status404NotFound:
                   return NotFound();
                   
                case StatusCodes.Status500InternalServerError:
                    return NotFound();

                default:
                    return Ok(r);

            }
        }     
    }
}
