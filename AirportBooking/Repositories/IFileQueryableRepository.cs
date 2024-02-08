namespace AirportBooking.Repositories;

public interface IFileQueryableRepository<Key, Value, Dto> : IFileRepository<Key, Value> where Key : IComparable<Key>
{
    IReadOnlyList<Value> FindBySearchParameters(Dto dto);
}
