using DataModelMapping.Models.Common;

namespace DataModelMapping.Models.Reservation;
public class GoogleReservation
{
    public int Id { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public DateTime ReservationTime { get; set; }
    public DateTime CheckIn { get; set; }
    public int StayDays { get; set; }
    public int NumberOfPerson { get; set; }
    public RoomType RoomType { get; set; }
    public int Price { get; set; }
}
