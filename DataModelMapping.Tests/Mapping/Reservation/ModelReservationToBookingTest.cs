using DataModelMapping.Extensions;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using DataModelMapping.Models.Common;
using DataModelMapping.Models.Reservation;
using DataModelMapping.Mapping;

namespace DataModelMapping.Tests.Mapping.Reservation;

public class ModelReservationToBookingTest
{
    private readonly MappingHandler _mappingHandler;
    public ModelReservationToBookingTest()
    {
        var serviceProvider = ServiceCollectionExtensions.Create();
        MappingRegistry.Initialize(serviceProvider);
        _mappingHandler = serviceProvider.GetRequiredService<MappingHandler>();
    }

    [Fact]
    public async Task Should_Map_Successfully_When_Data_Is_Valid()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var data = @"{
                ""id"": 1,
                ""hotelName"": ""Grand Berlin Hotel"",
                ""reservationTime"": ""2026-02-28T14:30:00"",
                ""checkIn"": ""2026-03-10T15:00:00"",
                ""checkOut"": ""2026-03-15T15:00:00"",
                ""numberOfPerson"": 3,
                ""roomType"": ""Deluxe"",
                ""price"": 750
                }";
        //  Act
        var result = await _mappingHandler.Map(data , "Model.Reservation", "Booking.Reservation", cts.Token);

        // Assert  
        result.IsSuccess.Should().BeTrue();
        
        var model = result.Value;
        model.Should().BeOfType<BookingReservation>();

        if(model is BookingReservation bookingReservation)
        {
            bookingReservation.HotelName.Should().Be("Grand Berlin Hotel");
            bookingReservation.StayDays.Should().Be(5);
        }
    }

    [Fact]
    public async Task Should_Map_Error_When_Invalid_Enum()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var data = @"{
                ""id"": 1,
                ""hotelName"": ""Grand Berlin Hotel"",
                ""reservationTime"": ""2026-02-28T14:30:00"",
                ""checkIn"": ""2026-03-10T15:00:00"",
                ""checkOut"": ""2026-03-15T15:00:00"",
                ""numberOfPerson"": 3,
                ""roomType"": ""InvalidRoom"",
                ""price"": 750
                }";
        //  Act
        var result = await _mappingHandler.Map(data , "Model.Reservation", "Booking.Reservation", cts.Token);

        // Assert  
        result.IsFailed.Should().BeTrue();
        result.Errors.Select(e => e.Message).Should().Contain(m => m.Contains("Invalid JSON: The JSON value could not be converted to DataModelMapping.Models.Common.RoomType"));
    }

    [Fact]
    public async Task Should_Map_Error_When_Missing_Property()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var data = @"{
                ""id"": 1,
                ""hotelName"": null,
                ""reservationTime"": ""2026-02-28T14:30:00"",
                ""checkIn"": ""2026-03-10T15:00:00"",
                ""numberOfPerson"": 3,
                ""roomType"": ""Deluxe"",
                ""price"": 750
                }";
        //  Act
        var result = await _mappingHandler.Map(data , "Model.Reservation", "Booking.Reservation", cts.Token);

        // Assert  
        result.IsFailed.Should().BeTrue();
        result.Errors.Select(e => e.Message).Should().Contain(m => m.Contains("was missing required properties including: 'CheckOut'"));
    }

    [Fact]
    public async Task Should_Map_Error_When_Invalid_Date()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var data = @"{
                ""id"": 1,
                ""hotelName"": ""Grand Berlin Hotel"",
                ""reservationTime"": ""2026-02-28T14:30:00"",
                ""checkIn"": ""Invalid Date"",
                ""checkOut"": ""2026-03-15T15:00:00"",
                ""numberOfPerson"": 3,
                ""roomType"": ""Deluxe"",
                ""price"": 750
                }";
        //  Act
        var result = await _mappingHandler.Map(data , "Model.Reservation", "Booking.Reservation", cts.Token);

        // Assert  
        result.IsFailed.Should().BeTrue();
        result.Errors.Select(e => e.Message).Should().Contain(c => c.Contains("Invalid JSON: The JSON value could not be converted to System.DateTime."));
    }

    [Fact]
    public async Task Should_Map_Error_When_Invalid_PersonCount()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var data = @"{
                ""id"": 1,
                ""hotelName"": ""Grand Berlin Hotel"",
                ""reservationTime"": ""2026-02-28T14:30:00"",
                ""checkIn"": ""2026-03-10T14:30:00"",
                ""checkOut"": ""2026-03-15T15:00:00"",
                ""numberOfPerson"": -1,
                ""roomType"": ""Deluxe"",
                ""price"": 750
                }";
        //  Act
        var result = await _mappingHandler.Map(data , "Model.Reservation", "Booking.Reservation", cts.Token);

        // Assert  
        result.IsFailed.Should().BeTrue();
        result.Errors.Select(e => e.Message).Should().Contain(c => c.Contains("'Number Of Person' must be"));
    }
}