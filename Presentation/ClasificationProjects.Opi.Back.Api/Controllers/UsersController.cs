using ClasificationProjects.Opi.Back.Application.Features.Users.Commands.Create;
using ClasificationProjects.Opi.Back.Application.Features.Users.Commands.Delete;
using ClasificationProjects.Opi.Back.Application.Features.Users.Commands.SetActive;
using ClasificationProjects.Opi.Back.Application.Features.Users.Commands.Update;
using ClasificationProjects.Opi.Back.Application.Features.Users.Queries.GetAll;
using ClasificationProjects.Opi.Back.Application.Features.Users.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClasificationProjects.Opi.Back.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(Roles = "ADMIN")]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;

    public UsersController(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserListItemDto>>> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetUsersQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDetailsDto>> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id), ct);
        return Ok(result);
    }

    public sealed record CreateUserRequest(
        string Email,
        Guid RoleId
    );

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateUserRequest body, CancellationToken ct)
    {
        var tenantId = _configuration["AzureAd:TenantId"];

        if (string.IsNullOrWhiteSpace(tenantId))
            throw new InvalidOperationException();

        var id = await _mediator.Send(new CreateUserCommand(tenantId, body.Email, body.RoleId), ct);
        return Ok(id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateUserCommand command,
        CancellationToken ct)
    {
        if (id != command.Id)
            return BadRequest(new { message = "Route id and body id must match." });

        await _mediator.Send(command, ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/active")]
    public async Task<IActionResult> SetActive(
        [FromRoute] Guid id,
        [FromQuery] UserActiveMode mode,
        CancellationToken ct)
    {
        var isActive = mode == UserActiveMode.Activar;

        await _mediator.Send(new SetUserActiveCommand(id, isActive), ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteById([FromRoute] Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteUserCommand(id), ct);
        return NoContent();
    }

    public enum UserActiveMode
    {
        Activar = 1,
        Desactivar = 2
    }
}