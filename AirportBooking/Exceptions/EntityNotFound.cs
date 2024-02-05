namespace AirportBooking;

public class EntityNotFound<T, K>(K key) : Exception($"The {typeof(T).Name} with \"{key}\" doesn't exist");