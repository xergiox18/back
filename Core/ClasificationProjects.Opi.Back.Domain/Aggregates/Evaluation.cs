using System.Collections.ObjectModel;
using ClasificationProjects.Opi.Back.Domain.Entities;
using ClasificationProjects.Opi.Back.Domain.Services;

namespace ClasificationProjects.Opi.Back.Domain.Aggregates;

public sealed class Evaluation
{
    private readonly List<EvaluationAnswer> _answers = new();

    private Evaluation(
        Guid id,
        Guid? templateId,
        Guid recommendedContractModelId,
        string templateName,
        string templateDescription,
        string projectName,
        string clientName,
        DateTime evaluatedAt,
        decimal score,
        decimal confidence)
    {
        Id = id;
        TemplateId = templateId;
        RecommendedContractModelId = recommendedContractModelId;
        TemplateName = templateName;
        TemplateDescription = templateDescription;
        ProjectName = projectName;
        ClientName = clientName;
        EvaluatedAt = evaluatedAt;
        Score = score;
        Confidence = confidence;
    }

    public Guid Id { get; private set; }
    public Guid? TemplateId { get; private set; }
    public Guid RecommendedContractModelId { get; private set; }

    public string TemplateName { get; private set; }
    public string TemplateDescription { get; private set; }

    public string ProjectName { get; private set; }
    public string ClientName { get; private set; }

    public DateTime EvaluatedAt { get; private set; }

    public decimal Score { get; private set; }
    public decimal Confidence { get; private set; }

    public IReadOnlyCollection<EvaluationAnswer> Answers => new ReadOnlyCollection<EvaluationAnswer>(_answers);

    public static Evaluation CreateFromAnswers(
        Guid templateId,
        string templateName,
        string templateDescription,
        string projectName,
        string clientName,
        IReadOnlyList<Question> questions,
        IReadOnlyDictionary<Guid, bool> answersByQuestionId,
        IEvaluationScoringService scoringService,
        DateTime now)
    {
        if (templateId == Guid.Empty)
            throw new ArgumentException(null, nameof(templateId));

        if (string.IsNullOrWhiteSpace(templateName))
            throw new ArgumentException(null, nameof(templateName));

        if (templateName.Length > 200)
            throw new ArgumentException(null, nameof(templateName));

        if (string.IsNullOrWhiteSpace(templateDescription))
            throw new ArgumentException(null, nameof(templateDescription));

        if (templateDescription.Length > 2000)
            throw new ArgumentException(null, nameof(templateDescription));

        if (string.IsNullOrWhiteSpace(projectName))
            throw new ArgumentException(null, nameof(projectName));

        if (projectName.Length > 200)
            throw new ArgumentException(null, nameof(projectName));

        if (string.IsNullOrWhiteSpace(clientName))
            throw new ArgumentException(null, nameof(clientName));

        if (clientName.Length > 200)
            throw new ArgumentException(null, nameof(clientName));

        if (questions is null || questions.Count == 0)
            throw new ArgumentException(null, nameof(questions));

        if (answersByQuestionId is null)
            throw new ArgumentException(null, nameof(answersByQuestionId));

        if (scoringService is null)
            throw new ArgumentException(null, nameof(scoringService));

        var scoringResult = scoringService.Compute(questions, answersByQuestionId);

        if (scoringResult.RecommendedContractModelId == Guid.Empty)
            throw new InvalidOperationException();

        if (!scoringResult.ScoresByContractModelId.TryGetValue(scoringResult.RecommendedContractModelId, out var topScore))
            topScore = 0m;

        var secondScore = scoringResult.ScoresByContractModelId
            .Where(x => x.Key != scoringResult.RecommendedContractModelId)
            .Select(x => x.Value)
            .OrderByDescending(x => x)
            .FirstOrDefault();

        var confidence = topScore <= 0m ? 0m : (topScore - secondScore) / topScore;

        var evaluation = new Evaluation(
            Guid.NewGuid(),
            templateId,
            scoringResult.RecommendedContractModelId,
            templateName.Trim(),
            templateDescription.Trim(),
            projectName.Trim(),
            clientName.Trim(),
            now,
            topScore,
            confidence);

        foreach (var kvp in answersByQuestionId)
            evaluation._answers.Add(EvaluationAnswer.Create(evaluation.Id, kvp.Key, kvp.Value));

        return evaluation;
    }

    public static Evaluation Rehydrate(
        Guid id,
        Guid? templateId,
        Guid recommendedContractModelId,
        string templateName,
        string templateDescription,
        string projectName,
        string clientName,
        DateTime evaluatedAt,
        decimal score,
        decimal confidence,
        IReadOnlyCollection<EvaluationAnswer> answers)
    {
        if (id == Guid.Empty)
            throw new ArgumentException(null, nameof(id));

        if (recommendedContractModelId == Guid.Empty)
            throw new ArgumentException(null, nameof(recommendedContractModelId));

        if (string.IsNullOrWhiteSpace(templateName))
            throw new ArgumentException(null, nameof(templateName));

        if (string.IsNullOrWhiteSpace(templateDescription))
            throw new ArgumentException(null, nameof(templateDescription));

        if (string.IsNullOrWhiteSpace(projectName))
            throw new ArgumentException(null, nameof(projectName));

        if (string.IsNullOrWhiteSpace(clientName))
            throw new ArgumentException(null, nameof(clientName));

        var evaluation = new Evaluation(
            id,
            templateId,
            recommendedContractModelId,
            templateName.Trim(),
            templateDescription.Trim(),
            projectName.Trim(),
            clientName.Trim(),
            evaluatedAt,
            score,
            confidence);

        if (answers is not null && answers.Count > 0)
            evaluation._answers.AddRange(answers);

        return evaluation;
    }
}