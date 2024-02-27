namespace AirportBooking.Validators.Flights;

public interface IFlightCsvValidator
{
    string[] Validate(string csvLine);
}