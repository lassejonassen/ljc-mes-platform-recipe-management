using RecipeManagement.Domain._Shared.DomainEvents;

namespace RecipeManagement.Domain.MaterialDefinitions.Events;

public sealed record MaterialDefinitionPropertyRemovedDomainEvent(
    Guid MaterialDefinitionId,
    string PropertyName) : DomainEvent;
