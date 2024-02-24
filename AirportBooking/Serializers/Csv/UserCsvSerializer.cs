using AirportBooking.Models;
using AirportBooking.Validators;

namespace AirportBooking.Serializers.Csv;

public class UserCsvSerializer : IUserCsvSerializer
{
    private readonly IUserCsvValidator _validator;

    public UserCsvSerializer(IUserCsvValidator validator)
    {
        _validator = validator;
    }

    public User From(string csvLine)
    {
        var data = _validator.Validate(csvLine);
        var (username, password, role) = (data[0], data[1], Enum.Parse<UserRole>(data[2], true));
        return new User
        {
            Username = username,
            Password = password,
            Role = role,
        };
    }

    public string To(User user) => string.Join(",", [user.Username, user.Password, user.Role]);

}
