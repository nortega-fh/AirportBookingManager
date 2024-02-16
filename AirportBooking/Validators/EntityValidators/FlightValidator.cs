using AirportBooking.Constants;
using AirportBooking.Exceptions;
using AirportBooking.Models;

namespace AirportBooking.Validators.EntityValidators;

public class FlightValidator
{
    public Flight Validate(Flight flight)
    {
        if (flight.Number.Equals(string.Empty))
        {
            throw new InvalidAttributeException<string>("Flight number", EntityValueRestriction.Restrictions[Restriction.Field]);
        }
        if (flight.OriginCountry.Equals(string.Empty))
        {
            throw new InvalidAttributeException<string>("Origin country", EntityValueRestriction.Restrictions[Restriction.Field]);
        }
        if (flight.DestinationCountry.Equals(string.Empty))
        {
            throw new InvalidAttributeException<string>("Destination country", EntityValueRestriction.Restrictions[Restriction.Field]);
        }
        if (flight.ArrivalDate < flight.DepartureDate)
        {
            throw new InvalidAttributeException<DateTime>("Arrival date", EntityValueRestriction.Restrictions[Restriction.Arrival]);
        }
        if (flight.OriginAirport.Equals(string.Empty))
        {
            throw new InvalidAttributeException<string>("Origin Airport", EntityValueRestriction.Restrictions[Restriction.Field]);
        }
        if (flight.DestinationAirport.Equals(string.Empty))
        {
            throw new InvalidAttributeException<string>("Destination Airport", EntityValueRestriction.Restrictions[Restriction.Field]);
        }
        return flight;
    }
}
