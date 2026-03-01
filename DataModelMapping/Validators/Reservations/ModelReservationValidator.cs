using DataModelMapping.Models.Reservation;
using FluentValidation;

namespace DataModelMapping.Validators.Reservations;

public class ModelReservationValidation: AbstractValidator<ModelReservation>
{
    public ModelReservationValidation()
    {
        RuleFor(x => x.HotelName).NotEmpty();
        RuleFor(x => x.NumberOfPerson).GreaterThan(0);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.CheckOut)
            .GreaterThan(x => x.CheckIn)
            .WithMessage("CheckOut must be after CheckIn");

        RuleFor(x => x.RoomType)
            .IsInEnum();
    }
}