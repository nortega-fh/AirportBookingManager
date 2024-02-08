namespace AirportBooking.Views;

public interface IView<T>
{
    void ShowAll();
    void ShowOne();
    void Delete();
}
