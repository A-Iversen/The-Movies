using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Movies.Model;

namespace The_Movies.Repository
{
    public class FileMovieRepository
    {
        private string _filePath;
        private List<Movie> movieList;

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }
        public List<Movie> MovieList
        {
            get { return movieList; }
            set { movieList = value; }
        }

        // Constructor
        public FileMovieRepository(string filePath)
        {
            _filePath = filePath;
            movieList = new List<Movie>();
            
            // Try to load existing movies from file
            LoadMoviesFromFile();
        }

        private void LoadMoviesFromFile()
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
                                    Movie movie = new Movie(parts[0], duration, parts[2]);
                                    movieList.Add(movie);
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
        public void AddMovie(Movie movie)
        {
            movieList.Add(movie);

            try
            {
                using var writer = new System.IO.StreamWriter(_filePath, true);
                writer.WriteLine($"{movie.Title},{movie.Duration},{movie.Genre}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save movie to file: {ex.Message}");
            }
        }
        
    }
}
