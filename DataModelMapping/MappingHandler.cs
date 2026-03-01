using System.Reflection;
using DataModelMapping.Mapping;
using DataModelMapping.Models.Common;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;

namespace DataModelMapping;

public class MappingHandler
{
    private readonly IServiceProvider _serviceProvider;
    public MappingHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<Result<object>> Map(object data, string sourceType, string targetType, CancellationToken cancellationToken = default)
    {
        try
        {
            Dictionary<MappingKey, IMappingStrategy> _mappingStrategies = getIMappingStrategies();

            if(data is null)
                return Result.Fail("Data not found");
            if(string.IsNullOrEmpty(sourceType))
               return Result.Fail("Source Type not found");
            if(string.IsNullOrEmpty(targetType))
                return Result.Fail("Target Type not found");

            var key = new MappingKey(sourceType, targetType);

            if (!_mappingStrategies.TryGetValue(key, out var strategy))
                return Result.Fail($"Strategy not found: {sourceType} → {targetType}");

            return await strategy.ExecuteAsync(data, cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error($"{sourceType} → {targetType} mapping not executed successfully").CausedBy(ex));
        }
    }

    private Dictionary<MappingKey, IMappingStrategy> getIMappingStrategies()
    {
        var mappingTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t =>!t.IsAbstract && typeof(IMappingStrategy).IsAssignableFrom(t));

        var mappingStrategies = mappingTypes
            .Select(t => (IMappingStrategy)ActivatorUtilities.CreateInstance(_serviceProvider, t)!);

        return mappingStrategies.ToDictionary
                (
                    s => s.Key,
                    s => s
                );
    }
}