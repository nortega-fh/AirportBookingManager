using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportBooking
{
    public class Interface
    {
        public User? CurrentUser { get; set; }

        private readonly UserRepository userFileRepository;

        public Interface() 
        {
            userFileRepository = new();
        }

        private void ShowPassengerMenu()
        {
            while (true)
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
                        return;
                }
                Console.Clear();
            }
        }

        private void ShowManagerMenu()
        {
            while (true)
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
                        return;
                }
                Console.Clear();
            }
        }
        
        private void ShowLoginMenu()
        {
            while(CurrentUser is null)
            {
                Console.WriteLine("""
                    - ############################################## -
                    
                                        Login

                    - ############################################## -
                    """);
                Console.WriteLine("Please write your username:");
                string? username = Console.ReadLine();

                Console.WriteLine("Please write your password:");
                string? password = Console.ReadLine();

                try
                {
                    CurrentUser = userFileRepository.Login(username ?? "", password ?? "");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Could not login: {ex.Message}");
                }
            }
            Console.Clear();
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

        private void ShowRegisterMenu()
        {
            while(CurrentUser is null)
            {
                Console.WriteLine("""
                    - ############################################## -
                    
                                        Register

                    - ############################################## -
                    """);
                Console.WriteLine("Please write your username:");
                string? username = Console.ReadLine();

                Console.WriteLine("Please write your password:");
                string? password = Console.ReadLine();

                try
                {
                    CurrentUser = userFileRepository.Save(new User(username ?? "", password ?? "", UserRole.Passenger.ToString()));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.Clear();
            ShowPassengerMenu();
        }

        public void ShowMainMenu()
        {
            while (true)
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
                        ShowLoginMenu();
                        break;
                    case "2":
                        ShowRegisterMenu();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid input, please try again");
                        Console.ReadLine();
                        break;
                };
                Console.Clear();
            }
        }
    }
}
