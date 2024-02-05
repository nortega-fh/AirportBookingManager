using System.Text;

namespace AirportBooking.Lib;

public class CSVReader(string entity, string? relatedEntity = null)
{
    private static readonly string rootPath = Directory.GetParent(Path.Combine("..", ".."))!.FullName;
    private readonly string filePath = Path.Combine(rootPath, "Data", entity, $"{entity}.csv");
    private readonly string relationshipFilePath = Path.Combine(rootPath, "Data", entity, $"{entity}-{relatedEntity}.csv");

    public IEnumerable<string> ReadEntityInformation()
    {
        List<string> lines = [];
        try
        {
            using var reader = File.OpenText(filePath);
            var line = reader.ReadLine();
            // Skip header line
            line = reader.ReadLine();
            while (line is not null && !line.Equals(""))
            {
                lines.Add(line);
                line = reader.ReadLine();
            }
        }
        catch (Exception e) when (e is IOException or DirectoryNotFoundException)
        {

            Console.WriteLine($"Couldn't handle file {filePath}: {e.Message}");
        }
        return lines;
    }

    public void WriteEntityInformation(string line)
    {
        try
        {
            using StreamWriter writer = new(filePath, true);
            writer.WriteLine(line);
        }
        catch (Exception e) when (e is IOException or DirectoryNotFoundException)
        {

            Console.WriteLine($"Couldn't handle file {filePath}: {e.Message}");
        }
    }

    public void UpdateEntityInformation(string key, string line)
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);
            for (var i = 0; i < lines.Length; i++)
            {
                var data = lines[i].Split(",");
                if (data[0].Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    lines[i] = line;
                    break;
                }
            }
            File.WriteAllLines(filePath, lines);
        }
        catch (Exception e) when (e is IOException or DirectoryNotFoundException)
        {
            Console.WriteLine($"Couldn't handle file {filePath}: {e.Message}");
        }
    }

    public void DeleteEntityInformation(string key, bool deleteRelationships = false)
    {
        try
        {
            DeleteFileLines(filePath, key);
            if (deleteRelationships) DeleteFileLines(relationshipFilePath, key);
        }
        catch (Exception e) when (e is IOException or DirectoryNotFoundException)
        {
            Console.WriteLine($"Couldn't delete information: {e.Message}");
        }
    }

    private static void DeleteFileLines(string filePath, string lineIdentifier)
    {
        var lines = File.ReadAllLines(filePath);
        lines = lines.Where(line =>
        {
            string[] data = line.Split(",");
            return !data[0].Equals(lineIdentifier, StringComparison.OrdinalIgnoreCase);
        }).ToArray();
        File.WriteAllLines(filePath, lines);
    }

    public Dictionary<string, List<string>> GetRelationshipInformation()
    {
        Dictionary<string, List<string>> mappedRelationship = [];
        try
        {
            using var reader = File.OpenText(relationshipFilePath);
            var line = reader.ReadLine();
            // Skip header line
            line = reader.ReadLine();
            while (line is not (null or ""))
            {
                var data = line.Split(",");
                line = reader.ReadLine();

                (string pk, string fk) = (data[0], data[1]);

                if (mappedRelationship.TryGetValue(pk, out List<string>? value))
                {
                    value.Add(fk);
                    mappedRelationship[pk] = value;
                    continue;
                }
                mappedRelationship.Add(pk, [fk]);
            }
        }
        catch (Exception e) when (e is IOException or DirectoryNotFoundException)
        {
            Console.WriteLine($"Couldn't handle file {relationshipFilePath}: {e.Message}");
        }
        return mappedRelationship;
    }

    public void WriteRelationshipInformation(string pk, List<string> fks)
    {
        try
        {
            using StreamWriter writer = new(relationshipFilePath, true);
            fks.ForEach(fk => writer.WriteLine(string.Join(",", [pk, fk])));
        }
        catch (Exception e) when (e is IOException or DirectoryNotFoundException)
        {
            Console.WriteLine($"Couldn't handle file {relationshipFilePath}: {e.Message}");
        }
    }

    private static byte[] StringToByteArray(string str)
    {
        return Encoding.ASCII.GetBytes(str);
    }
}