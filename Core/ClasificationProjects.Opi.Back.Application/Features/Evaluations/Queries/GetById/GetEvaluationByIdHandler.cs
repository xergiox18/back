using FluentValidation;
using MediatR;
using ClasificationProjects.Opi.Back.Domain.Repositories;

namespace ClasificationProjects.Opi.Back.Application.Features.Evaluations.Queries.GetById;

public sealed class GetEvaluationByIdHandler : IRequestHandler<GetEvaluationByIdQuery, EvaluationDetailsDto>
{
    private readonly IEvaluationRepository _repo;
    private readonly IValidator<GetEvaluationByIdQuery> _validator;

    public GetEvaluationByIdHandler(
        IEvaluationRepository repo,
        IValidator<GetEvaluationByIdQuery> validator)
    {
        _repo = repo;
        _validator = validator;
    }

    public async Task<EvaluationDetailsDto> Handle(GetEvaluationByIdQuery request, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        var evaluation = await _repo.GetByIdAsync(request.Id, ct);

        if (evaluation is null)
            throw new KeyNotFoundException();

        var templateId = evaluation.TemplateId ?? throw new InvalidOperationException();

        return new EvaluationDetailsDto(
            evaluation.Id,
            templateId,
            evaluation.RecommendedContractModelId,
            evaluation.ProjectName,
            evaluation.ClientName,
            evaluation.EvaluatedAt,
            evaluation.Score,
            evaluation.Confidence,
            evaluation.Answers
                .Select(a => new EvaluationAnswerDto(a.QuestionId, a.Answer))
                .ToList()
        );
    }
}