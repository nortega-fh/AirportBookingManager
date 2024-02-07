using AirportBooking.Enums;
using AirportBooking.Models;

namespace AirportBooking.Validators;

public class BookingValidator : IValidator<Booking>
{
    private readonly List<InvalidAttributeException> _errors = [];

    public void Validate(Booking booking)
    {
        if (booking.ReservationNumber < 1)
        {
            throw new InvalidAttributeException("Reservation Number", "Integer", ["Required", "Value > 0"]);
        }
        if (booking.Flights.Count < 1 || booking.Flights.Last().DepartureDate > booking.Flights.First().DepartureDate)
        {
            throw new InvalidAttributeException("Flights", "List Flights", ["Required", "Arrival > Departure"]);
        }
        bool isInvalidBookingType = !booking.BookingType.ToString().Equals(BookingType.OneWay.ToString())
            || !booking.BookingType.ToString().Equals(BookingType.RoundTrip.ToString());
        if (isInvalidBookingType)
        {
            throw new InvalidAttributeException("Booking Type", "Booking Type", ["Required",
                "Value = {OneWay, RoundTrip}"]);
        }
        if (booking.FlightClasses.Count < booking.Flights.Count)
        {
            throw new InvalidAttributeException("Flight Classes", "List Flight Class", ["Required",
                "Count = Flights Count"]);
        }
        if (booking.MainPassenger?.Username.Equals(string.Empty) ?? true)
        {
            throw new InvalidAttributeException("Main Passenger", "User", ["Required"]);
        }
    }
}
