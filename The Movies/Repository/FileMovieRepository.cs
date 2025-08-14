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
        }

        // Methods
        public void AddMovie(Movie movie)
        {
            movieList.Add(movie);

            using var writer = new System.IO.StreamWriter(_filePath, true);
            writer.WriteLine($"{movie.Title},{movie.Duration},{movie.Genre}");
        }
        
    }
}
