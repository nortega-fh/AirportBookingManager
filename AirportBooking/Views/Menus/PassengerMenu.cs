using AirportBooking.Controllers;
using AirportBooking.Enums;
using AirportBooking.Exceptions;
using AirportBooking.Globals;
using AirportBooking.Models;
using AirportBooking.Views.Builders;

namespace AirportBooking.Views.Menus;


public class PassengerMenu : BaseConsoleView, IMenu
{
    private readonly FlightsController _flightController;
    private readonly BookingsController _bookingController;
    private readonly BookingConsoleBuilder _bookingConsoleBuilder;
    public PassengerMenu(FlightsController flightController, BookingsController bookingsController)
    {
        _flightController = flightController;
        _bookingController = bookingsController;
        _bookingConsoleBuilder = new(flightController);
    }

    public void ShowMenu()
    {
        while (UserSession.GetLoggedUser() is not null)
        {
            const string _menuTitle = "Main menu";
            const string option1 = "Book a flight";
            const string option2 = "Search available flights";
            const string option3 = "Manage bookings";
            const string option4 = "Logout";
            var _menuOptions = new string[] { option1, option2, option3, option4 };
            var action = GetValue(_menuTitle, _menuOptions);
            switch (action)
            {
                case option1:
                    BookFlight();
                    break;
                case option2:
                    SearchFlights();
                    break;
                case option3:
                    ManageBookings();
                    break;
                case option4:
                    LogOutUser();
                    break;
            }
        }
    }

    private static void LogOutUser()
    {
        UserSession.LogOutUser();
        Console.WriteLine("Logged out succesfully");
        ClearOnInput();
    }

    void BookFlight()
    {
        if (UserSession.GetLoggedUser() is null) return;
        Console.Clear();
        var booking = _bookingConsoleBuilder
            .SetBookingType()
            .SetFlights()
            .SetFlightClasses()
            .SetPassenger(UserSession.GetLoggedUser()!)
            .GetBooking();
        _bookingController.Create(booking);
        ClearOnInput();
    }

    void SearchFlights()
    {
        var flightParameters = _flightController.ObtainFilterParameters();
        if (flightParameters is null) return;
        var foundFlights = _flightController.SearchByParameters(flightParameters)?.ToList();
        if (foundFlights is null || foundFlights.Count == 0)
        {
            Console.WriteLine("No flights matched your results search. Please try again");
            return;
        }
        foundFlights.ForEach(flight => _flightController.PrintToConsole(flight));
    }

    private void ManageBookings()
    {
        const string viewBookings = "View bookings";
        const string modifyBookings = "Modify a booking";
        const string cancelBooking = "Cancel a booking";
        var bookingManagerActions = new string[] { viewBookings, modifyBookings, cancelBooking };
        var actions = GetValue("Booking Manager", bookingManagerActions);
        switch (actions)
        {
            case viewBookings:
                ViewUserBookings();
                break;
            case modifyBookings:
                ModifyBooking();
                break;
            case cancelBooking:
                CancelBooking();
                break;
        }
    }

    private void CancelBooking()
    {
        try
        {
            var bookings = ShowUserBookings();
            if (bookings is null || bookings.Count < 1)
            {
                Console.WriteLine("There are no bookings for this user");
                return;
            }
            var selectedBookingReservationNumber = GetValue("Please select the booking to cancel",
                [.. bookings.Select(b => b.ReservationNumber)]);
            var booking = _bookingController.Find(selectedBookingReservationNumber)!;
            if (booking.Flights.First().DepartureDate < DateTime.Now)
            {
                Console.WriteLine("Can't cancel a booking that already started");
                return;
            }
            var confirmation = GetValue("Are you sure you want to cancel this booking?", ["Yes", "No"]);
            if (confirmation.Equals("No")) return;
            booking.Status = BookingStatus.Canceled;
            _bookingController.Update(selectedBookingReservationNumber, booking);
        }
        catch (Exception ex) when (ex is EntityNotFound<Booking, int>)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void ModifyBooking()
    {
        if (UserSession.GetLoggedUser() is null) return;
        var _userBookings = ShowUserBookings();
        var bookingToModify = GetValue("Please type the reservation number to modify",
            _userBookings.Select(b => b.ReservationNumber).ToList());
        bool isEditing = true;
        while (isEditing)
        {
            const string option1 = "Edit Type";
            const string option2 = "Edit flights";
            const string option3 = "Edit classes";
            const string option4 = "Go back";
            var action = GetValue("Please indicate how you want to edit this booking:", [option1, option2, option3, option4]);
            switch (action)
            {
                case option1:
                    EditBookingType(bookingToModify);
                    break;
                case option2:
                    EditBookingFlights(bookingToModify);
                    break;
                case option3:
                    EditBookingFlightsClasses(bookingToModify);
                    break;
                case option4:
                    isEditing = false;
                    break;
            }
        }

    }

    private void EditBookingType(int bookingToModify)
    {
        try
        {
            var booking = _bookingController.Find(bookingToModify)!;
            var bookingWithUpdatedType = _bookingConsoleBuilder.SetBookingType().SetFlights().SetFlightClasses().GetBooking();
            booking.BookingType = bookingWithUpdatedType.BookingType;
            booking.Flights = bookingWithUpdatedType.Flights;
            booking.FlightClasses = bookingWithUpdatedType.FlightClasses;
            _bookingController.Update(bookingToModify, booking);
        }
        catch (Exception ex) when (ex is EntityNotFound<Booking, int>)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void EditBookingFlights(int bookingToModify)
    {
        try
        {
            var booking = _bookingController.Find(bookingToModify)!;
            var bookingWithUpdatedFlihgts = _bookingConsoleBuilder.SetFlights(booking.BookingType).GetBooking();
            booking.Flights = bookingWithUpdatedFlihgts.Flights;
            _bookingController.Update(bookingToModify, booking);
        }
        catch (Exception ex) when (ex is EntityNotFound<Booking, int>)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void EditBookingFlightsClasses(int bookingToModify)
    {
        try
        {
            var booking = _bookingController.Find(bookingToModify)!;
            List<FlightClass> updatedClasses = [];
            booking.Flights.ForEach(f =>
            {
                var updatedClass = GetValue($"Select a class for the flight {f.Number}", f.ClassPrices.Keys.ToList());
                updatedClasses.Add(updatedClass);
            });
            booking.FlightClasses = updatedClasses;
            _bookingController.Update(bookingToModify, booking);
        }
        catch (Exception ex) when (ex is EntityNotFound<Booking, int>)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            ClearOnInput();
        }
    }

    private void ViewUserBookings()
    {
        if (UserSession.GetLoggedUser() is null) return;
        ShowUserBookings();
    }

    List<Booking> ShowUserBookings()
    {
        var _userBookings = _bookingController.FindAll().Where(
            booking => booking.MainPassenger!.Username == UserSession.GetLoggedUser()!.Username);
        if (!_userBookings.Any())
        {
            Console.WriteLine("There are no bookings yet");
            ClearOnInput();
            return [];
        }
        foreach (var booking in _userBookings)
        {
            _bookingController.PrintToConsole(booking);
        }
        return _userBookings.ToList();
    }
}
