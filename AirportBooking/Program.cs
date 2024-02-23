

using AirportBooking.FileReaders;
using AirportBooking.Repositories;
using AirportBooking.Serializers.ConsoleSerializers;
using AirportBooking.Serializers.Csv;
using AirportBooking.Validators.CsvValidators;
using AirportBooking.Views;

var csvFileReader = new CsvFileReader();

var userValidator = new UserCsvValidator();
var userSerializer = new UserCsvSerializer(userValidator);
var userRepository = new UserCsvRepository(csvFileReader, userSerializer);

var flightValidator = new FlightCsvValidator();
var flightSerializer = new FlightCsvSerializer(flightValidator);
var flightConsoleSerializer = new FlightConsoleSerializer();
var flightRepository = new FlightCsvRepository(csvFileReader, flightSerializer);

var mainView = new MainView(userRepository, flightRepository, flightConsoleSerializer);
mainView.Run();