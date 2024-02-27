using AirportBooking.Models.Bookings;

namespace AirportBooking.Repositories.Bookings;

public interface IBookingCsvRepository
{
    IReadOnlyList<Booking> Filter(params Predicate<Booking>[] filters);
    Booking? Find(int rerservationNumber);
    IReadOnlyList<Booking> FindAll();
    Booking Save(Booking booking);
    Booking Update(int reservationNumber, Booking booking);
}