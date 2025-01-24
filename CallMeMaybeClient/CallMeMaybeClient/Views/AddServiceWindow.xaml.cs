using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CallMeMaybeClient.ViewsModels;

namespace CallMeMaybeClient.Views
{
    /// <summary>
    /// Logique d'interaction pour AddSalarieWindow.xaml
    /// </summary>
    public partial class AddServiceWindow : Window
    {
        public AddServiceWindow()
        {
            InitializeComponent();
            this.DataContext = new AddSalarieViewModel();



        }
        private void AjouterButton_Click(object sender, RoutedEventArgs e)
        {
            // Instancier la nouvelle fenêtre
            var nouvelleFenetre = new AddServiceWindow();

            // Ouvrir la fenêtre
            nouvelleFenetre.ShowDialog();
        }


    }
}
