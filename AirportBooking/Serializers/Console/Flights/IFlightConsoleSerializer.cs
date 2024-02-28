using AirportBooking.Models.Flights;

namespace AirportBooking.Serializers.Console.Flights;

public interface IFlightConsoleSerializer
{
    void PrintToConsole(Flight entity);
    void PrintToConsoleWithPrices(Flight flight);
    void ShowFlightAvailableClasses(Flight flight);
}