namespace RecipeManagement.Application.MaterialDefinitions.DTOs;

public sealed record MaterialDefinitionPropertyDTO
{
    public required Guid Id { get; init; }
    public required Guid MaterialDefinitionId { get; init; }
    public required string Name { get; init; }
    public required string Value { get; init; }
    public string? Description { get; init; }
    public string? DataType { get; init; }
    public required DateTime CreatedAtUtc { get; init; }
    public required DateTime UpdatedAtUtc { get; init; }
}
