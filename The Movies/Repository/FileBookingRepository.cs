using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using The_Movies.Model;

namespace The_Movies.Repository
{
    public class FileBookingRepository
    {
        private string _filePath;
        private ObservableCollection<Booking> _bookingList;

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        public ObservableCollection<Booking> BookingList
        {
            get { return _bookingList; }
            set { _bookingList = value; }
        }

        // Constructor
        public FileBookingRepository(string filePath)
        {
            _filePath = filePath;
            _bookingList = new ObservableCollection<Booking>();

            LoadBookingsFromFile();
        }

        public void LoadBookingsFromFile()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    _bookingList.Clear();
                    string[] lines = File.ReadAllLines(_filePath);

                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        string[] parts = line.Split(',');
                        if (parts.Length >= 11) // Booking fields (3) + Show fields (8)
                        {
                            // Booking fields
                            string phoneText = parts[0].Trim();
                            string email = parts[1].Trim();
                            string qtyText = parts[2].Trim();

                            // Show fields
                            string title = parts[3].Trim();
                            string durationText = parts[4].Trim();
                            string genre = parts[5].Trim();
                            string director = parts[6].Trim();
                            string showTimeText = parts[7].Trim();
                            string premiereDateText = parts[8].Trim();
                            string cinemaName = parts[9].Trim();
                            string hallText = parts[10].Trim();

                            if (int.TryParse(phoneText, out int phone) &&
                                int.TryParse(qtyText, out int qtyTickets) &&
                                double.TryParse(durationText, out double minutes) &&
                                DateTime.TryParse(showTimeText, out DateTime showTime) &&
                                DateTime.TryParse(premiereDateText, out DateTime premiereDate) &&
                                Enum.TryParse(hallText, out Hall hall))
                            {
                                var movie = new Movie(title, minutes, genre, director);
                                var cinema = new Cinema { Name = cinemaName };
                                var show = new Show(movie, showTime, TimeSpan.FromMinutes(minutes), premiereDate, cinema, hall);

                                var booking = new Booking(phone, email, qtyTickets, show);
                                _bookingList.Add(booking);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading bookings: {ex.Message}");
            }
        }

        public void AddBooking(Booking booking)
        {
            _bookingList.Add(booking);
            SaveBookingsToFile();
        }

        public void RemoveBooking(Booking booking)
        {
            if (booking == null) return;

            _bookingList.Remove(booking);
            SaveBookingsToFile();
        }

        public Booking FindBooking(int phone)
        {
            foreach (var booking in _bookingList)
            {
                if (booking.Phone == phone)
                    return booking;
            }
            return null;
        }

        public void SaveBookingsToFile()
        {
            try
            {
                List<string> lines = new List<string>();
                foreach (var booking in _bookingList)
                {
                    string line = $"{booking.Phone},{booking.Email},{booking.QtyTickets}," +
                                  $"{booking.Show.Movie.Title},{booking.Show.Duration.TotalMinutes}," +
                                  $"{booking.Show.Movie.Genre},{booking.Show.Movie.Director}," +
                                  $"{booking.Show.ShowTime},{booking.Show.PremiereDate}," +
                                  $"{booking.Show.Cinema?.Name},{booking.Show.Hall}";
                    lines.Add(line);
                }
                File.WriteAllLines(_filePath, lines);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving bookings: {ex.Message}");
            }
        }
    }
}
