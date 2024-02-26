using AirportBooking.ConsoleInputHandler;
using AirportBooking.Controllers.Bookings;
using AirportBooking.Controllers.Flights;
using AirportBooking.FileReaders;
using AirportBooking.Repositories.Bookings;
using AirportBooking.Repositories.Flights;
using AirportBooking.Repositories.Users;
using AirportBooking.Serializers.Console.Bookings;
using AirportBooking.Serializers.Console.Flights;
using AirportBooking.Serializers.Csv.Bookings;
using AirportBooking.Serializers.Csv.Flights;
using AirportBooking.Serializers.Csv.Users;
using AirportBooking.Validators.Bookings;
using AirportBooking.Validators.Flights;
using AirportBooking.Validators.Users;
using AirportBooking.Views;
using AirportBooking.Views.Context;
using AirportBooking.Views.Menus.Manager;
using AirportBooking.Views.Menus.Passenger;

var csvFileReader = new CsvFileReader();
var consoleInputHandler = new ConsoleInputHandler();

var userValidator = new UserCsvValidator();
var userSerializer = new UserCsvSerializer(userValidator);
var userRepository = new UserCsvRepository(csvFileReader, userSerializer);

var flightValidator = new FlightCsvValidator();
var flightSerializer = new FlightCsvSerializer(flightValidator);
var flightConsoleSerializer = new FlightConsoleSerializer();
var flightRepository = new FlightCsvRepository(csvFileReader, flightSerializer);
var flightController = new FlightConsoleController(flightRepository, flightConsoleSerializer, consoleInputHandler);

var bookingCsvValidator = new BookingCsvValidator();
var bookingCsvSerializer = new BookingCsvSerializer(bookingCsvValidator);
var bookingConsoleSerializer = new BookingConsoleSerializer();
var bookingRepository = new BookingCsvRepository(csvFileReader, bookingCsvSerializer, userRepository, flightRepository);
var bookingConsoleController = new BookingConsoleController(consoleInputHandler, bookingRepository, bookingConsoleSerializer, flightController);

var userSession = new UserSession();
var passengerMenu = new PassengerMenu(userSession, flightController, bookingConsoleController);
var managerMenu = new ManagerMenu(userSession, flightController, bookingConsoleController);

var mainView = new LoginView(userRepository, userSession, managerMenu, passengerMenu, consoleInputHandler);
mainView.Run();