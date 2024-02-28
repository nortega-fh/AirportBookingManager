namespace AirportBooking.FileReaders;

public class CsvFileReader() : ICsvFileReader
{
    public string[] Read(string fileName)
    {
        try
        {
            using var reader = File.OpenText(fileName);
            var lines = reader.ReadToEnd().Split("\n");
            return lines.Where(line => line is not "").ToArray();
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
            using var writer = new StreamWriter(fileName, true);
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
            var fileData = File.ReadAllLines(fileName);
            for (var i = 0; i < fileData.Length; i++)
            {
                var lineData = fileData[i].Split(",");
                if (lineData[0].Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    fileData[i] = data;
                    break;
                }
            }
            File.WriteAllLines(fileName, fileData);
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
            var fileData = File.ReadAllLines(fileName);
            fileData = fileData.Where(line =>
            {
                string[] lineData = line.Split(",");
                return !lineData[0].Equals(lineKey, StringComparison.OrdinalIgnoreCase);
            }).ToArray();
            File.WriteAllLines(fileName, fileData);
        }
        catch (Exception e) when (e is IOException or DirectoryNotFoundException)
        {
            Console.WriteLine($"Couldn't delete information: {e.Message}");
        }
    }
}
