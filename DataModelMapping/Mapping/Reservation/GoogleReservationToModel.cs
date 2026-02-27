namespace DataModelMapping.Mapping.Reservation;

public class GoogleReservationToModel : IMappingStrategy
{
    public MappingKey Key => new(MappingType.GoogleReservation, MappingType.ModelReservation);

    public object Execute(object data)
    {
        return "Google to Model Reservation";
    }
}