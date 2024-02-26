using AirportBooking.Models.Users;

namespace AirportBooking.Serializers.Csv.Users;

public interface IUserCsvSerializer
{
    User From(string csvLine);
    string To(User user);
}
