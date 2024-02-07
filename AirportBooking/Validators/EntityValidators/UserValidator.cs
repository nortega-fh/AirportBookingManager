using AirportBooking.Exceptions;
using AirportBooking.Models;

namespace AirportBooking.Validators.EntityValidators;

public class UserValidator : IValidator<User>
{
    public void Validate(User user)
    {
        if (user.Username.Equals(string.Empty))
        {
            throw new InvalidAttributeException("Username", "string", ["Required"]);
        }
        if (user.Password.Equals(string.Empty))
        {
            throw new InvalidAttributeException("Password", "string", ["Required"]);
        }
    }
}
