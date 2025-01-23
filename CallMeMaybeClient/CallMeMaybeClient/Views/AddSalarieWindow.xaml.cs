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
    public partial class AddSalarieWindow : Window
    {
        public AddSalarieWindow()
        {
            InitializeComponent();
            this.DataContext = new AddSalarieViewModel();

        }


    }
}
