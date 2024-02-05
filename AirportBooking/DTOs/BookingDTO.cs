using AirportBooking.Enums;
using AirportBooking.Models;

namespace AirportBooking.DTOs;

public record BookingDTO(List<Flight> Flights, List<FlightClass> FlightClasses, BookingType Type, User Passenger, float TotalPrice)
{
    public override string ToString()
    {
        return $"""
                Flights: 

                {string.Join("\n", Flights)}

                Class: {string.Join(",", FlightClasses)}
                Type: {Type}
                Passenger: {Passenger}
                ----------------------------
                Total price: ${TotalPrice}
                ----------------------------
                """;
    }
}
