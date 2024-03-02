using AirportBooking.Exceptions;
using AirportBooking.Models.Users;
using AirportBooking.Validators.Users;
using FluentAssertions;

namespace AiportBooking.Tests.Validators;

public class UserCsvValidatorShould
{
    private readonly UserCsvValidator _sut = new();

    [Theory]
    [InlineData("admin,admin,manager")]
    [InlineData("passenger,pass,Passenger")]
    public void ReturnUserDataWhenValid(string data)
    {
        _sut.Validate(data).Should().NotBeNullOrEmpty().And.BeEquivalentTo(data.Split(","));
    }

    [Theory]
    [InlineData(",admin,manager")]
    [InlineData("null,admin,manager")]
    [InlineData("admin,,manager")]
    [InlineData("admin,null,manager")]
    [InlineData("passenger,,Passenger")]
    [InlineData("passenger,null,Passenger")]
    [InlineData(",passenger,Passenger")]
    [InlineData("null,passenger,Passenger")]
    public void ThrowInvalidAttributeExceptionWhenUsernameOrPasswordInvalid(string data)
    {
        _sut.Invoking(s => s.Validate(data)).Should().Throw<InvalidAttributeException<string>>();
    }

    [Theory]
    [InlineData("admin,admin,invalid")]
    [InlineData("admin,admin,")]
    public void ThrowInvalidAttributeExceptionWhenUserRoleIsInvalid(string data)
    {
        _sut.Invoking(s => s.Validate(data)).Should().Throw<InvalidAttributeException<UserRole>>();
    }
}
