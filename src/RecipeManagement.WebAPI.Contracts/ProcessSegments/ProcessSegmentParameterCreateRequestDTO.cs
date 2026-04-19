namespace RecipeManagement.WebAPI.Contracts.ProcessSegments;

public sealed record ProcessSegmentParameterCreateRequestDTO(
    Guid ProcessSegmentId,
    string Name,
    string Value,
    string? DataType,
    string? Description,
    bool IsReadOnly);
