using FluentResults;

namespace DataModelMapping.Mapping.Reservation;

public class ModelReservationToGoogle : IMappingStrategy
{
    public MappingKey Key => new("Model.Reservation", "Google.Reservation");

    public async Task<Result<object>> ExecuteAsync(object data, CancellationToken cancellationToken = default)
    {
        return Result.Ok<object>("Model to Google Reservation");
    }
}