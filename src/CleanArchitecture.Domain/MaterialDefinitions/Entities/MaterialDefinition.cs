using RecipeManagement.Domain._Shared;
using RecipeManagement.Domain.MaterialDefinitions.Errors;
using RecipeManagement.Domain.MaterialDefinitions.Events;
using RecipeManagement.SharedKernel;

namespace RecipeManagement.Domain.MaterialDefinitions.Entities;

public sealed class MaterialDefinition : AggregateRoot
{
    private MaterialDefinition() { }
    private MaterialDefinition(DateTime utcNow) : base(utcNow) { }

    public string Sku { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;

    private readonly List<MaterialDefinitionProperty> _properties = [];
    public IReadOnlyCollection<MaterialDefinitionProperty> Properties => _properties.AsReadOnly();

    public static Result<MaterialDefinition> Create(
        string sku,
        string name,
        DateTime utcNow)
    {
        var materialDefinition = new MaterialDefinition(utcNow)
        {
            Sku = sku,
            Name = name
        };

        return Result.Success(materialDefinition);
    }

    public Result Rename(string name)
    {
        Name = name;

        RaiseDomainEvent(new MaterialDefinitionRenamedDomainEvent(Id, Name));

        return Result.Success();
    }

    public Result<Guid> AddProperty(string name, string value, string? dataType, string? description, DateTime utcNow)
    {
        if (_properties.Any(x => x.Name == name))
            return Result.Failure<Guid>(MaterialDefinitionErrors.PropertyExists);

        var property = MaterialDefinitionProperty.Create(name, value, dataType, description, Id, utcNow);
        if (property.IsFailure)
            return Result.Failure<Guid>(property.Error);

        _properties.Add(property.Value);

        RaiseDomainEvent(new MaterialDefinitionPropertyAddedDomainEvent(Id, name));

        return Result.Success(property.Value.Id);
    }

    public Result UpdateProperty(string name, string value, string? dataType, string? description)
    {
        var property = _properties.FirstOrDefault(x => x.Name == name);
        if (property is null)
            return Result.Failure(MaterialDefinitionErrors.PropertyNotFound);

        var updateResult = property.UpdateValue(value, dataType, description);
        if (updateResult.IsFailure)
            return Result.Failure(updateResult.Error);

        RaiseDomainEvent(new MaterialDefinitionPropertyUpdatedDomainEvent(Id, value, dataType, description));

        return Result.Success();
    }

    public Result RemoveProperty(Guid id)
    {
        var property = _properties.FirstOrDefault(x => x.Id == id);

        if (property is null)
            return Result.Failure(MaterialDefinitionErrors.PropertyNotFound);

        _properties.Remove(property);

        RaiseDomainEvent(new MaterialDefinitionPropertyRemovedDomainEvent(Id, property.Name));

        return Result.Success();
    }
}
