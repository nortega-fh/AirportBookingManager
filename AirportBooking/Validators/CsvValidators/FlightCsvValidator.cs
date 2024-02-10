using AirportBooking.Exceptions;
using AirportBooking.Models;

namespace AirportBooking.Validators.CsvValidators;

public class FlightCsvValidator : ICsvValidator
{
    const int minLineLength = 8;
    public string[] Validate(string csvLine)
    {
        var data = csvLine.Split(",");
        var error = new EntitySerializationException<Flight>($"The data for the flight is incomplete");
        if (data.Length < minLineLength)
        {
            throw error;
        }
        var flightPricesInformation = GetFlightPrices(data);
        ValidatePricingInformation([.. flightPricesInformation]);
        var totalPricesForFlight = flightPricesInformation.Length;
        if (data.Length < minLineLength + totalPricesForFlight - 1)
        {
            throw error;
        }
        var (originCountryPosition, destinationCountryPosition) = (totalPricesForFlight + 1, totalPricesForFlight + 2);
        var (departureAirportPosition, destinationAirportPosition) = (totalPricesForFlight + 5, totalPricesForFlight + 6);
        var isAnyStringValueInvalid = IsStringInvalid(data[originCountryPosition])
            || IsStringInvalid(data[destinationCountryPosition])
            || IsStringInvalid(data[departureAirportPosition])
            || IsStringInvalid(data[destinationAirportPosition]);
        if (isAnyStringValueInvalid)
        {
            throw error;
        }
        var (departureDatePosition, arrivalDatePosition) = (totalPricesForFlight + 3, totalPricesForFlight + 4);
        var isAnyDateInvalid = IsDateTimeInvalid(data[departureDatePosition]) || IsDateTimeInvalid(data[arrivalDatePosition]);
        if (isAnyDateInvalid)
        {
            throw error;
        }
        return data;
    }
    private static string[] GetFlightPrices(string[] data)
    {
        return data.Where(pricePair => pricePair.Split(":").Length == 2).ToArray();
    }

    private static void ValidatePricingInformation(string[] prices)
    {
        if (prices.Length == 0)
            throw new EntitySerializationException<Flight>($"There is no price data");
        foreach (var price in prices)
        {
            ValidatePrice(price);
        }
    }

    private static void ValidatePrice(string price)
    {
        var amount = price.Split(":")[1].Replace(".", ",");
        if (!float.TryParse(amount, out float _))
        {
            throw new EntitySerializationException<Flight>($"Value for price is in incorrect format, " +
                $"decimal value should be after a dot (.)");
        }
    }

    private static bool IsDateTimeInvalid(string? value)
    {
        return !DateTime.TryParse(value, out var _);
    }

    private static bool IsStringInvalid(string? value)
    {
        return value is null || value.Equals(string.Empty);
    }
}
