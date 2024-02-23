namespace AirportBooking.FileReaders;

public interface ICsvFileReader
{
    string[] Read(string fileName);
    void Write(string fileName, string line);
    void UpdateLine(string fileName, string key, string data);
    void DeleteLine(string fileName, string lineKey);
}