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
using The_Movies.Repository;  // Husk at importere repository-navneområdet

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
            DataContext = mvm;
            mvm.LoadMoviesCommand.Execute(null);

            // Bind Shows tab til ShowViewModel, som bruger eksisterende movies
            svm = new ShowViewModel(mvm.MovieList);
            if (ShowsTab != null)
            {
                ShowsTab.DataContext = svm;
            }

            // --- HER TILFØJER VI BookingViewModel ---

           
            bookingRepository = new FileBookingRepository("bookings.txt");

            // Lav BookingViewModel med bookingRepository, showRepository og movieRepository
            bvm = new BookingViewModel(bookingRepository, mvm.MovieRepository);

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