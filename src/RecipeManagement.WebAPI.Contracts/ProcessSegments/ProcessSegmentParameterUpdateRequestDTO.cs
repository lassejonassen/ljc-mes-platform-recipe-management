namespace RecipeManagement.WebAPI.Contracts.ProcessSegments;

public sealed record ProcessSegmentParameterUpdateRequestDTO(
    Guid ProcessSegmentId,
    Guid ParameterId,
    string Name,
    string Value,
    string? DataType,
    string? Description,
    bool IsReadOnly);