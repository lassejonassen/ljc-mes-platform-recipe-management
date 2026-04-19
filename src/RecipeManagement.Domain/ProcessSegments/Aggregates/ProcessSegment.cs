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

    public Result<ProcessSegment> CreateNewDraft(DateTime utcNow)
    {
        // 1. Instantiate the new version of the Aggregate Root
        var newDraft = new ProcessSegment(utcNow)
        {
            Name = Name,
            StableId = StableId,
            Version = Version + 1,
            State = ProcessSegmentState.Draft
        };

        // 2. Iterate through existing parameters and create new instances for the draft
        foreach (var param in _parameters)
        {
            var parameterResult = ProcessSegmentParameter.Create(
                newDraft.Id, // Point to the NEW ProcessSegment Id
                param.Name,
                param.Value,
                param.DataType,
                param.Description,
                param.IsReadOnly,
                utcNow);

            if (parameterResult.IsFailure)
            {
                return Result.Failure<ProcessSegment>(parameterResult.Error);
            }

            newDraft._parameters.Add(parameterResult.Value);
        }

        return Result.Success(newDraft);
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
        bool isReadOnly)
    {
        var parameter = _parameters.FirstOrDefault(x => x.Id == id);
        if (parameter is null)
            return Result.Failure(ProcessSegmentErrors.ParameterNotFound);

        var updateResult = parameter.Update(
            name,
            value,
            dataType,
            description,
            isReadOnly
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
