﻿using CallMeMaybeClient.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Windows.Controls;
using System.Windows;

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
            // Instancier la nouvelle fenêtre
            var nouvelleFenetre = new AddSiteWindow();

            // Ouvrir la fenêtre
            nouvelleFenetre.ShowDialog();
        }

        private async Task UpdateSiteAsync(Site site)
        {
            using HttpClient client = new HttpClient();
            try
            {
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
    }
}
