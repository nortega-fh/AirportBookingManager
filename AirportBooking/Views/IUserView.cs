using AirportBooking.Models;

namespace AirportBooking.Views;

public interface IUserView : IVIew<User>
{
    User? Login();
    User? Register();
}
