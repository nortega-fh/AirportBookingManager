namespace AirportBooking.Views;

public interface IQueryableView<T, K> : IVIew<T>
{
    void SearchByParameters(K parameterDTO);
}
