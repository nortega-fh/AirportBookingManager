using AirportBooking.Exceptions;
using AirportBooking.Models.Flights;
using AirportBooking.Validators.Flights;
using FluentAssertions;

namespace AiportBooking.Tests.Validators;

public class FlightCsvValidatorShould
{
    private readonly FlightCsvValidator _sut = new();

    [Theory]
    [InlineData("A1,120,130,140,Colombia,United States of America (USA),2024-03-15T05:12:00Z,2024-03-16T12:12:00Z,BOG,MIA")]
    [InlineData("A2,120,130,140,United States of America (USA), Colombia,2024-03-20T05:12:00Z,2024-03-21T12:12:00Z,MIA,BOG")]
    [InlineData("A3,null,1260.84,null,Colombia,France,2024-02-10T11:30:00Z,2024-02-11T03:30:00Z,BOG,CDG")]
    public void ReturnDataSeparatedByCommasWhenCorrect(string data)
    {
        _sut.Validate(data).Should().NotBeEmpty().And.BeEquivalentTo(data.Split(","));
    }

    [Fact]
    public void ThrowEntityReadingExceptionWhenDataIsIncomplete()
    {
        var incompleteData = "1,2,3";

        _sut.Invoking(s => s.Validate(incompleteData)).Should().Throw<EntityReadingException<Flight>>();
    }

    [Theory]
    [InlineData(",120,130,140,Colombia,United States of America (USA),2024-03-15T05:12:00Z,2024-03-16T12:12:00Z,BOG,MIA")]
    [InlineData("A1,120,130,140,,United States of America (USA),2024-03-15T05:12:00Z,2024-03-16T12:12:00Z,BOG,MIA")]
    [InlineData("A1,120,130,140,Colombia,,2024-03-15T05:12:00Z,2024-03-16T12:12:00Z,BOG,MIA")]
    [InlineData("A1,120,130,140,Colombia,United States of America (USA),2024-03-15T05:12:00Z,2024-03-16T12:12:00Z,,MIA")]
    [InlineData("A1,120,130,140,Colombia,United States of America (USA),2024-03-15T05:12:00Z,2024-03-16T12:12:00Z,BOG,")]
    public void ThrowInvalidAttributeExceptionWhenStringValueMissingOrInvalid(string data)
    {
        _sut.Invoking(s => s.Validate(data)).Should().Throw<InvalidAttributeException<string>>();
    }

    [Theory]
    [InlineData("A1,,130,140,Colombia,United States of America (USA),2024-03-15T05:12:00Z,2024-03-16T12:12:00Z,BOG,MIA")]
    [InlineData("A1,120,,140,Colombia,United States of America (USA),2024-03-15T05:12:00Z,2024-03-16T12:12:00Z,BOG,MIA")]
    [InlineData("A1,120,130,,Colombia,United States of America (USA),2024-03-15T05:12:00Z,2024-03-16T12:12:00Z,BOG,MIA")]
    [InlineData("A1,text,130,140,Colombia,United States of America (USA),2024-03-15T05:12:00Z,2024-03-16T12:12:00Z,BOG,MIA")]
    [InlineData("A1,120,text,140,Colombia,United States of America (USA),2024-03-15T05:12:00Z,2024-03-16T12:12:00Z,BOG,MIA")]
    [InlineData("A1,120,130,text,Colombia,United States of America (USA),2024-03-15T05:12:00Z,2024-03-16T12:12:00Z,BOG,MIA")]
    public void ThrowInvalidAttributeExceptionWhenDecimalValueMissingOrInvalid(string data)
    {
        _sut.Invoking(s => s.Validate(data)).Should().Throw<InvalidAttributeException<decimal>>();
    }

    [Theory]
    [InlineData("A2,120,130,140,United States of America (USA), Colombia,unparsable,2024-03-21T12:12:00Z,MIA,BOG")]
    [InlineData("A2,120,130,140,United States of America (USA), Colombia,2024-03-20T05:12:00Z,unparsable,MIA,BOG")]
    public void ThrowInvalidAttributeExceptionWhenDateTimeIsNotInFormat(string data)
    {
        _sut.Invoking(s => s.Validate(data)).Should().Throw<InvalidAttributeException<DateTime>>();
    }
}
