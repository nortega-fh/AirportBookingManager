namespace AirportBooking.Exceptions;

public class EntitySerializationException<T>(string reason) : Exception($"Couldn't serialize {typeof(T).Name}: {reason}");
