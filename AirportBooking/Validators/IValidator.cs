namespace AirportBooking.Validators;

public interface IValidator<T>
{
    public void Validate(T value);
}
