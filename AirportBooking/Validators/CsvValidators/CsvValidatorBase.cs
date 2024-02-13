using AirportBooking.Constants;

namespace AirportBooking.Validators.CsvValidators;

public abstract class CsvValidatorBase
{

    protected static bool IsOptionalFieldInvalid(string value)
    {
        return !(value is CsvValueSkipper.ValueSkipper || value is not "");
    }

    protected static bool IsStringInvalid(string field)
    {
        return field is null || field.Equals(string.Empty);
    }

    protected static bool IsEnumInvalid<T>(string value) where T : struct
    {
        return !Enum.TryParse<T>(value, true, out var _);
    }
}
