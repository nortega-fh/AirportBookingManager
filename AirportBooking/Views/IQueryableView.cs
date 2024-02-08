namespace AirportBooking.Views;

public interface IQueryableView<T, K> : IView<T>
{
    T? SearchByParameters(K parameterDTO);
    K ObtainParameters();
}
