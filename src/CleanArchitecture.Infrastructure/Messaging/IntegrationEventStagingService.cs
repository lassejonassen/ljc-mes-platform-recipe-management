using RecipeManagement.Application.Abstractions;
using RecipeManagement.Application.Abstractions.IntegrationEvents;
using RecipeManagement.SharedKernel.IntegrationEvents;

namespace RecipeManagement.Infrastructure.Messaging;

public class IntegrationEventStagingService(
    IntegrationEventBuffer buffer,
    ICorrelationContext correlationContext) : IIntegrationEventPublisher
{
    public Task PublishAsync<T>(T integrationEvent, CancellationToken ct = default)
        where T : class, IIntegrationEvent
    {
        if (integrationEvent is IntegrationEvent baseEvent)
        {
            integrationEvent.CorrelationId = correlationContext.CorrelationId;
        }

        buffer.Add(integrationEvent);
        return Task.CompletedTask;
    }
}