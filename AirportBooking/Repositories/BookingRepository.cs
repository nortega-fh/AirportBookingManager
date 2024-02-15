using AirportBooking.Constants;
using AirportBooking.DTOs;
using AirportBooking.Exceptions;
using AirportBooking.FileReaders;
using AirportBooking.Models;
using AirportBooking.Serializers.Csv;

namespace AirportBooking.Repositories;

public class BookingRepository : IBookingRepository
{
    private static int _reservationNumber = 1;
    private static readonly string bookingsFilePath = DataDirectory.GetBookingsPath();
    private List<Booking> _bookings = [];
    private readonly CsvFileReader _reader = new();
    private static readonly BookingCsvSerializer _serializer = new();
    private readonly IUserRepository _userRepository;
    private readonly IFlightRepository _flightRepository;

    public BookingRepository(IUserRepository userRepository, IFlightRepository flightRepository)
    {
        _userRepository = userRepository;
        _flightRepository = flightRepository;
        try
        {
            Load();
        }
        catch (Exception e) when (e is SerializationException)
        {
            _bookings.Clear();
            Console.WriteLine("Couldn't load booking information due to incorrect data:");
            Console.WriteLine(e.Message);
            throw;
        }
    }

    private void Load()
    {
        var storedBookings = _reader.Read(bookingsFilePath).ToList();
        storedBookings.ForEach(line => _bookings.Add(_serializer.FromCsv(line)));
        _bookings = _bookings.Select(booking =>
        {
            booking.Flights = FindBookingFlights(booking);
            booking.MainPassenger = FindBookingMainPassenger(booking);
            return booking;
        }).ToList();
        var lastReadReservationNumber = _bookings.LastOrDefault()?.ReservationNumber ?? 1;
        _reservationNumber = lastReadReservationNumber > 1 ? lastReadReservationNumber + 1 : lastReadReservationNumber;
    }

    private List<Flight> FindBookingFlights(Booking booking)
    {
        return booking.Flights.Select(flight => _flightRepository.Find(flight.Number)).ToList();
    }

    private User FindBookingMainPassenger(Booking booking)
    {
        return _userRepository.Find(booking.MainPassenger!.Username);
    }

    public Booking Find(int rerservationNumber)
    {
        return _bookings.Find(b => b.ReservationNumber == rerservationNumber)
            ?? throw new EntityNotFound<Booking, int>(rerservationNumber);
    }

    public IReadOnlyList<Booking> FindAll()
    {
        return _bookings;
    }

    public IReadOnlyList<Booking> Filter(BookingParameters filters)
    {
        return [];
    }

    public Booking Save(Booking booking)
    {
        booking.ReservationNumber = _reservationNumber;
        _reservationNumber++;
        _reader.Write(bookingsFilePath, _serializer.ToCsv(booking));
        _bookings.Add(booking);
        return booking;
    }

    public Booking Update(int reservationNumber, Booking booking)
    {
        if (_bookings.Find(b => b.ReservationNumber == reservationNumber) is null)
        {
            throw new EntityNotFound<Booking, int>(reservationNumber);
        }
        _reader.UpdateLine(bookingsFilePath, reservationNumber.ToString(), _serializer.ToCsv(booking));
        _bookings = _bookings.Select(b => b.ReservationNumber == reservationNumber ? booking : b).ToList();
        return booking;
    }

    public void Delete(int reservationNumber)
    {
        if (_bookings.Find(b => b.ReservationNumber == reservationNumber) == null)
        {
            throw new EntityNotFound<Booking, int>(reservationNumber);
        }
        _reader.DeleteLine(bookingsFilePath, reservationNumber.ToString());
        _bookings = _bookings.Where(b => b.ReservationNumber != reservationNumber).ToList();
    }
}
