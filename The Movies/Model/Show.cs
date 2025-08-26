using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Movies.Model
{
    public class Show
    {
        //Properties

        private Movie _movie;
        private DateTime _showTime;
        private TimeSpan _duration;
        private DateTime _premiereDate;
        private Cinema _cinema;
        private Hall _hall;
        private HallClass _newHall;

        public Movie Movie
        {
            get { return _movie; }
            set { _movie = value; }
        }
        public DateTime ShowTime
        {
            get { return _showTime; }
            set { _showTime = value; }
        }
        public TimeSpan Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }
        public DateTime PremiereDate
        {
            get { return _premiereDate; }
            set { _premiereDate = value; }
        }

        public Cinema Cinema
        {
            get { return _cinema; }
            set { _cinema = value; }
        }

        public Hall Hall
        {
            get { return _hall; }
            set { _hall = value; }
        }

        public HallClass NewHall
        {
            get { return _newHall; }
            set { _newHall = value; }
        }

        public DateTime EndTime
        {
            get { return _showTime + _duration; }
        }

        // Readable display for Hall
        public string HallDisplay
        {
            get { return _hall.ToString().Replace("_", " "); }
        }

        // Constructor
        public Show(Movie movie, DateTime showTime, TimeSpan duration, DateTime premiereDate, Cinema cinema, HallClass newHall)
        {
            _movie = movie;
            _showTime = showTime;
            _duration = duration;
            _premiereDate = premiereDate;
            _cinema = cinema;
            _newHall = newHall;
        }

        // Formatting
        public override string ToString()
        {
            return $"{_movie.Title} - {_showTime.ToShortDateString()} {_showTime.ToShortTimeString()} at {_cinema?.Name}, Hall: {_newHall}";
        }

        // Methods
        public bool IsShowTimeValid()
        {
            return _showTime >= DateTime.Now && _showTime >= _premiereDate;
        }


    }
}
