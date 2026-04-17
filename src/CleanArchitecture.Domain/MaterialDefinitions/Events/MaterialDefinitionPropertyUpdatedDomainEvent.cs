using RecipeManagement.Domain._Shared.DomainEvents;

namespace RecipeManagement.Domain.MaterialDefinitions.Events;

public sealed record MaterialDefinitionPropertyUpdatedDomainEvent(
    Guid MaterialDefinitionPropertyId, string Value, string? DataType, string? Description) : DomainEvent;
