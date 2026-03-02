using System.Collections.Concurrent;
using System.Reflection;
using DataModelMapping.Models.Common;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;

namespace DataModelMapping.Mapping;

public static class MappingRegistry
{
    private static readonly Lazy<ConcurrentDictionary<MappingKey, IMappingStrategy>> _lazyStrategies
        = new Lazy<ConcurrentDictionary<MappingKey, IMappingStrategy>>(() =>
        {
            if (_serviceProvider == null)
                throw new InvalidOperationException("MappingRegistry not initialized with IServiceProvider");

            var dict = new ConcurrentDictionary<MappingKey, IMappingStrategy>();

            var mappingTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract && typeof(IMappingStrategy).IsAssignableFrom(t));

            foreach (var type in mappingTypes)
            {
                var instance = (IMappingStrategy)ActivatorUtilities.CreateInstance(_serviceProvider, type)!;
                dict.TryAdd(instance.Key, instance);
            }

            return dict;
        }, isThreadSafe: true);
    private static IServiceProvider? _serviceProvider;

    public static void Initialize(IServiceProvider serviceProvider)
    {
        if (_serviceProvider != null) return; 
        _serviceProvider = serviceProvider;

        var _ = _lazyStrategies.Value;
    }

    public static Result<IMappingStrategy> Get(MappingKey key)
    {
        if (!_lazyStrategies.Value.TryGetValue(key, out var strategy))
                return Result.Fail($"Mapping  not found: {key.Source} → {key.Target}");
                
        return Result.Ok(strategy);
    }

      public static IReadOnlyDictionary<MappingKey, IMappingStrategy> All => _lazyStrategies.Value;
}