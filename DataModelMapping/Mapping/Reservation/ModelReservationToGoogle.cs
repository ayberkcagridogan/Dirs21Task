using DataModelMapping.Models.Reservation;

namespace DataModelMapping.Mapping.Reservation;

public class ModelReservationToGoogle : IMappingStrategy
{
    public MappingKey Key => new(MappingType.ModelReservation, MappingType.GoogleReservation);

    public object Execute(object data)
    {
        return "Model to Google Reservation";
    }
}