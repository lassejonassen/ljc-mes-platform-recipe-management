namespace RecipeManagement.WebAPI.Contracts.ProcessSegments;

public sealed record ProcessSegmentParameterResponseDTO
{
    public required Guid Id { get; init; }
    public required DateTime CreatedAtUtc { get; init; }
    public required DateTime UpdatedAtUtc { get; init; }
    public required string Name { get; init; }
    public required string Value { get; init; }
    public string? DataType { get; init; }
    public string? Description { get; init; }
    public required bool IsReadOnly { get; init; }
    public required Guid ProcessSegmentId { get; init; }
}
