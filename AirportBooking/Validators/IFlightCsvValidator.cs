namespace AirportBooking.Validators
{
    public interface IFlightCsvValidator
    {
        string[] Validate(string csvLine);
    }
}