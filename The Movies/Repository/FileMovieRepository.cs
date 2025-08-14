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
        // filepath
        private string _filePath;
        public List<Movie> movieList;

        // Constructor
        public FileMovieRepository(string filePath)
        {
            _filePath = filePath;
        }
        // Methods to read and write movies to the file
        public void AddMovie(Movie movie)
        {
            movieList.Add(movie);

            using var writer = new System.IO.StreamWriter(_filePath, true);
            writer.WriteLine($"{movie.Title},{movie.Duration},{movie.Genre}");
        }

    }
}
