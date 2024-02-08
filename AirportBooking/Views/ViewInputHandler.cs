namespace AirportBooking.Views;

public abstract class ViewInputHandler
{
    public virtual void ClearOnInput()
    {
        Console.ReadLine();
        Console.Clear();
    }
    public virtual T GetValue<T>(IList<T> options)
    {
        PrintOptions(options);
        var answer = ObtainAnswer();
        var optionChosen = int.Parse(answer) - 1;
        return options[optionChosen];
    }

    public virtual string GetValue(string message)
    {
        Console.WriteLine(message);
        return ObtainAnswer();
    }

    private static void PrintOptions<T>(IList<T> options)
    {
        Console.WriteLine("Please choose one of the following options:");
        for (var i = 0; i < options.Count; i++)
            Console.WriteLine($"{i + 1}. {options[i]}");
    }

    private static string ObtainAnswer()
    {
        string answer = Console.ReadLine() ?? "";
        while (answer.Equals(""))
        {
            Console.WriteLine("Invalid input, please try again.");
            answer = Console.ReadLine() ?? "";
        }
        return answer;
    }
}
