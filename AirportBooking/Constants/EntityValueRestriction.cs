namespace AirportBooking.Constants;

public enum Restriction
{
    Field,
    OptionalField,
    Date,
    Price,
    UserRole,
    BookingType,
    BookingStatus,
    FlightClass,
    OptionalBookingType,
    OptionalBookingStatus,
    OptionalFlightClass
};
public static class EntityValueRestriction
{
    private const string dateFormat = "Format: YYYY:MM:DDTHH:mm:ssZ";
    private const string priceFormat = "Format: decimal number separated by dots ($$.$$)";
    private readonly static string[] required = ["Required"];
    public static Dictionary<Restriction, string[]> Restrictions { get; } = new()
    {
        { Restriction.Field, required },
        { Restriction.OptionalField, ["Values: any value or null"] },
        { Restriction.Date, [.. required, dateFormat] },
        { Restriction.Price, [.. required, priceFormat] },
        { Restriction.UserRole, [.. required, "Values: Passenger, Manager (ignore case)"] },
        { Restriction.BookingType, [.. required, "Values: OneWay, RoundTrip (ignore case)"] },
        { Restriction.BookingStatus, [.. required, "Values: Confirmed, Canceled (ignore case)"] },
        { Restriction.FlightClass, [.. required, "Values: Economy, Business, FirstClass (ignore case)"] },
        { Restriction.OptionalFlightClass, [ "Values: Economy, Business, FirstClass, null (ignore case)"] },
        { Restriction.OptionalBookingType, [ "Values: OneWay, RoundTrip, null (ignore case)" ] },
        { Restriction.OptionalBookingStatus, [ "Values: Confirmed, Canceled, null (ignore case)"] },
    };
}
