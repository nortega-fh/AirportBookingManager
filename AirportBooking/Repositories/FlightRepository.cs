using AirportBooking.Enums;
using AirportBooking.Helpers;
using AirportBooking.Lib;

namespace AirportBooking
{
    public class FlightRepository : IFileRepository<string, Flight>
    {
        private List<Flight> flights = [];
        private readonly CSVReader flightCSVReader = new("flights");

        public FlightRepository()
        {
            try
            {
                Load();
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidAttributeException<string> || ex is InvalidAttributeException<DateTime>)
            {
                flights.Clear();
                Console.WriteLine(ex.Message);
            }
        }

        private Flight Validate(string[] attributes)
        {
            if (attributes.Length < 10) throw new ArgumentException($"The data for the flight in line {string.Join(",", attributes)} is incomplete");

            try
            {
                (string number, string economyPrice, string businessPrice, string firstClassPrice,
                    string originCountry, string destinationCountry,
                    DateTime departureDate, DateTime arrivalDate,
                    string originAirport, string destinationAirport) = (attributes[0], attributes[1], attributes[2], attributes[3],
                                                                            attributes[4], attributes[5],
                                                                            DateTime.Parse(attributes[6]), DateTime.Parse(attributes[7]),
                                                                            attributes[8], attributes[9]);

                if (number is "") throw new InvalidAttributeException<string>(number, ["Required"]);
                if (originCountry is "") throw new InvalidAttributeException<string>(originCountry, ["Required"]);
                if (destinationCountry is "") throw new InvalidAttributeException<string>(destinationCountry, ["Required"]);
                if (originAirport is "") throw new InvalidAttributeException<string>(originAirport, ["Required"]);
                if (destinationAirport is "") throw new InvalidAttributeException<string>(destinationAirport, ["Required"]);
                if (!economyPrice.Equals("") && float.IsNaN(float.Parse(economyPrice)))
                    throw new InvalidAttributeException<string>(destinationAirport, ["Required", "Valid values (Can be a space \"\" (To skip) or numbers with decimal part after a dot (FF.FF))"]);
                if (!businessPrice.Equals("") && float.IsNaN(float.Parse(businessPrice)))
                    throw new InvalidAttributeException<string>(destinationAirport, ["Required", "Valid values (Can be a space \"\" (To skip) or numbers with decimal part after a dot  (FF.FF))"]);
                if (!firstClassPrice.Equals("") && float.IsNaN(float.Parse(firstClassPrice)))
                    throw new InvalidAttributeException<string>(destinationAirport, ["Required", "Valid values (Can be a space \"\" (To skip) or numbers numbers with decimal part after a dot (FF.FF))"]);
                if (arrivalDate < departureDate) throw new InvalidAttributeException<DateTime>(arrivalDate, ["Required", "Valid range (Greater than or equal to Departure Date)"]);

                SortedDictionary<FlightClass, float> prices = [];
                if (economyPrice is not "")
                    prices.Add(FlightClass.Economy, float.Parse(economyPrice.Replace(".", ",")));
                if (businessPrice is not "")
                    prices.Add(FlightClass.Business, float.Parse(businessPrice.Replace(".", ",")));
                if (firstClassPrice is not "")
                    prices.Add(FlightClass.FirstClass, float.Parse(firstClassPrice.Replace(".", ",")));

                return new Flight(number, prices, originCountry, destinationCountry, departureDate, arrivalDate, originAirport, destinationAirport);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"Invalid date format for line {string.Join(",", attributes)} - Dates should be in format YYYY-MM-DDTHH:mm:ssZ");
            }
        }

        private void TransformLineToFlight(string line)
        {
            var flight = Validate(line.Split(','));
            flights.Add(flight);
        }

        public void Load()
        {
            List<string> readFlights = flightCSVReader.ReadEntityInformation().ToList();
            readFlights.ForEach(TransformLineToFlight);
        }

        public Flight? Find(string flightNumber)
        {
            return flights.Find(f => f.Number.Equals(flightNumber, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Flight> FindBySearchParameters(FlightParameters parameters)
        {
            IEnumerable<Flight> resultFlights = flights
                .Where(f => f.OriginCountry.Contains(parameters.OriginCountry, StringComparison.OrdinalIgnoreCase))
                .Where(f => f.DestinationCountry.Contains(parameters.DestinationCountry, StringComparison.OrdinalIgnoreCase))
                .Where(f => parameters.DepartureDate <= f.DepartureDate && f.DepartureDate < parameters.DepartureDate.AddDays(1)
                && f.DepartureDate > parameters.DepartureDate.AddDays(-1))
                .Where(f => f.Prices.Values.Min() >= parameters.MinPrice && f.Prices.Values.Min() <= parameters.MaxPrice);

            if (parameters.DepartureAirport is (not null) and (not ""))
            {
                resultFlights = resultFlights.Where(f => f.OriginAirport.Equals(parameters.DepartureAirport, StringComparison.OrdinalIgnoreCase));
            }

            if (parameters.ArrivalAirport is (not null) and (not ""))
            {
                resultFlights = resultFlights.Where(f => f.DestinationAirport.Equals(parameters.ArrivalAirport, StringComparison.OrdinalIgnoreCase));
            }

            if (parameters.FlightClass is not null)
            {
                resultFlights = resultFlights.Where(f => f.Prices.ContainsKey(parameters.FlightClass.Value));
            }

            return resultFlights;
        }

        public IEnumerable<Flight> FindAll()
        {
            return flights;
        }

        public Flight Save(Flight flight)
        {
            Flight? existingFlight = flights.Find(f => f.Number.Equals(flight.Number, StringComparison.OrdinalIgnoreCase));
            if (existingFlight is not null)
            {
                throw new EntityAlreadyExists<Flight, string>(flight.Number);
            }
            flightCSVReader.WriteEntityInformation(flight.ToCSV());
            flights.Add(flight);
            return flight;
        }

        public Flight? Update(string flightNumber, Flight newFlight)
        {
            if (flights.Find(f => f.Number.Equals(flightNumber, StringComparison.OrdinalIgnoreCase)) is null)
            {
                throw new EntityNotFound<Flight, string>(flightNumber);
            }
            flightCSVReader.UpdateEntityInformation(flightNumber, newFlight.ToCSV());
            flights = flights.Select(f => f.Number.Equals(flightNumber,
                StringComparison.OrdinalIgnoreCase) ? newFlight : f).ToList();
            return newFlight;
        }

        public void Delete(string flightNumber)
        {
            if (flights.Find(f => f.Number.Equals(flightNumber, StringComparison.OrdinalIgnoreCase)) is null)
            {
                throw new EntityNotFound<Flight, string>(flightNumber);
            }
            flights = flights.Where(f => !f.Number.Equals(flightNumber, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}
