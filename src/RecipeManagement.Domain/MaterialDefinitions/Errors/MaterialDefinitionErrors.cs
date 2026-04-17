using RecipeManagement.Domain.MaterialDefinitions.Entities;
using RecipeManagement.SharedKernel;

namespace RecipeManagement.Domain.MaterialDefinitions.Errors;

public static class MaterialDefinitionErrors
{
    private const string Prefix = nameof(MaterialDefinitionProperty);

    public static readonly Error PropertyExists
        = new($"{Prefix}.PropertyExists", "The property already exists.", ErrorType.Failure);

    public static readonly Error PropertyNotFound
        = new($"{Prefix}.PropertyNotFound", "The property was not found.", ErrorType.Failure);

    public static readonly Error SkuAlreadyExists
        = new($"{Prefix}.SkuAlreadyExists", "The SKU already exists.", ErrorType.Failure);

    public static readonly Error NotFound
        = new($"{Prefix}.NotFound", "The material definition was not found.", ErrorType.Failure);
}
