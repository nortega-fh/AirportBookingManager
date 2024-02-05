using AirportBooking.DTOs;
using AirportBooking.Enums;
using AirportBooking.Models;
using AirportBooking.Repositories;
using AirportBooking.Views;

namespace AirportBooking;

public class Interface(UserView userView, FlightsView flightView, BookingRepository repository)
{
    private User? CurrentUser { get; set; }
    private readonly FlightsView flightView = flightView;
    private readonly UserView userView = userView;
    private readonly BookingRepository repository = repository;

    private void ShowAllBookings()
    {
        Console.Clear();
        repository.FindAll().ToList().ForEach(Console.WriteLine);
        Console.ReadLine();
        Console.Clear();
    }

    private void ShowUserBookings(User user)
    {
        Console.Clear();
        repository.FindAll().Where(b => b.MainPassenger.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)).ToList().ForEach(Console.WriteLine);
        Console.ReadLine();
        Console.Clear();
    }

    private Booking? FindBooking()
    {
        Booking? booking = null;
        if (CurrentUser == null)
        {
            return null;
        }
        Console.WriteLine("Please indicate the reservation number of the booking: ");
        string reservationNumber = Console.ReadLine() ?? "";
        while (reservationNumber is "")
        {
            Console.WriteLine("Invalid input, please try again.");
            reservationNumber = Console.ReadLine() ?? "";
        }
        try
        {
            booking = repository.Find(int.Parse(reservationNumber));
        }
        catch (Exception ex) when (ex is FormatException or OverflowException)
        {

            Console.WriteLine("Invalid number, please try again");
        }
        return booking;
    }

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
        var flightParameters = flightView.GetFlightFilters(bookingType);
        flightView.ShowFlights(flightParameters);

        var bookedFlights = new List<Flight>();
        var flightClasses = new List<FlightClass>();
        var totalPrice = 0f;

        Console.WriteLine("Departure flight:");
        Flight? departureFlight = flightView.GetFlight();
        while (departureFlight is null)
        {
            Console.WriteLine("Invalid input, please try again");
            departureFlight = flightView.GetFlight();
        }

        bookedFlights.Add(departureFlight);
        FlightClass departureClass = flightView.ChooseFlightPrice(departureFlight);
        flightClasses.Add(departureClass);
        totalPrice += departureFlight.Prices[departureClass];

        if (bookingType is BookingType.RoundTrip && flightParameters.ReturnDate is not null)
        {
            var returnParameters = flightParameters with { OriginCountry = flightParameters.DestinationCountry, DestinationCountry = flightParameters.OriginCountry, DepartureDate = flightParameters.ReturnDate.Value };

            flightView.ShowFlights(returnParameters);

            Console.WriteLine("Return flight:");
            Flight? returnFlight = flightView.GetFlight();
            while (returnFlight is null)
            {
                Console.WriteLine("Invalid input, please try again");
                returnFlight = flightView.GetFlight();
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

    private void ShowBookingManager(Booking booking)
    {
        Console.WriteLine("""
            What do you want to do with this booking?
            1. Edit
            2. Delete
            """);
        string action = Console.ReadLine() ?? "";
        while (action is not ("1" or "2"))
        {
            Console.WriteLine("Invalid input. Please try again");
            action = Console.ReadLine() ?? "";
        }
        Console.Clear();
        if (action is "1")
        {
            EditBooking(booking);
        }
        else
        {
            DeleteBooking(booking);
        }
    }

    private void EditBooking(Booking booking)
    {
        bool isEditing = true;
        while (isEditing)
        {
            Console.WriteLine("""
            1. Modify booking's type.
            2. Modify booking's flights.
            3. Modify booking flights' classes
            4. Confirm changes
            """);
            string answer = Console.ReadLine() ?? "";
            Console.Clear();
            switch (answer)
            {
                case "1":
                    booking = EditBookingType(booking);
                    break;
                case "2":
                    booking = EditBookingFlights(booking);
                    break;
                case "3":
                    booking = EditBookingFlightsClasses(booking);
                    break;
                case "4":
                    isEditing = false;
                    break;
            }
        }
        try
        {
            repository.Update(booking.ReservationNumber, booking);
        }
        catch (EntityNotFound<Booking, string> ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            Console.Clear();
        }
    }

    private Booking EditBookingType(Booking booking)
    {
        Booking modifiedBooking = new(booking.ReservationNumber, booking.Flights, booking.FlightClasses, booking.BookingType, booking.MainPassenger, booking.Price);
        BookingType changedBookingType = booking.BookingType.Equals(BookingType.RoundTrip) ? BookingType.OneWay : BookingType.RoundTrip;
        modifiedBooking.BookingType = changedBookingType;
        if (changedBookingType == BookingType.RoundTrip)
        {
            Console.WriteLine("Since you are changing the flight to Round Trip, you need to add a return flight:");
            flightView.ShowFlights(flightView.GetFlightFilters());
            Flight? flight = flightView.GetFlight();
            while (flight is null)
            {
                Console.WriteLine("No flight found. Please try again");
                flight = flightView.GetFlight();
            }
            var flightClass = flightView.ChooseFlightPrice(flight);
            modifiedBooking.FlightClasses.Add(flightClass);
            modifiedBooking.Flights.Add(flight);
        }
        else
        {
            modifiedBooking.Flights = modifiedBooking.Flights.Take(modifiedBooking.Flights.Count - 1).ToList();
            modifiedBooking.FlightClasses = modifiedBooking.FlightClasses.Take(modifiedBooking.FlightClasses.Count - 1).ToList();
        }
        modifiedBooking.UpdatePrice();
        return ConfirmUpdate(modifiedBooking, booking);
    }

    private Booking EditBookingFlights(Booking booking)
    {
        Booking modifiedBooking = new(booking.ReservationNumber, booking.Flights, booking.FlightClasses, booking.BookingType, booking.MainPassenger, booking.Price);
        var modifiedFlights = new List<Flight>();
        var modifiedClasses = new List<FlightClass>();
        flightView.ShowFlights(flightView.GetFlightFilters());
        Flight? departureFlight = flightView.GetFlight();
        while (departureFlight is null)
        {
            Console.WriteLine("Couldn't find the specified flight. Please try again");
            departureFlight = flightView.GetFlight();
        }
        modifiedClasses.Add(flightView.ChooseFlightPrice(departureFlight));
        modifiedFlights.Add(departureFlight);
        if (modifiedBooking.BookingType.Equals(BookingType.RoundTrip))
        {
            Console.WriteLine("Choose the return flight of the following list:");
            flightView.ShowFlights(flightView.GetFlightFilters());
            Flight? returnFlight = flightView.GetFlight();
            while (returnFlight is null)
            {
                Console.WriteLine("Couldn't find the specified flight. Please try again");
                returnFlight = flightView.GetFlight();
            }
            modifiedClasses.Add(flightView.ChooseFlightPrice(returnFlight));
            modifiedFlights.Add(returnFlight);
        }
        modifiedBooking.UpdatePrice();
        return ConfirmUpdate(modifiedBooking, booking);
    }

    private Booking EditBookingFlightsClasses(Booking booking)
    {
        Booking modifiedBooking = new(booking.ReservationNumber, booking.Flights, booking.FlightClasses, booking.BookingType, booking.MainPassenger, booking.Price);
        FlightClass[] allClasses = [FlightClass.Economy, FlightClass.Business, FlightClass.FirstClass];
        for (var i = 0; i < booking.Flights.Count; i++)
        {
            Console.WriteLine($"""
                    Select the new class for the following flight:
                    {booking.Flights[i]}
                    """);
            FlightClass[] availableOptions = allClasses.Where(c => !c.Equals(modifiedBooking.FlightClasses[i])).ToArray();
            for (var j = 0; j < availableOptions.Length; j++)
            {
                Console.WriteLine($"{j + 1}. {availableOptions[j]}");
            }
            string chosenClass = Console.ReadLine() ?? "";
            while (chosenClass is "" || int.Parse(chosenClass) < 0 || int.Parse(chosenClass) > availableOptions.Length)
            {
                Console.WriteLine("Invalid input. Please try again");
                chosenClass = Console.ReadLine() ?? "";
            }
            modifiedBooking.FlightClasses[i] = availableOptions[int.Parse(chosenClass) - 1];
        }
        modifiedBooking.UpdatePrice();
        return ConfirmUpdate(modifiedBooking, booking);
    }

    private Booking ConfirmUpdate(Booking modifiedBooking, Booking booking)
    {
        Console.WriteLine($"""
                The modified booking will be as follows:
                {modifiedBooking}
                Do you wish to proceed with the update?
                1. Yes
                2. No
                """);
        if (Console.ReadLine() is "1")
        {
            Console.WriteLine($"Booking modified succesfully to: {modifiedBooking}");
            Console.Clear();
            return modifiedBooking;
        }
        Console.Clear();
        return booking;
    }

    private void DeleteBooking(Booking booking)
    {
        Console.WriteLine($"""
                Are you sure you want to delete the booking with reservation number {booking.ReservationNumber}?
                1. Yes
                2. No
                """);
        string answer = Console.ReadLine() ?? "";
        while (answer is not ("1" or "2"))
        {
            Console.WriteLine("Invalid input. Please try again");
            answer = Console.ReadLine() ?? "";
        }
        if (answer is "1")
        {
            try
            {
                repository.Delete(booking.ReservationNumber);
            }
            catch (EntityNotFound<Booking, string> ex)
            {

                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
        Console.Clear();
    }

    private void ShowBookingManagerMenu()
    {
        bool exit = false;
        while (!exit && CurrentUser is not null)
        {
            Console.WriteLine("""
                - ############################################## -
                
                                 Booking Manager
                
                - ############################################## -

                1. See all bookings
                2. Search booking by reservation number
                """);

            string answer = Console.ReadLine() ?? "";
            while (answer is not ("1" or "2"))
            {
                Console.WriteLine("Invalid input, please try again.");
                answer = Console.ReadLine() ?? "";
            }
            Console.Clear();
            switch (answer)
            {
                case "1":
                    if (CurrentUser.Role is UserRole.Manager)
                    {
                        ShowAllBookings();
                        break;
                    }
                    ShowUserBookings(CurrentUser);
                    Console.WriteLine("""
                        Do you wish to edit or delete one of these bookings?
                        1. Yes
                        2. No
                        """);
                    answer = Console.ReadLine() ?? "";
                    Console.Clear();
                    if (answer is "1")
                    {
                        Console.Clear();
                        Booking? selectedBooking = FindBooking();
                        while (selectedBooking is null)
                        {
                            Console.WriteLine("Reservation number not found. Please try again");
                            selectedBooking = FindBooking();
                        }
                        ShowBookingManager(selectedBooking);
                    }
                    break;
                case "2":
                    Console.Clear();
                    Booking? foundBooking = FindBooking();
                    while (foundBooking is null)
                    {
                        Console.WriteLine("Reservation number not found. Please try again");
                        foundBooking = FindBooking();
                    }
                    ShowBookingManager(foundBooking);
                    break;
            }
        }
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

        var searchParameters = flightView.GetFlightFilters(bookingType);

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
                    ShowBookingManagerMenu();
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