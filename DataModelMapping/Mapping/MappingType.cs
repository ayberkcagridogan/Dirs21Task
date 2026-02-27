using System.ComponentModel;

namespace DataModelMapping.Mapping;
public enum MappingType
{
    [Description("Model.Reservation")]
    ModelReservation,
    [Description("Google.Reservation")]
    GoogleReservation,
    [Description("Booking.Reservation")]
    BookingReservation,
}