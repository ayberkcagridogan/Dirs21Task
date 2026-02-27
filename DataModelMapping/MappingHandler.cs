using System.Reflection;
using System.Text.RegularExpressions;
using DataModelMapping.Extensions;
using DataModelMapping.Mapping;

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

    public object Map(object data, string sourceType, string targetType)
    {
        if(data is null)
            throw new ArgumentNullException("Data Null");
        if(string.IsNullOrEmpty(sourceType))
            throw new ArgumentNullException("Data Null");
        if(string.IsNullOrEmpty(targetType))
            throw new ArgumentNullException("Data Null");

        var key = new MappingKey(EnumExtensions.FromDescription(sourceType), EnumExtensions.FromDescription(targetType));

       if (!_mappingStrategies.TryGetValue(key, out var strategy))
            throw new InvalidOperationException(
                $"Strategy not found: {sourceType} → {targetType}");

        return strategy.Execute(data);

    }
}