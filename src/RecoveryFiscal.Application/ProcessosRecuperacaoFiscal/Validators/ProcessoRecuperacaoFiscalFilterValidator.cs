using FluentValidation;
using RecoveryFiscal.Application.ProcessosRecuperacaoFiscal.Models;

namespace RecoveryFiscal.Application.ProcessosRecuperacaoFiscal.Validators;

public class ProcessoRecuperacaoFiscalFilterValidator : AbstractValidator<ProcessoRecuperacaoFiscalFilter>
{
    private static readonly string[] SortsPermitidos =
    [
        "numeroProcesso",
        "nifCliente",
        "nomeCliente",
        "dataAnalise",
        "valorRecuperavelEstimado",
        "prioridade",
        "statusProcesso",
        "criadoEmUtc"
    ];

    public ProcessoRecuperacaoFiscalFilterValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.SortDirection)
            .Must(v => v.Equals("asc", StringComparison.OrdinalIgnoreCase) || v.Equals("desc", StringComparison.OrdinalIgnoreCase))
            .WithMessage("sortDirection deve ser asc ou desc.");

        RuleFor(x => x.SortBy)
            .Must(v => SortsPermitidos.Contains(v, StringComparer.OrdinalIgnoreCase))
            .WithMessage($"sortBy inválido. Valores permitidos: {string.Join(", ", SortsPermitidos)}.");

        RuleFor(x => x)
            .Must(x => !x.DataAnaliseInicio.HasValue || !x.DataAnaliseFim.HasValue || x.DataAnaliseInicio <= x.DataAnaliseFim)
            .WithMessage("O período de análise é inválido.");
    }
}
