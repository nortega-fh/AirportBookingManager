using AirportBooking.FileReaders;
using AirportBooking.Models.Bookings;
using AirportBooking.Models.Flights;
using AirportBooking.Models.Users;
using AirportBooking.Repositories.Bookings;
using AirportBooking.Repositories.Flights;
using AirportBooking.Repositories.Users;
using AirportBooking.Serializers.Csv.Bookings;
using AutoFixture;
using FluentAssertions;
using Moq;

namespace AiportBooking.Tests.Repositories;

public class BookingCsvRepositoryShould : IDisposable
{
    private readonly static Fixture Fixture = new Fixture();
    private readonly BookingCsvRepository _sut;
    private readonly Mock<ICsvFileReader> _reader;
    private readonly Mock<IBookingCsvSerializer> _serializer;
    private readonly Mock<IUserCsvRepository> _userRepository;
    private readonly Mock<IFlightCsvRepository> _flightRepository;

    public BookingCsvRepositoryShould()
    {
        _reader = new Mock<ICsvFileReader>();
        _serializer = new Mock<IBookingCsvSerializer>();
        _userRepository = new Mock<IUserCsvRepository>();
        _flightRepository = new Mock<IFlightCsvRepository>();
        _sut = new(
            _reader.Object,
            _serializer.Object,
            _userRepository.Object,
            _flightRepository.Object
            );
    }

    public void Dispose()
    {
        _reader.Reset();
        _serializer.Reset();
        _userRepository.Reset();
        _flightRepository.Reset();
    }

    [Fact]
    public void ObtainAListOfBookingsFromACsvFile()
    {
        _reader.Setup(r => r.Read(It.IsAny<string>())).Returns(Fixture.Create<string[]>().Take(1).ToArray());
        _userRepository.Setup(u => u.Find(It.IsAny<string>())).Returns(Fixture.Create<User>());
        _flightRepository.Setup(f => f.Find(It.IsAny<string>())).Returns(Fixture.Create<Flight>());
        _serializer.Setup(s => s.FromCsv(It.IsAny<string>())).Returns(Fixture.Create<Booking>());

        var obtainedBookings = _sut.FindAll();

        _reader.Verify(r => r.Read(It.IsAny<string>()), Times.Once());
        _userRepository.Verify(u => u.Find(It.IsAny<string>()), Times.Once());
        _flightRepository.Verify(f => f.Find(It.IsAny<string>()), Times.AtLeastOnce());
        _serializer.Verify(s => s.FromCsv(It.IsAny<string>()), Times.AtLeastOnce());
        obtainedBookings.Should().NotBeNullOrEmpty().And.AllBeOfType<Booking>();
    }

    [Fact]
    public void FindABookingFromACsvFileWithBookingReservationNumber()
    {
        var expectedBooking = Fixture.Create<Booking>();
        _reader.Setup(r => r.Read(It.IsAny<string>())).Returns(Fixture.Create<string[]>().Take(1).ToArray());
        _userRepository.Setup(u => u.Find(It.IsAny<string>())).Returns(Fixture.Create<User>());
        _flightRepository.Setup(f => f.Find(It.IsAny<string>())).Returns(Fixture.Create<Flight>());
        _serializer.Setup(s => s.FromCsv(It.IsAny<string>())).Returns(expectedBooking);

        var obtainedBooking = _sut.Find(expectedBooking.ReservationNumber);

        _reader.Verify(r => r.Read(It.IsAny<string>()), Times.Once());
        _userRepository.Verify(u => u.Find(It.IsAny<string>()), Times.Once());
        _flightRepository.Verify(f => f.Find(It.IsAny<string>()), Times.AtLeastOnce());
        _serializer.Verify(s => s.FromCsv(It.IsAny<string>()), Times.AtLeastOnce());
        obtainedBooking.Should().BeEquivalentTo(expectedBooking);
    }

    [Fact]
    public void ReturnNullIfABookingDoesNotExists()
    {
        _reader.Setup(r => r.Read(It.IsAny<string>())).Returns(Fixture.Create<string[]>().Take(1).ToArray());
        _userRepository.Setup(u => u.Find(It.IsAny<string>())).Returns(Fixture.Create<User>());
        _flightRepository.Setup(f => f.Find(It.IsAny<string>())).Returns(Fixture.Create<Flight>());
        _serializer.Setup(s => s.FromCsv(It.IsAny<string>())).Returns(Fixture.Create<Booking>());

        var obtainedBooking = _sut.Find(Fixture.Create<int>());

        _reader.Verify(r => r.Read(It.IsAny<string>()), Times.Once());
        _serializer.Verify(s => s.FromCsv(It.IsAny<string>()), Times.AtLeastOnce());
        obtainedBooking.Should().BeNull();
    }

    [Fact]
    public void CreateABooking()
    {
        var createdBooking = Fixture.Create<Booking>();
        _serializer.Setup(s => s.ToCsv(createdBooking)).Returns(Fixture.Create<string>());
        _reader.Setup(r => r.Write(It.IsAny<string>(), It.IsAny<string>()));

        var obtainedBooking = _sut.Save(createdBooking);

        obtainedBooking.Should().BeEquivalentTo(createdBooking);
    }

    [Fact]
    public void UpdateExistingBooking()
    {
        var bookingToUpdate = Fixture.Create<Booking>();
        bookingToUpdate.Flights = bookingToUpdate.Flights.Take(1).ToList();
        var updateDetails = Fixture.Create<Booking>();
        updateDetails.ReservationNumber = bookingToUpdate.ReservationNumber;
        _reader.Setup(r => r.Read(It.IsAny<string>())).Returns(Fixture.Create<string[]>().Take(1).ToArray());
        _serializer.Setup(s => s.FromCsv(It.IsAny<string>())).Returns(bookingToUpdate);
        _userRepository.Setup(u => u.Find(It.IsAny<string>())).Returns(Fixture.Create<User>());
        _flightRepository.Setup(f => f.Find(It.IsAny<string>())).Returns(Fixture.Create<Flight>());

        var updatedBooking = _sut.Update(bookingToUpdate.ReservationNumber, updateDetails);

        updatedBooking.Should().BeEquivalentTo(updateDetails);
    }

    [Fact]
    public void FilterBookingListAfterFilters()
    {
        _reader.Setup(r => r.Read(It.IsAny<string>())).Returns(Fixture.Create<string[]>().Take(1).ToArray());
        _userRepository.Setup(u => u.Find(It.IsAny<string>())).Returns(Fixture.Create<User>());
        _flightRepository.Setup(f => f.Find(It.IsAny<string>())).Returns(Fixture.Create<Flight>());
        _serializer.Setup(s => s.FromCsv(It.IsAny<string>())).Returns(Fixture.Create<Booking>());
        var filters = Fixture.Create<Predicate<Booking>[]>();

        var filteredBookings = _sut.Filter(filters);

        foreach (var filter in filters)
        {
            filteredBookings.All(filter.Invoke).Should().BeTrue();
        }
    }
}
