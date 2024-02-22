﻿using AirportBooking.Exceptions;
using AirportBooking.FileReaders;
using AirportBooking.Models;
using AirportBooking.Serializers.Csv;

namespace AirportBooking.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IFileReader _reader;
    private readonly IUserCsvSerializer _serializer;
    private readonly static string UsersFile = Path.Combine("..", "..", "..", "Data", "users.csv");

    public UserRepository(IFileReader reader, IUserCsvSerializer serializer)
    {
        _reader = reader;
        _serializer = serializer;
    }

    public User? Find(string username)
    {
        return _reader.Read(UsersFile)
            .Select(_serializer.From)
            .Where(u => u.Username == username)
            .FirstOrDefault();
    }

    public User? Find(string username, string password)
    {
        return _reader.Read(UsersFile)
            .Select(_serializer.From)
            .Where(u => u.Username.Equals(username) && u.Password.Equals(password))
            .FirstOrDefault();
    }

    public User Create(User user)
    {
        var existingUser = Find(user.Username);
        if (existingUser is not null)
        {
            throw new EntityAlreadyExists<User, string>(user.Username);
        }
        _reader.Write(UsersFile, _serializer.To(user));
        return user;
    }
}
