using DataModelMapping.Models.Reservation;
using FluentValidation;

namespace DataModelMapping.Validators.Reservations;

public class BookingReservationValidation: AbstractValidator<BookingReservation>
{
    public BookingReservationValidation()
    {
        RuleFor(x => x.HotelName).NotEmpty();
        RuleFor(x => x.NumberOfPerson).GreaterThan(0);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.StayDays).GreaterThan(0);
    }
}