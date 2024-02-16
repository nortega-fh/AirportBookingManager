using AirportBooking.Constants;
using AirportBooking.Exceptions;
using AirportBooking.Models;

namespace AirportBooking.Validators.EntityValidators;

public class UserValidator
{
    public User Validate(User user)
    {
        if (user.Username.Equals(string.Empty))
        {
            throw new InvalidAttributeException<string>("Username", EntityValueRestriction.Restrictions[Restriction.Field]);
        }
        if (user.Password.Equals(string.Empty))
        {
            throw new InvalidAttributeException<string>("Password", EntityValueRestriction.Restrictions[Restriction.Field]);
        }
        return user;
    }
}
