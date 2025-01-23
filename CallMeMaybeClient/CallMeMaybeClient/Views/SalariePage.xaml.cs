using CallMeMaybeClient.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace CallMeMaybeClient.Views
{
    public partial class SalariePage : Page
    {
        public SalariePage()
        {
            InitializeComponent();
            this.DataContext = new SalarieViewModel();  
        }

        private void AjouterButton_Click(object sender, RoutedEventArgs e)
        {
            // Instancier la nouvelle fenêtre
            var nouvelleFenetre = new AddSalarieWindow();

            // Ouvrir la fenêtre
            nouvelleFenetre.ShowDialog();
        }

        private async void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.Row.Item is Salarie salarie)
            {
                await UpdateSalarieAsync(salarie);
            }
        }

        private async Task UpdateSalarieAsync(Salarie salarie)
        {
            using HttpClient client = new HttpClient();
            try
            {
                string url = $"http://localhost:5164/api/salarie/update/{salarie.id}";
                var content = new StringContent(JsonSerializer.Serialize(salarie), Encoding.UTF8, "application/json");
                var response = await client.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Mise à jour réussie.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Erreur lors de la mise à jour : {response.ReasonPhrase}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
