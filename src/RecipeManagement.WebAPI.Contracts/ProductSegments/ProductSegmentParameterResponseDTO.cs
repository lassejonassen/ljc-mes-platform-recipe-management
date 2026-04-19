namespace RecipeManagement.WebAPI.Contracts.ProductSegments;

public sealed record ProductSegmentParameterResponseDTO
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Value { get; init; }
    public string? Description { get; init; }
    public string? DataType { get; init; }
}
