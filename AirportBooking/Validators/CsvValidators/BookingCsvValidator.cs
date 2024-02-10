using AirportBooking.Enums;
using AirportBooking.Exceptions;
using AirportBooking.Models;

namespace AirportBooking.Validators.CsvValidators;

public class BookingCsvValidator : ICsvValidator
{
    public string[] Validate(string csvLine)
    {
        const int maxLineLength = 6;
        var data = csvLine.Split(",");
        var error = new EntitySerializationException<Booking>($"Incomplete data on line {csvLine}");
        if (data.Length < 2) throw error;
        var bookingType = Enum.Parse<BookingType>(data[1]);
        if (bookingType is BookingType.OneWay && data.Length < maxLineLength - 2) throw error;
        if (bookingType is BookingType.RoundTrip && data.Length < maxLineLength - 1) throw error;
        return data;
    }
}
