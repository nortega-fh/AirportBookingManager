namespace AirportBooking.Serializers;

public interface ICsvSerializer<T>
{
    public string ToCsv(T obj);
    public T FromCsv(string csvLine);
}
