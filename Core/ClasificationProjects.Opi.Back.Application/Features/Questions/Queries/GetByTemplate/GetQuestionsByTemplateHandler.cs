using FluentValidation;
using MediatR;
using ClasificationProjects.Opi.Back.Domain.Repositories;

namespace ClasificationProjects.Opi.Back.Application.Features.Questions.Queries.GetByTemplate;

public sealed class GetQuestionsByTemplateHandler : IRequestHandler<GetQuestionsByTemplateQuery, List<QuestionDto>>
{
    private readonly IChecklistTemplateRepository _templateRepo;
    private readonly IValidator<GetQuestionsByTemplateQuery> _validator;

    public GetQuestionsByTemplateHandler(
        IChecklistTemplateRepository templateRepo,
        IValidator<GetQuestionsByTemplateQuery> validator)
    {
        _templateRepo = templateRepo;
        _validator = validator;
    }

    public async Task<List<QuestionDto>> Handle(GetQuestionsByTemplateQuery request, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        var template = await _templateRepo.GetByIdAsync(request.TemplateId, ct);

        if (template is null)
            throw new KeyNotFoundException();

        return template.Questions
            .OrderBy(x => x.Order)
            .Select(x => new QuestionDto(
                x.Id,
                x.Category,
                x.Text,
                x.Order,
                x.Impacts
                    .Select(i => new QuestionImpactDto(i.ContractModelId, i.ImpactValue))
                    .ToList()
            ))
            .ToList();
    }
}