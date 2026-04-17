using RecipeManagement.SharedKernel.IntegrationEvents;

namespace RecipeManagement.Contracts.Templates;

public record TemplateCreatedIntegrationEvent : IntegrationEvent
{
    public required Guid TemplateId { get; init; }
    public required string TemplateName { get; init; }
}