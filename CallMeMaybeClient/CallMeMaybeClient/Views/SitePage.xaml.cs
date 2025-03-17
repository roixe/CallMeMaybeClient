using CallMeMaybeClient.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using CallMeMaybeClient.Services;
using CallMeMaybeClient.ViewsModels;

namespace CallMeMaybeClient.Views
{
    public partial class SitePage : Page
    {
        private SiteViewModel _viewModel;
        public SitePage()
        {
            InitializeComponent();
            _viewModel = new SiteViewModel();
            this.DataContext = _viewModel;
        }

        private async void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.Row.Item is Site site)
            {
                await UpdateSiteAsync(site);
            }
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            // Appeler la méthode LoadDataAsync pour recharger les salariés
            await _viewModel.LoadDataAsync();
        }

        private void AjouterButton_Click(object sender, RoutedEventArgs e)
        {
            {
                var addSiteWindow = new AddSiteWindow();
                if (addSiteWindow.DataContext is AddSiteViewModel addSiteVM)
                {
                    addSiteVM.SiteAdded += async (_) =>
                    {
                        await _viewModel.LoadDataAsync();
                        _viewModel.RefreshGrid();           //  Recharge toute la liste après ajout
                    };
                }

                addSiteWindow.ShowDialog();
            }
        }

        private async Task UpdateSiteAsync(Site site)
        {
            using HttpClient client = new HttpClient();
            if (RoleManager.IsAdmin())
            {
                try
            {
                string customHeaderValue = "CallMeMaybe";
                client.DefaultRequestHeaders.Add("X-App-Identifier", customHeaderValue);
                string url = $"http://localhost:5164/api/site/update/{site.id}";
                var content = new StringContent(JsonSerializer.Serialize(site), Encoding.UTF8, "application/json");
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
            else
            {
                MessageBox.Show($"Vous n'êtes pas admin");
            }
        }
    }
}
