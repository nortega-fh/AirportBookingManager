namespace AirportBooking.Views;

public class ManagerView : RoleConsoleView
{
    public override void ShowMenu()
    {
        while (UserSession.GetLoggedUser() is not null)
        {
            var action = GetValue("Manager Menu", ["Search bookings", "Upload flights", "Logout"]);
            switch (action)
            {
                case "Logout":
                    UserSession.LogOutUser();
                    Console.WriteLine("Logged out succesfully");
                    break;
            }
        }
        ClearOnInput();
    }
}
