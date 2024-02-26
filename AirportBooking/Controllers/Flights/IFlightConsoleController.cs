using AirportBooking.Models.Flights;

namespace AirportBooking.Controllers.Flights;

public interface IFlightConsoleController
{
    void RequestFlightsFileName();
    IReadOnlyList<Flight> SearchFlights();
    void ShowFlightClasses(Flight flight);
}