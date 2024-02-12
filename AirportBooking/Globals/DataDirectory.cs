namespace AirportBooking.Globals;

public static class DataDirectory
{
    private static readonly string rootPath = Directory.GetParent(Path.Combine("..", "..", "Data"))!.FullName;
    private static readonly string bookingsPath = Path.Combine(rootPath, "bookings");
    private static readonly string flightsPath = Path.Combine(rootPath, "flights");
    private static readonly string usersPath = Path.Combine(rootPath, "users");

    public static string GetFilePath(string path)
    {
        return Path.Combine(rootPath, path);
    }
    public static string GetRootPath()
    {
        return rootPath;
    }

    public static string GetBookingsPath()
    {
        return bookingsPath;
    }

    public static string GetFlightsPath()
    {
        return flightsPath;
    }

    public static string GetUsersPath()
    {
        return usersPath;
    }
}
