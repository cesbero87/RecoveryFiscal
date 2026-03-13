using RecoveryFiscal.Domain.Common;
using RecoveryFiscal.Domain.Enums;

namespace RecoveryFiscal.Domain.Entities;

public class ProcessoRecuperacaoFiscal : BaseAuditableEntity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string NumeroProcesso { get; private set; } = string.Empty;
    public string NifCliente { get; private set; } = string.Empty;
    public string NomeCliente { get; private set; } = string.Empty;
    public TipoCredito TipoCredito { get; private set; }
    public decimal ValorOriginalCredito { get; private set; }
    public decimal ValorRecuperavelEstimado { get; private set; }
    public DateOnly DataConstituicaoCredito { get; private set; }
    public DateOnly DataAnalise { get; private set; }
    public StatusProcesso StatusProcesso { get; private set; }
    public PrioridadeProcesso Prioridade { get; private set; }
    public string? Observacoes { get; private set; }

    private ProcessoRecuperacaoFiscal()
    {
    }

    public ProcessoRecuperacaoFiscal(
        string numeroProcesso,
        string nifCliente,
        string nomeCliente,
        TipoCredito tipoCredito,
        decimal valorOriginalCredito,
        decimal valorRecuperavelEstimado,
        DateOnly dataConstituicaoCredito,
        DateOnly dataAnalise,
        StatusProcesso statusProcesso,
        PrioridadeProcesso prioridade,
        string? observacoes,
        string criadoPor)
    {
        AtualizarDados(
            numeroProcesso,
            nifCliente,
            nomeCliente,
            tipoCredito,
            valorOriginalCredito,
            valorRecuperavelEstimado,
            dataConstituicaoCredito,
            dataAnalise,
            statusProcesso,
            prioridade,
            observacoes,
            criadoPor);

        CriadoEmUtc = DateTime.UtcNow;
        CriadoPor = criadoPor;
        Ativo = true;
    }

    public void AtualizarDados(
        string numeroProcesso,
        string nifCliente,
        string nomeCliente,
        TipoCredito tipoCredito,
        decimal valorOriginalCredito,
        decimal valorRecuperavelEstimado,
        DateOnly dataConstituicaoCredito,
        DateOnly dataAnalise,
        StatusProcesso statusProcesso,
        PrioridadeProcesso prioridade,
        string? observacoes,
        string utilizador)
    {
        Validar(valorOriginalCredito, valorRecuperavelEstimado, dataConstituicaoCredito, dataAnalise);

        NumeroProcesso = numeroProcesso.Trim();
        NifCliente = nifCliente.Trim();
        NomeCliente = nomeCliente.Trim();
        TipoCredito = tipoCredito;
        ValorOriginalCredito = valorOriginalCredito;
        ValorRecuperavelEstimado = valorRecuperavelEstimado;
        DataConstituicaoCredito = dataConstituicaoCredito;
        DataAnalise = dataAnalise;
        StatusProcesso = statusProcesso;
        Prioridade = prioridade;
        Observacoes = string.IsNullOrWhiteSpace(observacoes) ? null : observacoes.Trim();
        AtualizadoEmUtc = DateTime.UtcNow;
        AtualizadoPor = utilizador;
    }

    public void AtualizarEstadoAtivo(bool ativo, string utilizador)
    {
        Ativo = ativo;
        AtualizadoEmUtc = DateTime.UtcNow;
        AtualizadoPor = utilizador;
    }

    public void SoftDelete(string utilizador)
    {
        IsDeleted = true;
        Ativo = false;
        RemovidoEmUtc = DateTime.UtcNow;
        RemovidoPor = utilizador;
        AtualizadoEmUtc = DateTime.UtcNow;
        AtualizadoPor = utilizador;
    }

    private static void Validar(
        decimal valorOriginalCredito,
        decimal valorRecuperavelEstimado,
        DateOnly dataConstituicaoCredito,
        DateOnly dataAnalise)
    {
        if (valorOriginalCredito <= 0)
        {
            throw new ArgumentException("O valor original do crédito deve ser superior a zero.");
        }

        if (valorRecuperavelEstimado < 0)
        {
            throw new ArgumentException("O valor recuperável estimado não pode ser negativo.");
        }

        if (dataAnalise < dataConstituicaoCredito)
        {
            throw new ArgumentException("A data de análise não pode ser inferior à data de constituição do crédito.");
        }
    }
}
