using DataModelMapping.Data;
using DataModelMapping.Models.Common;
using DataModelMapping.Models.Reservation;
using DataModelMapping.Validators;
using FluentResults;

namespace DataModelMapping.Mapping.Reservation;

public class ModelReservationToBooking : IMappingStrategy
{
    private readonly IJsonValidationPipeline<ModelReservation> _validation;
    public ModelReservationToBooking(IJsonValidationPipeline<ModelReservation> validation)
    {
        _validation = validation;
    }

    public MappingKey Key => new("Model.Reservation", "Booking.Reservation");

    public async Task<Result<object>> ExecuteAsync(object data, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validation.ValidationAsync(data.ToString()!, cancellationToken);

        if(validationResult.IsFailed)
            return Result.Fail(validationResult.Errors.Select(e => e.Message));

        var modelReservation = validationResult.ValueOrDefault;

        var bookingReservation = new BookingReservation
        {
            HotelName = modelReservation.HotelName,
            ReservationTime = modelReservation.ReservationTime,
            CheckIn = modelReservation.CheckIn,
            StayDays = (modelReservation.CheckOut - modelReservation.CheckIn).Days,
            NumberOfPerson = modelReservation.NumberOfPerson,
            RoomType = modelReservation.RoomType.ToString(),
            Price = modelReservation.Price
        };

        return Result.Ok<object>(bookingReservation);
    }
}