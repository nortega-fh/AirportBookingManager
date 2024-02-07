namespace AirportBooking.Serializers;

public interface IConsoleSerializer<T>
{
    public T GetFromInputs();
    public void PrintToConsole(T entity);
}
