using RecipeManagement.SharedKernel.IntegrationEvents;

namespace RecipeManagement.Infrastructure.Messaging;

public interface IIntegrationEventBus
{
    Task SendAsync<T>(T integrationEvent, CancellationToken ct = default)
        where T : class, IIntegrationEvent;
}
