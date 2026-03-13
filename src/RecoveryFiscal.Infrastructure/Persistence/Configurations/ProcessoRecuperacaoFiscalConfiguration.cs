using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecoveryFiscal.Domain.Entities;

namespace RecoveryFiscal.Infrastructure.Persistence.Configurations;

public class ProcessoRecuperacaoFiscalConfiguration : IEntityTypeConfiguration<ProcessoRecuperacaoFiscal>
{
    public void Configure(EntityTypeBuilder<ProcessoRecuperacaoFiscal> builder)
    {
        builder.ToTable("processos_recuperacao_fiscal");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.NumeroProcesso)
            .HasColumnName("numero_processo")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.NifCliente)
            .HasColumnName("nif_cliente")
            .HasMaxLength(9)
            .IsRequired();

        builder.Property(x => x.NomeCliente)
            .HasColumnName("nome_cliente")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.TipoCredito)
            .HasColumnName("tipo_credito")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.ValorOriginalCredito)
            .HasColumnName("valor_original_credito")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.ValorRecuperavelEstimado)
            .HasColumnName("valor_recuperavel_estimado")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.DataConstituicaoCredito)
            .HasColumnName("data_constituicao_credito")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.DataAnalise)
            .HasColumnName("data_analise")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.StatusProcesso)
            .HasColumnName("status_processo")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Prioridade)
            .HasColumnName("prioridade")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Observacoes)
            .HasColumnName("observacoes")
            .HasMaxLength(4000);

        builder.Property(x => x.Ativo)
            .HasColumnName("ativo")
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasColumnName("is_deleted")
            .IsRequired();

        builder.Property(x => x.CriadoEmUtc)
            .HasColumnName("criado_em_utc")
            .HasColumnType("datetime(6)")
            .IsRequired();

        builder.Property(x => x.AtualizadoEmUtc)
            .HasColumnName("atualizado_em_utc")
            .HasColumnType("datetime(6)");

        builder.Property(x => x.CriadoPor)
            .HasColumnName("criado_por")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.AtualizadoPor)
            .HasColumnName("atualizado_por")
            .HasMaxLength(100);

        builder.Property(x => x.RemovidoEmUtc)
            .HasColumnName("removido_em_utc")
            .HasColumnType("datetime(6)");

        builder.Property(x => x.RemovidoPor)
            .HasColumnName("removido_por")
            .HasMaxLength(100);

        builder.HasIndex(x => x.NumeroProcesso)
            .IsUnique();

        builder.HasIndex(x => new { x.StatusProcesso, x.Prioridade, x.DataAnalise });
        builder.HasIndex(x => x.NifCliente);
        builder.HasIndex(x => x.NomeCliente);
        builder.HasIndex(x => x.Ativo);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
