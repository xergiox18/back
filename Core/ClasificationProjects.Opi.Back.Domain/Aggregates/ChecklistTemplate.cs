using System.Collections.ObjectModel;
using ClasificationProjects.Opi.Back.Domain.Entities;
using ClasificationProjects.Opi.Back.Domain.ValueObjects;

namespace ClasificationProjects.Opi.Back.Domain.Aggregates;

public sealed class ChecklistTemplate
{
    private readonly List<Question> _questions = new();

    private ChecklistTemplate(
        Guid id,
        string name,
        string description,
        bool isActive,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyCollection<Question> Questions => new ReadOnlyCollection<Question>(_questions);

    public static ChecklistTemplate Create(string name, string description, bool isActive, DateTime now)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(null, nameof(name));

        if (name.Length > 200)
            throw new ArgumentException(null, nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException(null, nameof(description));

        if (description.Length > 2000)
            throw new ArgumentException(null, nameof(description));

        return new ChecklistTemplate(Guid.NewGuid(), name.Trim(), description.Trim(), isActive, now, now);
    }

    public static ChecklistTemplate Rehydrate(
        Guid id,
        string name,
        string description,
        bool isActive,
        DateTime createdAt,
        DateTime updatedAt,
        IReadOnlyCollection<Question> questions)
    {
        if (id == Guid.Empty)
            throw new ArgumentException(null, nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(null, nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException(null, nameof(description));

        var aggregate = new ChecklistTemplate(
            id,
            name.Trim(),
            description.Trim(),
            isActive,
            createdAt,
            updatedAt);

        if (questions is not null && questions.Count > 0)
        {
            foreach (var q in questions)
                aggregate.AttachQuestion(q);
        }

        return aggregate;
    }

    public void UpdateInfo(string name, string description, DateTime now)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(null, nameof(name));

        if (name.Length > 200)
            throw new ArgumentException(null, nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException(null, nameof(description));

        if (description.Length > 2000)
            throw new ArgumentException(null, nameof(description));

        Name = name.Trim();
        Description = description.Trim();
        UpdatedAt = now;
    }

    public void SetActive(bool isActive, DateTime now)
    {
        if (IsActive == isActive)
            throw new InvalidOperationException();

        IsActive = isActive;
        UpdatedAt = now;
    }

    public Question AddQuestion(string category, string text, int order, IReadOnlyCollection<QuestionImpact> impacts, DateTime now)
    {
        if (order < 1)
            throw new ArgumentException(null, nameof(order));

        if (_questions.Any(x => x.Order == order))
            throw new InvalidOperationException();

        var question = Question.Create(Id, category, text, order, impacts, now);
        _questions.Add(question);
        UpdatedAt = now;

        return question;
    }

    public void AttachQuestion(Question question)
    {
        if (question.TemplateId != Id)
            throw new InvalidOperationException();

        if (_questions.Any(x => x.Id == question.Id))
            return;

        if (_questions.Any(x => x.Order == question.Order && x.Id != question.Id))
            throw new InvalidOperationException();

        _questions.Add(question);
    }
}