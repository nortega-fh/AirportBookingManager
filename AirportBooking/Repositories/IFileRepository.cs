namespace AirportBooking.Repositories;

public interface IFileRepository<Key, Value> where Key : IComparable<Key>
{
    public Value? Find(Key key);
    public IReadOnlyList<Value> FindAll();
    public Value? Save(Value value);
    public Value? Update(Key key, Value value);
    public void Delete(Key key);
}
