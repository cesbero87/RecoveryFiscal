using RecoveryFiscal.Application.Common.Models;
using RecoveryFiscal.Domain.Entities;

namespace RecoveryFiscal.Application.ProcessosRecuperacaoFiscal.Models;

public static class ProcessoRecuperacaoFiscalMappings
{
    public static ProcessoRecuperacaoFiscalResponse ToResponse(this ProcessoRecuperacaoFiscal entity) =>
        new(
            entity.Id,
            entity.NumeroProcesso,
            entity.NifCliente,
            entity.NomeCliente,
            entity.TipoCredito,
            entity.ValorOriginalCredito,
            entity.ValorRecuperavelEstimado,
            entity.DataConstituicaoCredito,
            entity.DataAnalise,
            entity.StatusProcesso,
            entity.Prioridade,
            entity.Observacoes,
            entity.Ativo,
            entity.CriadoEmUtc,
            entity.AtualizadoEmUtc,
            entity.CriadoPor,
            entity.AtualizadoPor);

    public static ProcessoRecuperacaoFiscalPagedResponse ToPagedResponse(this PagedResult<ProcessoRecuperacaoFiscal> pagedResult) =>
        new(
            pagedResult.Items.Select(ToResponse).ToArray(),
            pagedResult.PageNumber,
            pagedResult.PageSize,
            pagedResult.TotalRecords,
            pagedResult.TotalPages);
}
