using AirportBooking.Constants;
using AirportBooking.Exceptions;
using AirportBooking.Models;
using AirportBooking.Validators.CsvValidators;

namespace AirportBooking.Serializers.Csv;

public class BookingCsvSerializer
{
    private readonly BookingCsvValidator _validator = new();
    public Booking FromCsv(string csvLine)
    {
        var data = _validator.Validate(csvLine);
        (var reservationNumber, var bookingType) = (int.Parse(data[0]), Enum.Parse<BookingType>(data[1]));
        var flights = new List<Flight> { new() { Number = data[2] } };
        if (data[3] is not CsvValueSkipper.ValueSkipper)
        {
            flights.Add(new() { Number = data[3] });
        }
        var flightClasses = new List<FlightClass> { Enum.Parse<FlightClass>(data[4]) };
        if (data[5] is not CsvValueSkipper.ValueSkipper)
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
