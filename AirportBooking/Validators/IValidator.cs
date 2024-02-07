namespace AirportBooking.Validators;

public interface IValidator
{
    public void Validate<T>(T value);
}
