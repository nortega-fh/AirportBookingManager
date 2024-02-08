using AirportBooking.Models;

namespace AirportBooking.Serializers.ConsoleSerializers;

public class UserConsoleSerializer : IConsoleSerializer<User>
{
    public void PrintToConsole(User user)
    {
        Console.WriteLine($"User: {user.Username}");
    }
}
