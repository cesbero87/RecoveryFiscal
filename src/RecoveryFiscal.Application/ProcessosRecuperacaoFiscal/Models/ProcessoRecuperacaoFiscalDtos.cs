using RecoveryFiscal.Domain.Enums;

namespace RecoveryFiscal.Application.ProcessosRecuperacaoFiscal.Models;

public record ProcessoRecuperacaoFiscalRequest(
    string NumeroProcesso,
    string NifCliente,
    string NomeCliente,
    TipoCredito TipoCredito,
    decimal ValorOriginalCredito,
    decimal ValorRecuperavelEstimado,
    DateOnly DataConstituicaoCredito,
    DateOnly DataAnalise,
    StatusProcesso StatusProcesso,
    PrioridadeProcesso Prioridade,
    string? Observacoes,
    bool Ativo);

public record ProcessoRecuperacaoFiscalResponse(
    Guid Id,
    string NumeroProcesso,
    string NifCliente,
    string NomeCliente,
    TipoCredito TipoCredito,
    decimal ValorOriginalCredito,
    decimal ValorRecuperavelEstimado,
    DateOnly DataConstituicaoCredito,
    DateOnly DataAnalise,
    StatusProcesso StatusProcesso,
    PrioridadeProcesso Prioridade,
    string? Observacoes,
    bool Ativo,
    DateTime CriadoEmUtc,
    DateTime? AtualizadoEmUtc,
    string CriadoPor,
    string? AtualizadoPor);

public record ProcessoRecuperacaoFiscalPagedResponse(
    IReadOnlyCollection<ProcessoRecuperacaoFiscalResponse> Items,
    int PageNumber,
    int PageSize,
    long TotalRecords,
    int TotalPages);

public class ProcessoRecuperacaoFiscalFilter
{
    public string? NumeroProcesso { get; init; }
    public string? NifCliente { get; init; }
    public string? NomeCliente { get; init; }
    public StatusProcesso? Status { get; init; }
    public PrioridadeProcesso? Prioridade { get; init; }
    public DateOnly? DataAnaliseInicio { get; init; }
    public DateOnly? DataAnaliseFim { get; init; }
    public bool? Ativo { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string SortBy { get; init; } = "dataAnalise";
    public string SortDirection { get; init; } = "desc";
}
