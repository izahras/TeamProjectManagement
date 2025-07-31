using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using TeamProjectManagement.Enums;

namespace TeamProjectManagement.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeRolesAttribute : Attribute, IAuthorizationFilter
    {
        private readonly UserRole[] _roles;

        public AuthorizeRolesAttribute(params UserRole[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (_roles.Length > 0)
            {
                var userRoleClaim = user.FindFirst(ClaimTypes.Role);
                if (userRoleClaim == null || !Enum.TryParse<UserRole>(userRoleClaim.Value, out var userRole))
                {
                    context.Result = new ForbidResult();
                    return;
                }

                if (!_roles.Contains(userRole))
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
        }
    }
} 