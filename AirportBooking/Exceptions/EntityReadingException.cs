namespace AirportBooking.Exceptions;

public class EntityReadingException<T>(string reason) : Exception($"Couldn't serialize {typeof(T).Name}: {reason}");
