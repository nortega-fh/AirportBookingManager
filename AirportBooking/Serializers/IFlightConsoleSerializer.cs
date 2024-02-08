using AirportBooking.Models;

namespace AirportBooking.Serializers;

public interface IFlightConsoleSerializer : IConsoleSerializer<Flight>
{
    void PrintToConsoleWithPrices(Flight flight);
}
