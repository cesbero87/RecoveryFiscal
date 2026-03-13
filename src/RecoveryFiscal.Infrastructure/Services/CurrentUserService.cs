using Microsoft.AspNetCore.Http;
using RecoveryFiscal.Application.Common.Abstractions;

namespace RecoveryFiscal.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentUsername()
    {
        return _httpContextAccessor.HttpContext?.Request.Headers["X-User-Name"].FirstOrDefault()
            ?? "sistema-local";
    }

    public string GetCorrelationId()
    {
        return _httpContextAccessor.HttpContext?.TraceIdentifier
            ?? Guid.NewGuid().ToString("N");
    }
}
