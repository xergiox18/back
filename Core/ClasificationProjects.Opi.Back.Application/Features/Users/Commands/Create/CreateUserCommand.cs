using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.Users.Commands.Create;

public sealed record CreateUserCommand(
    string TenantId,
    string Email,
    Guid RoleId
) : IRequest<Guid>;