namespace AirportBooking.Models;

public class Booking
{
    public int ReservationNumber { get; set; }
    public List<Flight> Flights { get; set; } = [];
    public BookingType BookingType { get; set; }
    public List<FlightClass> FlightClasses { get; set; } = [];
    public User? MainPassenger { get; set; } = null;

    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;

    public decimal CalculatePrice()
    {
        var price = 0m;
        for (var i = 0; i < Flights.Count; i++)
        {
            price += Flights[i].ClassPrices[FlightClasses[i]];
        }
        return price;
    }
}