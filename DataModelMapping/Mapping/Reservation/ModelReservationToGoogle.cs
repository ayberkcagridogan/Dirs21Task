using DataModelMapping.Models.Common;
using DataModelMapping.Models.Reservation;
using DataModelMapping.Validators;
using FluentResults;

namespace DataModelMapping.Mapping.Reservation;

public class ModelReservationToGoogle : IMappingStrategy
{
    private readonly IJsonValidationPipeline<ModelReservation> _validation;
    public ModelReservationToGoogle(IJsonValidationPipeline<ModelReservation> validation)
    {
        _validation = validation;
    }
    public MappingKey Key => new("Model.Reservation", "Google.Reservation");

    public async Task<Result<object>> ExecuteAsync(object data, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validation.ValidationAsync(data.ToString()!);

        if(validationResult.IsFailed)
            return Result.Fail(validationResult.Errors.Select(e => e.Message));
        
        var modelReservation = validationResult.ValueOrDefault;

        var googleReservation = new GoogleReservation
        {
            HotelName = modelReservation.HotelName,
            ReservationTime = modelReservation.ReservationTime,
            CheckIn = modelReservation.CheckIn,
            StayDays = (modelReservation.CheckOut - modelReservation.CheckIn).Days,
            NumberOfPerson = modelReservation.NumberOfPerson,
            RoomType = modelReservation.RoomType.ToString(),
            Price = modelReservation.Price
        };

        return Result.Ok<object>(googleReservation);
    }
}