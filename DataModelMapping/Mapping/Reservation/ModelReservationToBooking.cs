using FluentResults;

namespace DataModelMapping.Mapping.Reservation;

public class ModelReservationToBooking() : IMappingStrategy
{
    public MappingKey Key => new("Model.Reservation", "Booking.Reservation");

    public async Task<Result<object>> ExecuteAsync(object data, CancellationToken cancellationToken = default)
    {
       return Result.Ok<object>("Model to Booking Reservation");
    }
}