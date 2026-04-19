using RecipeManagement.Domain.ProcessSegments.Entities;
using RecipeManagement.Domain.ProcessSegments.Enums;
using RecipeManagement.Domain.ProcessSegments.Errors;
using RecipeManagement.Domain.ProcessSegments.Events;
using RecipeManagement.Domain.ProductSegments.Aggregates;

namespace RecipeManagement.Domain.ProcessSegments.Aggregates;

public sealed class ProcessSegment : AggregateRoot
{
    private ProcessSegment() { }
    private ProcessSegment(DateTime utcNow) : base(utcNow) { }

    public string Name { get; private set; } = string.Empty;
    public Guid StableId { get; private set; }
    public int Version { get; private set; }
    public ProcessSegmentState State { get; private set; }

    private readonly List<ProcessSegmentParameter> _parameters = [];
    public IReadOnlyCollection<ProcessSegmentParameter> Parameters => _parameters.AsReadOnly();

    private readonly List<ProductSegment> _productSegments = [];
    public IReadOnlyCollection<ProductSegment> ProductSegments => _productSegments.AsReadOnly();

    public static Result<ProcessSegment> Create(
        string name,
        DateTime utcNow)
    {
        var processSegment = new ProcessSegment(utcNow)
        {
            Name = name,
            StableId = Guid.NewGuid(),
            Version = 1,
            State = ProcessSegmentState.Draft
        };

        return Result.Success(processSegment);
    }

    public Result<ProcessSegment> CreateNewDraft(int version, DateTime utcNow)
    {
        var processSegment = new ProcessSegment(utcNow)
        {
            Name = Name,
            StableId = StableId,
            Version = version,
            State = ProcessSegmentState.Draft
        };

        return Result.Success(processSegment);
    }

    public Result Rename(string name)
    {
        Name = name;
        return Result.Success();
    }

    public Result AddParameter(
        string name,
        string value,
        string? dataType,
        string? description,
        bool isReadOnly,
        string defaultValue,
        DateTime utcNow)
    {
        if (_parameters.Any(x => x.Name == name))
            return Result.Failure(ProcessSegmentErrors.ParameterAlreadyExists);


        var parameter = ProcessSegmentParameter.Create(
            Id,
            name,
            value,
            dataType,
            description,
            isReadOnly,
            defaultValue,
            utcNow);

        if (parameter.IsFailure)
            return Result.Failure(parameter.Error);

        _parameters.Add(parameter.Value);

        RaiseDomainEvent(new ProcessSegmentParameterAddedDomainEvent(Id, name));

        return Result.Success();
    }

    public Result UpdateParameter(Guid id,
         string name,
        string value,
        string? dataType,
        string? description,
        bool isReadOnly,
        string defaultValue)
    {
        var parameter = _parameters.FirstOrDefault(x => x.Id == id);
        if (parameter is null)
            return Result.Failure(ProcessSegmentErrors.ParameterNotFound);

        var updateResult = parameter.Update(
            name,
            value,
            dataType,
            description,
            isReadOnly,
            defaultValue
            );

        if (updateResult.IsFailure)
            return Result.Failure(updateResult.Error);

        RaiseDomainEvent(new ProcessSegmentParameterUpdatedDomainEvent(Id, parameter.Id, name));

        return Result.Success();
    }

    public Result RemoveParameter(Guid id)
    {
        var parameter = _parameters.FirstOrDefault(x => x.Id == id);
        if (parameter is null)
            return Result.Failure(ProcessSegmentErrors.ParameterNotFound);

        _parameters.Remove(parameter);

        RaiseDomainEvent(new ProcessSegmentParameterRemovedDomainEvent(Id, parameter.Name));

        return Result.Success();
    }
}
