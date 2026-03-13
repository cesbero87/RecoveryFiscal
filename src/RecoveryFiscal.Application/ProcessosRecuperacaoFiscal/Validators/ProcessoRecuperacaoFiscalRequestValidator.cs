using FluentValidation;
using RecoveryFiscal.Application.ProcessosRecuperacaoFiscal.Models;

namespace RecoveryFiscal.Application.ProcessosRecuperacaoFiscal.Validators;

public class ProcessoRecuperacaoFiscalRequestValidator : AbstractValidator<ProcessoRecuperacaoFiscalRequest>
{
    public ProcessoRecuperacaoFiscalRequestValidator()
    {
        RuleFor(x => x.NumeroProcesso)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.NifCliente)
            .NotEmpty()
            .Length(9);

        RuleFor(x => x.NomeCliente)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.ValorOriginalCredito)
            .GreaterThan(0);

        RuleFor(x => x.ValorRecuperavelEstimado)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.DataAnalise)
            .GreaterThanOrEqualTo(x => x.DataConstituicaoCredito)
            .WithMessage("A data de análise não pode ser inferior à data de constituição do crédito.");

        RuleFor(x => x.Observacoes)
            .MaximumLength(4000);
    }
}
