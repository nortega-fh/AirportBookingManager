using AirportBooking.Models;

namespace AirportBooking.Serializers.ConsoleSerializers;

public class BookingConsoleSerializer
{
    public void PrintToConsole(Booking booking)
    {
        Console.WriteLine($""""
            ######################################
              Reservation number: {booking.ReservationNumber}
            ######################################
            Flights: {string.Join(",", [.. booking.Flights.Select(f => f.Number)])}
            Booking type: {booking.BookingType}
            Class: {string.Join(",", booking.FlightClasses)}
            Passenger:
            {booking.MainPassenger!.Username}
            Status: {booking.Status}
            --------------------------------------
               Total price: {string.Format("{0:C}", booking.CalculatePrice())}
            --------------------------------------
            """");
    }
}
