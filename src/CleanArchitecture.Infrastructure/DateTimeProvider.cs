using RecipeManagement.SharedKernel;

namespace RecipeManagement.Infrastructure;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}