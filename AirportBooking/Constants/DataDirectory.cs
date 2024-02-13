namespace AirportBooking.Constants;

public static class DataDirectory
{
    private static readonly string rootPath = Directory.GetParent(Path.Combine("..", "..", "Data"))!.FullName;
    private static readonly string bookingsPath = Path.Combine(rootPath, "bookings");
    private static readonly string flightsPath = Path.Combine(rootPath, "flights");
    private static readonly string usersPath = Path.Combine(rootPath, "users");
    private const string fileExtension = ".csv";

    public static string GetFilePath(string path)
    {
        return Path.Combine(rootPath, path + fileExtension);
    }
    public static string GetRootPath()
    {
        return rootPath + fileExtension;
    }

    public static string GetBookingsPath()
    {
        return bookingsPath + fileExtension;
    }

    public static string GetFlightsPath()
    {
        return flightsPath + fileExtension;
    }

    public static string GetUsersPath()
    {
        return usersPath + fileExtension;
    }
}
