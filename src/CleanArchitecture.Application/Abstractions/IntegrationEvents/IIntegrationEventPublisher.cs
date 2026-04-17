using RecipeManagement.SharedKernel.IntegrationEvents;

namespace RecipeManagement.Application.Abstractions.IntegrationEvents;

public interface IIntegrationEventPublisher
{
    Task PublishAsync<T>(T integrationEvent, CancellationToken ct = default)
        where T : class, IIntegrationEvent;
}
