using AirportBooking.Constants;
using AirportBooking.Enums;
using AirportBooking.Exceptions;
using AirportBooking.Models;

namespace AirportBooking.Validators.CsvValidators;

public class UserCsvValidator : CsvValidatorBase, IUserCsvValidator
{
    const int minLineLenght = 3;
    public string[] Validate(string csvLine)
    {
        var data = csvLine.Split(',');
        if (data.Length < minLineLenght)
        {
            throw new EntityReadingException<User>("User data is not complete");
        }
        if (IsStringInvalid(data[0]) || IsStringInvalid(data[1]))
        {
            throw new InvalidAttributeException<string>("Username", EntityValueRestriction.Restrictions[Restriction.Field]);
        }
        if (IsEnumInvalid<UserRole>(data[2]))
        {
            throw new InvalidAttributeException<UserRole>("User role", EntityValueRestriction.Restrictions[Restriction.UserRole]);
        }
        return data;
    }
}
