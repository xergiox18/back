using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.Users.Commands.SetActive;

public sealed record SetUserActiveCommand(
    Guid Id,
    bool IsActive
) : IRequest;