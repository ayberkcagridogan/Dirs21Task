using FluentResults;

namespace DataModelMapping.Mapping;

public interface IMappingStrategy
{
    MappingKey Key {get;}
    Task<Result<object>> ExecuteAsync (object data, CancellationToken cancellationToken = default);

}