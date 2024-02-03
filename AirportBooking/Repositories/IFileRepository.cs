using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportBooking
{
    public interface IFileRepository<Key, Value> where Key : IComparable<Key>
    {
        public Value? Find(Key key);

        public IEnumerable<Value> FindAll();

        public Value? Save(Value value);

        public Value? Update(Key key, Value value);

        public void Delete(Key key);
    }
}
