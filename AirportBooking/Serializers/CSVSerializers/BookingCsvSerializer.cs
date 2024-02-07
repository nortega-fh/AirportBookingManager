using AirportBooking.Enums;
using AirportBooking.Exceptions;
using AirportBooking.Models;

namespace AirportBooking.Serializers.CSVSerializers;

public class BookingCsvSerializer : ICSVSerializer<Booking>
{
    public Booking FromCsv(string csvLine)
    {
        ValidateCsvLine(csvLine);
        var data = csvLine.Split(",");
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
        }; ;
    }

    private static void ValidateCsvLine(string csvLine)
    {
        var data = csvLine.Split(",");
        var error = new EntitySerializationException($"Incomplete data on line {csvLine}");
        if (data.Length < 2) throw error;
        var bookingType = Enum.Parse<BookingType>(data[1]);
        if (bookingType is BookingType.OneWay && data.Length < 4) throw error;
        if (bookingType is BookingType.RoundTrip && data.Length < 5) throw error;
    }

    public string ToCsv(Booking booking)
    {
        if (booking.MainPassenger is null) throw new EntitySerializationException("Booking passenger can't be null");
        return string.Join(",", [booking.ReservationNumber,
            booking.BookingType,
            .. booking.FlightClasses,
            booking.MainPassenger.Username]);
    }
}
