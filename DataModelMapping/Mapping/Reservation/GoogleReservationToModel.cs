using DataModelMapping.Models.Common;
using DataModelMapping.Models.Reservation;
using DataModelMapping.Validators;
using FluentResults;
using DataModelMapping.Extensions;

namespace DataModelMapping.Mapping.Reservation;

public class GoogleReservationToModel : IMappingStrategy
{
    private readonly IJsonValidationPipeline<GoogleReservation> _validation;
    public GoogleReservationToModel(IJsonValidationPipeline<GoogleReservation> validation)
    {
        _validation = validation;
    }
    public MappingKey Key => new("Google.Reservation", "Model.Reservation");

    public async Task<Result<object>> ExecuteAsync(object data, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validation.ValidationAsync(data.ToString()!, cancellationToken);

        if(validationResult.IsFailed)
            return Result.Fail(validationResult.Errors.Select(x => x.Message));

        var googleReservation = validationResult.ValueOrDefault;

        var roomTypeResult = googleReservation.RoomType.ToEnum<RoomType>();
        if(roomTypeResult.IsFailed)
            return Result.Fail(roomTypeResult.Errors.Select(x => x.Message));

        var modelReservation = new ModelReservation
        {
            Id = 0, 
            HotelName = googleReservation.HotelName,
            ReservationTime = googleReservation.ReservationTime,
            CheckIn = googleReservation.CheckIn,
            CheckOut = googleReservation.CheckIn.AddDays(googleReservation.StayDays),
            NumberOfPerson = googleReservation.NumberOfPerson,
            RoomType = roomTypeResult.ValueOrDefault,
            Price = googleReservation.Price
        }; 

        return Result.Ok<object>(modelReservation);
    }
}