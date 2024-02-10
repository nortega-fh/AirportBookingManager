using AirportBooking.Enums;
using AirportBooking.Exceptions;
using AirportBooking.Models;
using AirportBooking.Views.Builders;
using AirportBooking.Views.Controllers;

namespace AirportBooking.Views;


public class PassengerView : RoleConsoleView
{
    private readonly FlightsController _flightController;
    private readonly BookingsController _bookingController;
    private readonly BookingConsoleBuilder _bookingConsoleBuilder;
    public PassengerView(FlightsController flightController, BookingsController bookingsController)
    {
        _flightController = flightController;
        _bookingController = bookingsController;
        _bookingConsoleBuilder = new(flightController);
    }

    public override void ShowMenu()
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
                    LogoutUser();
                    break;
            }
        }
    }

    private static void LogoutUser()
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
        throw new NotImplementedException();
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
                    break;
                case option2:
                    break;
                case option3:
                    break;
                case option4:
                    isEditing = false;
                    break;
            }
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
