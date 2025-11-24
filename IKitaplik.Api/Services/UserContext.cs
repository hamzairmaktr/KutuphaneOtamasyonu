using Core.Contexts;
using System.Security.Claims;

namespace IKitaplik.Api.Services
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor accessor)
        {
            _httpContextAccessor = accessor;
        }

        public string UserId =>
        _httpContextAccessor.HttpContext?
           .User?
           .FindFirst(ClaimTypes.NameIdentifier)?
           .Value ?? "0";
    }
}
