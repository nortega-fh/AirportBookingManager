using AirportBooking.Models;

namespace AirportBooking.Serializers.CSVSerializers;

public class FlightCsvSerializer : ICSVSerializer<Flight>
{
    public Flight FromCsv(string csvLine)
    {
        throw new NotImplementedException();
    }

    public string ToCsv(Flight obj)
    {
        throw new NotImplementedException();
    }
}
