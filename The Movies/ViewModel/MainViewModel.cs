using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using The_Movies.Repository;
using The_Movies.ViewModel;

namespace The_Movies.ViewModel
{
   public class MainViewModel
    {

        public MovieViewModel MovieVM { get; }
        public ShowViewModel ShowVM { get; }

        public MainViewModel()
        {
            MovieVM = new MovieViewModel();
            ShowVM = new ShowViewModel();
        }



    }
}
