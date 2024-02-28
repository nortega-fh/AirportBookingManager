namespace AirportBooking.ConsoleInputHandler;

public interface IConsoleInputHandler
{
    string GetNotEmptyString(string message);
    int GetInteger();
    decimal GetDecimal();
}