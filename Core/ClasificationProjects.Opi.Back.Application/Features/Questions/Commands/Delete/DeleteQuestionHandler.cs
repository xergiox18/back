using FluentValidation;
using MediatR;
using ClasificationProjects.Opi.Back.Domain.Repositories;

namespace ClasificationProjects.Opi.Back.Application.Features.Questions.Commands.Delete;

public sealed class DeleteQuestionHandler : IRequestHandler<DeleteQuestionCommand>
{
    private readonly IQuestionRepository _repo;
    private readonly IValidator<DeleteQuestionCommand> _validator;

    public DeleteQuestionHandler(
        IQuestionRepository repo,
        IValidator<DeleteQuestionCommand> validator)
    {
        _repo = repo;
        _validator = validator;
    }

    public async Task Handle(DeleteQuestionCommand request, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        await _repo.DeleteHardAsync(request.Id, ct);
    }
}