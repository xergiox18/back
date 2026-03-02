namespace ClasificationProjects.Opi.Back.Domain.ValueObjects;

public sealed record QuestionImpact(Guid ContractModelId, int ImpactValue)
{
    public static QuestionImpact Create(Guid contractModelId, int impactValue)
    {
        if (contractModelId == Guid.Empty)
            throw new ArgumentException(null, nameof(contractModelId));

        return new QuestionImpact(contractModelId, impactValue);
    }
}