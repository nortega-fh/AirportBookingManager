using AirportBooking.Models.Bookings;
using AirportBooking.Models.Flights;
using AirportBooking.Serializers.Csv.Bookings;
using AirportBooking.Validators.Bookings;
using FluentAssertions;
using Moq;

namespace AiportBooking.Tests.Serializers;

public class BookingCsvSerializerShould
{
    [Fact]
    public void ValidatesCsvLineBeforeSerializing()
    {
        var validator = new Mock<IBookingCsvValidator>();
        var data = "1,OneWay,A1,null,Economy,null,passenger,Confirmed";
        validator.Setup(v => v.Validate(data)).Returns(data.Split(","));
        var sut = new BookingCsvSerializer(validator.Object);

        sut.FromCsv(data);

        validator.Verify(v => v.Validate(data), Times.Once());
    }

    [Fact]
    public void SerializeFromCsv()
    {
        var validator = new Mock<IBookingCsvValidator>();
        var data = "1,OneWay,A1,null,Economy,null,passenger,Confirmed";
        validator.Setup(v => v.Validate(data)).Returns(data.Split(","));
        var sut = new BookingCsvSerializer(validator.Object);

        var obtainedBooking = sut.FromCsv(data);

        obtainedBooking.ReservationNumber.Should().Be(1);
        obtainedBooking.BookingType.Should().Be(BookingType.OneWay);
        obtainedBooking.Flights.Should().NotBeNullOrEmpty().And.AllBeOfType<Flight>();
        obtainedBooking.Flights.Count.Should().Be(1);
        obtainedBooking.FlightClasses.Should().NotBeNullOrEmpty().And.AllBeOfType<FlightClass>();
        obtainedBooking.FlightClasses.Count.Should().Be(1);
        obtainedBooking.MainPassenger.Should().NotBeNull();
        obtainedBooking.MainPassenger?.Username.Should().NotBeNull().And.NotBeEmpty().And.Be("passenger");
        obtainedBooking.Status.Should().Be(BookingStatus.Confirmed);
    }

    [Fact]
    public void SerializeToCsv()
    {
        var bookingToSerialize = new Booking
        {
            ReservationNumber = 1,
            BookingType = BookingType.OneWay,
            Flights = [new Flight { Number = "A1" }],
            FlightClasses = [FlightClass.Business],
            MainPassenger = new() { Username = "Passenger" },
            Status = BookingStatus.Confirmed
        };

        var validator = new Mock<IBookingCsvValidator>();
        var sut = new BookingCsvSerializer(validator.Object);

        var obtainedCsv = sut.ToCsv(bookingToSerialize);
        obtainedCsv.Should().NotBeNull().And.Be("1,OneWay,A1,null,Business,null,Passenger,Confirmed");
    }
}
