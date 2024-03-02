using AirportBooking.Models.Users;
using AirportBooking.Serializers.Csv.Users;
using AirportBooking.Validators.Users;
using AutoFixture;
using FluentAssertions;
using Moq;

namespace AiportBooking.Tests.Serializers;

public class UserCsvSerializerShould
{

    [Fact]
    public void ValidateCsvLineBeforeDeserializing()
    {
        var userData = "user,user,Passenger";
        var validator = new Mock<IUserCsvValidator>();
        validator.Setup(v => v.Validate(It.IsAny<string>())).Returns(userData.Split(','));
        var sut = new UserCsvSerializer(validator.Object);

        sut.From(userData);

        validator.Verify(v => v.Validate(userData), Times.Once());
    }

    [Fact]
    public void SerializeEntityToCsv()
    {
        var fixture = new Fixture();
        var user = fixture.Create<User>();
        var validator = new Mock<IUserCsvValidator>();
        var sut = new UserCsvSerializer(validator.Object);

        var serializedUser = sut.To(user);

        serializedUser.Should().Be($"{user.Username},{user.Password},{user.Role}");
    }

    [Fact]
    public void DeserializeCsvToUserEntity()
    {
        var csvLine = "user,user,Passenger";
        var validator = new Mock<IUserCsvValidator>();
        validator.Setup(v => v.Validate(csvLine)).Returns(csvLine.Split(','));
        var sut = new UserCsvSerializer(validator.Object);

        var obtainedUser = sut.From(csvLine);

        obtainedUser.Should().NotBeNull();
        obtainedUser.Username.Should().Be("user");
        obtainedUser.Password.Should().Be("user");
        obtainedUser.Role.Should().Be(UserRole.Passenger);
    }
}
