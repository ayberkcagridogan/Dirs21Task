using DataModelMapping.Extensions;
using DataModelMapping.Models.Common;
using DataModelMapping.Models.Reservation;
using DataModelMapping.Validators;
using FluentResults;

namespace DataModelMapping.Mapping.Reservation;

public class BookingReservationToModel : IMappingStrategy
{
    private readonly IJsonValidationPipeline<BookingReservation> _validation;
    public BookingReservationToModel(IJsonValidationPipeline<BookingReservation> validation)
    {
        _validation = validation;
    }
    public MappingKey Key => new("Booking.Reservation", "Model.Reservation");

    public async Task<Result<object>> ExecuteAsync(object data, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validation.ValidationAsync(data.ToString()!, cancellationToken);

        if(validationResult.IsFailed)
            return Result.Fail(validationResult.Errors.Select(e => e.Message));

        var bookingReservation = validationResult.ValueOrDefault;

        var roomTypeResult = bookingReservation.RoomType.ToEnum<RoomType>();
        if(roomTypeResult.IsFailed)
            return Result.Fail(roomTypeResult.Errors.Select(x => x.Message));

        var modelReservation = new ModelReservation
        {
            Id = 0, 
            HotelName = bookingReservation.HotelName,
            ReservationTime = bookingReservation.ReservationTime,
            CheckIn = bookingReservation.CheckIn,
            CheckOut = bookingReservation.CheckIn.AddDays(bookingReservation.StayDays),
            NumberOfPerson = bookingReservation.NumberOfPerson,
            RoomType = roomTypeResult.ValueOrDefault,
            Price = bookingReservation.Price
        }; 

        return Result.Ok<object>(modelReservation);
    }
}