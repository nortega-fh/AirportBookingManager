using AiportBooking.Tests.Serializers.Attributes;
using AirportBooking.Serializers.Csv.Flights;
using AirportBooking.Validators.Flights;
using FluentAssertions;
using Moq;

namespace AiportBooking.Tests.Serializers;

public class FlightCsvSerializerShould
{
    [Fact]
    public void ValidateEntityBeforeSerializing()
    {
        var data = "A1,120,130,140,Colombia,United States of America (USA),2024-03-15T05:12:00Z,2024-03-16T12:12:00Z,BOG,MIA";
        var validator = new Mock<IFlightCsvValidator>();
        validator.Setup(v => v.Validate(data)).Returns(data.Split(','));
        var sut = new FlightCsvSerializer(validator.Object);

        sut.FromCsv(data);
        validator.Verify(v => v.Validate(data), Times.Once);
    }

    [Theory]
    [FlightDeserializeValues]
    public void DeserializeCsvIntoEntity(string price1, string price2, string price3, decimal[] expectedPrices)
    {
        var data = $"A1,{price1},{price2},{price3},Colombia,United States of America (USA),2024-03-15T05:12:00Z,2024-03-16T12:12:00Z,BOG,MIA";
        var validator = new Mock<IFlightCsvValidator>();
        validator.Setup(v => v.Validate(data)).Returns(data.Split(','));
        var sut = new FlightCsvSerializer(validator.Object);

        var obtainedFlight = sut.FromCsv(data);

        obtainedFlight.Number.Should().Be("A1");
        obtainedFlight.OriginCountry.Should().Be("Colombia");
        obtainedFlight.DestinationCountry.Should().Be("United States of America (USA)");
        obtainedFlight.DepartureDate.Should().Be(DateTime.Parse("2024-03-15T05:12:00Z"));
        obtainedFlight.ArrivalDate.Should().Be(DateTime.Parse("2024-03-16T12:12:00Z"));
        obtainedFlight.OriginAirport.Should().Be("BOG");
        obtainedFlight.DestinationAirport.Should().Be("MIA");
        obtainedFlight.ClassPrices.Values.Should().BeEquivalentTo(expectedPrices);
    }
}
