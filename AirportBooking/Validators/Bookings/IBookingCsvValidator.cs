namespace AirportBooking.Validators.Bookings
{
    public interface IBookingCsvValidator
    {
        string[] Validate(string csvLine);
    }
}