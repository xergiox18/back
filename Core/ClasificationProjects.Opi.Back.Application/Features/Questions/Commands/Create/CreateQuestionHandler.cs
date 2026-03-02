using FluentValidation;
using MediatR;
using ClasificationProjects.Opi.Back.Domain.Repositories;
using ClasificationProjects.Opi.Back.Domain.ValueObjects;

namespace ClasificationProjects.Opi.Back.Application.Features.Questions.Commands.Create;

public sealed class CreateQuestionsHandler : IRequestHandler<CreateQuestionsCommand, List<Guid>>
{
    private readonly IChecklistTemplateRepository _templateRepo;
    private readonly IQuestionRepository _questionRepo;
    private readonly IValidator<CreateQuestionsCommand> _validator;

    public CreateQuestionsHandler(
        IChecklistTemplateRepository templateRepo,
        IQuestionRepository questionRepo,
        IValidator<CreateQuestionsCommand> validator)
    {
        _templateRepo = templateRepo;
        _questionRepo = questionRepo;
        _validator = validator;
    }

    public async Task<List<Guid>> Handle(CreateQuestionsCommand request, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        var template = await _templateRepo.GetByIdAsync(request.TemplateId, ct);

        if (template is null)
            throw new KeyNotFoundException();

        var now = DateTime.UtcNow;

        var createdQuestions = new List<Domain.Entities.Question>(request.Questions.Count);
        var createdIds = new List<Guid>(request.Questions.Count);

        foreach (var q in request.Questions)
        {
            var impacts = q.Impacts
                .Select(i => QuestionImpact.Create(i.ContractModelId, i.ImpactValue))
                .ToList();

            var created = template.AddQuestion(q.Category, q.Text, q.Order, impacts, now);

            createdQuestions.Add(created);
            createdIds.Add(created.Id);
        }

        await _questionRepo.AddManyAsync(template.Id, createdQuestions, ct);
        await _templateRepo.UpdateAsync(template, ct);

        return createdIds;
    }
}