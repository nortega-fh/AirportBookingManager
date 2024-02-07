using AirportBooking;
using AirportBooking.Repositories;
using AirportBooking.Serializers.CSVSerializers;
using AirportBooking.Validators.EntityValidators;
using AirportBooking.Views;

var userValidator = new UserValidator();
var userSerializer = new UserCsvSerializer(userValidator);

var userRepository = new UserRepository(userSerializer, userValidator);
var flightRepository = new FlightRepository();
var bookingRepository = new BookingRepository(userRepository, flightRepository);

var flightView = new FlightsView(flightRepository);
var userView = new UserView(userRepository);

MainView userInterface = new(userView, flightView, bookingRepository);
userInterface.ShowMainMenu();