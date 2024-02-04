using AirportBooking.Enums;
using AirportBooking.Helpers;

namespace AirportBooking
{
    public class FlightsView(FlightRepository repository)
    {
        private readonly FlightRepository repository = repository;

        public FlightParameters ObtainFlightFilters(BookingType bookingType = BookingType.OneWay)
        {
            Console.WriteLine("Please write the country of origin:");
            string originCountry = Console.ReadLine() ?? "";
            while (originCountry.Equals(string.Empty))
            {
                Console.WriteLine("Invalid input. Please try again.");
                originCountry = Console.ReadLine() ?? "";
            }

            Console.WriteLine("Please write the destination country:");
            string destinationCountry = Console.ReadLine() ?? "";
            while (destinationCountry.Equals(string.Empty))
            {
                Console.WriteLine("Invalid input. Please try again.");
                destinationCountry = Console.ReadLine() ?? "";
            }

            Console.WriteLine("""
                Please write the departure date in format "YYYY-MM-DD":
                Y - Year number
                M - Month number
                D - Day of month number
                """);
            string date = Console.ReadLine() ?? "";
            while (date.Equals(string.Empty))
            {
                Console.WriteLine("Invalid input. Please try again.");
                date = Console.ReadLine() ?? "";
            }
            DateTime departureDate = DateTime.Parse(date);

            DateTime? returnDate = null;
            if (bookingType == BookingType.RoundTrip)
            {
                Console.WriteLine("""
                Please write the return date in format "YYYY-MM-DD":
                Y - Year number
                M - Month number
                D - Day of month number
                """);
                date = Console.ReadLine() ?? "";
                while (date.Equals(string.Empty))
                {
                    Console.WriteLine("Invalid input. Please try again.");
                    date = Console.ReadLine() ?? "";
                }
                returnDate = DateTime.Parse(date);
            }

            Console.WriteLine("(Optional) Write a preferred departure airport:");
            string? departureAirport = Console.ReadLine();
            if (departureAirport is "") departureAirport = null;

            Console.WriteLine("(Optional) Write a preferred arrival airport:");
            string? arrivalAirport = Console.ReadLine();
            if (arrivalAirport is "") arrivalAirport = null;

            Console.WriteLine("""
                (Optional) Select the class that the flight must include in its options: 
                1. Economy
                2. Business
                3. First class
                """);
            FlightClass? flightClass = Console.ReadLine() switch
            {
                "1" => FlightClass.Economy,
                "2" => FlightClass.Business,
                "3" => FlightClass.FirstClass,
                _ => null
            };

            Console.WriteLine("(Optional) Minimum price for the flight: ");
            string price = Console.ReadLine() ?? "";
            float minPrice = 0f;
            if (!price.Equals(string.Empty)) minPrice = float.Parse(price);

            Console.WriteLine("(Optional) Maximum price for the flight: ");
            price = Console.ReadLine() ?? "";
            float maxPrice = float.MaxValue;
            if (!price.Equals(string.Empty)) maxPrice = float.Parse(price);

            return new FlightParameters(originCountry, destinationCountry, departureDate, returnDate, departureAirport, arrivalAirport, flightClass, minPrice, maxPrice);
        }

        public Flight? ShowFlight(string flightNumber)
        {
            if (flightNumber is "") return null;
            Flight? flight = repository.Find(flightNumber);
            Console.WriteLine(flight is not null ? flight : $"Flight with number {flightNumber} not found");
            return flight;
        }

        public FlightClass ChooseFlightPrice(Flight flight)
        {
            Console.WriteLine(flight.PricesToString());
            Console.WriteLine("""
                Please choose the class with which you want to flight (Default Economy):
                1. Economy
                2. Business
                3. First Class
                """);
            return Console.ReadLine() switch
            {
                "" or "1" => FlightClass.Economy,
                "2" => FlightClass.Business,
                "3" => FlightClass.FirstClass,
                _ => FlightClass.Economy
            };
        }

        public List<Flight> ShowFlights(FlightParameters? parameters = null)
        {
            Console.Clear();
            var flights = parameters is null ? repository.FindAll().ToList() : repository.FindBySearchParameters(parameters).ToList();
            flights.ForEach(Console.WriteLine);
            return flights;
        }
    }
}
