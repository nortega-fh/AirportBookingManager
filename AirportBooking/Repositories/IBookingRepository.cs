using AirportBooking.DTOs;
using AirportBooking.Models;

namespace AirportBooking.Repositories;

public interface IBookingRepository
{
    Booking Find(int reservationNumber);
    IReadOnlyList<Booking> FindAll();
    Booking Save(Booking booking);
    IReadOnlyList<Booking> Filter(BookingParameters filters);
    Booking Update(int reservationNumber, Booking value);
    void Delete(int reservationNumber);
}
