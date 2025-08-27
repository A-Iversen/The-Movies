using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using The_Movies.ViewModel;
using The_Movies.Repository;
using The_Movies.Model;
using System.Collections.ObjectModel;
// Husk at importere repository-navneområdet

namespace The_Movies
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MovieViewModel mvm = new MovieViewModel();
        ShowViewModel svm;
        BookingViewModel bvm;
        FileBookingRepository bookingRepository; // Tilføjet


        public MainWindow()
        {
            InitializeComponent();

            var sharedCinemas = new ObservableCollection<Cinema>
{
    new Cinema("Biffen")
    {
        Halls = new List<Hall>
        {
            new Hall("Sal 1", 100),
            new Hall("Sal 2", 80),
            new Hall("Sal 3", 50)
        }
    },
    new Cinema("Popcorn")
    {
        Halls = new List<Hall>
        {
            new Hall("Sal 1", 120),
            new Hall("Sal 2", 90)
        }
    },
    new Cinema("Den tredje")
    {
        Halls = new List<Hall>
        {
            new Hall("Sal 1", 150),
            new Hall("Sal 2", 100),
            new Hall("Sal 3", 70)
        }
    }
};
            var showRepository = new FileShowRepository("shows.txt", sharedCinemas);
            showRepository.LoadShowsFromFile();

            DataContext = mvm;
            mvm.LoadMoviesCommand.Execute(null);

            // Bind Shows tab til ShowViewModel, som bruger eksisterende movies
            svm = new ShowViewModel(mvm.MovieList, showRepository, sharedCinemas);
            if (ShowsTab != null)
            {
                ShowsTab.DataContext = svm;
            }

            // --- HER TILFØJER VI BookingViewModel ---

           
            bookingRepository = new FileBookingRepository("bookings.txt");

            // Lav BookingViewModel med bookingRepository, showRepository og movieRepository
            bvm = new BookingViewModel(bookingRepository, mvm.MovieRepository, showRepository, sharedCinemas);

            if (BookingTab != null)
            {
                BookingTab.DataContext = bvm;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // ...
        }
    }
}