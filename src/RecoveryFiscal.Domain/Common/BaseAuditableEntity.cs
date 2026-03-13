namespace RecoveryFiscal.Domain.Common;

public abstract class BaseAuditableEntity
{
    public DateTime CriadoEmUtc { get; set; }
    public DateTime? AtualizadoEmUtc { get; set; }
    public string CriadoPor { get; set; } = string.Empty;
    public string? AtualizadoPor { get; set; }
    public bool Ativo { get; set; } = true;
    public bool IsDeleted { get; set; }
    public DateTime? RemovidoEmUtc { get; set; }
    public string? RemovidoPor { get; set; }
}
