namespace RecipeManagement.Domain._Shared;

public abstract class Entity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; set; }

    protected Entity()
    {
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = CreatedAtUtc;
    }

    protected Entity(DateTime utcNow)
    {
        CreatedAtUtc = utcNow;
        UpdatedAtUtc = utcNow;
    }
}
