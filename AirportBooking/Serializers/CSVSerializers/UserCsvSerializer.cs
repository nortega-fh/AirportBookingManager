using AirportBooking.Models;

namespace AirportBooking.Serializers.CSVSerializers;

public class UserCsvSerializer : ICSVSerializer<User>
{
    public User FromCsv(string csvLine)
    {
        throw new NotImplementedException();
    }

    public string ToCsv(User obj)
    {
        throw new NotImplementedException();
    }
}
