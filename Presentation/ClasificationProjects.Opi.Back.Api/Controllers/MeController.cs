using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClasificationProjects.Opi.Back.Api.Controllers;

[ApiController]
[Route("api/me")]
[Authorize]
public sealed class MeController : ControllerBase
{
    [HttpGet]
    public ActionResult<MeDto> Get()
    {
        var id = User.FindFirstValue("user_id") ?? string.Empty;
        var email = User.FindFirstValue("user_email") ?? string.Empty;
        var name = User.FindFirstValue("user_name") ?? User.FindFirstValue("name") ?? string.Empty;
        var role = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;

        return Ok(new MeDto(id, name, email, role));
    }
}

public sealed record MeDto(
    string Id,
    string Name,
    string Email,
    string Role
);