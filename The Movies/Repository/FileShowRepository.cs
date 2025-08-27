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

        private ObservableCollection<Cinema> _cinemas;
    

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

        public ObservableCollection<Cinema> Cinemas
        {
            get { return _cinemas; }
            set { _cinemas = value; }
        }


        // Constructor
        public FileShowRepository(string filePath, ObservableCollection<Cinema> cinemas)
        {
            _filePath = filePath;
            _showList = new ObservableCollection<Show>();
            _cinemas = cinemas;

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
                                DateTime.TryParse(premiereDateText, out DateTime premiereDate))
                            {
                                // Find den rigtige biograf
                                var cinema = Cinemas.FirstOrDefault(c => c.Name.Equals(cinemaName, StringComparison.OrdinalIgnoreCase));

                                if (cinema != null)
                                {
                                    // Find den rigtige sal i biografen
                                    var hall = cinema.Halls.FirstOrDefault(h => h.Name.Equals(hallText, StringComparison.OrdinalIgnoreCase));

                                    if (hall != null)
                                    {
                                        var movie = new Movie(title, minutes, genre, director);
                                        var show = new Show(movie, showTime, TimeSpan.FromMinutes(minutes), premiereDate, cinema, hall);
                                        _showList.Add(show);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Sal '{hallText}' findes ikke i biografen '{cinemaName}'.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Biografen '{cinemaName}' blev ikke fundet.");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fejl ved indlæsning af shows: {ex.Message}");
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
                    string line = $"{show.Movie.Title},{show.Duration.TotalMinutes},{show.Movie.Genre},{show.Movie.Director},{show.ShowTime},{show.PremiereDate},{show.Cinema?.Name},{show.Hall?.Name}";
                    lines.Add(line);
                    Console.WriteLine($"Saving show line: {line}"); // Debug output
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
