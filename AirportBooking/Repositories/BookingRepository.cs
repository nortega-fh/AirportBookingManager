using AirportBooking.Exceptions;
using AirportBooking.Lib;
using AirportBooking.Models;
using AirportBooking.Serializers.CSVSerializers;
using AirportBooking.Validators;

namespace AirportBooking;

public class BookingRepository : IFileRepository<int, Booking>
{
    private static int reservationNumber = 1;
    private List<Booking> bookings = [];
    private static readonly BookingCsvSerializer bookingCsvSerializer = new();
    private readonly BookingValidator validator = new();
    private readonly CSVReader csvReader = new("bookings", "flights");

    public BookingRepository()
    {
        try
        {
            Load();
        }
        catch (Exception e) when (e is ArgumentException or EntitySerializationException or InvalidAttributeException)
        {
            Console.WriteLine("Couldn't load booking information due to incorrect data:");
            Console.WriteLine(e.Message);
        }
    }

    public void Load()
    {
        var storedBookings = csvReader.ReadEntityInformation().ToList();
        storedBookings.ForEach(line => bookings.Add(bookingCsvSerializer.FromCsv(line)));
        bookings.ForEach(validator.Validate);
        var lastReadReservationNumber = bookings.LastOrDefault()?.ReservationNumber ?? 1;
        reservationNumber = lastReadReservationNumber > 1 ? lastReadReservationNumber + 1 : lastReadReservationNumber;
    }
    public Booking? Find(int rerservationNumber)
    {
        return bookings.Find(b => b.ReservationNumber == rerservationNumber);
    }

    public IEnumerable<Booking> FindAll()
    {
        return bookings;
    }

    public Booking Save(Booking booking)
    {
        booking.ReservationNumber = reservationNumber;
        validator.Validate(booking);
        reservationNumber++;
        csvReader.WriteEntityInformation(bookingCsvSerializer.ToCsv(booking));
        csvReader.WriteRelationshipInformation(booking.ReservationNumber.ToString(),
            booking.Flights.Select(f => f.Number).ToList());
        bookings.Add(booking);
        return booking;
    }

    public Booking? Update(int reservationNumber, Booking booking)
    {
        if (bookings.Find(b => b.ReservationNumber == reservationNumber) is null)
        {
            throw new EntityNotFound<Booking, int>(reservationNumber);
        }
        csvReader.UpdateEntityInformation(reservationNumber.ToString(), bookingCsvSerializer.ToCsv(booking));
        bookings = bookings.Select(b => b.ReservationNumber == reservationNumber ? booking : b).ToList();
        return booking;
    }

    public void Delete(int reservationNumber)
    {
        if (bookings.Find(b => b.ReservationNumber == reservationNumber) == null)
        {
            throw new EntityNotFound<Booking, int>(reservationNumber);
        }
        csvReader.DeleteEntityInformation(reservationNumber.ToString(), true);
        bookings = bookings.Where(b => b.ReservationNumber != reservationNumber).ToList();
    }
}
