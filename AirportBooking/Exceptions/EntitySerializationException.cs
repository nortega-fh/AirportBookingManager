namespace AirportBooking.Exceptions;

public class EntitySerializationException(string reason) : Exception($"Couldn't serialize: {reason}");
