using AirportBooking.Exceptions;
using AirportBooking.Models.Bookings;
using AirportBooking.Models.Flights;
using AirportBooking.Validators.Bookings;
using FluentAssertions;

namespace AiportBooking.Tests.Validators;

public class BookingsCsvValidatorShould
{
    private readonly BookingCsvValidator _sut = new();

    [Theory]
    [InlineData("3,RoundTrip,A1,null,Business,null,passenger,Confirmed")]
    [InlineData("3,RoundTrip,A1,A2,Business,Business,passenger,Confirmed")]
    public void ProvideDataSeparatedByCommas(string data)
    {
        var expected = data.Split(",");

        _sut.Validate(data).Should().NotBeEmpty().And.BeEquivalentTo(expected);
    }

    [Fact]
    public void AllowSkippableValues()
    {
        var data = "3,RoundTrip,A1,null,Business,null,passenger,Confirmed";

        _sut.Invoking(s => s.Validate(data)).Should().NotThrow<InvalidAttributeException<string>>();
    }

    [Fact]
    public void ThrowAnEntityReadingExceptionWhenCsvDataIsIncomplete()
    {
        var incompleteData = "incomplete,data";

        _sut.Invoking(s => s.Validate(incompleteData)).Should().Throw<EntityReadingException<Booking>>();
    }

    [Fact]
    public void ThrowAInvalidAttributeExceptionWhenBookingTypeIsIncorrect()
    {
        var incorrectData = "3,IncorrectBookingType,A3,A4,Business,Business,passenger,Confirmed";

        _sut.Invoking(s => s.Validate(incorrectData)).Should().Throw<InvalidAttributeException<BookingType>>();
    }

    [Theory]
    [InlineData("3,RoundTrip,A1,,Business,Business,passenger,Confirmed")]
    [InlineData("3,RoundTrip,null,A4,Business,Business,passenger,Confirmed")]
    [InlineData("3,RoundTrip,,,Business,Business,passenger,Confirmed")]
    [InlineData("3,RoundTrip,,null,Business,Business,passenger,Confirmed")]
    [InlineData("3,RoundTrip,A1,A2,Business,Business,null,Confirmed")]
    [InlineData("3,RoundTrip,A1,A2,Business,Business,,Confirmed")]
    public void ThrowAInvalidAttributeExceptionWhenStringDataIsIncorrect(string incorrectData)
    {
        _sut.Invoking(s => s.Validate(incorrectData)).Should().Throw<InvalidAttributeException<string>>();
    }

    [Theory]
    [InlineData("3,RoundTrip,A1,A2,Business,,passenger,Confirmed")]
    [InlineData("3,RoundTrip,A1,A2,NoClass,FirstClass,passenger,Confirmed")]
    [InlineData("3,RoundTrip,A1,A2,Business,SpecialFakeClass,passenger,Confirmed")]
    [InlineData("3,RoundTrip,A1,A2,,Business,passenger,Confirmed")]
    [InlineData("3,RoundTrip,A1,A2,,,passenger,Confirmed")]
    [InlineData("3,RoundTrip,A1,null,FirstClass,,passenger,Confirmed")]
    [InlineData("3,RoundTrip,A1,null,FirstClass,NotValidClass,passenger,Confirmed")]
    public void ThrowAInvalidAttributeExceptionWhenFlightClassesAreIncorrect(string incorrectData)
    {
        _sut.Invoking(s => s.Validate(incorrectData)).Should().Throw<InvalidAttributeException<FlightClass>>();
    }

    [Fact]
    public void ThrowAInvalidAttributeExceptionWhenBookingStatusIsIncorrect()
    {
        var incorrectData = "3,RoundTrip,A1,A2,Business,FirstClass,passenger,NotValidBookingStatus";

        _sut.Invoking(s => s.Validate(incorrectData)).Should().Throw<InvalidAttributeException<BookingStatus>>();
    }
}
