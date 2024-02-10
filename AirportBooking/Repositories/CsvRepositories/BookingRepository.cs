using AirportBooking.Exceptions;
using AirportBooking.FileReaders;
using AirportBooking.Models;
using AirportBooking.Serializers.CSVSerializers;
using AirportBooking.Validators.EntityValidators;

namespace AirportBooking.Repositories.CsvRepositories;

public class BookingRepository : IFileRepository<int, Booking>
{
    private static int _reservationNumber = 1;
    private List<Booking> _bookings = [];
    private readonly CSVReader _reader = new("bookings", "flights");
    private static readonly BookingCsvSerializer _serializer = new();
    private readonly BookingValidator _validator = new();
    private readonly IFileRepository<string, User> _userRepository;
    private readonly IFileRepository<string, Flight> _flightRepository;

    public BookingRepository(IFileRepository<string, User> userRepository,
        IFileRepository<string, Flight> flightRepository)
    {
        _userRepository = userRepository;
        _flightRepository = flightRepository;
        try
        {
            Load();
        }
        catch (Exception e) when (e is ArgumentException or EntitySerializationException<Booking> or InvalidAttributeException)
        {
            _bookings.Clear();
            Console.WriteLine("Couldn't load booking information due to incorrect data:");
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public void Load()
    {
        var storedBookings = _reader.ReadEntityInformation().ToList();
        storedBookings.ForEach(line => _bookings.Add(_serializer.FromCsv(line)));
        _bookings.ForEach(booking =>
        {
            booking.Flights = FindBookingFlights(booking.ReservationNumber);
            booking.MainPassenger = FindBookingMainPassenger(booking.MainPassenger?.Username);
            _validator.Validate(booking);
        });
        var lastReadReservationNumber = _bookings.LastOrDefault()?.ReservationNumber ?? 1;
        _reservationNumber = lastReadReservationNumber > 1 ? lastReadReservationNumber + 1 : lastReadReservationNumber;
    }

    private List<Flight> FindBookingFlights(int bookingId)
    {
        var hasFlights = _reader.GetRelationshipInformation().TryGetValue(bookingId.ToString(), out var flightNumbers);
        if (!hasFlights)
        {
            throw new InvalidAttributeException("Flights", "List of Flights", ["Required", "Departure < Arrival"]);
        }
        var flights = flightNumbers?.ToList().Select(number => _flightRepository.Find(number)
        ?? throw new EntityNotFound<Flight, string>(number)).ToList();

        return flights ?? throw new InvalidAttributeException("Flights", "List of Flights", ["Required", "Departure < Arrival"]);
    }

    private User FindBookingMainPassenger(string? username)
    {
        if (username is null) throw new InvalidAttributeException("Main Passenger", "User", ["Required"]);
        return _userRepository.Find(username) ?? throw new EntityNotFound<User, string>(username);
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

    public Booking Save(Booking booking)
    {
        booking.ReservationNumber = _reservationNumber;
        _validator.Validate(booking);
        _reservationNumber++;
        _reader.WriteEntityInformation(_serializer.ToCsv(booking));
        _reader.WriteRelationshipInformation(booking.ReservationNumber.ToString(),
            booking.Flights.Select(f => f.Number).ToList());
        _bookings.Add(booking);
        return booking;
    }

    public Booking Update(int reservationNumber, Booking booking)
    {
        if (_bookings.Find(b => b.ReservationNumber == reservationNumber) is null)
        {
            throw new EntityNotFound<Booking, int>(reservationNumber);
        }
        _reader.WriteRelationshipInformation(booking.ReservationNumber.ToString(),
            booking.Flights.Select(f => f.Number).ToList());
        _reader.UpdateEntityInformation(reservationNumber.ToString(), _serializer.ToCsv(booking));
        _bookings = _bookings.Select(b => b.ReservationNumber == reservationNumber ? booking : b).ToList();
        return booking;
    }

    public void Delete(int reservationNumber)
    {
        if (_bookings.Find(b => b.ReservationNumber == reservationNumber) == null)
        {
            throw new EntityNotFound<Booking, int>(reservationNumber);
        }
        _reader.DeleteEntityInformation(reservationNumber.ToString(), true);
        _bookings = _bookings.Where(b => b.ReservationNumber != reservationNumber).ToList();
    }
}
