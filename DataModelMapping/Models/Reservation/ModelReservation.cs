using DataModelMapping.Models.Common;

namespace DataModelMapping.Models.Reservation;
public class ModelReservation
{
    public int Id { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public DateTime ReservationTime { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int NumberOfPerson { get; set; }
    public RoomType RoomType { get; set; }
    public int Price { get; set; }
}
