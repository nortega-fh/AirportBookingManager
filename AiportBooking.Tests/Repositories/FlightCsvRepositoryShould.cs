using AiportBooking.Tests.Repositories.Attributes;
using AirportBooking.FileReaders;
using AirportBooking.Models.Flights;
using AirportBooking.Repositories.Flights;
using AirportBooking.Serializers.Csv.Flights;
using AutoFixture;
using FluentAssertions;
using Moq;

namespace AiportBooking.Tests.Repositories;

public class FlightCsvRepositoryShould : IDisposable
{
    private readonly Mock<IFlightCsvSerializer> _serializer;
    private readonly Mock<ICsvFileReader> _reader;
    private readonly FlightCsvRepository _sut;

    public FlightCsvRepositoryShould()
    {
        _reader = new Mock<ICsvFileReader>();
        _serializer = new Mock<IFlightCsvSerializer>();
        _sut = new(_reader.Object, _serializer.Object);
    }

    public void Dispose()
    {
        _reader.Reset();
        _serializer.Reset();
    }

    [Theory]
    [FlightListData]
    public void FindAllFlightsFromCsvFile(string[] data, Flight flight1, Flight flight2)
    {
        _reader.Setup(r => r.Read(It.IsAny<string>())).Returns(data);
        _serializer.SetupSequence(s => s.FromCsv(It.IsAny<string>()))
            .Returns(flight1)
            .Returns(flight2);

        var flightList = _sut.FindAll();

        _reader.Verify(r => r.Read(It.IsAny<string>()), Times.Once());
        _serializer.Verify(s => s.FromCsv(It.IsAny<string>()), Times.Exactly(2));
        flightList
            .Should()
            .NotBeNull()
            .And
            .AllBeOfType<Flight>()
            .And
            .Contain(flight1)
            .And
            .Contain(flight2);
    }

    [Fact]
    public void AddANewFileForLoadingFlights()
    {
        var fixture = new Fixture();
        var fileName = fixture.Create<string>();
        _reader.Setup(r => r.Read(fileName)).Returns(["some,flight,data,to,read"]);

        _sut.AddFileToLoad(fileName);
        _sut.FindAll();

        _reader.Verify(r => r.Read(fileName), Times.Exactly(2));
        _serializer.Verify(s => s.FromCsv(It.IsAny<string>()), Times.AtLeastOnce());
    }

    [Theory]
    [FlightListData]
    public void FindFlightByFlightNumberFromCsvFile(string[] data, Flight flight1, Flight flight2)
    {
        _reader.Setup(r => r.Read(It.IsAny<string>())).Returns(data);
        _serializer.SetupSequence(s => s.FromCsv(It.IsAny<string>()))
            .Returns(flight1)
            .Returns(flight2);

        var obtainedFlight = _sut.Find(flight1.Number);

        obtainedFlight.Should().BeEquivalentTo(flight1);
    }

    [Theory]
    [FlightListFilterData]
    public void FilterListOfFlights(string[] data, Flight flight1, Flight flight2, Predicate<Flight> filter)
    {
        _reader.Setup(r => r.Read(It.IsAny<string>())).Returns(data);
        _serializer.SetupSequence(s => s.FromCsv(It.IsAny<string>()))
            .Returns(flight1)
            .Returns(flight2);

        var obtainedList = _sut.Filter(filter);

        obtainedList.All(filter.Invoke).Should().BeTrue();
    }
}