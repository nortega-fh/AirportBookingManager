namespace AirportBooking.Views;

public interface IVIew<T>
{
    void ShowAll();
    void ShowOne();
    void Create();
    void Update();
    void Delete();
}
