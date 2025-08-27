using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Movies.Model;
using The_Movies.Repository;
using The_Movies.RelayCommand;
using System.Windows.Data;

namespace The_Movies.ViewModel
{
    public class ShowViewModel : INotifyPropertyChanged
    {
        private Show _selectedShow;
        private FileShowRepository _repository; 
        private ObservableCollection<Movie> _movieList;
        public ObservableCollection<Cinema> Cinemas { get; } = new();
        public ObservableCollection<TimeSpan> AvailableTimes { get; } = new();
        public event PropertyChangedEventHandler PropertyChanged;
        private Cinema _selectedCinema;
        private Hall _selectedHall;
        private Movie _selectedMovieForShow;
        private string _durationMinutesText;
        private DateTime? _selectedDate;
        private TimeSpan _selectedTime;

        public ObservableCollection<Show> ShowList
        {
            get => _repository.ShowList;
            set
            {
                _repository.ShowList = value;
                OnPropertyChanged(nameof(ShowList));
                ShowsView = CollectionViewSource.GetDefaultView(_repository.ShowList);
                if (ShowsView != null)
                {
                    ShowsView.Filter = FilterShowByCinema;
                }
            }
        }
        public ObservableCollection<Movie> MovieList
        {
            get => _movieList;
            set { _movieList = value; OnPropertyChanged(nameof(MovieList)); }
        }
        public Show SelectedShow
        {
            get => _selectedShow;
            set
            {
                _selectedShow = value;
                OnPropertyChanged(nameof(SelectedShow));
            }
        }
        public Cinema SelectedCinema
        {
            get { return _selectedCinema; }
            set 
            { _selectedCinema = value; OnPropertyChanged(nameof(SelectedCinema)); 
                OnPropertyChanged(nameof(AvailableHalls)); 
                ShowsView?.Refresh(); 
            }
        }

        public List<Hall> AvailableHalls => SelectedCinema?.Halls;
       
        
        public Hall SelectedHall
        {
            get { return _selectedHall; }
            set { _selectedHall = value; OnPropertyChanged(nameof(SelectedHall)); ShowsView?.Refresh(); }
        }
        public Movie SelectedMovieForShow
        {
            get { return _selectedMovieForShow; }
            set
            {
                _selectedMovieForShow = value;
                OnPropertyChanged(nameof(SelectedMovieForShow));
                UpdateDurationFromSelectedMovie();
            }
        }
        public DateTime? SelectedDate
        {
            get { return _selectedDate; }
            set { _selectedDate = value; OnPropertyChanged(nameof(SelectedDate)); }
        }
        public TimeSpan SelectedTime
        {
            get { return _selectedTime; }
            set { _selectedTime = value; OnPropertyChanged(nameof(SelectedTime)); }
        }
        public string DurationMinutesText
        {
            get { return _durationMinutesText; }
            set { _durationMinutesText = value; OnPropertyChanged(nameof(DurationMinutesText)); }
        }

        public ShowViewModel(ObservableCollection<Movie> movieList)
        {
            _selectedShow = null;
            _repository = new FileShowRepository("shows.txt", Cinemas.ToList());
            _movieList = movieList;

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

            // Seed sample cinemas
            //Cinemas.Add(new Cinema { Name = "Downtown Cinema", Halls = { "Sal_1", "Sal_2", "Sal_3" } });
            //Cinemas.Add(new Cinema { Name = "Riverside Multiplex", Halls = { "Sal_1", "Sal_2" } });
            //Cinemas.Add(new Cinema { Name = "Grand Palace", Halls = { "Sal_1", "Sal_2", "Sal_3" } });
            // Halls from enum
            //foreach (var hall in Enum.GetValues(typeof(Hall)).Cast<Hall>())
           // {
              //  Halls.Add(hall);
               // HallOptions.Add(new KeyValuePair<Hall, string>(hall, hall.ToString().Replace("_", " ")));
           // }

            // 15-minute intervals times
            for (int h = 0; h < 24; h++)
            {
                for (int m = 0; m < 60; m += 15)
                {
                    AvailableTimes.Add(new TimeSpan(h, m, 0));
                }
            }

            CreateShowCommand = new RelayCommand.RelayCommand(CreateShow);
            RemoveShowCommand = new RelayCommand.RelayCommand(RemoveSelectedShow, CanRemoveSelectedShow);

            // Create a filtered view for shows
            ShowsView = CollectionViewSource.GetDefaultView(ShowList);
            if (ShowsView != null)
            {
                ShowsView.Filter = FilterShowByCinema;
            }
        }

       

        public ICollectionView ShowsView { get; private set; }

        public RelayCommand.RelayCommand CreateShowCommand { get; }
        public RelayCommand.RelayCommand RemoveShowCommand { get; }
        private void CreateShow(object parameter)
        {
            try
            {
                if (SelectedMovieForShow == null)
                {
                    System.Windows.MessageBox.Show("Please select a movie.");
                    return;
                }

                if (SelectedDate == null)
                {
                    System.Windows.MessageBox.Show("Vælg en dato.");
                    return;
                }

                DateTime showTime = SelectedDate.Value.Date + SelectedTime;

                // Calculate minutes from selected movie duration + 30 minutes cleaning
                double minutes = (SelectedMovieForShow?.Duration ?? 0) + 30;
                if (minutes <= 0)
                {
                    System.Windows.MessageBox.Show("Ugyldig varighed.");
                    return;
                }

                if (SelectedCinema == null)
                {
                    System.Windows.MessageBox.Show("Vælg en biograf.");
                    return;
                }

                // Ensure a valid hall is selected (allow Sal_1 too)
                //if (!Halls.Contains(SelectedHall))
                //{
                  //  System.Windows.MessageBox.Show("Vælg en gyldig sal (f.eks. Sal_1, Sal_2, Sal_3).");
                  //  return;
                //}

                var cinema = SelectedCinema;

                // Validate no overlap in same cinema and hall
                TimeSpan duration = TimeSpan.FromMinutes(minutes);
                if (!IsHallAvailable(showTime, duration, cinema, SelectedHall))
                {
                    System.Windows.MessageBox.Show("Denne sal er optaget på det tidspunkt. Vælg et andet tidspunkt eller en anden sal.");
                    return;
                }

                var show = new Show(SelectedMovieForShow, showTime, duration, showTime.Date, cinema, SelectedHall);

                _repository.AddShow(show);
                OnPropertyChanged(nameof(ShowList));
                ShowsView?.Refresh();
                System.Windows.MessageBox.Show("Show created successfully.");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error creating show: {ex.Message}");
            }
        }

        private bool IsHallAvailable(DateTime newStart, TimeSpan newDuration, Cinema cinema, Hall hall)
        {
            DateTime newEnd = newStart + newDuration;
            foreach (var existing in ShowList)
            {
                if (!string.Equals(existing.Cinema?.Name, cinema?.Name, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                if (!string.Equals(existing.Hall?.Name, hall?.Name, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                DateTime existStart = existing.ShowTime;
                DateTime existEnd = existStart + existing.Duration;

                // Overlap if windows intersect
                bool overlap = newStart < existEnd && existStart < newEnd;
                if (overlap)
                {
                    return false;
                }
            }
            return true;
        }

        private bool CanRemoveSelectedShow(object parameter)
        {
            return SelectedShow != null;
        }

        private void RemoveSelectedShow(object parameter)
        {
            try
            {
                if (SelectedShow == null)
                {
                    return;
                }

                _repository.RemoveShow(SelectedShow);
                OnPropertyChanged(nameof(ShowList));
                ShowsView?.Refresh();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error removing show: {ex.Message}");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

      

        private bool FilterShowByCinema(object obj)
        {
            if (SelectedCinema == null)
            {
                return true;
            }
            if (obj is Show show)
            {
                bool sameCinema = string.Equals(show.Cinema?.Name, SelectedCinema.Name, StringComparison.OrdinalIgnoreCase);
                if (!sameCinema)
                {
                    return false;
                }
                // If a hall is selected, also filter by that hall
                if (SelectedHall != null)
                {
                    return string.Equals(show.Hall?.Name, SelectedHall.Name, StringComparison.OrdinalIgnoreCase);
                }
                return true;
            }
            return true;
        }

        // Show creation
       

        private void UpdateDurationFromSelectedMovie()
        {
            double minutes = (SelectedMovieForShow?.Duration ?? 0) + 30;
            DurationMinutesText = minutes > 0 ? minutes.ToString() : string.Empty;
        }


       
    }
}
