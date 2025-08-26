using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Movies.Model;

namespace The_Movies.Repository
{
    public class FileShowRepository
    {
        private string _filePath;
        private ObservableCollection<Show> _showList;

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }
        public ObservableCollection<Show> ShowList
        {
            get { return _showList; }
            set { _showList = value; }
        }

        // Constructor
        public FileShowRepository(string filePath)
        {
            _filePath = filePath;
            _showList = new ObservableCollection<Show>();
            
            // Try to load existing movies from file
            LoadShowsFromFile();
        }

        public void LoadShowsFromFile()
        {
            try
            {
                if (System.IO.File.Exists(_filePath))
                {
                    _showList.Clear();
                    string[] lines = System.IO.File.ReadAllLines(_filePath);
                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        string[] parts = line.Split(',');
                        if (parts.Length >= 8)
                        {
                            string title = parts[0].Trim();
                            string durationText = parts[1].Trim();
                            string genre = parts[2].Trim();
                            string director = parts[3].Trim();
                            string showTimeText = parts[4].Trim();
                            string premiereDateText = parts[5].Trim();
                            string cinemaName = parts[6].Trim();
                            string hallText = parts[7].Trim();

                            if (double.TryParse(durationText, out double minutes) &&
                                DateTime.TryParse(showTimeText, out DateTime showTime) &&
                                DateTime.TryParse(premiereDateText, out DateTime premiereDate) &&
                                Enum.TryParse(hallText, out Hall hall))
                            {
                                var movie = new Movie(title, minutes, genre, director);
                                var cinema = new Cinema { Name = cinemaName };
                                var show = new Show(movie, showTime, TimeSpan.FromMinutes(minutes), premiereDate, cinema, hall);
                                _showList.Add(show);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // If there's an error loading, just start with an empty list
                System.Diagnostics.Debug.WriteLine($"Error loading movies: {ex.Message}");
            }
        }

        // Methods
        public void AddShow(Show show)
        {
            _showList.Add(show);
            SaveShowsToFile();
        }

        public void RemoveShow(Show show)
        {
            if (show == null)
            {
                return;
            }
            _showList.Remove(show);
            SaveShowsToFile();
        }
        private void SaveShowsToFile()
        {
            try
            {
                List<string> lines = new List<string>();
                foreach (var show in _showList)
                {
                    string line = $"{show.Movie.Title},{show.Duration.TotalMinutes},{show.Movie.Genre},{show.Movie.Director},{show.ShowTime},{show.PremiereDate},{show.Cinema?.Name},{show.Hall}";
                    lines.Add(line);
                }
                System.IO.File.WriteAllLines(_filePath, lines);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving shows: {ex.Message}");
            }
        }


    }
}
