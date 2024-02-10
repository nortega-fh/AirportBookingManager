using AirportBooking.Enums;
using AirportBooking.Exceptions;
using AirportBooking.Models;
using AirportBooking.Validators.CsvValidators;

namespace AirportBooking.Serializers.CSVSerializers;

public class BookingCsvSerializer : ICSVSerializer<Booking>
{
    private readonly BookingCsvValidator _validator = new();
    public Booking FromCsv(string csvLine)
    {
        var data = _validator.Validate(csvLine);
        (var reservationNumber, var bookingType) = (int.Parse(data[0]), Enum.Parse<BookingType>(data[1]));
        var flightClass1 = Enum.Parse<FlightClass>(data[2]);
        var flightClasses = new List<FlightClass> { flightClass1 };
        if (bookingType is BookingType.RoundTrip)
        {
            flightClasses.Add(Enum.Parse<FlightClass>(data[3]));
        }
        var username = flightClasses.Count > 1 ? data[4] : data[3];
        return new Booking
        {
            ReservationNumber = reservationNumber,
            FlightClasses = flightClasses,
            BookingType = bookingType,
            MainPassenger = new User { Username = username }
        };
    }

    public string ToCsv(Booking booking)
    {
        if (booking.MainPassenger is null) throw new EntitySerializationException<Booking>("Booking passenger can't be null");
        return string.Join(",", [
            booking.ReservationNumber,
            booking.BookingType,
            .. booking.FlightClasses,
            booking.MainPassenger.Username,
            booking.Status
            ]);
    }
}
