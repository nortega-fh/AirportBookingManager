using AirportBooking.Exceptions;
using AirportBooking.Models.Bookings;
using AirportBooking.Models.Flights;
using AirportBooking.Models.Users;
using AirportBooking.Validators.Bookings;

namespace AirportBooking.Serializers.Csv.Bookings;

public class BookingCsvSerializer : IBookingCsvSerializer
{
    private readonly IBookingCsvValidator _validator;

    public BookingCsvSerializer(IBookingCsvValidator validator)
    {
        _validator = validator;
    }
    public Booking FromCsv(string csvLine)
    {
        var data = _validator.Validate(csvLine);
        (var reservationNumber, var bookingType) = (int.Parse(data[0]), Enum.Parse<BookingType>(data[1]));
        var flights = new List<Flight> { new() { Number = data[2] } };
        if (data[3] is not "null")
        {
            flights.Add(new() { Number = data[3] });
        }
        var flightClasses = new List<FlightClass> { Enum.Parse<FlightClass>(data[4]) };
        if (data[5] is not "null")
        {
            flightClasses.Add(Enum.Parse<FlightClass>(data[5]));
        }
        var username = data[6];
        var bookingStatus = Enum.Parse<BookingStatus>(data[7], true);
        return new Booking
        {
            ReservationNumber = reservationNumber,
            BookingType = bookingType,
            Flights = flights,
            FlightClasses = flightClasses,
            MainPassenger = new User { Username = username },
            Status = bookingStatus
        };
    }

    public string ToCsv(Booking booking)
    {
        if (booking.MainPassenger is null) throw new EntityReadingException<Booking>("Booking passenger can't be null");
        return string.Join(",", [
            booking.ReservationNumber,
            booking.BookingType,
            ListToString(booking.Flights.Select(f => f.Number)),
            ListToString(booking.FlightClasses),
            booking.MainPassenger.Username,
            booking.Status
            ]);
    }

    private static string ListToString<T>(IEnumerable<T> list)
    {
        var csvValues = string.Join(",", list);
        if (csvValues.Split(",").Length < 2)
        {
            csvValues += ",null";
        }
        return csvValues;
    }
}
