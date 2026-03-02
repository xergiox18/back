using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.Evaluations.Commands.Delete;

public sealed record DeleteEvaluationCommand(Guid Id) : IRequest;
