using AirportBooking.Constants;
using AirportBooking.Exceptions;
using AirportBooking.Models;

namespace AirportBooking.Validators.CsvValidators;

public class BookingCsvValidator : IBookingCsvValidator
{
    private const int minLineLength = 8;
    public string[] Validate(string csvLine)
    {
        var data = csvLine.Split(",");

        if (data.Length < minLineLength)
        {
            throw new EntityReadingException<Booking>($"Incomplete data on line {csvLine}");
        }
        if (!Enum.TryParse<BookingType>(data[1], out var _))
        {
            throw new InvalidAttributeException<BookingType>("Booking Type", EntityValueRestriction.Restrictions[Restriction.Field]);
        }
        if (data[2] is "" or null or "null")
        {
            throw new InvalidAttributeException<string>("First flight number", EntityValueRestriction.Restrictions[Restriction.Field]);
        }
        if (data[3] is "" || data[3] is not "null")
        {
            throw new InvalidAttributeException<string>("Second flight number", EntityValueRestriction.Restrictions[Restriction.OptionalField]);
        }
        if (!Enum.TryParse<FlightClass>(data[4], true, out var _))
        {
            throw new InvalidAttributeException<FlightClass>("First flight class", EntityValueRestriction.Restrictions[Restriction.FlightClass]);
        }
        if (data[5] is "" || data[5] is not "null" || !Enum.TryParse<FlightClass>(data[4], true, out var _))
        {
            throw new InvalidAttributeException<FlightClass>("Second flight class", EntityValueRestriction.Restrictions[Restriction.OptionalFlightClass]);
        }
        if (data[6] is "" or "null")
        {
            throw new InvalidAttributeException<string>("Passenger's username", EntityValueRestriction.Restrictions[Restriction.Field]);
        }
        if (!Enum.TryParse<BookingStatus>(data[7], true, out var _))
        {
            throw new InvalidAttributeException<BookingStatus>("Booking status", EntityValueRestriction.Restrictions[Restriction.BookingStatus]);
        }
        return data;
    }
}
