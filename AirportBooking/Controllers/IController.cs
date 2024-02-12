namespace AirportBooking.Controllers;

public interface IController<Key, Entity>
{
    public void FindAll();
    public void Find();
    public void Create(Entity entity);
    public void Update(Key key, Entity entity);
    public void Delete(Key key);
}
