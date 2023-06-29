using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WarehouseWeb.Contracts;
using WarehouseWeb.Services;

namespace WarehouseWeb.Authentication
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {

            string? memberId = context.User.Claims.FirstOrDefault(
                x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(memberId, out long parsedMemberId))
            {
                return;
            }
            using IServiceScope scope = _serviceScopeFactory.CreateScope();

            IUserService _userService = scope.ServiceProvider
                .GetRequiredService<IUserService>();

            Result result = await _userService.GetAllClaims(parsedMemberId);  

            List<IEnumerable<string>> claims = (List<IEnumerable<string>>)result.Value;

            if (claims.Any(x => x.Contains(requirement.Permission)))
            {
                context.Succeed(requirement);
            }
        }
    }
}
