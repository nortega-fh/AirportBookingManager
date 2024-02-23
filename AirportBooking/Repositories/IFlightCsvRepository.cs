using AirportBooking.Models;

namespace AirportBooking.Repositories
{
    public interface IFlightCsvRepository
    {
        IReadOnlyList<Flight> Filter(params Predicate<Flight>[] filters);
        Flight? Find(string flightNumber);
        IReadOnlyList<Flight> FindAll();
    }
}