using Microsoft.AspNetCore.Http;
using ProcurementSys.Application.Abstractions.Services;

namespace ProcurementSys.Infrastructure.Identity;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUserService(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    public int UserId => int.Parse(_httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value ?? "0");
    public string Role => _httpContextAccessor.HttpContext?.User.FindFirst("role")?.Value ?? string.Empty;
}
