using AirportBooking.Models;

namespace AirportBooking.Serializers.Csv
{
    public interface IBookingCsvSerializer
    {
        Booking FromCsv(string csvLine);
        string ToCsv(Booking booking);
    }
}