using AirportBooking.Enums;

namespace AirportBooking.DTOs;

public class FlightSearchParametersBuilder
{
    private FlightSearchParameters _searchParameters;

    public FlightSearchParametersBuilder()
    {
        _searchParameters = new FlightSearchParameters();
    }

    public FlightSearchParametersBuilder SetOriginCountry(string originCountry)
    {
        _searchParameters.OriginCountry = originCountry;
        return this;
    }

    public FlightSearchParametersBuilder SetDestinationCountry(string destinationCountry)
    {
        _searchParameters.DestinationCountry = destinationCountry;
        return this;
    }

    public FlightSearchParametersBuilder SetDepartureDate(DateTime departureDate)
    {
        _searchParameters.DepartureDate = departureDate;
        return this;
    }

    public FlightSearchParametersBuilder SetDepartureAirport(string departureAirport)
    {
        _searchParameters.DepartureAirport = departureAirport;
        return this;
    }

    public FlightSearchParametersBuilder SetArrivalAirport(string arrivalAirport)
    {
        _searchParameters.ArrivalAirport = arrivalAirport;
        return this;
    }

    public FlightSearchParametersBuilder SetFlightClass(FlightClass flightClass)
    {
        _searchParameters.FlightClass = flightClass;
        return this;
    }

    public FlightSearchParametersBuilder SetMinPrice(float minPrice)
    {
        _searchParameters.MinPrice = minPrice;
        return this;
    }

    public FlightSearchParametersBuilder SetMaxPrice(float maxPrice)
    {
        _searchParameters.MaxPrice = maxPrice;
        return this;
    }

    public void Reset()
    {
        _searchParameters = new FlightSearchParameters();
    }

    public FlightSearchParameters GetFlightSearchParameters()
    {
        var searchParameters = _searchParameters;
        Reset();
        return searchParameters;
    }
}
