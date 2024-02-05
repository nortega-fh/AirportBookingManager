using AirportBooking.Enums;

namespace AirportBooking
{
    public class Booking : ICSVEntity
    {
        public int ReservationNumber { get; set; }
        public List<Flight> Flights { get; set; }
        public float Price { get; set; }
        public BookingType BookingType { get; set; }
        public List<FlightClass> FlightClasses { get; set; }
        public User MainPassenger { get; set; }

        public Booking(List<Flight> flights, List<FlightClass> flightClasses, BookingType type, User passenger, float totalPrice)
        {
            Flights = flights;
            Price = totalPrice;
            BookingType = type;
            FlightClasses = flightClasses;
            MainPassenger = passenger;
        }

        public Booking(int reservationNumber, List<Flight> flights, List<FlightClass> flightClasses, BookingType type, User passenger, float totalPrice)
        {
            ReservationNumber = reservationNumber;
            Flights = flights;
            Price = totalPrice;
            BookingType = type;
            FlightClasses = flightClasses;
            MainPassenger = passenger;
        }

        public void UpdatePrice()
        {
            Price = 0f;
            for (var i = 0; i < Flights.Count; i++)
            {
                Price += Flights[i].Prices[FlightClasses[i]];
            }

        }

        public string ToCSV() => string.Join(",", [ReservationNumber, Price, BookingType, FlightClasses.First(), FlightClasses.Last(), MainPassenger.Username]);

        public string BookingFlightsToString()
        {
            string format = "";
            Flights.ForEach(f => format += f.ToString() + "\n");
            return format;
        }

        public string FlightClassesToString() => string.Join(",", FlightClasses);

        public override string ToString()
        {
            return $"""
                ######################################
                         Booking {ReservationNumber}
                ######################################
                Flights:
                {BookingFlightsToString()}
                Booking type: {BookingType}
                Class: {FlightClassesToString()}
                Passenger:
                {MainPassenger}
                --------------------------------------
                    Total price: ${Price}
                --------------------------------------
                """;
        }

    }
}