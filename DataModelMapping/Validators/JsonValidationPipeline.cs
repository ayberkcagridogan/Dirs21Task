using DataModelMapping.Data;
using FluentResults;
using FluentValidation;

namespace DataModelMapping.Validators;

public class JsonValidationPipeline<T> : IJsonValidationPipeline<T>
{
    private readonly IValidator<T> _validator;
    private readonly IJsonProcesses<T> _jsonProcesses;

    public JsonValidationPipeline(IValidator<T> validator, IJsonProcesses<T> jsonProcesses)
    {
        _validator = validator;
        _jsonProcesses = jsonProcesses;
    }

    public async Task<Result<T>> ValidationAsync(string json, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(json))
            return Result.Fail("JSON payload is empty.");
        
        var result = await _jsonProcesses.DeserializeModelAsync(json , cancellationToken);
        if(!result.IsSuccess)
            return Result.Fail(result.Errors.Select(e => e.Message));
        
        var model = result.Value;

        var validationResult = await _validator.ValidateAsync(model,cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => new Error(e.ErrorMessage))
                .ToList();

            return Result.Fail(errors);
        }

        return Result.Ok(model);        
    }
}