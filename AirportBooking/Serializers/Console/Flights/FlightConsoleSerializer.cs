using AirportBooking.Models.Flights;

namespace AirportBooking.Serializers.Console.Flights;

public class FlightConsoleSerializer : IFlightConsoleSerializer
{
    public void PrintToConsole(Flight entity)
    {
        System.Console.WriteLine($"""
            -------------------------------------------------------------
                       Flight Number: {entity.Number} 
            -------------------------------------------------------------
            From: {entity.OriginCountry} ({entity.OriginAirport}) 
            To: {entity.DestinationCountry} ({entity.DestinationAirport})  
            Departs: {entity.DepartureDate} 
            Arrives: {entity.ArrivalDate}
            -------------------------------------------------------------
            """);
    }

    public void PrintToConsoleWithPrices(Flight flight)
    {
        System.Console.WriteLine($"""
            -------------------------------------------------------------
                    Flight Number: {flight.Number} 
            -------------------------------------------------------------
            From: {flight.OriginCountry} ({flight.OriginAirport}) 
            To: {flight.DestinationCountry} ({flight.DestinationAirport})  
            Departs: {flight.DepartureDate} 
            Arrives: {flight.ArrivalDate}
            -------------------------------------------------------------
                                        Prices
            _____________________________________________________________
            {GetPricesToString(flight)}
            """);
    }

    public void ShowFlightAvailableClasses(Flight flight)
    {
        for (var i = 1; i < flight.ClassPrices.Count + 1; i++)
        {
            var classPrice = flight.ClassPrices.ElementAt(i - 1);
            System.Console.WriteLine($"{i}. {classPrice.Key} : {string.Format("{0:C}", classPrice.Value)}");
        }
    }

    private static string GetPricesToString(Flight flight)
    {
        var serializedPrices = "";
        foreach (var pair in flight.ClassPrices)
        {
            serializedPrices += string.Format("{0}: {1:C}\n", pair.Key, pair.Value);
        }
        return serializedPrices;
    }
}
