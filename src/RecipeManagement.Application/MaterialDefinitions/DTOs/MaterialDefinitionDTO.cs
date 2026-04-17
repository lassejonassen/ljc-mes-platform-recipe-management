namespace RecipeManagement.Application.MaterialDefinitions.DTOs;

public sealed record MaterialDefinitionDTO
{
    public required Guid Id { get; init; }
    public required string Sku { get; init; }
    public required string Name { get; init; }
    public required DateTime CreatedAtUtc { get; init; }
    public required DateTime UpdatedAtUtc { get; init; }
    public List<MaterialDefinitionPropertyDTO>? Properties { get; init; }
}
