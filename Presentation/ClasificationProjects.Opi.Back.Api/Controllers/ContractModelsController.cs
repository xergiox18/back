using ClasificationProjects.Opi.Back.Application.Features.ContractModels.Queries.Get;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClasificationProjects.Opi.Back.Api.Queries.Controllers;

[ApiController]
[Route("api/contract-models")]
[Authorize]
public sealed class ContractModelsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContractModelsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "ADMIN,PM")]
    public async Task<ActionResult<List<ContractModelDto>>> Get(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetContractModelsQuery(), ct);
        return Ok(result);
    }
}