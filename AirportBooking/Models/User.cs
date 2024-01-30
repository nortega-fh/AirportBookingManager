using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportBooking
{
    public class User(string username, string password, string role)
    {
        public string Username { get; set; } = username;
        public string Password { get; set; } = password;

        public UserRole Role { get; set; } = Enum.Parse<UserRole>(role, true);

        public string ToCSV() => string.Join(",", [Username, Password, Role.ToString()]);
        
    }
}
