using AirportBooking.Enums;
using AirportBooking.Models;

namespace AirportBooking.DTOs;

public class BookingParametersBuilder
{
    private BookingParameters _bookingParameters;

    public BookingParametersBuilder()
    {
        _bookingParameters = new BookingParameters();
    }

    public BookingParametersBuilder SetFlightNumber(string flightNumber)
    {
        _bookingParameters.FlightNumber = flightNumber;
        return this;
    }

    public BookingParametersBuilder SetPassenger(User passenger)
    {
        _bookingParameters.Passenger = passenger.Username;
        return this;
    }

    public BookingParametersBuilder SetBookingType(BookingType bookingType)
    {
        _bookingParameters.BookingType = bookingType;
        return this;
    }

    public BookingParametersBuilder SetBookingStatus(BookingStatus bookingStatus)
    {
        _bookingParameters.Status = bookingStatus;
        return this;
    }

    public BookingParametersBuilder SetFlightSearchParameters(FlightSearchParameters flightSearchParameters)
    {
        _bookingParameters.FlightSearchParameters = flightSearchParameters;
        return this;
    }

    public void Reset()
    {
        _bookingParameters = new BookingParameters();
    }

    public BookingParameters Build()
    {
        var builtParameters = _bookingParameters;
        Reset();
        return builtParameters;
    }
}
