using RecipeManagement.Domain.ProcessSegments.Entities;
using RecipeManagement.Domain.ProductSegments.Aggregates;

namespace RecipeManagement.Domain.ProductSegments.Entities;

public class ProductSegmentParameter : ParameterDefinition
{
    private ProductSegmentParameter()  { }
    private ProductSegmentParameter(Guid productSegmentId, string name, string value, string? dataType, string? description, Guid sourceId, DateTime utcNow)
        : base(name, value, dataType, description, utcNow)   
    {
        ProductSegmentId = productSegmentId;
        Value = value;
        ProcessSegmentParameterId = sourceId;
    }

    public Guid ProcessSegmentParameterId { get; private set; }
    public ProcessSegmentParameter ProcessSegmentParameter { get; } = null!;
    public Guid ProductSegmentId { get; private set; }
    public ProductSegment ProductSegment { get; } = null!;

    public static Result<ProductSegmentParameter> CreateFromTemplate(Guid productSegmentId, ProcessSegmentParameter template, DateTime utcNow)
    {
        var parameter = new ProductSegmentParameter(
            productSegmentId,
            template.Name,
            template.Value,
            template.DataType,
            template.Description,
            template.Id,
            utcNow);

        return Result.Success(parameter);
    }

    public Result UpdateValue(string value)
    {
        Value = value;

        return Result.Success();
    }
}