using AirportBooking.Enums;

namespace AirportBooking
{
    public class Interface(UserView userView, FlightsView flightView, BookingRepository repository)
    {
        private User? CurrentUser { get; set; }
        private readonly FlightsView flightView = flightView;
        private readonly UserView userView = userView;
        private readonly BookingRepository repository = repository;

        private Booking? CreateBooking()
        {
            if (CurrentUser is null) return null;

            Console.WriteLine("""
                - ############################################## -
                
                                  Create Booking
                
                - ############################################## -
                
                Select the type of booking you want to create:
                1. One way
                2. Round trip
                3. Go back
                """);
            string answer = Console.ReadLine() ?? "";
            while (answer.Equals(""))
            {
                Console.WriteLine("Invalid input, please try again");
                answer = Console.ReadLine() ?? "";
            }
            if (answer is "3") return null;
            BookingType bookingType = answer switch
            {
                "1" => BookingType.OneWay,
                _ => BookingType.RoundTrip
            };
            var flightParameters = flightView.ObtainFlightFilters(bookingType);
            flightView.ShowFlights(flightParameters);

            var bookedFlights = new List<Flight>();
            var flightClasses = new List<FlightClass>();
            var totalPrice = 0f;

            Console.WriteLine("From the previous list, please select the departure flight by writing the flight number:");
            Flight? departureFlight = flightView.ShowFlight(Console.ReadLine() ?? "");
            while (departureFlight is null)
            {
                Console.WriteLine("Invalid input, please try again");
                departureFlight = flightView.ShowFlight(Console.ReadLine() ?? "");
            }

            bookedFlights.Add(departureFlight);
            FlightClass departureClass = flightView.ChooseFlightPrice(departureFlight);
            flightClasses.Add(departureClass);
            totalPrice += departureFlight.Prices[departureClass];

            if (bookingType is BookingType.RoundTrip && flightParameters.ReturnDate is not null)
            {
                var returnParameters = flightParameters with { OriginCountry = flightParameters.DestinationCountry, DestinationCountry = flightParameters.OriginCountry, DepartureDate = flightParameters.ReturnDate.Value };

                flightView.ShowFlights(returnParameters);

                Console.WriteLine("From the previous list, please select the return flight by writing the flight number:");
                Flight? returnFlight = flightView.ShowFlight(Console.ReadLine() ?? "");
                while (returnFlight is null)
                {
                    Console.WriteLine("Invalid input, please try again");
                    returnFlight = flightView.ShowFlight(Console.ReadLine() ?? "");
                }

                bookedFlights.Add(returnFlight);
                FlightClass returnClass = flightView.ChooseFlightPrice(returnFlight);
                flightClasses.Add(returnClass);
                totalPrice += returnFlight.Prices[returnClass];
            }

            var obtainedBooking = new BookingDTO(bookedFlights, flightClasses, bookingType, CurrentUser, totalPrice);

            Console.WriteLine($"""
                Summary of your booking:

                {obtainedBooking}

                Do you wish to proceed with this booking?
                1. Yes
                2. Cancel
                """);

            if (Console.ReadLine() is not "1") return null;

            Console.Clear();

            return repository.Save(obtainedBooking);
        }

        private void SearchAvailableFlights()
        {
            Console.WriteLine("""
                - ############################################## -
                
                                Flight searcher
                
                - ############################################## -
                """);

            Console.WriteLine("""
                Do you wish to search for a return fligth?
                1. Yes
                2. No
                """);

            var bookingType = Console.ReadLine() is not "1" ? BookingType.OneWay : BookingType.RoundTrip;

            var searchParameters = flightView.ObtainFlightFilters(bookingType);

            Console.WriteLine("Available departure flights:");
            flightView.ShowFlights(searchParameters);
            Console.ReadLine();
            Console.Clear();

            if (bookingType is BookingType.RoundTrip && searchParameters.ReturnDate is not null)
            {
                Console.WriteLine("Available return flights:");
                flightView.ShowFlights(searchParameters with
                {
                    OriginCountry = searchParameters.DestinationCountry,
                    DestinationCountry = searchParameters.OriginCountry,
                    DepartureDate = searchParameters.ReturnDate.Value
                });
                Console.ReadLine();
                Console.Clear();
            }
        }

        public void ShowPassengerMenu()
        {
            while (CurrentUser is not null)
            {
                Console.WriteLine("""
                    - ############################################## -
                    
                                      Main menu

                    - ############################################## -

                    1. Book a new flight.
                    2. Search flights.
                    3. Manage bookings.
                    4. Logout.
                    """);

                string? option = Console.ReadLine();
                Console.Clear();
                switch (option)
                {
                    case "1":
                        CreateBooking();
                        break;
                    case "2":
                        SearchAvailableFlights();
                        break;
                    case "3":
                        break;
                    case "4":
                        CurrentUser = null;
                        break;
                }
            }
            ShowMainMenu();
        }

        public void ShowManagerMenu()
        {
            while (CurrentUser is not null)
            {
                Console.WriteLine("""
                    - ############################################## -
                    
                                      Main menu

                    - ############################################## -

                    1. Manage users.
                    2. Search booking.
                    3. Upload file data.
                    4. Logout.
                    """);

                string? option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        break;
                    case "2":
                        break;
                    case "3":
                        break;
                    case "4":
                        CurrentUser = null;
                        break;
                }
                Console.Clear();
            }
            ShowMainMenu();
        }
        public void ShowMainMenu()
        {
            while (CurrentUser is null)
            {
                Console.WriteLine("""
                - ############################################## -

                Welcome to the Airport's Booking Management system
                To continue please select one of the options below

                - ############################################## -

                1. Login
                2. Register
                3. Exit
                """);
                string? answer = Console.ReadLine();
                Console.Clear();
                switch (answer)
                {
                    case "1":
                        CurrentUser = userView.ShowLoginMenu();
                        break;
                    case "2":
                        CurrentUser = userView.ShowRegisterMenu();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid input, please try again");
                        Console.ReadLine();
                        break;
                };
            }
            switch (CurrentUser.Role)
            {
                case UserRole.Manager:
                    ShowManagerMenu();
                    break;
                case UserRole.Passenger:
                    ShowPassengerMenu();
                    break;
            }
        }
    }
}
