namespace RecipeManagement.WebAPI.Contracts.ProductSegments;

public sealed record ProductSegmentResponseDTO
{
    public required Guid Id { get; init; }
    public required Guid MaterialDefinitionId { get; init; }
    public required Guid ProcessSegmentId { get; init; }
    public required string MaterialSku { get; init; }
    public required string MaterialName { get; init; }
    public required string ProcessSegmentName { get; init; }
    public required string State { get; init; }
    public required int Version { get; init; }
    public List<ProductSegmentParameterResponseDTO>? Parameters { get; init; }
}
