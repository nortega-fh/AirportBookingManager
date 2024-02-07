using AirportBooking.Enums;
using AirportBooking.Exceptions;
using AirportBooking.Models;

namespace AirportBooking.Validators.CsvValidators;

public class UserCsvValidator : ICsvValidator
{
    const int minLineLenght = 3;
    public string[] Validate(string csvLine)
    {
        var data = csvLine.Split(',');
        if (data.Length < minLineLenght
            || IsStringInvalid(data[0])
            || IsStringInvalid(data[1])
            || !Enum.TryParse<UserRole>(data[2], true, out var _))
        {
            throw new EntitySerializationException<User>("User data is not complete");
        }
        return data;
    }

    private static bool IsStringInvalid(string? data)
    {
        return data is null || data.Equals(string.Empty);
    }
}
