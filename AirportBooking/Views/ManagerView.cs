using AirportBooking.Views.Controllers;

namespace AirportBooking.Views;

public class ManagerView : RoleConsoleView
{
    private readonly BookingsController _bookingsController;

    public ManagerView(BookingsController controller)
    {
        _bookingsController = controller;
    }

    public override void ShowMenu()
    {
        while (UserSession.GetLoggedUser() is not null)
        {
            const string option1 = "Search bookings";
            const string option2 = "Upload flights";
            const string option3 = "Logout";
            var action = GetValue<string>("Manager Menu", [option1, option2, option3]);
            switch (action)
            {
                case option1:
                    FilterBookings();
                    break;
                case option2:
                    UploadFlights();
                    break;
                case option3:
                    UserSession.LogOutUser();
                    Console.WriteLine("Logged out succesfully");
                    break;
            }
        }
        ClearOnInput();
    }

    private void UploadFlights()
    {
        throw new NotImplementedException();
    }

    private void FilterBookings()
    {
        throw new NotImplementedException();
    }
}
