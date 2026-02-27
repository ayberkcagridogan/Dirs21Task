namespace DataModelMapping.Mapping.Reservation;

public class BookingReservationToModel : IMappingStrategy
{
    public MappingKey Key => new(MappingType.BookingReservation, MappingType.ModelReservation);

    public object Execute(object data)
    {
        return "Booking to Model Reservation";
    }
}