namespace AirportBooking.ConsoleInputHandler;

public class ConsoleInputHandler : IConsoleInputHandler
{
    public string GetNotEmptyString(string message)
    {
        var input = "";
        while (input is "")
        {
            Console.WriteLine(message);
            input = Console.ReadLine() ?? "";
            if (input is "")
            {
                Console.WriteLine($"Invalid input. Please try again");
            }
        }
        return input;
    }

    public int GetInteger()
    {
        var hasChosenNumber = int.TryParse(Console.ReadLine(), out var number);
        while (!hasChosenNumber)
        {
            Console.WriteLine("The value provided is not a number. Please try again");
            hasChosenNumber = int.TryParse(Console.ReadLine(), out number);
        }
        return number;
    }

    public decimal GetDecimal()
    {
        var hasTypedNumber = decimal.TryParse(Console.ReadLine(), out var decimalNumber);
        while (!hasTypedNumber)
        {
            Console.WriteLine("The value provided is not a number. Please try again");
            hasTypedNumber = decimal.TryParse(Console.ReadLine(), out decimalNumber);
        }
        return decimalNumber;
    }
}
