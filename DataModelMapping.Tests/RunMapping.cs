using DataModelMapping.Extensions;
using DataModelMapping.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace DataModelMapping.Tests;

public class RunMapping
{
    private readonly MappingHandler _mappingHandler;
    public RunMapping()
    {
        var serviceProvider = ServiceCollectionExtensions.Create();
         MappingRegistry.Initialize(serviceProvider);
        _mappingHandler = serviceProvider.GetRequiredService<MappingHandler>();
    }
    [Fact]
    public async Task Run()
    {
        var cts = new CancellationTokenSource();
        var data = @"{
                ""id"": 1,
                ""hotelName"": ""Grand Berlin Hotel"",
                ""reservationTime"": ""2026-02-28T14:30:00"",
                ""checkIn"": ""2026-03-10T15:00:00"",
                ""stayDays"": 5,
                ""numberOfPerson"": 3,
                ""roomType"": ""Deluxe"",
                ""price"": 750
                }";

        var result = await _mappingHandler.Map(data , "Model.Reservation", "Google.Reservation", cts.Token);

        if(result.IsFailed)
             Console.WriteLine(result);
        else
            Console.WriteLine(result.ValueOrDefault);
    }
}
