using CallMeMaybeClient.ViewsModels;
using System.Windows.Controls;

namespace CallMeMaybeClient.Views
{
    public partial class SalariePage : Page
    {
        public SalariePage()
        {
           
            InitializeComponent();
            this.DataContext = new SalarieViewModel();
        }
    }
}
