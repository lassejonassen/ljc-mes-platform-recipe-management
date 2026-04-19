using RecipeManagement.Domain.MaterialDefinitions.Aggregates;
using RecipeManagement.Domain.ProcessSegments.Aggregates;
using RecipeManagement.Domain.ProcessSegments.Entities;
using RecipeManagement.Domain.ProductSegments.Entities;
using RecipeManagement.Domain.ProductSegments.Enums;
using RecipeManagement.Domain.ProductSegments.Errors;

namespace RecipeManagement.Domain.ProductSegments.Aggregates;

public sealed class ProductSegment : AggregateRoot
{
    private ProductSegment() { }
    private ProductSegment(DateTime utcNow) : base(utcNow) { }

    public Guid MaterialDefinitionId { get; private set; }
    public MaterialDefinition MaterialDefinition { get; } = null!;
    public Guid ProcessSegmentId { get; private set; }
    public ProcessSegment ProcessSegment { get; } = null!;
    public ProductSegmentState State { get; private set; }
    public int Version { get; private set; }
    private readonly List<ProductSegmentParameter> _parameters = [];
    public IReadOnlyCollection<ProductSegmentParameter> Parameters => _parameters.AsReadOnly();

    public static Result<ProductSegment> Create(
        Guid materialDefinitionId,
        Guid processSegmentId,
        DateTime utcNow)
    {
        var productSegment = new ProductSegment(utcNow)
        {
            MaterialDefinitionId = materialDefinitionId,
            ProcessSegmentId = processSegmentId,
            State = ProductSegmentState.Draft
        };

        return Result.Success(productSegment);
    }

    public Result<ProductSegment> CreateNewDraft(int version, DateTime utcNow)
    {
        var productSegment = new ProductSegment(utcNow)
        {
            MaterialDefinitionId = MaterialDefinitionId,
            ProcessSegmentId = ProcessSegmentId,
            Version = version,
            State = ProductSegmentState.Draft
        };

        return Result.Success(productSegment);
    }

    public Result AddParameter(ProcessSegmentParameter template, DateTime utcNow)
    {
        if (_parameters.Any(x => x.Name == template.Name))
            return Result.Failure(ProductSegmentErrors.ParameterAlreadyExists);

        var parameter = ProductSegmentParameter.CreateFromTemplate(Id, template, utcNow);

        if (parameter.IsFailure)
            return Result.Failure(parameter.Error);

        _parameters.Add(parameter.Value);

        return Result.Success();
    }

    public Result UpdateParameter(Guid parameterId, string value)
    {
        var parameter = _parameters.FirstOrDefault(x => x.Id == parameterId);
        if (parameter is null)
            return Result.Failure(ProductSegmentErrors.ParameterNotFound);

        parameter.UpdateValue(value);

        return Result.Success();
    }
}
