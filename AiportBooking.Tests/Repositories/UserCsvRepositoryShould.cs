using AirportBooking.Exceptions;
using AirportBooking.FileReaders;
using AirportBooking.Models.Users;
using AirportBooking.Repositories.Users;
using AirportBooking.Serializers.Csv.Users;
using AutoFixture;
using FluentAssertions;
using Moq;

namespace AiportBooking.Tests.Repositories;

public class UserCsvRepositoryShould : IDisposable
{
    private readonly Mock<ICsvFileReader> _reader = new();
    private readonly Mock<IUserCsvSerializer> _serializer = new();
    private readonly Fixture _fixture;
    private readonly UserCsvRepository _sut;

    public UserCsvRepositoryShould()
    {
        _sut = new(_reader.Object, _serializer.Object);
        _fixture = new Fixture();
    }

    public void Dispose()
    {
        _reader.Reset();
        _serializer.Reset();
    }

    [Fact]
    public void FindUserByUsername()
    {
        var userData = "user,user,manager";
        var splittedData = userData.Split(',');
        var expectedUser = new User
        {
            Username = splittedData[0],
            Password = splittedData[1],
            Role = Enum.Parse<UserRole>(splittedData[2], true)
        };

        _reader.Setup(reader => reader.Read(It.IsAny<string>())).Returns([userData]);
        _serializer.Setup(s => s.From(userData)).Returns(expectedUser);

        var obtainedUser = _sut.Find(splittedData[0]);

        obtainedUser.Should().NotBeNull();
        obtainedUser?.Username.Should().Be(expectedUser.Username);
        obtainedUser?.Password.Should().Be(expectedUser.Password);
        obtainedUser?.Role.Should().Be(expectedUser.Role);
    }

    [Fact]
    public void FindUserByUsernameAndPassword()
    {
        var userData = "user,user,manager";
        var splittedData = userData.Split(',');
        var expectedUser = new User
        {
            Username = splittedData[0],
            Password = splittedData[1],
            Role = Enum.Parse<UserRole>(splittedData[2], true)
        };

        _reader.Setup(reader => reader.Read(It.IsAny<string>())).Returns([userData]);
        _serializer.Setup(s => s.From(userData)).Returns(expectedUser);

        var obtainedUser = _sut.Find(splittedData[0], splittedData[1]);

        obtainedUser.Should().NotBeNull();
        obtainedUser?.Username.Should().Be(expectedUser.Username);
        obtainedUser?.Password.Should().Be(expectedUser.Password);
        obtainedUser?.Role.Should().Be(expectedUser.Role);
    }

    [Fact]
    public void CreateUser()
    {
        var expectedUser = _fixture.Create<User>();

        var obtainedUser = _sut.Create(expectedUser);

        _reader.Verify(r => r.Write(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        obtainedUser.Should().NotBeNull();
        obtainedUser?.Username.Should().Be(expectedUser.Username);
        obtainedUser?.Password.Should().Be(expectedUser.Password);
        obtainedUser?.Role.Should().Be(expectedUser.Role);
    }

    [Fact]
    public void ThrowWhenCreatingUserThatExists()
    {
        var alreadySavedUser = _fixture.Create<User>();
        var savedUserToCsv = $"{alreadySavedUser.Username},{alreadySavedUser.Password},{alreadySavedUser.Role}";

        _reader.Setup(r => r.Read(It.IsAny<string>())).Returns([savedUserToCsv]);
        _serializer.Setup(s => s.From(It.IsAny<string>())).Returns(alreadySavedUser);

        _sut.Invoking(s => s.Create(alreadySavedUser)).Should().Throw<EntityAlreadyExists<User, string>>();
    }
}
