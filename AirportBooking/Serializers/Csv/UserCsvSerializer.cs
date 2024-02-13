using AirportBooking.Enums;
using AirportBooking.Models;
using AirportBooking.Validators.CsvValidators;

namespace AirportBooking.Serializers.Csv;

public class UserCsvSerializer
{
    private readonly UserCsvValidator _csvValidator = new();

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

    public string ToCsv(User user) => string.Join(",", [user.Username, user.Password, user.Role]);

}
