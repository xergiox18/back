namespace ClasificationProjects.Opi.Back.Domain.Entities;

public sealed class ContractModel
{
    private ContractModel(Guid id, string code, string displayName)
    {
        Id = id;
        Code = code;
        DisplayName = displayName;
    }

    public Guid Id { get; private set; }
    public string Code { get; private set; }
    public string DisplayName { get; private set; }

    public static ContractModel Create(Guid id, string code, string displayName)
    {
        if (id == Guid.Empty)
            throw new ArgumentException(null, nameof(id));

        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException(null, nameof(code));

        if (code.Length > 50)
            throw new ArgumentException(null, nameof(code));

        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException(null, nameof(displayName));

        if (displayName.Length > 200)
            throw new ArgumentException(null, nameof(displayName));

        return new ContractModel(id, code.Trim(), displayName.Trim());
    }

    public void UpdateDisplayName(string displayName)
    {
        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException(null, nameof(displayName));

        if (displayName.Length > 200)
            throw new ArgumentException(null, nameof(displayName));

        DisplayName = displayName.Trim();
    }
}