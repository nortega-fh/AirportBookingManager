using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportBooking.Lib
{
    public class CSVReader(string entity)
    {
        private static readonly string rootPath = Directory.GetParent(Path.Combine("..", ".."))!.FullName;

        private readonly string filePath = Path.Combine(rootPath, "Data", entity, $"{entity}.csv");
        public IEnumerable<string> ReadEntityCSV()
        {
            List<string> lines = [];
            try
            {
                using var reader = File.OpenText(filePath);
                var line = reader.ReadLine();
                while (line is not null && !line.Equals(""))
                {
                    lines.Add(line);
                    line = reader.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return lines;
        }

        public void WriteEntity(string line)
        {
            try
            {
                using StreamWriter writer = new(filePath, true);
                writer.WriteLine(line);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static byte[] StringToByteArray(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }
    }
}
