namespace RecipeManagement.WebAPI.Contracts.ProductSegments;

public sealed record ProductSegmentParameterUpdateRequestDTO(
    Guid ProductSegmentId,
    Guid ProductSegmentParameterId,
    string Value);
