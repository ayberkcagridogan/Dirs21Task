using System.Reflection;
using System.Text.RegularExpressions;
using DataModelMapping.Extensions;
using DataModelMapping.Mapping;
using FluentResults;

namespace DataModelMapping;

public class MappingHandler
{
     private readonly Dictionary<MappingKey, IMappingStrategy> _mappingStrategies;
    public MappingHandler()
    {
        var mappingTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t =>
                !t.IsAbstract &&
                typeof(IMappingStrategy).IsAssignableFrom(t));

        var mappingStrategies = mappingTypes
            .Select(t => (IMappingStrategy)Activator.CreateInstance(t)!);

        _mappingStrategies = mappingStrategies.ToDictionary(
            s => s.Key,
            s => s);
    }

    public async Task<Result<object>> Map(object data, string sourceType, string targetType, CancellationToken cancellationToken = default)
    {
        if(data is null)
            return Result.Fail("Data not found");
        if(string.IsNullOrEmpty(sourceType))
           return Result.Fail("Source Type not found");
        if(string.IsNullOrEmpty(targetType))
            return Result.Fail("Target Type not found");


        var keyResult = createKey(sourceType, targetType);
        if(keyResult.IsFailed)
            return Result.Fail(keyResult.Errors);

       if (!_mappingStrategies.TryGetValue(keyResult.Value, out var strategy))
            return Result.Fail($"Strategy not found: {sourceType} → {sourceType}");

        return await strategy.ExecuteAsync(data,cancellationToken);

    }

    private Result<MappingKey> createKey(string sourceType, string targetType)
    {
        return EnumExtensions.FromDescription(sourceType)
            .Bind(source =>
            EnumExtensions.FromDescription(targetType)
                .Map(target => new MappingKey(source, target))
            );
    }
    
}