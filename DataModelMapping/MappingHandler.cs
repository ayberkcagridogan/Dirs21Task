using System.Reflection;
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


        var key = new MappingKey(sourceType, targetType);

       if (!_mappingStrategies.TryGetValue(key, out var strategy))
            return Result.Fail($"Strategy not found: {sourceType} → {targetType}");

        return await strategy.ExecuteAsync(data,cancellationToken);

    }    
}