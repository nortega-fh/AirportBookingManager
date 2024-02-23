using AirportBooking.Models;

namespace AirportBooking.Serializers.ConsoleSerializers;

public class FlightConsoleSerializer : IFlightConsoleSerializer
{
    public void PrintToConsole(Flight entity)
    {
        Console.WriteLine($"""
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
        Console.WriteLine($"""
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
