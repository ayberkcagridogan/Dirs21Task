using DataModelMapping.Mapping;
using DataModelMapping.Models.Common;
using FluentResults;

namespace DataModelMapping;

public class MappingHandler
{

    public async Task<Result<object>> Map(object data, string sourceType, string targetType, CancellationToken cancellationToken = default)
    {
        try
        {
            if(data is null)
                return Result.Fail("Data not found");
            if(string.IsNullOrEmpty(sourceType))
               return Result.Fail("Source Type not found");
            if(string.IsNullOrEmpty(targetType))
                return Result.Fail("Target Type not found");

            var key = new MappingKey(sourceType, targetType);

            var mappingResult = MappingRegistry.Get(key);
            if(mappingResult.IsFailed)
                return Result.Fail(mappingResult.Errors.Select(e => e.Message));

            return await mappingResult.Value.ExecuteAsync(data, cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error($"{sourceType} → {targetType} mapping not executed successfully").CausedBy(ex));
        }
    }
}