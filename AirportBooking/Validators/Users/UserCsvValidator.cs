using AirportBooking.Constants;
using AirportBooking.Exceptions;
using AirportBooking.Models.Users;

namespace AirportBooking.Validators.Users;

public class UserCsvValidator : IUserCsvValidator
{
    const int minLineLenght = 3;
    public string[] Validate(string csvLine)
    {
        var data = csvLine.Split(',');
        if (data.Length < minLineLenght)
        {
            throw new EntityReadingException<User>("User data is not complete");
        }
        if (IsUsernameOrPasswordInvalid(data))
        {
            throw new InvalidAttributeException<string>("Username", EntityValueRestriction.Restrictions[Restriction.Field]);
        }
        if (!Enum.TryParse<UserRole>(data[2], true, out var _))
        {
            throw new InvalidAttributeException<UserRole>("User role", EntityValueRestriction.Restrictions[Restriction.UserRole]);
        }
        return data;
    }

    private bool IsUsernameOrPasswordInvalid(string[] data)
    {
        return data[0] is null
            || data[0] is "null"
            || data[0].Equals(string.Empty)
            || data[1] is null
            || data[0] is "null"
            || data[1].Equals(string.Empty);
    }
}
