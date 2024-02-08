namespace AirportBooking.Serializers;

public interface IConsoleSerializer<T>
{
    public void PrintToConsole(T entity);
}
