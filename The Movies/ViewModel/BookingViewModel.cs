using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using The_Movies.Model;
using The_Movies.Repository;

namespace The_Movies.ViewModel
{
    public class BookingViewModel : INotifyPropertyChanged
    {
        private FileBookingRepository _bookingRepository;
        private FileShowRepository _showRepository;
        private FileMovieRepository _movieRepository;
        private ShowViewModel _showViewModel;

        // Collections bound to UI
        public ObservableCollection<Cinema> Cinemas { get; }
        public ObservableCollection<Movie> MovieList { get; }
        public ObservableCollection<Show> AvailableShows { get; }

        // Booking fields
        private string _email;
        private int _phoneNumber;
        private int _qtyTickets;

        // Selections
        private Cinema _selectedCinema;
        private Movie _selectedMovie;
        private Show _selectedShow;
        private DateTime _selectedDate;

        public BookingViewModel(FileBookingRepository bookingRepository, FileMovieRepository movieRepository)
        {
            _bookingRepository = bookingRepository;
            _movieRepository = movieRepository;
             MovieList = new ObservableCollection<Movie>();
             AvailableShows = new ObservableCollection<Show>();

            _showRepository = new FileShowRepository("shows.txt", Cinemas);

            Cinemas = new ObservableCollection<Cinema>
            {
            new Cinema("Biffen")
            {
                Halls = new List<Hall>
                {
                    new Hall("Sal 1", 100 ),
                    new Hall("Sal 2", 80 ),
                    new Hall("Sal 3", 50 )
                }
            },
            new Cinema("Popcorn")
            {
                Halls = new List<Hall>
                {
                    new Hall("Sal 1", 120 ),
                    new Hall("Sal 2", 90 )
                }
            },
            new Cinema("Den tredje")
            {
                Halls = new List<Hall>
                {
                    new Hall("Sal 1", 150 ),
                    new Hall("Sal 2", 100 ),
                    new Hall("Sal 3", 70 )
                }
            }
            };




            SelectedDate = DateTime.Today;

            CreateBookingCommand = new RelayCommand.RelayCommand(_ => CreateBooking(), _ => CanCreateBooking());
            RemoveBookingCommand = new RelayCommand.RelayCommand(_ => RemoveBooking(), _ => SelectedBooking != null);
            ClearFormCommand = new RelayCommand.RelayCommand(_ => ClearForm());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public ObservableCollection<Booking> BookingList
        {
            get => _bookingRepository.BookingList;
            set
            {
                _bookingRepository.BookingList = value;
                OnPropertyChanged();
            }
        }

        private Booking _selectedBooking;
        public Booking SelectedBooking
        {
            get => _selectedBooking;
            set
            {
                if (_selectedBooking != value)
                {
                    _selectedBooking = value;
                    OnPropertyChanged();
                }
            }
        }

        public Cinema SelectedCinema
        {
            get => _selectedCinema;
            set
            {
                if (_selectedCinema != value)
                {
                    _selectedCinema = value;
                    OnPropertyChanged();
                    UpdateMovies();
                    SelectedMovie = null;
                    AvailableShows.Clear();
                    SelectedShow = null;
                }
            }
        }

        private void UpdateMovies()
        {
            MovieList.Clear();
            if (SelectedCinema == null)
                return;

            // Find unikke film der vises i den valgte biograf via shows
            var movies = _showRepository.ShowList
                .Where(s => s.Cinema == SelectedCinema)
                .Select(s => s.Movie)
                .Distinct()
                .OrderBy(m => m.Title);

            foreach (var movie in movies)
            {
                MovieList.Add(movie);
            }
        }

        public Movie SelectedMovie
        {
            get => _selectedMovie;
            set
            {
                if (_selectedMovie != value)
                {
                    _selectedMovie = value;
                    OnPropertyChanged();
                    UpdateAvailableShows();
                    SelectedShow = null;
                }
            }
        }

        public Show SelectedShow
        {
            get => _selectedShow;
            set
            {
                if (_selectedShow != value)
                {
                    _selectedShow = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged();
                    UpdateAvailableShows();
                    SelectedShow = null;
                }
            }
        }

        private void UpdateAvailableShows()
        {
            AvailableShows.Clear();
            if (SelectedCinema == null || SelectedMovie == null)
                return;

            var shows = _showRepository.ShowList
                .Where(s => s.Cinema == SelectedCinema &&
                            s.Movie == SelectedMovie &&
                            s.ShowTime.Date == SelectedDate.Date)
                .OrderBy(s => s.ShowTime);

            foreach (var show in shows)
            {
                AvailableShows.Add(show);
            }
        }

        // Booking fields
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        public int PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public int QtyTickets
        {
            get => _qtyTickets;
            set
            {
                if (_qtyTickets != value)
                {
                    _qtyTickets = value;
                    OnPropertyChanged();
                }
            }
        }

        // Commands
        public ICommand CreateBookingCommand { get; }
        public ICommand RemoveBookingCommand { get; }
        public ICommand ClearFormCommand { get; }

        private bool CanCreateBooking()
        {
            return !string.IsNullOrWhiteSpace(Email) &&
                   PhoneNumber > 0 &&
                   QtyTickets > 0 &&
                   SelectedShow != null;
        }

        private void CreateBooking()
        {
            if (!CanCreateBooking())
                return;

            var booking = new Booking(PhoneNumber, Email, QtyTickets, SelectedShow);
            _bookingRepository.AddBooking(booking);

            OnPropertyChanged(nameof(BookingList));

            ClearForm();
        }

        private void RemoveBooking()
        {
            if (SelectedBooking != null)
            {
                _bookingRepository.RemoveBooking(SelectedBooking);
                OnPropertyChanged(nameof(BookingList));
            }
        }

        private void ClearForm()
        {
            Email = string.Empty;
            PhoneNumber = 0;
            QtyTickets = 0;
            SelectedCinema = null;
            MovieList.Clear();
            SelectedMovie = null;
            AvailableShows.Clear();
            SelectedShow = null;
            SelectedDate = DateTime.Today;
        }
    }
}