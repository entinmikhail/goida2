using System.Security.Claims;

namespace goida.Services;

public interface ICurrentUserService
{
    string? UserId { get; }
    bool IsInRole(string role);
}

public class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
{
    public string? UserId => accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

    public bool IsInRole(string role) => accessor.HttpContext?.User.IsInRole(role) ?? false;
}
