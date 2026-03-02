namespace ClasificationProjects.Opi.Back.Domain.Entities;

public sealed class EvaluationAnswer
{
    private EvaluationAnswer(Guid evaluationId, Guid questionId, bool answer)
    {
        EvaluationId = evaluationId;
        QuestionId = questionId;
        Answer = answer;
    }

    public Guid EvaluationId { get; private set; }
    public Guid QuestionId { get; private set; }
    public bool Answer { get; private set; }

    public static EvaluationAnswer Create(Guid evaluationId, Guid questionId, bool answer)
    {
        if (evaluationId == Guid.Empty)
            throw new ArgumentException(null, nameof(evaluationId));

        if (questionId == Guid.Empty)
            throw new ArgumentException(null, nameof(questionId));

        return new EvaluationAnswer(evaluationId, questionId, answer);
    }
}