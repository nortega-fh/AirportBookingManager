namespace AirportBooking.Exceptions;

public class EntityReadingException<T>(string reason) : SerializationException($"Couldn't serialize {typeof(T).Name}: {reason}");
