using DataModelMapping.Models.Common;

namespace DataModelMapping.Models.Reservation;
public class ModelReservation
{
    public required int Id { get; set; }
    public required string HotelName { get; set; }
    public required DateTime ReservationTime { get; set; }
    public required DateTime CheckIn { get; set; }
    public required DateTime CheckOut { get; set; }
    public required int NumberOfPerson { get; set; }
    public required RoomType RoomType { get; set; }
    public required int Price { get; set; }
}
