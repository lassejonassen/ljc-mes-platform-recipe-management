namespace RecipeManagement.Application.ProductSegments.DTOs;

public sealed record ProductSegmentDTO
{
    public required Guid Id { get; init; }
    public required Guid MaterialDefinitionId { get; init; }
    public required Guid ProcessSegmentId { get; init; }
    public required string MaterialSku { get; init; }
    public required string MaterialName { get; init; }
    public required string ProcessSegmentName { get; init; }
    public required string State { get; init; }
    public required int Version { get; init; }
    public List<ProductSegmentParameterDTO>? Parameters { get; init; }
}
