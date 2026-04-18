using RecipeManagement.Domain.ProcessSegments.Aggregates;

namespace RecipeManagement.Domain.ProcessSegments.Errors;

public static class ProcessSegmentErrors
{
    private const string Prefix = nameof(ProcessSegment);

    public static readonly Error LinkedToProduct
        = new($"{Prefix}.LinkedToProduct", "The process segment is linked to a product.", ErrorType.Failure);

    public static readonly Error ParameterLinkedToProduct
        = new($"{Prefix}.ParameterLinkedToProduct", "The process segment parameter is linked to a product.", ErrorType.Failure);

    public static readonly Error NotFound
        = new($"{Prefix}.NotFound", "The process segment was not found.", ErrorType.NotFound);

    public static readonly Error ParameterNotFound
        = new($"{Prefix}.ParameterNotFound", "The process segment parameter was not found.", ErrorType.NotFound);
}
