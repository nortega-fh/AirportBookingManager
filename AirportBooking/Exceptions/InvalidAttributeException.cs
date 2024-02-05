namespace AirportBooking;

public class InvalidAttributeException<T>(T attribute, string[] constraints) : Exception($"""
        * {nameof(attribute).Replace(nameof(attribute)[0], char.ToUpper(nameof(attribute)[0]))}:
            * Type: {typeof(T).Name}
            * Constraint: {string.Join(",", constraints)}
        """);
