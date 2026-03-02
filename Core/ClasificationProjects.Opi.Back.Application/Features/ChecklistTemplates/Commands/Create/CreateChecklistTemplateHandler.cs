using FluentValidation;
using MediatR;
using ClasificationProjects.Opi.Back.Domain.Aggregates;
using ClasificationProjects.Opi.Back.Domain.Repositories;

namespace ClasificationProjects.Opi.Back.Application.Features.ChecklistTemplates.Commands.Create;

public sealed class CreateChecklistTemplateHandler
    : IRequestHandler<CreateChecklistTemplateCommand, Guid>
{
    private readonly IChecklistTemplateRepository _repo;
    private readonly IValidator<CreateChecklistTemplateCommand> _validator;

    public CreateChecklistTemplateHandler(
        IChecklistTemplateRepository repo,
        IValidator<CreateChecklistTemplateCommand> validator)
    {
        _repo = repo;
        _validator = validator;
    }

    public async Task<Guid> Handle(CreateChecklistTemplateCommand request, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        var now = DateTime.UtcNow;

        var aggregate = ChecklistTemplate.Create(
            request.Name,
            request.Description,
            request.IsActive,
            now);

        await _repo.AddAsync(aggregate, ct);

        return aggregate.Id;
    }
}