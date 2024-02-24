namespace AirportBooking.Validators
{
    public interface IUserCsvValidator
    {
        string[] Validate(string csvLine);
    }
}