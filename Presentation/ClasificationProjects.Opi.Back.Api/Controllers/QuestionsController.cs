using ClasificationProjects.Opi.Back.Application.Features.Questions.Commands.Create;
using ClasificationProjects.Opi.Back.Application.Features.Questions.Commands.Delete;
using ClasificationProjects.Opi.Back.Application.Features.Questions.Commands.Update;
using ClasificationProjects.Opi.Back.Application.Features.Questions.Commands.UpdateImpacts;
using ClasificationProjects.Opi.Back.Application.Features.Questions.Queries.GetByTemplate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClasificationProjects.Opi.Back.Api.Controllers;

[ApiController]
[Route("api")]
[Authorize]
public sealed class QuestionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public QuestionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("templates/{templateId:guid}/questions")]
    [Authorize(Roles = "ADMIN,PM")]
    public async Task<ActionResult<List<QuestionDto>>> GetByTemplate(
        [FromRoute] Guid templateId,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new GetQuestionsByTemplateQuery(templateId), ct);
        return Ok(result);
    }

    [HttpPost("questions/batch")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<List<Guid>>> CreateBatch(
        [FromBody] CreateQuestionsCommand command,
        CancellationToken ct)
    {
        var ids = await _mediator.Send(command, ct);
        return Ok(ids);
    }

    [HttpPut("questions/{id:guid}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateQuestionCommand command,
        CancellationToken ct)
    {
        if (id != command.Id)
            return BadRequest(new { message = "Route id and body id must match." });

        await _mediator.Send(command, ct);
        return NoContent();
    }

    [HttpPut("questions/{id:guid}/impacts")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> SetImpacts(
        [FromRoute] Guid id,
        [FromBody] SetQuestionImpactsCommand command,
        CancellationToken ct)
    {
        if (id != command.QuestionId)
            return BadRequest(new { message = "Route id and body QuestionId must match." });

        await _mediator.Send(command, ct);
        return NoContent();
    }

    [HttpDelete("questions/{id:guid}")]
    [Authorize(Roles = "ADMIN")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteById([FromRoute] Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteQuestionCommand(id), ct);
        return NoContent();
    }
}