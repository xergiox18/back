using FluentValidation;
using MediatR;
using ClasificationProjects.Opi.Back.Domain.Repositories;

namespace ClasificationProjects.Opi.Back.Application.Features.ChecklistTemplates.Commands.Update;

public sealed class UpdateChecklistTemplateHandler : IRequestHandler<UpdateChecklistTemplateCommand>
{
    private readonly IChecklistTemplateRepository _repo;
    private readonly IValidator<UpdateChecklistTemplateCommand> _validator;

    public UpdateChecklistTemplateHandler(
        IChecklistTemplateRepository repo,
        IValidator<UpdateChecklistTemplateCommand> validator)
    {
        _repo = repo;
        _validator = validator;
    }

    public async Task Handle(UpdateChecklistTemplateCommand request, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        var aggregate = await _repo.GetByIdAsync(request.Id, ct);

        if (aggregate is null)
            throw new KeyNotFoundException();

        aggregate.UpdateInfo(request.Name, request.Description, DateTime.UtcNow);

        await _repo.UpdateAsync(aggregate, ct);
    }
}