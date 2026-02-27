using FluentResults;

namespace DataModelMapping.Mapping.Reservation;

public class BookingReservationToModel : IMappingStrategy
{
    public MappingKey Key => new("Booking.Reservation", "Model.Reservation");

    public async Task<Result<object>> ExecuteAsync(object data, CancellationToken cancellationToken = default)
    {
        return Result.Ok<object>("Booking to Model Reservation");
    }
}