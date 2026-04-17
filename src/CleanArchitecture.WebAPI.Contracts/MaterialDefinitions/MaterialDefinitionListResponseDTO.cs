namespace RecipeManagement.WebAPI.Contracts.MaterialDefinitions;

public sealed record MaterialDefinitionListResponseDTO
{
    public required IEnumerable<MaterialDefinitionResponseDTO> Data { get; init; }
}
