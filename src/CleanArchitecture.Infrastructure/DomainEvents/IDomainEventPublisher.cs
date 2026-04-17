using RecipeManagement.Domain._Shared.DomainEvents;

namespace RecipeManagement.Infrastructure.DomainEvents;

public interface IDomainEventPublisher
{
    Task PublishAsync(IDomainEvent domainEvent, CancellationToken ct = default);
}