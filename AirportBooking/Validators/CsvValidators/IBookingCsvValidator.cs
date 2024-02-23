namespace AirportBooking.Validators.CsvValidators
{
    public interface IBookingCsvValidator
    {
        string[] Validate(string csvLine);
    }
}