namespace RecipeManagement.WebAPI.Contracts.ProductSegments;

public sealed record ProductSegmentListResponseDTO
{
    public required IEnumerable<ProductSegmentResponseDTO> Data { get; init; } = [];
}
