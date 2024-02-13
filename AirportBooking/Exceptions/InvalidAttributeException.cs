namespace AirportBooking.Exceptions;

public class InvalidAttributeException<T>(string attributeName, string[] constraints) : Exception($"""
        * {attributeName}:
            * Type: {typeof(T).Name}
            * Constraint: {string.Join(",", constraints)}
        """);
