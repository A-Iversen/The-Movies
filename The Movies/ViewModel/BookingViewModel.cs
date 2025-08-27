using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using The_Movies.Model;
using The_Movies.Repository;

namespace The_Movies.ViewModel
{
    public class BookingViewModel : INotifyPropertyChanged
    {
        public int PhoneNumber { get; set; }
        public string Email { get; set; }
        public int QtyTickets { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private Booking _selectedBooking;
        private FileBookingRepository _repository;
        private FileShowRepository _showRepository;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BookingViewModel(FileBookingRepository bookingRepository, FileShowRepository showRepository)
        {
            _repository = bookingRepository;
            _showRepository = showRepository;

            _repository.LoadBookingsFromFile();

            CreateBookingCommand = new RelayCommand.RelayCommand(_ => CreateBooking(), _ => CanCreateBooking());
            RemoveBookingCommand = new RelayCommand.RelayCommand(_ => RemoveBooking(), _ => SelectedBooking != null);
            ClearFormCommand = new RelayCommand.RelayCommand(_ => ClearForm());
        }

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
        public ObservableCollection<Booking> BookingList
        {
            get { return _repository.BookingList; }
            set
            {
                _repository.BookingList = value;
                OnPropertyChanged(nameof(BookingList));
            }
        }

        private void LoadBookings()
        {
            _repository.LoadBookingsFromFile();
        }

        private bool CanCreateBooking()
        {
            return !string.IsNullOrWhiteSpace(Email) && PhoneNumber > 0 && QtyTickets > 0;
        }

        public ICommand CreateBookingCommand { get; }
        public ICommand RemoveBookingCommand { get; }
        public ICommand ClearFormCommand { get; }

        private void RemoveBooking()
        {
            if (SelectedBooking != null)
            {
                _repository.BookingList.Remove(SelectedBooking);
                _repository.SaveBookingsToFile();
            }
        }

        private void CreateBooking()
        {
            if (CanCreateBooking())
            {
                Booking newBooking = new Booking(PhoneNumber, Email, QtyTickets, null);
                _repository.BookingList.Add(newBooking);
                _repository.SaveBookingsToFile();
                OnPropertyChanged(nameof(BookingList));
            }
        }

        private void ClearForm()
        {
            PhoneNumber = 0;
            Email = string.Empty;
            QtyTickets = 0;
            OnPropertyChanged(nameof(PhoneNumber));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(QtyTickets));
        }
    }
}
