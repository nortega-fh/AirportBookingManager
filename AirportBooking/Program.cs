using AirportBooking.FileReaders;
using AirportBooking.Repositories;
using AirportBooking.Serializers.ConsoleSerializers;
using AirportBooking.Serializers.Csv;
using AirportBooking.Validators;
using AirportBooking.Views;

var csvFileReader = new CsvFileReader();

var userValidator = new UserCsvValidator();
var userSerializer = new UserCsvSerializer(userValidator);
var userRepository = new UserCsvRepository(csvFileReader, userSerializer);

var flightValidator = new FlightCsvValidator();
var flightSerializer = new FlightCsvSerializer(flightValidator);
var flightConsoleSerializer = new FlightConsoleSerializer();
var flightRepository = new FlightCsvRepository(csvFileReader, flightSerializer);
var flightController = new FlightConsoleController(flightRepository, flightConsoleSerializer);

var userSession = new UserSession();
var passengerMenu = new PassengerMenu(userSession, flightController);
var managerMenu = new ManagerMenu(userSession, flightController);

var mainView = new LoginView(userRepository, userSession, managerMenu, passengerMenu);
mainView.Run();