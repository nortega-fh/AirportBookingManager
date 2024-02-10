﻿using AirportBooking.Exceptions;
using AirportBooking.FileReaders;
using AirportBooking.Models;
using AirportBooking.Serializers.CSVSerializers;
using AirportBooking.Views.Controllers;

namespace AirportBooking.Views;

public class ManagerView : RoleConsoleView
{
    private readonly BookingsController _bookingsController;
    private readonly FlightsController _flightsController;
    private readonly CSVReader _csvReader;
    private readonly FlightCsvSerializer _serializer;

    public ManagerView(BookingsController controller, CSVReader csvReader, FlightsController flightsController, FlightCsvSerializer serializer)
    {
        _bookingsController = controller;
        _csvReader = csvReader;
        _flightsController = flightsController;
        _serializer = serializer;
    }

    public override void ShowMenu()
    {
        while (UserSession.GetLoggedUser() is not null)
        {
            const string option1 = "Search bookings";
            const string option2 = "Upload flights";
            const string option3 = "Logout";
            var action = GetValue<string>("Manager Menu", [option1, option2, option3]);
            switch (action)
            {
                case option1:
                    FilterBookings();
                    break;
                case option2:
                    UploadFlights();
                    break;
                case option3:
                    UserSession.LogOutUser();
                    Console.WriteLine("Logged out succesfully");
                    break;
            }
        }
        ClearOnInput();
    }

    private void UploadFlights()
    {
        while (true)
        {
            var rootPath = Directory.GetParent(Path.Combine("..", ".."))!.FullName;
            var flightsPath = Path.Combine(rootPath, "Data", "flights");
            var existingFiles = Directory.GetFiles(flightsPath, "*.csv");
            Console.WriteLine("Please upload the flight data inside the Data directory at the flights folder. The file must be a csv");
            Console.WriteLine("Press enter once you have placed the new data");
            ClearOnInput();
            var newFiles = Directory.GetFiles(flightsPath, "*.csv");
            if (!(newFiles.Length > existingFiles.Length))
            {
                Console.WriteLine("No file has been introduced. Please try again");
                return;
            }
            var latestFile = newFiles[^1];
            string[] uploadedFlights = File.ReadAllLines(latestFile);
            var errorList = new List<Exception>();
            var originalFlightList = _flightsController.FindAll().ToList();
            foreach (var line in uploadedFlights)
            {
                try
                {
                    _flightsController.Create(_serializer.FromCsv(line));
                }
                catch (Exception ex) when (ex is EntitySerializationException<Flight>)
                {
                    errorList.Add(ex);
                }
            }
            if (errorList.Count == 0) return;
            foreach (var error in errorList)
            {
                Console.WriteLine(error.Message);
            }
            _flightsController.FindAll().ToList().ForEach(f => _flightsController.Delete(f.Number));
            originalFlightList.ForEach(f => _flightsController.Create(f));
        }
    }

    private void FilterBookings()
    {
        var filter = _bookingsController.ObtainFilterParameters();
        var bookings = _bookingsController.FilterByParameters(filter) ?? [];
        if (bookings.Count == 0)
        {
            Console.WriteLine("No bookings matched your characteristics");
            return;
        }
        foreach (var booking in bookings)
        {
            _bookingsController.PrintToConsole(booking);
        }
    }
}
