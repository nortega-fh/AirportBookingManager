using AirportBooking.Models.Users;

namespace AirportBooking.Controllers.Bookings;

public interface IBookingConsoleController
{
    void CancelBooking();
    void CreateBooking(User user);
    void SearchBookings();
    void ShowUserBookings(User user);
    void UpdateBooking(User user);
}