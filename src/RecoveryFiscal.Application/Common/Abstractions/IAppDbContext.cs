using Microsoft.EntityFrameworkCore;
using RecoveryFiscal.Domain.Entities;

namespace RecoveryFiscal.Application.Common.Abstractions;

public interface IAppDbContext
{
    DbSet<ProcessoRecuperacaoFiscal> ProcessosRecuperacaoFiscal { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
