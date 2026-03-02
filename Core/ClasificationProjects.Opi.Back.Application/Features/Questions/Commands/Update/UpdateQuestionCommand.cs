using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.Questions.Commands.Update;

public sealed record UpdateQuestionCommand(
    Guid Id,
    string Category,
    string Text,
    int Order
) : IRequest;

