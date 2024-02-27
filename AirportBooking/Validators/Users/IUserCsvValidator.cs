namespace AirportBooking.Validators.Users;

public interface IUserCsvValidator
{
    string[] Validate(string csvLine);
}