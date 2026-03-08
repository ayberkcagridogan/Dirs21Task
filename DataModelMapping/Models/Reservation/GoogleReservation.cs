namespace DataModelMapping.Models.Reservation;
public class GoogleReservation
{
    public int Id { get; set; }
    public required string HotelName { get; set; }
    public required DateTime ReservationTime { get; set; }
    public required DateTime CheckIn { get; set; }
    public required int StayDays { get; set; }
    public required int NumberOfPerson { get; set; }
    public required string RoomType { get; set; }
    public required int Price { get; set; }
}
