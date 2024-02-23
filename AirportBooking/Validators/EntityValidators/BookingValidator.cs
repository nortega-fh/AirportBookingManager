using AirportBooking.Constants;
using AirportBooking.Exceptions;
using AirportBooking.Models;

namespace AirportBooking.Validators.EntityValidators;

public class BookingValidator
{
    public Booking Validate(Booking booking)
    {
        const int reservationNumberMinimumValue = 1;
        if (booking.ReservationNumber < reservationNumberMinimumValue)
        {
            throw new InvalidAttributeException<int>("Reservation Number", EntityValueRestriction.Restrictions[Restriction.PositiveNumber]);
        }
        if (HasInvalidFlights(booking))
        {
            throw new InvalidAttributeException<List<Flight>>("Flights", EntityValueRestriction.Restrictions[Restriction.Arrival]);
        }
        if (HasInvalidBookingType(booking))
        {
            throw new InvalidAttributeException<string>("Booking Type", EntityValueRestriction.Restrictions[Restriction.BookingType]);
        }
        if (booking.FlightClasses.Count < booking.Flights.Count)
        {
            throw new InvalidAttributeException<List<Flight>>("Flight Classes", EntityValueRestriction.Restrictions[Restriction.FlightClass]);
        }
        if (DoesNotHavePassenger(booking))
        {
            throw new InvalidAttributeException<string>("Passenger", EntityValueRestriction.Restrictions[Restriction.Field]);
        }
        return booking;
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
