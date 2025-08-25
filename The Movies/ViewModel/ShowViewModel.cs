using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Movies.Model;
using The_Movies.Repository;

namespace The_Movies.ViewModel
{
    public class ShowViewModel : INotifyPropertyChanged
    {
        private Show _selectedShow;
        private FileShowRepository _repository; 
        public ObservableCollection<Cinema> Cinemas { get; } = new();
        public event PropertyChangedEventHandler PropertyChanged;

        public ShowViewModel(Cinema cinema)
        {
            _selectedShow = new Show(null, DateTime.Now, TimeSpan.Zero, DateTime.Now, null);
            _repository = new FileShowRepository("shows.txt");
            Cinemas.Add(new Cinema { Name = "Downtown Cinema", Halls = { "Hall 1", "Hall 2", "Hall 3" } });
            Cinemas.Add(new Cinema { Name = "Riverside Multiplex", Halls = { "A", "B", "C", "D" } });
            Cinemas.Add(new Cinema { Name = "Grand Palace", Halls = { "Main", "Blue", "Red" } });
            CreateShowCommand = new RelayCommand.RelayCommand(CreateShow);
        }

        public ObservableCollection<Show> ShowList
        {
            get => _repository.ShowList;
            set
            {
                _repository.ShowList = value;
                OnPropertyChanged(nameof(ShowList));
            }
        }

        public RelayCommand.RelayCommand CreateShowCommand { get; }
        private void CreateShow(object parameter)
        {
            if (_selectedShow != null && _selectedShow.IsShowTimeValid())
            {
                _repository.AddShow(_selectedShow);
                OnPropertyChanged(nameof(ShowList));
            }
            else
            {
                // Handle invalid show time (e.g., show a message to the user)
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
    }
}
