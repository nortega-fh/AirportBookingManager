namespace AirportBooking.Views;

public abstract class ConsoleViewBase
{
    public static void ClearOnInput()
    {
        Console.ReadLine();
        Console.Clear();
    }

    public static string GetValue(string message)
    {
        Console.WriteLine(message);
        return ObtainAnswer();
    }

    public static string? GetOptionalValue(string message)
    {
        Console.WriteLine(message);
        var entry = ObtainAnswer(true);
        return entry.Equals(string.Empty) ? null : entry;
    }

    public static T GetValue<T>(string message, IList<T> options)
    {
        Console.WriteLine(message);
        PrintOptions(options);
        var answer = ObtainAnswer();
        var optionChosen = int.Parse(answer) - 1;
        return options[optionChosen];
    }

    public static bool GetOptionalValue<T>(string message, IList<T> options, out T? result)
    {
        Console.WriteLine(message);
        PrintOptions(options);
        var answer = ObtainAnswer(true);
        var skippedOption = answer.Equals(string.Empty);
        result = skippedOption ? default : options[int.Parse(answer) - 1];
        return !skippedOption;
    }

    public static float? GetOptionalFloatValue(string message)
    {
        Console.WriteLine(message);
        var answer = ObtainAnswer(true);
        if (answer.Equals(string.Empty)) { return null; }
        return HandleFloat(answer);
    }

    private static float HandleFloat(string value)
    {
        return float.Parse(value.Replace(".", ","));
    }

    private static void PrintOptions<T>(IList<T> options)
    {
        for (var i = 0; i < options.Count; i++)
            Console.WriteLine($"{i + 1}. {options[i]}");
    }

    private static string ObtainAnswer(bool skippable = false)
    {
        string answer = Console.ReadLine() ?? "";
        while (answer.Equals("") && !skippable)
        {
            Console.WriteLine("Invalid input, please try again.");
            answer = Console.ReadLine() ?? "";
        }
        return answer;
    }
}
