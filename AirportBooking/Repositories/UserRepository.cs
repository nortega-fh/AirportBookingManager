using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportBooking.Lib;

namespace AirportBooking
{
    internal class UserRepository : IFileRepository<string, User> 
    {
        private readonly List<User> userList = [];

        private readonly CSVReader userCSVReader;

        public UserRepository()
        {
            userCSVReader = new("users");
            LoadUsers();
        }

        private void AddUserFromCSVLine(string line)
        {
            string[] data = line.Split(',');
            (string username, string password, string role) = (data[0].Trim(), data[1].Trim(), data[2].Trim());
            userList.Add(new User(username, password, role));
        }

        private void LoadUsers()
        {
            List<string> readUsers = userCSVReader.ReadEntityCSV().ToList();
            readUsers.ForEach(AddUserFromCSVLine);
        }

        public IEnumerable<User> FindAll()
        {
            return userList;
        }

        public User? Find(string username)
        {
            return userList.Find(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public User Login(string username, string password)
        {
            User? user = userList.Find(user => user.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && user.Password.Equals(password, StringComparison.OrdinalIgnoreCase));
            if(user is null)
            {
                throw new EntityNotFound<User, string>(username);
            }
            return user;
        }

        public User Save(User user)
        {
            User? existingUser = userList.Find(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase));
            if (existingUser is not null)
            {
                throw new EntityAlreadyExists<User, string>(user.Username);
            }
            userCSVReader.WriteEntity(user.ToCSV());
            return user;
        }

        public User? Update(string username, User value)
        {
            throw new NotImplementedException();
        }

        public void Delete(string username)
        {
            throw new NotImplementedException();
        }
    }
}
