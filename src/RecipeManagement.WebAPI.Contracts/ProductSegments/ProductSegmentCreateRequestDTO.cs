namespace RecipeManagement.WebAPI.Contracts.ProductSegments;

public sealed record ProductSegmentCreateRequestDTO(Guid MaterialDefinitionId, Guid ProcessSegmentId);
