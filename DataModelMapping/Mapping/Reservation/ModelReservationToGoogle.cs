using FluentResults;

namespace DataModelMapping.Mapping.Reservation;

public class ModelReservationToGoogle : IMappingStrategy
{
    public MappingKey Key => new(MappingType.ModelReservation, MappingType.GoogleReservation);

    public async Task<Result<object>> ExecuteAsync(object data, CancellationToken cancellationToken = default)
    {
        return Result.Ok<object>("Model to Google Reservation");
    }
}