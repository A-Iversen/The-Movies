using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Movies.Model
{
    public class Hall
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
        public Hall(string name, int numberOfSeats)
        {
            _name = name;
            _numberOfSeats = numberOfSeats;
        }

        public override string ToString() => Name;
    }
}


