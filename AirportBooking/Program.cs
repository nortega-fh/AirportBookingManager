using AirportBooking.Filters;
using AirportBooking.Repositories.CsvRepositories;
using AirportBooking.Serializers.ConsoleSerializers;
using AirportBooking.Serializers.CSVSerializers;
using AirportBooking.Validators.EntityValidators;
using AirportBooking.Views;
using AirportBooking.Views.Controllers;

var userValidator = new UserValidator();
var userCsvSerializer = new UserCsvSerializer(userValidator);

var userRepository = new UserRepository(userCsvSerializer, userValidator);
var flightRepository = new FlightRepository();
var bookingRepository = new BookingRepository(userRepository, flightRepository);

var userConsoleSerializer = new UserConsoleSerializer();
var flightConsoleSerializer = new FlightConsoleSerializer();
var bookingConsoleSerializer = new BookingConsoleSerializer();

var flightsFilter = new FlightFilter();
var bookingsFilter = new BookingFilter(flightsFilter, flightRepository);

var flightController = new FlightsController(flightRepository, flightConsoleSerializer, flightsFilter);
var userController = new UserController(userRepository, userConsoleSerializer);
var bookingController = new BookingsController(bookingRepository, bookingConsoleSerializer, bookingsFilter);

var passengerView = new PassengerView(flightController, bookingController);
var managerView = new ManagerView();

MainView userInterface = new(userController, passengerView, managerView);
userInterface.Show();