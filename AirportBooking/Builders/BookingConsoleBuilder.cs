using AirportBooking.Controllers;
using AirportBooking.Enums;
using AirportBooking.Models;

namespace AirportBooking.Views.Builders;

public class BookingConsoleBuilder : BaseConsoleView
{
    private Booking _booking;
    private readonly IController<Flight> _flightsController;

    public BookingConsoleBuilder(FlightsController flightController)
    {
        _booking = new Booking();
        _flightsController = flightController;
    }

    public BookingConsoleBuilder ReservationNumber()
    {
        _booking.ReservationNumber = int.Parse(GetValue("Please indicate the booking's reservation number"));
        return this;
    }

    public BookingConsoleBuilder SetFlights(BookingType? bookingType = null)
    {
        var type = bookingType ?? _booking.BookingType;
        var totalFlightsToBook = type == BookingType.OneWay ? 1 : 2;
        while (_booking.Flights.Count < totalFlightsToBook)
        {
            _booking.Flights.Add(BookFlight());
        }
        return this;
    }

    private Flight BookFlight()
    {
        IReadOnlyList<Flight>? resultingFlights = null;
        while (resultingFlights is null || resultingFlights.Count < 1)
        {
            resultingFlights = _flightsController.SearchByParameters(_flightsController.ObtainFilterParameters());
        }
        var chosenFlight = GetValue("Please select one of the flights listed above", [.. resultingFlights.Select(f => f.Number)]);
        return resultingFlights.ToList().Find(f => f.Number == chosenFlight) ?? resultingFlights[0];
    }

    public BookingConsoleBuilder SetBookingType()
    {
        _booking.BookingType = GetValue("Please indicate the booking's type", [BookingType.OneWay, BookingType.RoundTrip]);
        return this;
    }

    public BookingConsoleBuilder SetFlightClasses()
    {
        var flightClasses = new List<FlightClass>();
        _booking.Flights.ForEach(flight =>
        {
            var selectedClass = GetValue($"Please select a class for the flight {flight.Number}", [FlightClass.Economy,
                FlightClass.Business,
                FlightClass.FirstClass]);
            flightClasses.Add(selectedClass);
        });
        _booking.FlightClasses = flightClasses;
        return this;
    }

    public BookingConsoleBuilder SetPassenger(User user)
    {
        _booking.MainPassenger = user;
        return this;
    }

    public void Reset()
    {
        _booking = new Booking();
    }

    public Booking GetBooking()
    {
        var booking = _booking;
        Reset();
        return booking;
    }
}
