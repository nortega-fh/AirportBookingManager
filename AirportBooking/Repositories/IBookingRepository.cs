using AirportBooking.Models;

namespace AirportBooking.Repositories
{
    public interface IBookingRepository
    {
        void Delete(int reservationNumber);
        IReadOnlyList<Booking> Filter(params Predicate<Booking>[] filters);
        Booking? Find(int rerservationNumber);
        IReadOnlyList<Booking> FindAll();
        Booking Save(Booking booking);
        Booking Update(int reservationNumber, Booking booking);
    }
}