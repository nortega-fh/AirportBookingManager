using AirportBooking.Enums;
using AirportBooking.Models;
using AirportBooking.Views.Controllers;

namespace AirportBooking.Views.Builders;

public class BookingConsoleBuilder : ConsoleViewBase
{
    private Booking _booking;
    private readonly FlightsController _flightsController;

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

    public BookingConsoleBuilder SetFlights()
    {
        var totalFlightsToBook = _booking.BookingType == BookingType.OneWay ? 1 : 2;
        while (_booking.Flights.Count < totalFlightsToBook)
        {
            _booking.Flights.Add(BookFlight());
        }
        return this;
    }

    Flight BookFlight()
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
        if (_booking.Flights.Count == 0)
            SetFlights();
        var flightClasses = new List<FlightClass>();
        _booking.Flights.ForEach(flight => flightClasses.Add(GetValue($"Please select a class for the flight {flight.Number}",
            [FlightClass.Economy, FlightClass.Business, FlightClass.FirstClass])));
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
