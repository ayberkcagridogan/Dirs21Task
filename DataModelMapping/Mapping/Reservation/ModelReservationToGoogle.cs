using System.Runtime.CompilerServices;
using System.Text.Json;
using DataModelMapping.Data;
using DataModelMapping.Models.Reservation;
using DataModelMapping.Validators;
using FluentResults;

namespace DataModelMapping.Mapping.Reservation;

public class ModelReservationToGoogle : IMappingStrategy
{
    private readonly IJsonValidationPipeline<ModelReservation> _validation;
    private readonly IJsonProcesses<GoogleReservation> _jsonProcesses;
    public ModelReservationToGoogle(IJsonValidationPipeline<ModelReservation> validation, IJsonProcesses<GoogleReservation> jsonProcesses)
    {
        _validation = validation;
        _jsonProcesses = jsonProcesses;
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

        var jsonResult = await _jsonProcesses.SerializeModelAsync(googleReservation);

        if(jsonResult.IsFailed)
            return Result.Fail(jsonResult.Errors.Select(e => e.Message));

        return Result.Ok<object>(jsonResult.ValueOrDefault);
    }
}