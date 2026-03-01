using DataModelMapping.Data;
using DataModelMapping.Models.Reservation;
using DataModelMapping.Validators;
using FluentResults;

namespace DataModelMapping.Mapping.Reservation;

public class ModelReservationToBooking : IMappingStrategy
{
    private readonly IJsonValidationPipeline<ModelReservation> _validation;
    private readonly IJsonProcesses<BookingReservation> _jsonProcesses;
    public ModelReservationToBooking(IJsonValidationPipeline<ModelReservation> validation, IJsonProcesses<BookingReservation> jsonProcesses)
    {
        _validation = validation;
        _jsonProcesses = jsonProcesses;
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

        var jsonResult = await _jsonProcesses.SerializeModelAsync(bookingReservation, cancellationToken);
        if(jsonResult.IsFailed)
            return Result.Fail(jsonResult.Errors.Select(x => x.Message));

        return Result.Ok<object>(jsonResult.ValueOrDefault);
    }
}