using AirportBooking.DTOs;
using AirportBooking.Models;
using System.Collections.Immutable;

namespace AirportBooking.Filters;

public class FlightFilter : IFilter<Flight, FlightSearchParameters>
{
    public IReadOnlyList<Flight> SearchByParameters(FlightSearchParameters parameters, IList<Flight> originalList)
    {
        IEnumerable<Flight> resultFlights = originalList
            .Where(f => f.OriginCountry.Contains(parameters.OriginCountry, StringComparison.OrdinalIgnoreCase))
            .Where(f => f.DestinationCountry.Contains(parameters.DestinationCountry, StringComparison.OrdinalIgnoreCase))
            .Where(f => parameters.DepartureDate <= f.DepartureDate && f.DepartureDate < parameters.DepartureDate.AddDays(1)
            && f.DepartureDate > parameters.DepartureDate.AddDays(-1))
            .Where(f => f.ClassPrices.Values.Min() >= parameters.MinPrice && f.ClassPrices.Values.Min() <= parameters.MaxPrice);
        if (parameters.DepartureAirport is not null and not "")
        {
            resultFlights = resultFlights.Where(f => f.OriginAirport.Equals(parameters.DepartureAirport,
                StringComparison.OrdinalIgnoreCase));
        }
        if (parameters.ArrivalAirport is not null and not "")
        {
            resultFlights = resultFlights.Where(f => f.DestinationAirport.Equals(parameters.ArrivalAirport,
                StringComparison.OrdinalIgnoreCase));
        }
        if (parameters.FlightClass is not null)
        {
            resultFlights = resultFlights.Where(f => f.ClassPrices.ContainsKey(parameters.FlightClass.Value));
        }
        return resultFlights.ToImmutableList();
    }
}
