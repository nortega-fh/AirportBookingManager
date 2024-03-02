using AirportBooking.Models.Flights;
using System.Reflection;
using Xunit.Sdk;

namespace AiportBooking.Tests.Repositories.Attributes;

public class FlightListFilterDataAttribute : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var data = new string[]
        {
            "A1,120,130,140,Colombia,United States of America (USA),2024-03-15T05:12:00Z,2024-03-16T12:12:00Z,BOG,MIA",
            "A2,120,130,140,United States of America (USA), Colombia,2024-03-20T05:12:00Z,2024-03-21T12:12:00Z,MIA,BOG"
        };
        var flight1 = new Flight
        {
            Number = "A1",
            OriginCountry = "Colombia",
            DestinationCountry = "United States of America (USA)",
            DepartureDate = DateTime.Parse("2024-03-15T05:12:00Z"),
            ArrivalDate = DateTime.Parse("2024-03-16T12:12:00Z"),
            OriginAirport = "BOG",
            DestinationAirport = "MIA",
            ClassPrices =
            {
                { FlightClass.Economy, 120 },
                { FlightClass.Business, 130 },
                { FlightClass.FirstClass, 140 }
            }
        };
        var flight2 = new Flight
        {
            Number = "A2",
            OriginCountry = "United States of America (USA)",
            DestinationCountry = "Colombia",
            DepartureDate = DateTime.Parse("2024-03-20T05:12:00Z"),
            ArrivalDate = DateTime.Parse("2024-03-21T12:12:00Z"),
            OriginAirport = "MIA",
            DestinationAirport = "BOG",
            ClassPrices =
            {
                { FlightClass.Economy, 120 },
                { FlightClass.Business, 130 },
                { FlightClass.FirstClass, 140 }
            }
        };
        Predicate<Flight> filter1 = (Flight flight) => flight.OriginCountry == "Colombia";
        Predicate<Flight> filter2 = (Flight flight) => flight.OriginCountry.Contains("col");
        Predicate<Flight> filter3 = (Flight flight) => flight.OriginCountry.Contains("usa", StringComparison.OrdinalIgnoreCase);
        yield return new object[] { data, flight1, flight2, filter1 };
        yield return new object[] { data, flight1, flight2, filter2 };
        yield return new object[] { data, flight1, flight2, filter3 };
    }
}
