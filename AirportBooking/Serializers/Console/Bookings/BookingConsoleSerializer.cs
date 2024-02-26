using AirportBooking.Models.Bookings;

namespace AirportBooking.Serializers.Console.Bookings;

public class BookingConsoleSerializer : IBookingConsoleSerializer
{
    public void PrintToConsole(Booking booking)
    {
        var isNewBooking = booking.ReservationNumber == 0;
        if (isNewBooking)
        {
            Console.WriteLine($"""
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
        Console.WriteLine($""""
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
        ShowEnumList<BookingType>();
    }

    public void ShowBookingStatuses()
    {
        ShowEnumList<BookingStatus>();
    }

    private static void ShowEnumList<T>() where T : struct, Enum
    {
        var namesList = Enum.GetNames<T>();
        for (var i = 0; i < namesList.Length; ++i)
        {
            Console.WriteLine($"{i + 1}. {namesList[i]}");
        }
    }
}
