using System;
using System.Collections.Generic;
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

        public event PropertyChangedEventHandler PropertyChanged;

        public MovieViewModel()
        {
            _repository = new FileMovieRepository("movies.txt");
            _currentMovie = new Movie("", 0, "");
            CreateMovieCommand = new RelayCommand.RelayCommand(CreateMovie, CanCreateMovie);
        }

        public ICommand CreateMovieCommand { get; private set; }

        private bool CanCreateMovie(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Title) && 
                   !string.IsNullOrWhiteSpace(Genre) && 
                   Duration > 0;
        }

        private void CreateMovie(object parameter)
        {
            try
            {
                // Opretter en ny film med de nuværende værdier
                Movie newMovie = new Movie(Title, Duration, Genre);
                
                // Gemmer film til repository
                _repository.AddMovie(newMovie);
                
                MessageBox.Show($"Movie created successfully: {Title} - {Genre} ({Duration} minutes)");
                
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
            
            // Tell the UI to update
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Duration));
            OnPropertyChanged(nameof(Genre));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

    }

}
