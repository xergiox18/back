using MediatR;
using ClasificationProjects.Opi.Back.Domain.Aggregates;
using ClasificationProjects.Opi.Back.Domain.Repositories;
using ClasificationProjects.Opi.Back.Domain.Services;

namespace ClasificationProjects.Opi.Back.Application.Features.Evaluations.Commands.Create;

public sealed class CreateEvaluationHandler
    : IRequestHandler<CreateEvaluationCommand, CreateEvaluationResultDto>
{
    private readonly IChecklistTemplateRepository _templates;
    private readonly IEvaluationRepository _evaluations;
    private readonly IEvaluationScoringService _scoring;

    public CreateEvaluationHandler(
        IChecklistTemplateRepository templates,
        IEvaluationRepository evaluations,
        IEvaluationScoringService scoring)
    {
        _templates = templates;
        _evaluations = evaluations;
        _scoring = scoring;
    }

    public async Task<CreateEvaluationResultDto> Handle(CreateEvaluationCommand request, CancellationToken ct)
    {
        var template = await _templates.GetByIdAsync(request.TemplateId, ct);

        if (template is null)
            throw new KeyNotFoundException();

        if (!template.IsActive)
            throw new InvalidOperationException();

        var answersByQuestionId = request.Answers
            .GroupBy(x => x.QuestionId)
            .ToDictionary(g => g.Key, g => g.Last().Answer);

        var evaluation = Evaluation.CreateFromAnswers(
            request.TemplateId,
            template.Name,
            template.Description,
            request.ProjectName,
            request.ClientName,
            template.Questions.ToList(),
            answersByQuestionId,
            _scoring,
            DateTime.UtcNow
        );

        await _evaluations.AddAsync(evaluation, ct);

        return new CreateEvaluationResultDto(
            evaluation.Id,
            evaluation.RecommendedContractModelId,
            evaluation.Score,
            evaluation.Confidence
        );
    }
}