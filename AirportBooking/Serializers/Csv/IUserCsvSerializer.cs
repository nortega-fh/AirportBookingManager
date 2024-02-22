using AirportBooking.Models;

namespace AirportBooking.Serializers.Csv;

public interface IUserCsvSerializer
{
    User From(string csvLine);
    string To(User user);
}
