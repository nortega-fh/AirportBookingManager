using AirportBooking.Enums;
using AirportBooking.Models;
using AirportBooking.Repositories;

namespace AirportBooking;

public class UserView(UserRepository userRepository)
{
    private readonly UserRepository userRepository = userRepository;

    public User? ShowLoginMenu()
    {
        User? foundUser = null;
        while (foundUser is null)
        {
            Console.WriteLine("""
                - ############################################## -
                
                                    Login

                - ############################################## -
                """);
            Console.WriteLine("Please write your username:");
            var username = Console.ReadLine();

            Console.WriteLine("Please write your password:");
            var password = Console.ReadLine();

            try
            {
                foundUser = userRepository.Login(username ?? "", password ?? "");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not login: {ex.Message}");
            }
        }
        Console.Clear();
        return foundUser;
    }

    public User? ShowRegisterMenu()
    {
        User? foundUser = null;
        while (foundUser is null)
        {
            Console.WriteLine("""
                - ############################################## -
                
                                    Register

                - ############################################## -
                """);
            Console.WriteLine("Please write your username:");
            var username = Console.ReadLine();

            Console.WriteLine("Please write your password:");
            var password = Console.ReadLine();

            try
            {
                foundUser = userRepository.Save(new User
                {
                    Username = username ?? "",
                    Password = password ?? "",
                    Role = UserRole.Passenger
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        Console.Clear();
        return foundUser;
    }
}
