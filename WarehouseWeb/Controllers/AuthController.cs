﻿


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseWeb.Authentication;
using WarehouseWeb.Contracts;
using WarehouseWeb.Model;
using WarehouseWeb.Services;

namespace WarehouseWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]

        public async Task<ActionResult<Result<bool>>> Register(RegistrationDTO request)
        {
            Result r = await _userService.Register(request);
             return GetReturnResultByStatusCode(r);

        }

        [HttpPost("login")]

        public async Task<ActionResult<Result<bool>>> Login(LoginDto request)
        {
            Result r = await _userService.Login(request);
            return GetReturnResultByStatusCode(r);

        }
        [HttpPost("logout")]

        public async Task<ActionResult<Result<bool>>> Logout()
        {
            Result r = await _userService.Logout();
            return GetReturnResultByStatusCode(r);

        }

      //  [Authorize(AuthenticationSchemes = "Bearer")]
      [HasPermission(Permission.addOrder)]
        [Authorize]
        [HttpGet]
        [Route("api/controller/GetAllClaimsByID")]
        public async Task<ActionResult<Result<IEnumerable<Claims>>>> GetAllClaimsByID(long id)
        {
            Result r = await _userService.GetAllClaims(id);
            return GetReturnResultByStatusCode(r);

        }
    }
}
