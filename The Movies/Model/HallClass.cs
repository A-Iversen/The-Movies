using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Movies.Model
{
    public class HallClass
    {
        private string _name;
        private int _numberOfSeats;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int NumberOfSeats
        {
            get { return _numberOfSeats; }
            set { _numberOfSeats = value; }
        }
        public HallClass(string name, int numberOfSeats)
        {
            _name = name;
            _numberOfSeats = numberOfSeats;
        }

        public override string ToString()
        {
            return $"{_name} ({_numberOfSeats} seats)";
        }
    }
}

