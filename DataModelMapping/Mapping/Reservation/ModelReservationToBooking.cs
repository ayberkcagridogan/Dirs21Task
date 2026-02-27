using FluentResults;

namespace DataModelMapping.Mapping.Reservation;

public class ModelReservationToBooking() : IMappingStrategy
{
    public MappingKey Key => new(MappingType.ModelReservation, MappingType.BookingReservation);

    public async Task<Result<object>> ExecuteAsync(object data, CancellationToken cancellationToken = default)
    {
       return Result.Ok<object>("Model to Booking Reservation");
    }
}