

using AirportBooking.FileReaders;
using AirportBooking.Repositories;
using AirportBooking.Serializers.Csv;
using AirportBooking.Validators.CsvValidators;
using AirportBooking.Views;

var csvFileReader = new CsvFileReader();

var userValidator = new UserCsvValidator();
var userSerializer = new UserCsvSerializer(userValidator);
var userRepository = new UserRepository(csvFileReader, userSerializer);

var mainView = new MainView(userRepository);
mainView.Run();