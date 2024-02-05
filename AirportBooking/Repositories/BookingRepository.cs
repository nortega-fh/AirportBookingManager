
using AirportBooking.Enums;
using AirportBooking.Lib;

namespace AirportBooking
{
    public class BookingRepository : IFileRepository<int, Booking>
    {
        private static int reservationNumber = 1;
        private List<Booking> bookings = [];
        private readonly UserRepository userRepository;
        private readonly FlightRepository flightRepository;
        private readonly CSVReader csvReader = new("bookings", "flights");

        public BookingRepository(UserRepository userRepository, FlightRepository flightRepository)
        {
            this.userRepository = userRepository;
            this.flightRepository = flightRepository;
            try
            {
                Load();
            }
            catch (Exception e) when (e is ArgumentException or InvalidAttributeException<string> or InvalidAttributeException<List<Flight>> or InvalidAttributeException<int>)
            {
                Console.WriteLine(e.Message);
            }
        }

        private Booking Validate(string line)
        {
            var data = line.Split(",");
            if (data.Length < 6) throw new ArgumentException($"Error in booking line {line}: the data is incomplete");

            List<FlightClass> flightClasses = [];

            try
            {
                (int reservationNumber, float price, BookingType bookingType, FlightClass flightClass1, FlightClass flightClass2, string username) = (int.Parse(data[0]), float.Parse(data[1]), Enum.Parse<BookingType>(data[2]), Enum.Parse<FlightClass>(data[3]), Enum.Parse<FlightClass>(data[4]), data[5]);

                flightClasses.Add(flightClass1);

                if (!flightClass2.Equals(flightClass1)) flightClasses.Add(flightClass2);

                User user = userRepository.Find(username) ?? throw new InvalidAttributeException<string>(username, ["Required", $"{username} is not an existing User's username"]);

                List<string>? bookingFlightNumbers = csvReader.GetRelationshipInformation().GetValueOrDefault(data[0]);

                List<Flight> flights = [];

                bookingFlightNumbers?.ForEach(flightNumber => flights.Add(flightRepository.Find(flightNumber)
                    ?? throw new InvalidAttributeException<string>(flightNumber, ["Required", $"{flightNumber} is not an existing Flight's number"])));

                if (flights.Count == 0) throw new InvalidAttributeException<List<Flight>>(flights, ["Required", "There must be at least 1 flight for each booking"]);

                Booking booking = new(reservationNumber, flights, flightClasses, bookingType, user, price);

                return booking;
            }
            catch (FormatException)
            {
                reservationNumber = 0;
                throw new InvalidAttributeException<int>(reservationNumber, ["Required", "Valid values [ Integer numbers ]"]);
            }
        }

        public void Load()
        {
            var storedBookings = csvReader.ReadEntityInformation().ToList();
            storedBookings.ForEach(line => bookings.Add(Validate(line)));
            var lastBookingCreated = storedBookings.LastOrDefault();
            reservationNumber = lastBookingCreated is null ? reservationNumber : int.Parse(lastBookingCreated.Split(",")[0]) + 1;
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
            Booking? existingBooking = bookings.Find(b => b.ReservationNumber == booking.ReservationNumber);
            if (existingBooking is not null)
            {
                throw new EntityAlreadyExists<Booking, int>(booking.ReservationNumber);
            }
            reservationNumber++;

            csvReader.WriteEntityInformation(booking.ToCSV());

            csvReader.WriteRelationshipInformation(booking.ReservationNumber.ToString(), booking.Flights.Select(f => f.Number).ToList());

            bookings.Add(booking);

            return booking;
        }

        public Booking Save(BookingDTO booking)
        {
            var createdBooking = new Booking(reservationNumber, booking.Flights, booking.FlightClasses, booking.Type, booking.Passenger, booking.TotalPrice);

            reservationNumber++;

            csvReader.WriteEntityInformation(createdBooking.ToCSV());

            csvReader.WriteRelationshipInformation(createdBooking.ReservationNumber.ToString(), booking.Flights.Select(f => f.Number).ToList());

            bookings.Add(createdBooking);

            return createdBooking;
        }

        public Booking? Update(int reservationNumber, Booking booking)
        {
            if (bookings.Find(b => b.ReservationNumber == reservationNumber) == null)
            {
                throw new EntityNotFound<Booking, int>(reservationNumber);
            }
            csvReader.UpdateEntityInformation(reservationNumber.ToString(), booking.ToCSV());
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
}
