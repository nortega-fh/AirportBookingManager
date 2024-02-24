namespace AirportBooking.Validators
{
    public interface IBookingCsvValidator
    {
        string[] Validate(string csvLine);
    }
}