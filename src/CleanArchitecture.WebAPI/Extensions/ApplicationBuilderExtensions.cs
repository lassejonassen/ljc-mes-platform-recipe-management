using Microsoft.EntityFrameworkCore;
using RecipeManagement.Infrastructure.Persistence.DbContexts;

namespace RecipeManagement.WebAPI.Extensions;

public static partial class ApplicationBuilderExtensions
{
    public static IApplicationBuilder MigrateDatabases(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>()
            .Database.Migrate();

        return app;
    }
}
