namespace AirportBooking.Validators;

public interface ICsvValidator
{
    string[] Validate(string csvLine);
}
