namespace RecipeManagement.WebAPI.Contracts.ProcessSegments;

public sealed record ProcessSegmentUpdateRequestDTO(Guid ProcessSegmentId, string Name);