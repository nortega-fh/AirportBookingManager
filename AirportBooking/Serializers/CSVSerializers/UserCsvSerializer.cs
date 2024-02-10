using AirportBooking.Enums;
using AirportBooking.Models;
using AirportBooking.Validators;
using AirportBooking.Validators.CsvValidators;

namespace AirportBooking.Serializers.CSVSerializers;

public class UserCsvSerializer : ICSVSerializer<User>
{
    private readonly IValidator<User> _entityValidator;
    private readonly UserCsvValidator _csvValidator = new();

    public UserCsvSerializer(IValidator<User> validator)
    {
        _entityValidator = validator;
    }
    public User FromCsv(string csvLine)
    {
        var data = _csvValidator.Validate(csvLine);
        var (username, password, role) = (data[0], data[1], Enum.Parse<UserRole>(data[2], true));
        return new User
        {
            Username = username,
            Password = password,
            Role = role,
        };
    }

    public string ToCsv(User user)
    {
        _entityValidator.Validate(user);
        return string.Join(",", [user.Username, user.Password, user.Role]);
    }
}
