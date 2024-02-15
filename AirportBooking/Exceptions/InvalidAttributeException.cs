namespace AirportBooking.Exceptions;

public class InvalidAttributeException<T>(string attributeName, string[] constraints) : SerializationException($"""
        * {attributeName}:
            * Type: {typeof(T).Name}
            * Constraint: {string.Join(",", constraints)}
        """);
