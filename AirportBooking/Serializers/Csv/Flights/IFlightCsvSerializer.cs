using AirportBooking.Models.Flights;

namespace AirportBooking.Serializers.Csv.Flights;

public interface IFlightCsvSerializer
{
    Flight FromCsv(string csvLine);
}