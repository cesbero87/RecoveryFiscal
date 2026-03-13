using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using RecoveryFiscal.Application.Common.Abstractions;
using RecoveryFiscal.Application.Common.Exceptions;
using RecoveryFiscal.Application.ProcessosRecuperacaoFiscal.Models;
using RecoveryFiscal.Domain.Entities;

namespace RecoveryFiscal.Application.ProcessosRecuperacaoFiscal.Services;

public class ProcessoRecuperacaoFiscalService : IProcessoRecuperacaoFiscalService
{
    private readonly IProcessoRecuperacaoFiscalRepository _repository;
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IValidator<ProcessoRecuperacaoFiscalRequest> _requestValidator;
    private readonly IValidator<ProcessoRecuperacaoFiscalFilter> _filterValidator;

    public ProcessoRecuperacaoFiscalService(
        IProcessoRecuperacaoFiscalRepository repository,
        IAppDbContext dbContext,
        ICurrentUserService currentUserService,
        IValidator<ProcessoRecuperacaoFiscalRequest> requestValidator,
        IValidator<ProcessoRecuperacaoFiscalFilter> filterValidator)
    {
        _repository = repository;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _requestValidator = requestValidator;
        _filterValidator = filterValidator;
    }

    public async Task<ProcessoRecuperacaoFiscalResponse> CriarAsync(ProcessoRecuperacaoFiscalRequest request, CancellationToken cancellationToken)
    {
        await ValidarRequestAsync(request, cancellationToken);

        if (await _repository.NumeroProcessoExisteAsync(request.NumeroProcesso, null, cancellationToken))
        {
            throw new ConflictException("Já existe um processo com o número informado.");
        }

        var utilizador = _currentUserService.GetCurrentUsername();

        var entity = new ProcessoRecuperacaoFiscal(
            request.NumeroProcesso,
            request.NifCliente,
            request.NomeCliente,
            request.TipoCredito,
            request.ValorOriginalCredito,
            request.ValorRecuperavelEstimado,
            request.DataConstituicaoCredito,
            request.DataAnalise,
            request.StatusProcesso,
            request.Prioridade,
            request.Observacoes,
            utilizador);

        entity.AtualizarEstadoAtivo(request.Ativo, utilizador);

        await _repository.AdicionarAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity.ToResponse();
    }

    public async Task<ProcessoRecuperacaoFiscalResponse> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await ObterEntidadeOuFalharAsync(id, cancellationToken);
        return entity.ToResponse();
    }

    public async Task<ProcessoRecuperacaoFiscalPagedResponse> ListarAsync(ProcessoRecuperacaoFiscalFilter filter, CancellationToken cancellationToken)
    {
        await ValidarFiltroAsync(filter, cancellationToken);
        var result = await _repository.ListarAsync(filter, cancellationToken);
        return result.ToPagedResponse();
    }

    public async Task<ProcessoRecuperacaoFiscalResponse> AtualizarAsync(Guid id, ProcessoRecuperacaoFiscalRequest request, CancellationToken cancellationToken)
    {
        await ValidarRequestAsync(request, cancellationToken);

        var entity = await ObterEntidadeOuFalharAsync(id, cancellationToken);

        if (await _repository.NumeroProcessoExisteAsync(request.NumeroProcesso, id, cancellationToken))
        {
            throw new ConflictException("Já existe um processo com o número informado.");
        }

        var utilizador = _currentUserService.GetCurrentUsername();

        entity.AtualizarDados(
            request.NumeroProcesso,
            request.NifCliente,
            request.NomeCliente,
            request.TipoCredito,
            request.ValorOriginalCredito,
            request.ValorRecuperavelEstimado,
            request.DataConstituicaoCredito,
            request.DataAnalise,
            request.StatusProcesso,
            request.Prioridade,
            request.Observacoes,
            utilizador);

        entity.AtualizarEstadoAtivo(request.Ativo, utilizador);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity.ToResponse();
    }

    public async Task<ProcessoRecuperacaoFiscalResponse> PatchAsync(Guid id, JsonPatchDocument<ProcessoRecuperacaoFiscalRequest> patchDocument, CancellationToken cancellationToken)
    {
        var entity = await ObterEntidadeOuFalharAsync(id, cancellationToken);

        var current = new ProcessoRecuperacaoFiscalRequest(
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
            entity.Ativo);

        patchDocument.ApplyTo(current);

        return await AtualizarAsync(id, current, cancellationToken);
    }

    public async Task RemoverAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await ObterEntidadeOuFalharAsync(id, cancellationToken);
        entity.SoftDelete(_currentUserService.GetCurrentUsername());
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<ProcessoRecuperacaoFiscal> ObterEntidadeOuFalharAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _repository.ObterPorIdAsync(id, cancellationToken);

        if (entity is null)
        {
            throw new NotFoundException("Processo não encontrado.");
        }

        return entity;
    }

    private async Task ValidarRequestAsync(ProcessoRecuperacaoFiscalRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            IReadOnlyDictionary<string, string[]> errors =
                new Dictionary<string, string[]>(validationResult.ToDictionary());

            throw new ValidationAppException(errors);
        }
    }

    private async Task ValidarFiltroAsync(ProcessoRecuperacaoFiscalFilter filter, CancellationToken cancellationToken)
    {
        var validationResult = await _filterValidator.ValidateAsync(filter, cancellationToken);

        if (!validationResult.IsValid)
        {
            IReadOnlyDictionary<string, string[]> errors =
                new Dictionary<string, string[]>(validationResult.ToDictionary());

            throw new ValidationAppException(errors);
        }
    }
}
