using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Movies.Model;

namespace The_Movies.Repository
{
    public class FileMovieRepository
    {
        private string _filePath;
        private ObservableCollection<Movie> movieList;

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }
        public ObservableCollection<Movie> MovieList
        {
            get { return movieList; }
            set { movieList = value; }
        }

        // Constructor
        public FileMovieRepository(string filePath)
        {
            _filePath = filePath;
            movieList = new ObservableCollection<Movie>();
            
            // Try to load existing movies from file
            LoadMoviesFromFile();
        }

        public void LoadMoviesFromFile()
        {
            try
            {
                if (System.IO.File.Exists(_filePath))
                {
                    // Ensure we don't duplicate entries when reloading
                    movieList.Clear();
                    string[] lines = System.IO.File.ReadAllLines(_filePath);
                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }

                        string[] parts = line.Split(',');
                        if (parts.Length >= 4)
                        {
                            string title = parts[0].Trim();
                            string durationText = parts[1].Trim();
                            string genre = parts[2].Trim();
                            string director = parts[3].Trim();

                            if (double.TryParse(durationText, out double duration))
                            {
                                Movie movie = new Movie(title, duration, genre, director);
                                movieList.Add(movie);
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

        public void SaveMoviesToFile()
        {
            try
            {
                using var writer = new System.IO.StreamWriter(_filePath, false);
                foreach (var movie in movieList)
                {
                    writer.WriteLine($"{movie.Title},{movie.Duration},{movie.Genre},{movie.Director}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save movies to file: {ex.Message}");
            }
        }

        // Methods
        public void AddMovie(Movie movie)
        {
            movieList.Add(movie);

            try
            {
                using var writer = new System.IO.StreamWriter(_filePath, true);
                writer.WriteLine($"{movie.Title},{movie.Duration},{movie.Genre},{movie.Director}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save movie to file: {ex.Message}");
            }
        }
        
    }
}
