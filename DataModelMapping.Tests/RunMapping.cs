namespace DataModelMapping.Tests;

public class RunMapping
{
    [Fact]
    public async Task Run()
    {
        var mappingHandler = new MappingHandler();
        var cts = new CancellationTokenSource();
        
        var result = await mappingHandler.Map("" , "Google.Reservati", "Model.Reservation", cts.Token);
        
        Console.WriteLine(result);
    }
}
