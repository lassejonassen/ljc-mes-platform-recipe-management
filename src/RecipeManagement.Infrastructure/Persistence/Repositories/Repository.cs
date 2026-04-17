using Microsoft.EntityFrameworkCore;
using RecipeManagement.Domain._Shared;

namespace RecipeManagement.Infrastructure.Persistence.Repositories;

internal abstract class Repository<TEntity>(DbContext context) : IRepository<TEntity>
    where TEntity : AggregateRoot
{
    protected DbContext DbContext => context;

    public TEntity Add(TEntity entity)
    {
        return context.Set<TEntity>().Add(entity).Entity;
    }

    public void Delete(TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
    }
}
