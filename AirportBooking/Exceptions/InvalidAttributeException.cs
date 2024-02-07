namespace AirportBooking.Exceptions;

public class InvalidAttributeException(string attributeName, string attributeType, string[] constraints) : Exception($"""
        * {attributeName}:
            * Type: {attributeType}
            * Constraint: {string.Join(",", constraints)}
        """);
