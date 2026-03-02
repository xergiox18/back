using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.Questions.Commands.Delete;

public sealed record DeleteQuestionCommand(Guid Id) : IRequest;
