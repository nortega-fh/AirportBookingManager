using AirportBooking.Models.Bookings;

namespace AirportBooking.Serializers.Csv.Bookings
{
    public interface IBookingCsvSerializer
    {
        Booking FromCsv(string csvLine);
        string ToCsv(Booking booking);
    }
}