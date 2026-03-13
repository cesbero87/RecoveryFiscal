using Microsoft.EntityFrameworkCore;
using RecoveryFiscal.Application.Common.Abstractions;
using RecoveryFiscal.Domain.Entities;
using RecoveryFiscal.Infrastructure.Persistence.Configurations;

namespace RecoveryFiscal.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<ProcessoRecuperacaoFiscal> ProcessosRecuperacaoFiscal => Set<ProcessoRecuperacaoFiscal>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasCharSet("utf8mb4");
        modelBuilder.UseCollation("utf8mb4_unicode_ci");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProcessoRecuperacaoFiscalConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
