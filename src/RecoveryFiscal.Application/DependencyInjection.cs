using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RecoveryFiscal.Application.ProcessosRecuperacaoFiscal.Models;
using RecoveryFiscal.Application.ProcessosRecuperacaoFiscal.Services;
using RecoveryFiscal.Application.ProcessosRecuperacaoFiscal.Validators;

namespace RecoveryFiscal.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProcessoRecuperacaoFiscalService, ProcessoRecuperacaoFiscalService>();

        services.AddValidatorsFromAssemblyContaining<ProcessoRecuperacaoFiscalRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ProcessoRecuperacaoFiscalFilterValidator>();

        return services;
    }
}