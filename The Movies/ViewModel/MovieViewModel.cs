using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using The_Movies.Model;
using The_Movies.RelayCommand;
using The_Movies.Repository;

namespace The_Movies.ViewModel 
{
    public class MovieViewModel : INotifyPropertyChanged
    {
        private Movie _currentMovie;
        private FileMovieRepository _repository;
        private FileShowRepository _showRepository;
        private Movie _selectedMovie;
        public event PropertyChangedEventHandler PropertyChanged;


        public Movie SelectedMovie
        {
            get => _selectedMovie;
            set
            {
                _selectedMovie = value;
                OnPropertyChanged(nameof(SelectedMovie));
            }
        }
        public string Title
        {
            get { return _currentMovie.Title; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    MessageBox.Show("Title cannot be empty.");
                    return;
                }
                _currentMovie.Title = value;
                OnPropertyChanged();
            }
        }
        public double Duration
        {
            get { return _currentMovie.Duration; }
            set
            {
                if (value <= 0)
                {
                    MessageBox.Show("Duration must be greater than zero.");
                    return;
                }
                _currentMovie.Duration = value;
                OnPropertyChanged();
            }
        }
        public string Genre
        {
            get { return _currentMovie.Genre; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    MessageBox.Show("Genre cannot be empty.");
                    return;
                }
                _currentMovie.Genre = value;
                OnPropertyChanged();
            }
        }
        public string Director
        {
            get { return _currentMovie.Director; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    MessageBox.Show("Director cannot be empty.");
                    return;
                }
                _currentMovie.Director = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Movie> MovieList
        {
            get { return _repository.MovieList; }
            set
            {
                _repository.MovieList = value;
                OnPropertyChanged();
            }
        }


        // Constructor
        public MovieViewModel()
        {
            _repository = new FileMovieRepository("movies.txt");
            _showRepository = new FileShowRepository("shows.txt");
            _currentMovie = new Movie("", 0, "", "");
            LoadMoviesCommand = new RelayCommand.RelayCommand(LoadMovies);
            CreateMovieCommand = new RelayCommand.RelayCommand(CreateMovie, CanCreateMovie);
            RemoveMovieCommand = new RelayCommand.RelayCommand(RemoveMovie);
        }
        
        public ICommand LoadMoviesCommand { get; private set; }
        public ICommand CreateMovieCommand { get; private set; }
        public ICommand RemoveMovieCommand { get; }

       

        private void LoadMovies(object obj)
        {
            _repository.LoadMoviesFromFile();
            MovieList = _repository.MovieList;
            OnPropertyChanged(nameof(MovieList));
        }
        private bool CanCreateMovie(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Title) && 
                   !string.IsNullOrWhiteSpace(Genre) && 
                   Duration > 0 &&
                   !string.IsNullOrWhiteSpace(Director);
        }

        private void RemoveMovie(object parameter)
        {
            if (SelectedMovie != null)
            {
                MovieList.Remove(SelectedMovie);
                SelectedMovie = null;
                _repository.SaveMoviesToFile();
            }
        }

        private void CreateMovie(object parameter)
        {
            try
            {
                // Opretter en ny film med de nuværende værdier
                Movie newMovie = new Movie(Title, Duration, Genre, Director);
                
                // Gemmer film til repository
                _repository.AddMovie(newMovie);
                MessageBox.Show($"Movie created successfully: {Title} - {Genre} ({Duration} minutes) - {Director}");
                
                // Nulstiller formularen så vi kan oprette en ny film
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating movie: {ex.Message}");
            }
        }

        private void ClearForm()
        {
            // Set the Movie object directly (bypasses property setters)
            _currentMovie.Title = "";
            _currentMovie.Duration = 0;
            _currentMovie.Genre = "";
            _currentMovie.Director = "";

            // Tell the UI to update
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Duration));
            OnPropertyChanged(nameof(Genre));
            OnPropertyChanged(nameof(Director));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        

    }

}
