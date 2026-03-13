using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecoveryFiscal.Application.Common.Abstractions;
using RecoveryFiscal.Infrastructure.Persistence;
using RecoveryFiscal.Infrastructure.Persistence.Repositories;
using RecoveryFiscal.Infrastructure.Services;

namespace RecoveryFiscal.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MySql")
            ?? configuration["ConnectionStrings__MySql"]
            ?? throw new InvalidOperationException("A connection string 'MySql' não foi configurada.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        // services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
        // services.AddHttpContextAccessor();
        // services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
        services.AddScoped<IProcessoRecuperacaoFiscalRepository, ProcessoRecuperacaoFiscalRepository>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
