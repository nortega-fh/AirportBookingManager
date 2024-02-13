using AirportBooking.DTOs;
using AirportBooking.Models;

namespace AirportBooking.Repositories;

public interface IFlightRepository
{
    Flight Find(string flightNumber);
    IReadOnlyList<Flight> FindAll();
    Flight Save(Flight flight);
    Flight Update(string flightNumber, Flight flight);
    IReadOnlyList<Flight> Filter(FlightSearchParameters filters);
    void Delete(string flightNumber);
}
