using AirportBooking;
using AirportBooking.Repositories;
using AirportBooking.Views;

var userRepository = new UserRepository();
var flightRepository = new FlightRepository();
var bookingRepository = new BookingRepository(userRepository, flightRepository);

var flightView = new FlightsView(flightRepository);
var userView = new UserView(userRepository);

Interface userInterface = new(userView, flightView, bookingRepository);
userInterface.ShowMainMenu();