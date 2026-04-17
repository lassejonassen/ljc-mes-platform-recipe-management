using RecipeManagement.Domain._Shared.DomainEvents;

namespace RecipeManagement.Domain.MaterialDefinitions.Events;

public sealed record MaterialDefinitionRenamedDomainEvent(Guid MaterialDefinitionId, string NewName) : DomainEvent;
