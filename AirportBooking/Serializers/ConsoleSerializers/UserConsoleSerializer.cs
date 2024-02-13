using AirportBooking.Models;

namespace AirportBooking.Serializers.ConsoleSerializers;

public class UserConsoleSerializer
{
    public void PrintToConsole(User user)
    {
        Console.WriteLine($"User: {user.Username}");
    }
}
