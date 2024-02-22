namespace AirportBooking.Validators.CsvValidators
{
    public interface IUserCsvValidator
    {
        string[] Validate(string csvLine);
    }
}