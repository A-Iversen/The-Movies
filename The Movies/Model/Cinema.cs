using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Movies.Model
{
    public class Cinema
    {
        private string _name;
        private List<Show> _showList;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public List<Show> ShowList
        {
            get { return _showList; }
            set { _showList = value; }
        }

        // Constructor
        public Cinema(string name)
        {
            _name = name;
            _showList = new List<Show>();
        }

        // cinema examples
        public static List<Cinema> GetExampleCinemas()
        {
            return new List<Cinema>
            {
                new Cinema("Biffen"),
                new Cinema("Popcorn"),
                new Cinema("Den Tredje")
            };
        }



    }
}
