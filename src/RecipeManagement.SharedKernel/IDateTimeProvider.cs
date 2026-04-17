namespace RecipeManagement.SharedKernel;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}