using Asp.Versioning;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RecoveryFiscal.Application.ProcessosRecuperacaoFiscal.Models;
using RecoveryFiscal.Application.ProcessosRecuperacaoFiscal.Services;

namespace RecoveryFiscal.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/processos-recuperacao-fiscal")]
public class ProcessosRecuperacaoFiscalController : ControllerBase
{
    private readonly IProcessoRecuperacaoFiscalService _service;

    public ProcessosRecuperacaoFiscalController(IProcessoRecuperacaoFiscalService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ProcessoRecuperacaoFiscalRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CriarAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id, version = "1" }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.ObterPorIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] ProcessoRecuperacaoFiscalFilter filter, CancellationToken cancellationToken)
    {
        var result = await _service.ListarAsync(filter, cancellationToken);

        Response.Headers.Append("X-Pagination-PageNumber", result.PageNumber.ToString());
        Response.Headers.Append("X-Pagination-PageSize", result.PageSize.ToString());
        Response.Headers.Append("X-Pagination-TotalRecords", result.TotalRecords.ToString());
        Response.Headers.Append("X-Pagination-TotalPages", result.TotalPages.ToString());

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] ProcessoRecuperacaoFiscalRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.AtualizarAsync(id, request, cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<ProcessoRecuperacaoFiscalRequest> patchDocument, CancellationToken cancellationToken)
    {
        var result = await _service.PatchAsync(id, patchDocument, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _service.RemoverAsync(id, cancellationToken);
        return NoContent();
    }
}
