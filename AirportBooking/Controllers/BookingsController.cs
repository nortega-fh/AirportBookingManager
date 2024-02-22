namespace AirportBooking.Controllers;

public class BookingsController
{
    //private readonly IBookingRepository<int, Booking> _repository;
    //private readonly IFilter<Booking, BookingParameters> _filter;
    //private readonly IConsoleSerializer<Booking> _serializer;

    //public BookingsController(IBookingRepository<int, Booking> repository,
    //    IConsoleSerializer<Booking> serializer,
    //    IFilter<Booking, BookingParameters> filter)
    //{
    //    _repository = repository;
    //    _serializer = serializer;
    //    _filter = filter;
    //}

    //public IReadOnlyList<Booking> FindAll()
    //{
    //    return _repository.FindAll();
    //}

    //public Booking? Find(int bookingNumber)
    //{
    //    return _repository.Find(bookingNumber);
    //}

    //public IReadOnlyList<Booking>? FilterByParameters(BookingParameters parameters)
    //{
    //    var resultBookings = _filter.SearchByParameters(parameters, _repository.FindAll().ToList());
    //    if (resultBookings.Count == 0)
    //    {
    //        return null;
    //    }
    //    return resultBookings;
    //}

    //public Booking? Create(Booking booking)
    //{
    //    return _repository.Save(booking);
    //}

    //public Booking? Update(int reservationNumber, Booking booking)
    //{
    //    return _repository.Update(reservationNumber, booking);
    //}

    //public void Delete(int reservationNumber)
    //{
    //    _repository.Delete(reservationNumber);
    //}

    //public BookingParameters ObtainFilterParameters()
    //{
    //    var flightNumber = GetOptionalValue("(Optional) Please indicate the booking's flight number to look for");
    //    var minPrice = GetOptionalFloatValue("(Optional) Please indicate the booking's minimum price to look for");
    //    var maxPrice = GetOptionalFloatValue("(Optional) Please indicate the booking's maximum price to look for");
    //    var departureCountry = GetOptionalValue("(Optional) Please indicate the booking's departure country to look for");
    //    var destinationCountry = GetOptionalValue("(Optional) Please indicate the booking's destination country to look for");
    //    DateTime? departureDate = DateTime.TryParse(GetValue("(Optional) Please indicate the booking's departure date to look for"),
    //        out var result)
    //        ? result : null;
    //    var departureAirport = GetOptionalValue("(Optional) Please indicate the booking's departure airport to look for");
    //    var arrivalAirport = GetOptionalValue("(Optional) Please indicate the booking's arrival airport to look for");
    //    var passenger = GetOptionalValue("(Optional) Please indicate the booking's passenger username to look for");
    //    FlightClass? flightClass = GetOptionalValue("(Optional) Please indicate the booking's flight class to look for",
    //                [FlightClass.Economy, FlightClass.Business, FlightClass.FirstClass], out var resultClass)
    //        ? resultClass : null;
    //    BookingType? bookingType = GetOptionalValue("(Optional) Please indicate the booking's type to look for",
    //        [BookingType.OneWay, BookingType.RoundTrip], out var resultType) ? resultType : null;
    //    BookingStatus? bookingStatus = GetOptionalValue("(Optional) Please indicate the booking's status to look for",
    //        [BookingStatus.Confirmed, BookingStatus.Canceled], out var statusType) ? statusType : null;
    //    return new BookingParameters(
    //            flightNumber,
    //            minPrice,
    //            maxPrice,
    //            departureCountry,
    //            destinationCountry,
    //            departureDate,
    //            departureAirport,
    //            arrivalAirport,
    //            passenger,
    //            flightClass,
    //            bookingType,
    //            bookingStatus
    //        );
    //}

    //public void PrintToConsole(Booking booking)
    //{
    //    _serializer.PrintToConsole(booking);
    //}

    //void IController<int, Booking>.FindAll()
    //{
    //    throw new NotImplementedException();
    //}

    //public void Find()
    //{
    //    throw new NotImplementedException();
    //}

    //void IController<int, Booking>.Create(Booking entity)
    //{
    //    throw new NotImplementedException();
    //}

    //void IController<int, Booking>.Update(int key, Booking entity)
    //{
    //    throw new NotImplementedException();
    //}
}
