using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Movies.Model;

namespace The_Movies.Repository
{
    public class FileShowRepository
    {
        private string _filePath;
        private List<Show> _showList;

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }
        public List<Show> ShowList
        {
            get { return _showList; }
            set { _showList = value; }
        }

        // Constructor
        public FileShowRepository(string filePath)
        {
            _filePath = filePath;
            _showList = new List<Show>();
            
            // Try to load existing movies from file
            LoadShowsFromFile();
        }

        private void LoadShowsFromFile()
        {
            try
            {
                if (System.IO.File.Exists(_filePath))
                {
                    string[] lines = System.IO.File.ReadAllLines(_filePath);
                    foreach (string line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            string[] parts = line.Split(',');
                            if (parts.Length == 3)
                            {
                                if (double.TryParse(parts[1], out double duration))
                                {
                                    Show show = new Show(new Movie(parts[0], duration, parts[2], parts[3]), DateTime.Parse(parts[4]), TimeSpan.FromMinutes(duration), DateTime.Parse(parts[5]), new Cinema(parts[6]), new Hall(parts[7]));

                                    _showList.Add(show);
                                }
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
        private void SaveShowsToFile()
        {
            try
            {
                List<string> lines = new List<string>();
                foreach (var show in _showList)
                {
                    string line = $"{show.Movie.Title},{show.Duration.TotalMinutes},{show.Movie.Genre},{show.Movie.Director},{show.ShowTime},{show.PremiereDate},{show.Cinema.Name},{show.Hall.Name}";
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
