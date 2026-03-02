using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.Users.Commands.Update;

public sealed record UpdateUserCommand(
    Guid Id,
    Guid RoleId,
    string Email,
    bool IsActive
) : IRequest;