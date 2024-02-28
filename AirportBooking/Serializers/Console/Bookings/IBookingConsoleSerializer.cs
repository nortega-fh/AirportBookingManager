using AirportBooking.Models.Bookings;

namespace AirportBooking.Serializers.Console.Bookings;

public interface IBookingConsoleSerializer
{
    void PrintToConsole(Booking booking);
    void ShowBookingTypes();
}