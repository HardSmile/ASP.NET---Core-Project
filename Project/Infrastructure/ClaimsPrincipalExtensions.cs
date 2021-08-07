using System.Security.Claims;

namespace Project.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal user)=>
            user.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}
