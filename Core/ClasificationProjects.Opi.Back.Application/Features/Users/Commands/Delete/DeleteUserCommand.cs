using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.Users.Commands.Delete;

public sealed record DeleteUserCommand(Guid Id) : IRequest;