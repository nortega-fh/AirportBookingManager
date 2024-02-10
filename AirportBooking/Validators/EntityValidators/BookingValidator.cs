using AirportBooking.Enums;
using AirportBooking.Exceptions;
using AirportBooking.Models;

namespace AirportBooking.Validators.EntityValidators;

public class BookingValidator : IValidator<Booking>
{
    public void Validate(Booking booking)
    {
        const int reservationNumberMinimumValue = 1;
        if (booking.ReservationNumber < reservationNumberMinimumValue)
        {
            throw new InvalidAttributeException("Reservation Number", "Integer", ["Required", "Value > 0"]);
        }
        if (HasInvalidFlights(booking))
        {
            throw new InvalidAttributeException("Flights", "List Flights", ["Required", "Arrival > Departure"]);
        }
        if (HasInvalidBookingType(booking))
        {
            throw new InvalidAttributeException("Booking Type", "Booking Type", ["Required",
                "Value = {OneWay, RoundTrip}"]);
        }
        if (booking.FlightClasses.Count < booking.Flights.Count)
        {
            throw new InvalidAttributeException("Flight Classes", "List Flight Class", ["Required",
                "Count = Flights Count"]);
        }
        if (DoesNotHavePassenger(booking))
        {
            throw new InvalidAttributeException("Main Passenger", "User", ["Required"]);
        }
    }

    private static bool HasInvalidFlights(Booking booking)
    {
        return booking.Flights.Count < 1 || booking.Flights.Last().DepartureDate < booking.Flights.First().DepartureDate;
    }

    private static bool HasInvalidBookingType(Booking booking)
    {
        return booking.BookingType < BookingType.OneWay || booking.BookingType > BookingType.RoundTrip;
    }

    private static bool DoesNotHavePassenger(Booking booking)
    {
        return booking.MainPassenger?.Username.Equals(string.Empty) ?? true;
    }
}
