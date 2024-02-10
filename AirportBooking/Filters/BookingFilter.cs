using AirportBooking.DTOs;
using AirportBooking.Enums;
using AirportBooking.Models;
using AirportBooking.Repositories;
using System.Collections.Immutable;

namespace AirportBooking.Filters;

internal class BookingFilter : IFilter<Booking, BookingParameters>
{
    private readonly IFileRepository<string, Flight> _fightRepository;
    private readonly IFilter<Flight, FlightSearchParameters> _flightFilter;

    public BookingFilter(IFilter<Flight, FlightSearchParameters> flightFilter, IFileRepository<string, Flight> flightRepository)
    {
        _fightRepository = flightRepository;
        _flightFilter = flightFilter;
    }

    public IReadOnlyList<Booking> SearchByParameters(BookingParameters filterParams, IList<Booking> originalList)
    {
        return FilterBookings(filterParams, [.. originalList]);
    }

    private IReadOnlyList<Booking> FilterBookings(BookingParameters filterParams, List<Booking> originalList)
    {

        originalList = FilterByValue(filterParams.FlightNumber, originalList, FilterByFlightNumber);
        originalList = FilterByValue(filterParams.Passenger, originalList, FilterByUser);
        originalList = FilterByValue(filterParams.BookingType, originalList, FilterByBookingType);
        var flightParameters = ObtainFlightParametersFromBookingParamaters(filterParams);
        originalList = flightParameters is null ? originalList : FilterByFlightParameters(flightParameters, originalList);
        return originalList;
    }

    private List<Booking> FilterByValue<T>(T? value, List<Booking> originalList,
        Func<T?, List<Booking>, List<Booking>> filter)
    {
        if (value is null) return originalList;
        return filter(value, originalList);
    }

    private FlightSearchParameters? ObtainFlightParametersFromBookingParamaters(BookingParameters filterParams)
    {
        var (_, minPrice, maxPrice, departureCountry, destinationCountry,
            departureDate, departureAirport, arrivalAirport, _, flightClass, _) = filterParams;
        if (departureCountry is null || destinationCountry is null || departureDate is null) return null;
        return new FlightSearchParameters(
                departureCountry,
                destinationCountry,
                departureDate.Value,
                departureAirport,
                arrivalAirport,
                flightClass,
                minPrice,
                maxPrice
            );
    }

    private List<Booking> FilterByFlightNumber(string? flightNumber, List<Booking> originalList)
    {
        if (flightNumber == null) return originalList;
        return originalList.Where(b => b.Flights.Any(f =>
            f.Number.Equals(flightNumber, StringComparison.OrdinalIgnoreCase))).ToList();
    }

    private List<Booking> FilterByUser(string? user, List<Booking> originalList)
    {
        if (user is null) return originalList;
        return originalList.Where(b => b.MainPassenger?.Username == user).ToList();
    }

    private List<Booking> FilterByBookingType(BookingType? bookingType, List<Booking> originalList)
    {
        if (bookingType == null) return originalList;
        return originalList.Where(b => b.BookingType == bookingType).ToList();
    }

    private List<Booking> FilterByFlightParameters(FlightSearchParameters flightSearchParameters, List<Booking> originalList)
    {
        var resultingFlights = _flightFilter.SearchByParameters(flightSearchParameters, [.. _fightRepository.FindAll()]);
        return originalList.Where(b => b.Flights.Any(f => resultingFlights.Contains(f))).ToList();
    }
}
