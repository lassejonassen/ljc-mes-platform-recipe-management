namespace RecipeManagement.WebAPI.Contracts.MaterialDefinitions;

public sealed record MaterialDefinitionPropertyUpdateRequestDTO(
    Guid MaterialDefinitionId,
    Guid PropertyId,
    string Name,
    string Value,
    string? DataType,
    string? Description);