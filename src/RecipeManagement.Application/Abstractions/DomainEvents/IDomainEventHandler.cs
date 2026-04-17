using RecipeManagement.Domain._Shared.DomainEvents;

namespace RecipeManagement.Application.Abstractions.DomainEvents;

public interface IDomainEventHandler<in TEvent>
    where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent, CancellationToken ct = default);
}
