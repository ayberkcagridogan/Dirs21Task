using FluentResults;

namespace DataModelMapping.Mapping.Reservation;

public class GoogleReservationToModel : IMappingStrategy
{
    public MappingKey Key => new(MappingType.GoogleReservation, MappingType.ModelReservation);

    public async Task<Result<object>> ExecuteAsync(object data, CancellationToken cancellationToken = default)
    {
       return Result.Ok<object>("Google to Model Reservation");
    }
}