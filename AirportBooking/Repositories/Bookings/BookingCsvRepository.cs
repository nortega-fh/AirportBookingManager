using AirportBooking.Exceptions;
using AirportBooking.FileReaders;
using AirportBooking.Models.Bookings;
using AirportBooking.Models.Flights;
using AirportBooking.Models.Users;
using AirportBooking.Repositories.Flights;
using AirportBooking.Repositories.Users;
using AirportBooking.Serializers.Csv.Bookings;

namespace AirportBooking.Repositories.Bookings;

public class BookingCsvRepository : IBookingCsvRepository
{
    private static int _reservationNumber = 1;
    private static readonly string BookingsFilePath = Path.Combine("..", "..", "..", "Data", "bookings.csv");
    private readonly ICsvFileReader _reader;
    private readonly IBookingCsvSerializer _serializer;
    private readonly IUserCsvRepository _userRepository;
    private readonly IFlightCsvRepository _flightRepository;

    public BookingCsvRepository(
        ICsvFileReader reader,
        IBookingCsvSerializer serializer,
        IUserCsvRepository userRepository,
        IFlightCsvRepository flightRepository)
    {
        _reader = reader;
        _serializer = serializer;
        _userRepository = userRepository;
        _flightRepository = flightRepository;
    }

    public Booking? Find(int rerservationNumber)
    {
        var booking = _reader.Read(BookingsFilePath)
            .Select(_serializer.FromCsv)
            .Where(booking => booking.ReservationNumber == rerservationNumber)
            .FirstOrDefault();
        return booking is not null ? GetBookingInformation(booking) : booking;
    }

    public IReadOnlyList<Booking> FindAll()
    {
        return _reader.Read(BookingsFilePath)
            .Select(csvLine => GetBookingInformation(_serializer.FromCsv(csvLine)))
            .ToList();
    }

    private Booking GetBookingInformation(Booking booking)
    {
        booking.MainPassenger = _userRepository.Find(booking.MainPassenger!.Username)
            ?? throw new EntityNotFound<User, string>(booking.MainPassenger.Username);
        booking.Flights = booking.Flights
            .Select(flight => _flightRepository.Find(flight.Number)
                ?? throw new EntityNotFound<Flight, string>(flight.Number))
            .ToList();
        return booking;
    }

    public IReadOnlyList<Booking> Filter(params Predicate<Booking>[] filters)
    {
        var bookingList = FindAll();
        foreach (var filter in filters)
        {
            bookingList = bookingList.Where(filter.Invoke).ToList();
        }
        return bookingList;
    }

    public Booking Save(Booking booking)
    {
        booking.ReservationNumber = _reservationNumber;
        _reservationNumber++;
        _reader.Write(BookingsFilePath, _serializer.ToCsv(booking));
        return booking;
    }

    public Booking Update(int reservationNumber, Booking booking)
    {
        if (Find(reservationNumber) is null)
        {
            throw new EntityNotFound<Booking, int>(reservationNumber);
        }
        _reader.UpdateLine(BookingsFilePath, reservationNumber.ToString(), _serializer.ToCsv(booking));
        return booking;
    }
}
