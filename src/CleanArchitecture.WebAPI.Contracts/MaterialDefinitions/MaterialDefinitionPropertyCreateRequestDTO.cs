namespace RecipeManagement.WebAPI.Contracts.MaterialDefinitions;

public sealed record MaterialDefinitionPropertyCreateRequestDTO(
    Guid MaterialDefinitionId,
    string Name,
    string Value,
    string? DataType,
    string? Description);