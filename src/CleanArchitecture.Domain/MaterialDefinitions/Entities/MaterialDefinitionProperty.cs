using RecipeManagement.Domain._Shared;
using RecipeManagement.SharedKernel;

namespace RecipeManagement.Domain.MaterialDefinitions.Entities;

public sealed class MaterialDefinitionProperty : Entity
{
    private MaterialDefinitionProperty() { }
    private MaterialDefinitionProperty(DateTime utcNow) : base(utcNow) { }

    public string Name { get; private set; } = string.Empty;
    public string Value { get; private set; } = string.Empty;
    public string? DataType { get; private set; }
    public string? Description { get; private set; }
    public Guid MaterialDefinitionId { get; init; }
    public MaterialDefinition MaterialDefinition { get; } = null!;

    public static Result<MaterialDefinitionProperty> Create(
        string name,
        string value,
        string? dataType,
        string? description,
        Guid materialDefinitionId,
        DateTime utcNow)
    {
        var materialDefinitionProperty = new MaterialDefinitionProperty(utcNow)
        {
            Name = name,
            Value = value,
            DataType = dataType,
            Description = description,
            MaterialDefinitionId = materialDefinitionId
        };

        return Result.Success(materialDefinitionProperty);
    }

    public Result UpdateValue(string value, string? dataType, string? description)
    {
        Value = value;
        DataType = dataType;
        Description = description;
        return Result.Success();
    }
}
