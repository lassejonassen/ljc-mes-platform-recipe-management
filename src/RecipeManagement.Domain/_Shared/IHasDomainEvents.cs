using RecipeManagement.Domain._Shared.DomainEvents;

namespace RecipeManagement.Domain._Shared;

public interface IHasDomainEvents
{
    IReadOnlyList<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}
