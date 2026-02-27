using FluentResults;

namespace DataModelMapping.Mapping.Reservation;

public class BookingReservationToModel : IMappingStrategy
{
    public MappingKey Key => new(MappingType.BookingReservation, MappingType.ModelReservation);

    public async Task<Result<object>> ExecuteAsync(object data, CancellationToken cancellationToken = default)
    {
        return Result.Ok<object>("Booking to Model Reservation");
    }
}