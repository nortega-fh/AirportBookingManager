using AirportBooking.DTOs;
using AirportBooking.Models;

namespace AirportBooking.Repositories;

public interface IFlightRepository
{
    void Load(string? fileName = null);
    Flight Find(string flightNumber);
    IReadOnlyList<Flight> FindAll();

    IReadOnlyList<Flight> Filter(FlightSearchParameters filters);
}
