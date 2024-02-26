using AirportBooking.Models.Flights;

namespace AirportBooking.Repositories.Flights
{
    public interface IFlightCsvRepository
    {
        IReadOnlyList<Flight> Filter(params Predicate<Flight>[] filters);
        Flight? Find(string flightNumber);
        IReadOnlyList<Flight> FindAll();
        void AddFileToLoad(string fileName);
    }
}