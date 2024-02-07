namespace AirportBooking.Exceptions;

public class EntityAlreadyExists<T, K>(K key) : Exception($"The {typeof(T).Name} with unique identifier '{key}' already exists.");
