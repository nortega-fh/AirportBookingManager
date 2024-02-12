namespace AirportBooking.Views;

public abstract class BaseConsoleView
{
    public static void ClearOnInput()
    {
        Console.ReadLine();
        Console.Clear();
    }

    public static string? GetOptionalValue(string message)
    {
        var entry = GetValue(message, true);
        return entry.Equals(string.Empty) ? null : entry;
    }

    public static float? GetOptionalFloatValue(string message)
    {
        var answer = GetValue(message, true);
        if (answer.Equals(string.Empty)) { return null; }
        return float.Parse(answer.Replace(".", ","));
    }

    public static string GetValue(string message, bool isSkippable = false)
    {
        Console.WriteLine(message);
        return ObtainAnswer(isSkippable);
    }

    public static T GetValue<T>(string message, IList<T> options)
    {
        var answer = GetAnswerFromOptionList(message, options, false);
        var optionChosen = int.Parse(answer) - 1;
        return options[optionChosen];
    }

    public static bool GetOptionalValue<T>(string message, IList<T> options, out T? result)
    {
        var answer = GetAnswerFromOptionList(message, options, true);
        var skippedOption = answer.Equals(string.Empty);
        result = skippedOption ? default : options[int.Parse(answer) - 1];
        return !skippedOption;
    }

    private static string GetAnswerFromOptionList<T>(string message, IList<T> options, bool isSkippable)
    {
        Console.WriteLine(message);
        PrintOptions(options);
        var answer = ObtainAnswer(isSkippable);
        return answer;
    }

    private static void PrintOptions<T>(IList<T> options)
    {
        for (var i = 0; i < options.Count; i++)
            Console.WriteLine($"{i + 1}. {options[i]}");
    }

    private static string ObtainAnswer(bool isSkippable = false)
    {
        string answer = Console.ReadLine() ?? "";
        while (answer.Equals("") && !isSkippable)
        {
            Console.WriteLine("Invalid input, please try again.");
            answer = Console.ReadLine() ?? "";
        }
        Console.Clear();
        return answer;
    }
}
