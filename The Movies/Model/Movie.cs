using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Movies.Model
{
    public class Movie
    {
        // Properties
        private string _title;
        private double _duration;
        private string _genre;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        public double Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }
        public string Genre
        {
            get { return _genre; }
            set { _genre = value; }
        }

        // Constructor
        public Movie(string title, double duration, string genre)
        {
            _title = title;
            _duration = duration;
            _genre = genre;
        }

        // Methods

    }

    
}
