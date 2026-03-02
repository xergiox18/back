using System.Collections.ObjectModel;
using ClasificationProjects.Opi.Back.Domain.ValueObjects;

namespace ClasificationProjects.Opi.Back.Domain.Entities;

public sealed class Question
{
    private readonly List<QuestionImpact> _impacts = new();

    private Question(
        Guid id,
        Guid templateId,
        string category,
        string text,
        int order,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        TemplateId = templateId;
        Category = category;
        Text = text;
        Order = order;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public Guid Id { get; private set; }
    public Guid TemplateId { get; private set; }
    public string Category { get; private set; }
    public string Text { get; private set; }
    public int Order { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyCollection<QuestionImpact> Impacts => new ReadOnlyCollection<QuestionImpact>(_impacts);

    public static Question Create(Guid templateId, string category, string text, int order, IReadOnlyCollection<QuestionImpact> impacts, DateTime now)
    {
        if (templateId == Guid.Empty)
            throw new ArgumentException(null, nameof(templateId));

        ValidateCategory(category);
        ValidateText(text);

        if (order < 1)
            throw new ArgumentException(null, nameof(order));

        ValidateImpacts(impacts);

        var q = new Question(Guid.NewGuid(), templateId, category.Trim(), text.Trim(), order, now, now);
        q._impacts.AddRange(impacts);

        return q;
    }

    public static Question Rehydrate(
        Guid id,
        Guid templateId,
        string category,
        string text,
        int order,
        DateTime createdAt,
        DateTime updatedAt,
        IReadOnlyCollection<QuestionImpact> impacts)
    {
        if (id == Guid.Empty)
            throw new ArgumentException(null, nameof(id));

        if (templateId == Guid.Empty)
            throw new ArgumentException(null, nameof(templateId));

        ValidateCategory(category);
        ValidateText(text);

        if (order < 1)
            throw new ArgumentException(null, nameof(order));

        ValidateImpacts(impacts);

        var q = new Question(id, templateId, category.Trim(), text.Trim(), order, createdAt, updatedAt);
        q._impacts.AddRange(impacts);

        return q;
    }

    public void Update(string category, string text, int order, DateTime now)
    {
        ValidateCategory(category);
        ValidateText(text);

        if (order < 1)
            throw new ArgumentException(null, nameof(order));

        Category = category.Trim();
        Text = text.Trim();
        Order = order;
        UpdatedAt = now;
    }

    public void SetImpacts(IReadOnlyCollection<QuestionImpact> impacts, DateTime now)
    {
        ValidateImpacts(impacts);

        _impacts.Clear();
        _impacts.AddRange(impacts);
        UpdatedAt = now;
    }

    private static void ValidateCategory(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException(null, nameof(category));

        if (category.Length > 100)
            throw new ArgumentException(null, nameof(category));
    }

    private static void ValidateText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException(null, nameof(text));

        if (text.Length > 2000)
            throw new ArgumentException(null, nameof(text));
    }

    private static void ValidateImpacts(IReadOnlyCollection<QuestionImpact> impacts)
    {
        if (impacts is null)
            throw new ArgumentException(null, nameof(impacts));

        if (impacts.Count != 4)
            throw new ArgumentException(null, nameof(impacts));

        var ids = impacts.Select(x => x.ContractModelId).ToList();

        if (ids.Any(x => x == Guid.Empty))
            throw new ArgumentException(null, nameof(impacts));

        if (ids.Distinct().Count() != 4)
            throw new ArgumentException(null, nameof(impacts));
    }
}