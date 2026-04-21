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

    public static readonly Error ParameterAlreadyExists
        = new($"{Prefix}.ParameterAlreadyExists", "A process segment parameter with the same name already exists.", ErrorType.Failure);

    public static readonly Error NameIsAlreadyInUse
        = new($"{Prefix}.NameIsAlreadyInUse", "A process segment with the same name already exists.", ErrorType.Failure);

    public static readonly Error InvalidStateChange
    = new($"{Prefix}.InvalidStateChange", "The state change is invalid", ErrorType.Failure);

    public static readonly Error DraftFromDraftIsInvalid
        = new($"{Prefix}.DraftFromDraftIsInvalid", "You cannot create a draft from another draft", ErrorType.Failure);

    public static readonly Error NotLatestVersion
           = new($"{Prefix}.NotLatestVersion", "A new release can only be created from the latest version of the SKU", ErrorType.Failure);
}
