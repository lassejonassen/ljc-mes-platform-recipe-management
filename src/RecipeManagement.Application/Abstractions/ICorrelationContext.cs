namespace RecipeManagement.Application.Abstractions;

public interface ICorrelationContext
{
    Guid CorrelationId { get; }
}