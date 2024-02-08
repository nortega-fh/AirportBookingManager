using AirportBooking.Models;

namespace AirportBooking.Views;

public interface IUserView : IView<User>
{
    User? Login();
    User? Register();
}
