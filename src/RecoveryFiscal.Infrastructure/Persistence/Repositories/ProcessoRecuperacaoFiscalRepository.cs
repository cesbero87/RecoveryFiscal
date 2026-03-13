using Microsoft.EntityFrameworkCore;
using RecoveryFiscal.Application.Common.Abstractions;
using RecoveryFiscal.Application.Common.Models;
using RecoveryFiscal.Application.ProcessosRecuperacaoFiscal.Models;
using RecoveryFiscal.Domain.Entities;

namespace RecoveryFiscal.Infrastructure.Persistence.Repositories;

public class ProcessoRecuperacaoFiscalRepository : IProcessoRecuperacaoFiscalRepository
{
    private readonly AppDbContext _dbContext;

    public ProcessoRecuperacaoFiscalRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> NumeroProcessoExisteAsync(string numeroProcesso, Guid? ignorarId, CancellationToken cancellationToken)
    {
        return await _dbContext.ProcessosRecuperacaoFiscal
            .AnyAsync(x => x.NumeroProcesso == numeroProcesso && (!ignorarId.HasValue || x.Id != ignorarId.Value), cancellationToken);
    }

    public Task<ProcessoRecuperacaoFiscal?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.ProcessosRecuperacaoFiscal
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PagedResult<ProcessoRecuperacaoFiscal>> ListarAsync(ProcessoRecuperacaoFiscalFilter filter, CancellationToken cancellationToken)
    {
        var query = _dbContext.ProcessosRecuperacaoFiscal.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.NumeroProcesso))
            query = query.Where(x => x.NumeroProcesso.Contains(filter.NumeroProcesso));

        if (!string.IsNullOrWhiteSpace(filter.NifCliente))
            query = query.Where(x => x.NifCliente == filter.NifCliente);

        if (!string.IsNullOrWhiteSpace(filter.NomeCliente))
            query = query.Where(x => x.NomeCliente.Contains(filter.NomeCliente));

        if (filter.Status.HasValue)
            query = query.Where(x => x.StatusProcesso == filter.Status.Value);

        if (filter.Prioridade.HasValue)
            query = query.Where(x => x.Prioridade == filter.Prioridade.Value);

        if (filter.DataAnaliseInicio.HasValue)
            query = query.Where(x => x.DataAnalise >= filter.DataAnaliseInicio.Value);

        if (filter.DataAnaliseFim.HasValue)
            query = query.Where(x => x.DataAnalise <= filter.DataAnaliseFim.Value);

        if (filter.Ativo.HasValue)
            query = query.Where(x => x.Ativo == filter.Ativo.Value);

        query = ApplySorting(query, filter);

        var totalRecords = await query.LongCountAsync(cancellationToken);

        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<ProcessoRecuperacaoFiscal>
        {
            Items = items,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalRecords = totalRecords
        };
    }

    public async Task AdicionarAsync(ProcessoRecuperacaoFiscal entity, CancellationToken cancellationToken)
    {
        await _dbContext.ProcessosRecuperacaoFiscal.AddAsync(entity, cancellationToken);
    }

    private static IQueryable<ProcessoRecuperacaoFiscal> ApplySorting(IQueryable<ProcessoRecuperacaoFiscal> query, ProcessoRecuperacaoFiscalFilter filter)
    {
        var desc = filter.SortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);

        return filter.SortBy.ToLowerInvariant() switch
        {
            "numeroprocesso" => desc ? query.OrderByDescending(x => x.NumeroProcesso) : query.OrderBy(x => x.NumeroProcesso),
            "nifcliente" => desc ? query.OrderByDescending(x => x.NifCliente) : query.OrderBy(x => x.NifCliente),
            "nomecliente" => desc ? query.OrderByDescending(x => x.NomeCliente) : query.OrderBy(x => x.NomeCliente),
            "valorrecuperavelestimado" => desc ? query.OrderByDescending(x => x.ValorRecuperavelEstimado) : query.OrderBy(x => x.ValorRecuperavelEstimado),
            "prioridade" => desc ? query.OrderByDescending(x => x.Prioridade) : query.OrderBy(x => x.Prioridade),
            "statusprocesso" => desc ? query.OrderByDescending(x => x.StatusProcesso) : query.OrderBy(x => x.StatusProcesso),
            "criadoemutc" => desc ? query.OrderByDescending(x => x.CriadoEmUtc) : query.OrderBy(x => x.CriadoEmUtc),
            _ => desc ? query.OrderByDescending(x => x.DataAnalise) : query.OrderBy(x => x.DataAnalise)
        };
    }
}
