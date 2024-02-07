using AirportBooking.Enums;

namespace AirportBooking.Models;

public class Booking
{
    public int ReservationNumber { get; set; }
    public List<Flight> Flights { get; set; } = [];
    public BookingType BookingType { get; set; }
    public List<FlightClass> FlightClasses { get; set; } = [];
    public User? MainPassenger { get; set; } = null;

    public float CalculatePrice()
    {
        var price = 0f;
        for (var i = 0; i < Flights.Count; i++)
        {
            price += Flights[i].ClassPrices[FlightClasses[i]];
        }
        return price;
    }

    public override string ToString()
    {
        return $"""
                ######################################
                         Booking {ReservationNumber}
                ######################################
                Flights:
                {string.Join("\n", Flights)}
                Booking type: {BookingType}
                Class: {string.Join(",", FlightClasses)}
                Passenger:
                {MainPassenger}
                --------------------------------------
                    Total price: ${CalculatePrice()}
                --------------------------------------
                """;
    }
}