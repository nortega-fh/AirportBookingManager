using AirportBooking.Models;

namespace AirportBooking.Serializers.ConsoleSerializers;

public interface IFlightConsoleSerializer
{
    void PrintToConsole(Flight entity);
    void PrintToConsoleWithPrices(Flight flight);
}