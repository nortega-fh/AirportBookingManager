using AirportBooking.Constants;
using AirportBooking.Exceptions;
using AirportBooking.Models;

namespace AirportBooking.Validators.CsvValidators;

public class FlightCsvValidator : CsvValidatorBase, IFlightCsvValidator
{
    const int minLineLength = 10;
    public string[] Validate(string csvLine)
    {
        var data = csvLine.Split(",");
        if (data.Length < minLineLength)
        {
            throw new EntityReadingException<Flight>($"""
                The data for the flight is incomplete, there should be at least {minLineLength} fields on each line
                Error in line
                {csvLine}
                """);
        }
        if (IsStringInvalid(data[0]))
        {
            throw new InvalidAttributeException<string>("Flight number", EntityValueRestriction.Restrictions[Restriction.Field]);
        }
        for (int i = 1; i <= 3; i++)
        {
            if (data[i] is not CsvValueSkipper.ValueSkipper && !decimal.TryParse(data[i].Replace(".", ","), out var _))
            {
                throw new InvalidAttributeException<decimal>("Flight price", EntityValueRestriction.Restrictions[Restriction.Price]);
            }
        }
        if (IsStringInvalid(data[4]))
        {
            throw new InvalidAttributeException<string>("Origin country", EntityValueRestriction.Restrictions[Restriction.Field]);
        }
        if (IsStringInvalid(data[5]))
        {
            throw new InvalidAttributeException<string>("Destination country", EntityValueRestriction.Restrictions[Restriction.Field]);
        }
        if (!DateTime.TryParse(data[6], out var _))
        {
            throw new InvalidAttributeException<DateTime>("Departure date", EntityValueRestriction.Restrictions[Restriction.Date]);
        }
        if (!DateTime.TryParse(data[7], out var _))
        {
            throw new InvalidAttributeException<DateTime>("Arrival date", EntityValueRestriction.Restrictions[Restriction.Date]);
        }
        if (IsStringInvalid(data[8]))
        {
            throw new InvalidAttributeException<string>("Origin airport", EntityValueRestriction.Restrictions[Restriction.Field]);
        }
        if (IsStringInvalid(data[9]))
        {
            throw new InvalidAttributeException<string>("Destination airport", EntityValueRestriction.Restrictions[Restriction.Field]);
        }
        return data;
    }
}
