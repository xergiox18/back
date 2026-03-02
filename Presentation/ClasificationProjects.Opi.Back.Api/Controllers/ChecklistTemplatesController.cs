using ClasificationProjects.Opi.Back.Application.Features.ChecklistTemplates.Commands.Create;
using ClasificationProjects.Opi.Back.Application.Features.ChecklistTemplates.Commands.Delete;
using ClasificationProjects.Opi.Back.Application.Features.ChecklistTemplates.Commands.SetActive;
using ClasificationProjects.Opi.Back.Application.Features.ChecklistTemplates.Commands.Update;
using ClasificationProjects.Opi.Back.Application.Features.ChecklistTemplates.Queries.Get;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClasificationProjects.Opi.Back.Api.Controllers;

[ApiController]
[Route("api/checklist-templates")]
[Authorize]
public sealed class ChecklistTemplatesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChecklistTemplatesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "ADMIN,PM")]
    public async Task<ActionResult<List<ChecklistTemplateDto>>> Get(
        [FromQuery] ChecklistTemplateStatusFilter status = ChecklistTemplateStatusFilter.Active,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetChecklistTemplatesQuery(status), ct);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateChecklistTemplateCommand command,
        CancellationToken ct)
    {
        var id = await _mediator.Send(command, ct);
        return Ok(id);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateChecklistTemplateCommand command,
        CancellationToken ct)
    {
        if (id != command.Id)
            return BadRequest(new { message = "Route id and body id must match." });

        await _mediator.Send(command, ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/active")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> SetActive(
        [FromRoute] Guid id,
        [FromQuery] ChecklistTemplateActiveMode mode,
        CancellationToken ct)
    {
        var isActive = mode == ChecklistTemplateActiveMode.Activar;

        await _mediator.Send(new ChecklistTemplateActiveCommand(id, isActive), ct);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "ADMIN")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteById([FromRoute] Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteChecklistTemplateCommand(id), ct);
        return NoContent();
    }

    public enum ChecklistTemplateActiveMode
    {
        Activar = 1,
        Desactivar = 2
    }
}