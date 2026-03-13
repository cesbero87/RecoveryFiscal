namespace RecoveryFiscal.Application.Common.Models;

public class PagedResult<T>
{
    public IReadOnlyCollection<T> Items { get; init; } = Array.Empty<T>();
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public long TotalRecords { get; init; }
    public int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);
}
