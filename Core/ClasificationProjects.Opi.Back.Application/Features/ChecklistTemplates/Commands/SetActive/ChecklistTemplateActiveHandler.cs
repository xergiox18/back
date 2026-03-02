using FluentValidation;
using MediatR;
using ClasificationProjects.Opi.Back.Domain.Repositories;

namespace ClasificationProjects.Opi.Back.Application.Features.ChecklistTemplates.Commands.SetActive;

public sealed class ChecklistTemplateActiveHandler : IRequestHandler<ChecklistTemplateActiveCommand>
{
    private readonly IChecklistTemplateRepository _repo;
    private readonly IValidator<ChecklistTemplateActiveCommand> _validator;

    public ChecklistTemplateActiveHandler(
        IChecklistTemplateRepository repo,
        IValidator<ChecklistTemplateActiveCommand> validator)
    {
        _repo = repo;
        _validator = validator;
    }

    public async Task Handle(ChecklistTemplateActiveCommand request, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        var aggregate = await _repo.GetByIdAsync(request.Id, ct);

        if (aggregate is null)
            throw new KeyNotFoundException();

        aggregate.SetActive(request.IsActive, DateTime.UtcNow);

        await _repo.UpdateAsync(aggregate, ct);
    }
}