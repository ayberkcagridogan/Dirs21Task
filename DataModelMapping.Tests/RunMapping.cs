namespace DataModelMapping.Tests;

public class RunMapping
{
    [Fact]
    public void Run()
    {
        var mappingHandler = new MappingHandler();

        var result = mappingHandler.Map("" , "Booking.Reservation", "Model.Reservation");
        
        Console.WriteLine(result);
    }
}
