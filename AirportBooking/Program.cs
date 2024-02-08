using AirportBooking;
using AirportBooking.Repositories;
using AirportBooking.Serializers.ConsoleSerializers;
using AirportBooking.Serializers.CSVSerializers;
using AirportBooking.Validators.EntityValidators;
using AirportBooking.Views;

var userValidator = new UserValidator();
var userCsvSerializer = new UserCsvSerializer(userValidator);

var userRepository = new UserRepository(userCsvSerializer, userValidator);
var flightRepository = new FlightRepository();
var bookingRepository = new BookingRepository(userRepository, flightRepository);

var flightView = new FlightsView(flightRepository);
var userConsoleSerializer = new UserConsoleSerializer();
var userView = new UserView(userRepository, userConsoleSerializer);

MainView userInterface = new(userView, flightView, bookingRepository);
userInterface.ShowMainMenu();