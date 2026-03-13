using Microsoft.AspNetCore.JsonPatch;
using RecoveryFiscal.Application.ProcessosRecuperacaoFiscal.Models;

namespace RecoveryFiscal.Application.ProcessosRecuperacaoFiscal.Services;

public interface IProcessoRecuperacaoFiscalService
{
    Task<ProcessoRecuperacaoFiscalResponse> CriarAsync(ProcessoRecuperacaoFiscalRequest request, CancellationToken cancellationToken);
    Task<ProcessoRecuperacaoFiscalResponse> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task<ProcessoRecuperacaoFiscalPagedResponse> ListarAsync(ProcessoRecuperacaoFiscalFilter filter, CancellationToken cancellationToken);
    Task<ProcessoRecuperacaoFiscalResponse> AtualizarAsync(Guid id, ProcessoRecuperacaoFiscalRequest request, CancellationToken cancellationToken);
    Task<ProcessoRecuperacaoFiscalResponse> PatchAsync(Guid id, JsonPatchDocument<ProcessoRecuperacaoFiscalRequest> patchDocument, CancellationToken cancellationToken);
    Task RemoverAsync(Guid id, CancellationToken cancellationToken);
}
