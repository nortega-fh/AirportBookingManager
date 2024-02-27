using AirportBooking.Models.Flights;

namespace AirportBooking.Controllers.Flights;

public interface IFlightConsoleController
{
    Flight GetFlightToBook();
    void RequestFlightsFileName();
    IReadOnlyList<Flight> SearchFlights();
    void ShowFlightClasses(Flight flight);
    FlightClass GetClassForFlight(Flight flight);
}