using ClasificationProjects.Opi.Back.Domain.Entities;

namespace ClasificationProjects.Opi.Back.Domain.Services;

public interface IEvaluationScoringService
{
    EvaluationScoringResult Compute(
        IReadOnlyList<Question> questions,
        IReadOnlyDictionary<Guid, bool> answersByQuestionId);
}

public sealed record EvaluationScoringResult(
    Guid RecommendedContractModelId,
    IReadOnlyDictionary<Guid, decimal> ScoresByContractModelId
);

public sealed class EvaluationScoringService : IEvaluationScoringService
{
    public EvaluationScoringResult Compute(
        IReadOnlyList<Question> questions,
        IReadOnlyDictionary<Guid, bool> answersByQuestionId)
    {
        if (questions is null)
            throw new ArgumentException(null, nameof(questions));

        if (questions.Count == 0)
            throw new ArgumentException(null, nameof(questions));

        if (answersByQuestionId is null)
            throw new ArgumentException(null, nameof(answersByQuestionId));

        var questionIds = questions.Select(q => q.Id).ToHashSet();

        if (answersByQuestionId.Keys.Any(id => !questionIds.Contains(id)))
            throw new ArgumentException(null, nameof(answersByQuestionId));

        if (questionIds.Any(id => !answersByQuestionId.ContainsKey(id)))
            throw new ArgumentException(null, nameof(answersByQuestionId));

        var scores = new Dictionary<Guid, decimal>();

        foreach (var q in questions)
        {
            if (!answersByQuestionId[q.Id])
                continue;

            foreach (var impact in q.Impacts)
            {
                if (!scores.TryGetValue(impact.ContractModelId, out var current))
                    current = 0m;

                scores[impact.ContractModelId] = current + impact.ImpactValue;
            }
        }

        if (scores.Count == 0)
        {
            var firstModelId = questions
                .SelectMany(x => x.Impacts)
                .Select(x => x.ContractModelId)
                .Distinct()
                .OrderBy(x => x)
                .First();

            return new EvaluationScoringResult(
                firstModelId,
                new Dictionary<Guid, decimal> { [firstModelId] = 0m });
        }

        var recommended = scores
            .OrderByDescending(x => x.Value)
            .ThenBy(x => x.Key)
            .First()
            .Key;

        return new EvaluationScoringResult(recommended, scores);
    }
}