using FluentValidation;
using MediatR;
using ClasificationProjects.Opi.Back.Domain.Repositories;

namespace ClasificationProjects.Opi.Back.Application.Features.ChecklistTemplates.Commands.Delete;

public sealed class DeleteChecklistTemplateHandler : IRequestHandler<DeleteChecklistTemplateCommand>
{
    private readonly IChecklistTemplateRepository _repo;
    private readonly IValidator<DeleteChecklistTemplateCommand> _validator;

    public DeleteChecklistTemplateHandler(
        IChecklistTemplateRepository repo,
        IValidator<DeleteChecklistTemplateCommand> validator)
    {
        _repo = repo;
        _validator = validator;
    }

    public async Task Handle(DeleteChecklistTemplateCommand request, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        await _repo.DeleteHardAsync(request.Id, ct);
    }
}