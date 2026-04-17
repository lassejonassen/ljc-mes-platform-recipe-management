using Microsoft.Extensions.DependencyInjection;
using RecipeManagement.SharedKernel.Messaging;

namespace RecipeManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatorHandlers(typeof(DependencyInjection).Assembly);

        return services;
    }
}