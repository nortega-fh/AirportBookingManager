using AirportBooking.Models.Bookings;

namespace AirportBooking.Serializers.Console.Bookings;

public class BookingConsoleSerializer : IBookingConsoleSerializer
{
    public void PrintToConsole(Booking booking)
    {
        var isNewBooking = booking.ReservationNumber == 0;
        if (isNewBooking)
        {
            System.Console.WriteLine($"""
                ######################################
                    Booking {booking.BookingType}
                --------------------------------------
                Flights: {string.Join(",", [.. booking.Flights.Select(f => f.Number)])}
                Class: {string.Join(",", booking.FlightClasses)}
                Passenger: {booking.MainPassenger?.Username ?? ""}
                --------------------------------------
                   Total price: {string.Format("{0:C}", booking.CalculatePrice())}
                ######################################
                """);
            return;
        }
        System.Console.WriteLine($""""
            ######################################
              Reservation number: {booking.ReservationNumber}
            ######################################
            Flights: {string.Join(",", [.. booking.Flights.Select(f => f.Number)])}
            Booking type: {booking.BookingType}
            Class: {string.Join(",", booking.FlightClasses)}
            Passenger: {booking.MainPassenger?.Username ?? ""}
            Status: {booking.Status}
            --------------------------------------
               Total price: {string.Format("{0:C}", booking.CalculatePrice())}
            --------------------------------------
            """");
    }

    public void ShowBookingTypes()
    {
        var namesList = Enum.GetNames<BookingType>();
        for (var i = 0; i < namesList.Length; ++i)
        {
            System.Console.WriteLine($"{i + 1}. {namesList[i]}");
        }
    }
}
