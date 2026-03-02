using DataModelMapping.Data;
using DataModelMapping.Mapping;
using DataModelMapping.Validators;
using DataModelMapping.Validators.Reservations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DataModelMapping.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMappingServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IJsonProcesses<>), typeof(JsonProcesses<>));
        services.AddScoped(typeof(IJsonValidationPipeline<>),typeof(JsonValidationPipeline<>));
        services.AddValidatorsFromAssemblyContaining<ModelReservationValidation>();
        services.AddScoped<MappingHandler>();

        return services;
    }

    public static IServiceProvider Create()
    {
        var services = new ServiceCollection();
        services.AddMappingServices();
        return services.BuildServiceProvider();
    }
}