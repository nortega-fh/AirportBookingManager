using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportBooking
{
    public class EntityNotFound<T, K>(K key): Exception($"The {typeof(T).Name} with \"{key}\" doesn't exist")
    {
    }
}
