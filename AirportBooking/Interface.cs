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
        
        
        private void ShowLoginMenu()
        {
            Console.WriteLine("Login menu");
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
                2. Exit
                """);
                string? answer = Console.ReadLine();
                Console.Clear();
                switch (answer)
                {
                    case "1":
                        ShowLoginMenu();
                        break;
                    case "2":
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
