using FluentResults;

namespace DataModelMapping.Data;

public interface IJsonProcesses<T>
{
     Task<Result<T>> DeserializeModelAsync(string json, CancellationToken cancellationToken = default);
     Task<Result<string>> SerializeModelAsync(T model, CancellationToken cancellationToken = default);
}