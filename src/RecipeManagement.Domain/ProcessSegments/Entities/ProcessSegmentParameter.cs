using RecipeManagement.Domain.ProcessSegments.Aggregates;
using RecipeManagement.Domain.ProductSegments.Entities;

namespace RecipeManagement.Domain.ProcessSegments.Entities;

public sealed class ProcessSegmentParameter : ParameterDefinition
{
    private ProcessSegmentParameter() { }
    private ProcessSegmentParameter(Guid processSegmentId, string name, string value, string? dataType, string? description, bool isReadOnly, DateTime utcNow)
        : base(name, value, dataType, description, utcNow)
    {
        ProcessSegmentId = processSegmentId;
        IsReadOnly = isReadOnly;
    }
    public bool IsReadOnly { get; private set; }
    public Guid ProcessSegmentId { get; private set; }
    public ProcessSegment ProcessSegment { get; } = null!;
    private readonly List<ProductSegmentParameter> _productSegmentParameters = [];
    public IReadOnlyCollection<ProductSegmentParameter> ProductSegmentParameters => _productSegmentParameters.AsReadOnly();

    public static Result<ProcessSegmentParameter> Create(Guid processSegmentId, string name, string value, string? dataType, string? description, bool isReadOnly, DateTime utcNow)
    {
        var entity = new ProcessSegmentParameter(
            processSegmentId,
            name,
            value, 
            dataType,
            description,
            isReadOnly,
            utcNow);

        return Result.Success(entity);
    }

    public Result Update(string name, string value, string? dataType, string? description, bool isReadOnly)
    {
        Name = name;
        Value = value;
        DataType = dataType;
        Description = description;
        IsReadOnly = isReadOnly;

        return Result.Success();
    }
}
