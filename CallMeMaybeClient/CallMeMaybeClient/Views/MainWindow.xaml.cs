using System.Net.NetworkInformation;
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
using CallMeMaybeClient.Services;
using CallMeMaybeClient.Views;
using static CallMeMaybeClient.Services.RoleManager;


namespace CallMeMaybeClient.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private List<Key> _pressedKeys = new List<Key>(); // Liste pour suivre les touches pressées
        private readonly List<Key> _secretCode = new List<Key> { Key.Space }; // La combinaison de touches
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new SalariePage());
            this.KeyDown += MainWindow_KeyDown; // Capture de la touche pressée


        }

        private void NavigateToSalaries(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SalariePage());
        }

        private void NavigateToServices(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ServicePage());
        }

        private void NavigateToSites(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SitePage()); 
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            _pressedKeys.Add(e.Key);

            // Si la taille des touches pressées dépasse la taille de la combinaison, on retire les touches précédentes
            if (_pressedKeys.Count > _secretCode.Count)
                _pressedKeys.RemoveAt(0);

            // Si les touches pressées correspondent à la combinaison secrète
            if (_pressedKeys.SequenceEqual(_secretCode))
            {
                var adminWindow = new InputDialogueAdmin();
                adminWindow.ShowDialog();

                    MainWindow newWindow = new MainWindow();
                    Application.Current.MainWindow = newWindow;
                    this.Visibility = System.Windows.Visibility.Collapsed;
                    newWindow.Show();


                

            }
        }
    }
}

