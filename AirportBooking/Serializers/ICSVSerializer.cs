namespace AirportBooking.Serializers;

public interface ICSVSerializer<T>
{
    public string ToCsv(T obj);
    public T FromCsv(string csvLine);
}
