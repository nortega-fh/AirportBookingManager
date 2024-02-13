using AirportBooking.Globals;

namespace AirportBooking.FileReaders;

public class CsvFileReader()
{
    private static readonly string root = DataDirectory.GetRootPath();

    public string[] Read(string fileName)
    {
        try
        {
            using var reader = File.OpenText(GetFilePath(fileName));
            var lines = reader.ReadToEnd().Split("\n");
            return lines[1..];
        }
        catch (Exception e) when (e is IOException or DirectoryNotFoundException)
        {

            Console.WriteLine($"Couldn't handle file {fileName}: {e.Message}");
        }
        return [];
    }

    public void Write(string fileName, string line)
    {
        try
        {
            using var writer = new StreamWriter(GetFilePath(fileName), true);
            writer.WriteLine(line);
        }
        catch (Exception e) when (e is IOException or DirectoryNotFoundException)
        {
            Console.WriteLine($"Couldn't handle file {fileName}: {e.Message}");
        }
    }

    public void UpdateLine(string fileName, string key, string data)
    {
        try
        {
            var fileData = File.ReadAllLines(GetFilePath(fileName));
            for (var i = 0; i < fileData.Length; i++)
            {
                var lineData = fileData[i].Split(",");
                if (lineData[0].Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    lineData[i] = data;
                    break;
                }
            }
            File.WriteAllLines(GetFilePath(fileName), fileData);
        }
        catch (Exception e) when (e is IOException or DirectoryNotFoundException)
        {
            Console.WriteLine($"Couldn't handle file {fileName}: {e.Message}");
        }
    }

    public void DeleteLine(string fileName, string lineKey)
    {
        try
        {
            var filePath = GetFilePath(fileName);
            var fileData = File.ReadAllLines(filePath);
            fileData = fileData.Where(line =>
            {
                string[] lineData = line.Split(",");
                return !lineData[0].Equals(lineKey, StringComparison.OrdinalIgnoreCase);
            }).ToArray();
            File.WriteAllLines(filePath, fileData);
        }
        catch (Exception e) when (e is IOException or DirectoryNotFoundException)
        {
            Console.WriteLine($"Couldn't delete information: {e.Message}");
        }
    }

    private string GetFilePath(string filePath)
    {
        return Path.Combine(root, filePath);
    }
}
