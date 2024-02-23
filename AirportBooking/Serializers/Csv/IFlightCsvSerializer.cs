using AirportBooking.Models;

namespace AirportBooking.Serializers.Csv
{
    public interface IFlightCsvSerializer
    {
        Flight FromCsv(string csvLine);
    }
}