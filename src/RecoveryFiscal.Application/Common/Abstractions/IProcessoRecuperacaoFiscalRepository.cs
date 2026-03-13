using RecoveryFiscal.Application.Common.Models;
using RecoveryFiscal.Application.ProcessosRecuperacaoFiscal.Models;
using RecoveryFiscal.Domain.Entities;

namespace RecoveryFiscal.Application.Common.Abstractions;

public interface IProcessoRecuperacaoFiscalRepository
{
    Task<bool> NumeroProcessoExisteAsync(string numeroProcesso, Guid? ignorarId, CancellationToken cancellationToken);
    Task<ProcessoRecuperacaoFiscal?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResult<ProcessoRecuperacaoFiscal>> ListarAsync(ProcessoRecuperacaoFiscalFilter filter, CancellationToken cancellationToken);
    Task AdicionarAsync(ProcessoRecuperacaoFiscal entity, CancellationToken cancellationToken);
}
