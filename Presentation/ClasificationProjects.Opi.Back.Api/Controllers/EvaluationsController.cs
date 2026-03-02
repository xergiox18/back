using ClasificationProjects.Opi.Back.Application.Features.Evaluations.Commands.Create;
using ClasificationProjects.Opi.Back.Application.Features.Evaluations.Commands.Delete;
using ClasificationProjects.Opi.Back.Application.Features.Evaluations.Queries.GetAll;
using ClasificationProjects.Opi.Back.Application.Features.Evaluations.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClasificationProjects.Opi.Back.Api.Controllers;

[ApiController]
[Route("api/evaluations")]
[Authorize]
public sealed class EvaluationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EvaluationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "ADMIN,PM")]
    public async Task<ActionResult<List<EvaluationListItemDto>>> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetEvaluationsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "ADMIN,PM")]
    public async Task<ActionResult<EvaluationDetailsDto>> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetEvaluationByIdQuery(id), ct);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN,PM")]
    public async Task<ActionResult<CreateEvaluationResultDto>> Create(
        [FromBody] CreateEvaluationCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "ADMIN,PM")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteById([FromRoute] Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteEvaluationCommand(id), ct);
        return NoContent();
    }
}