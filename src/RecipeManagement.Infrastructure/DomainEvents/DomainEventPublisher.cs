using Microsoft.Extensions.DependencyInjection;
using RecipeManagement.Application.Abstractions.DomainEvents;
using RecipeManagement.Domain._Shared.DomainEvents;

namespace RecipeManagement.Infrastructure.DomainEvents;

public class DomainEventPublisher(IServiceProvider serviceProvider) : IDomainEventPublisher
{
    public async Task PublishAsync(IDomainEvent domainEvent, CancellationToken ct = default)
    {
        var eventType = domainEvent.GetType();
        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);
        var handlers = serviceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            if (handler is null) continue;

            // Using dynamic to resolve the specific HandleAsync(TEvent) call
            await ((dynamic)handler).HandleAsync((dynamic)domainEvent, ct);
        }
    }
}