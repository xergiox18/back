using FluentValidation;
using MediatR;
using ClasificationProjects.Opi.Back.Domain.Repositories;

namespace ClasificationProjects.Opi.Back.Application.Features.Evaluations.Commands.Delete;

public sealed class DeleteEvaluationHandler : IRequestHandler<DeleteEvaluationCommand>
{
    private readonly IEvaluationRepository _repo;
    private readonly IValidator<DeleteEvaluationCommand> _validator;

    public DeleteEvaluationHandler(
        IEvaluationRepository repo,
        IValidator<DeleteEvaluationCommand> validator)
    {
        _repo = repo;
        _validator = validator;
    }

    public async Task Handle(DeleteEvaluationCommand request, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        await _repo.DeleteHardAsync(request.Id, ct);
    }
}