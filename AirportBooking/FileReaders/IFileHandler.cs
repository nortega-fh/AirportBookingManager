namespace AirportBooking.FileReaders;

public interface IFileHandler
{
    string[] Read(string fileName);
    void Write(string fileName, string line);
    void UpdateLine(string fileName, string lineKey, string data);
    void DeleteLine(string fileName, string lineKey);
}
