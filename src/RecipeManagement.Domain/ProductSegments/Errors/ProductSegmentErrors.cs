using RecipeManagement.Domain.ProductSegments.Aggregates;

namespace RecipeManagement.Domain.ProductSegments.Errors;

public static class ProductSegmentErrors
{
    private const string Prefix = nameof(ProductSegment);

    public static readonly Error ParameterIsReadOnly
        = new($"{Prefix}.ParameterIsReadOnly", "Parameter is read-only and cannot be changed.", ErrorType.Failure);

    public static readonly Error ParameterAlreadyExists
        = new($"{Prefix}.ParameterAlreadyExists", "The parameter already exists", ErrorType.Failure);

    public static readonly Error NotFound
        = new($"{Prefix}.NotFound", "Product Segment not found", ErrorType.NotFound);

    public static readonly Error ParameterNotFound
        = new($"{Prefix}.ParameterNotFound", "Product Segment Parameter not found", ErrorType.NotFound);

    public static readonly Error InvalidStateChange
    = new($"{Prefix}.InvalidStateChange", "The state change is invalid", ErrorType.Failure);

    public static readonly Error DraftFromDraftIsInvalid
        = new($"{Prefix}.DraftFromDraftIsInvalid", "You cannot create a draft from another draft", ErrorType.Failure);

    public static readonly Error ProcessSegmentNotReleased
                = new($"{Prefix}.ProcessSegmentNotReleased", "You cannot create a draft from Process Segment which is not released", ErrorType.Failure);

    public static readonly Error MaterialDefinitionNotReleased
                = new($"{Prefix}.ProcessSegmentNotReleased", "You cannot create a draft from Material Definition which is not released", ErrorType.Failure);

}
