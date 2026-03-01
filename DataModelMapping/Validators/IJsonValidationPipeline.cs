using FluentResults;

namespace DataModelMapping.Validators;

public interface IJsonValidationPipeline<T>
{
    Task<Result<T>> ValidationAsync(string json, CancellationToken cancellationToken = default);
}