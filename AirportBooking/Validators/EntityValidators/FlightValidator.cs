using AirportBooking.Models;

namespace AirportBooking.Validators.EntityValidators;

public class FlightValidator : IValidator<Flight>
{
    public void Validate(Flight flight)
    {
        if (flight.Number.Equals(string.Empty))
            throw new InvalidAttributeException("Flight number", "string", ["Required"]);
        if (flight.OriginCountry.Equals(string.Empty))
            throw new InvalidAttributeException("Origin country", "string", ["Required"]);
        if (flight.DestinationCountry.Equals(string.Empty))
            throw new InvalidAttributeException("Destination country", "string", ["Required"]);
        if (flight.ArrivalDate < flight.DepartureDate)
            throw new InvalidAttributeException("Arrival date", "Date time", ["Required", "Arrival Date > Departure Date"]);
        if (flight.OriginAirport.Equals(string.Empty))
            throw new InvalidAttributeException("Origin Airport", "string", ["Required"]);
        if (flight.DestinationAirport.Equals(string.Empty))
            throw new InvalidAttributeException("Destination Airport", "string", ["Required"]);
    }
}
