namespace DataModelMapping.Mapping.Reservation;

public class ModelReservationToBooking() : IMappingStrategy
{
    public MappingKey Key => new(MappingType.ModelReservation, MappingType.BookingReservation);

    public object Execute(object data)
    {
        return "Model to Booking Reservation";
    }
}