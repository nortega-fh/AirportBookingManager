using AirportBooking.Constants;
using AirportBooking.Exceptions;
using AirportBooking.Models;

namespace AirportBooking.Validators.CsvValidators;

public class FlightCsvValidator : IFlightCsvValidator
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
        if (data[0] is null || data[0].Equals(string.Empty))
        {
            throw new InvalidAttributeException<string>("Flight number", EntityValueRestriction.Restrictions[Restriction.Field]);
        }
        for (int i = 1; i <= 3; i++)
        {
            if (data[i] is not "null" && !decimal.TryParse(data[i].Replace(".", ","), out var _))
            {
                throw new InvalidAttributeException<decimal>("Flight price", EntityValueRestriction.Restrictions[Restriction.Price]);
            }
        }
        if (data[4] is null || data[4].Equals(string.Empty))
        {
            throw new InvalidAttributeException<string>("Origin country", EntityValueRestriction.Restrictions[Restriction.Field]);
        }
        if (data[5] is null || data[5].Equals(string.Empty))
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
        if (data[8] is null || data[8].Equals(string.Empty))
        {
            throw new InvalidAttributeException<string>("Origin airport", EntityValueRestriction.Restrictions[Restriction.Field]);
        }
        if (data[9] is null || data[9].Equals(string.Empty))
        {
            throw new InvalidAttributeException<string>("Destination airport", EntityValueRestriction.Restrictions[Restriction.Field]);
        }
        return data;
    }
}
