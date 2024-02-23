namespace AirportBooking.Validators.CsvValidators
{
    public interface IFlightCsvValidator
    {
        string[] Validate(string csvLine);
    }
}