﻿using CallMeMaybeClient.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using CallMeMaybeClient.Services;

namespace CallMeMaybeClient.Views
{
    public partial class SalariePage : Page
    {
        private SalarieViewModel _viewModel;
        public SalariePage()
        {
            InitializeComponent();
            _viewModel = new SalarieViewModel();
            this.DataContext = _viewModel;
            AdminButtonVisibility = RoleManager.IsAdmin() ? Visibility.Visible : Visibility.Collapsed;

        }

        public Visibility AdminButtonVisibility { get; set; }

        private async void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.Row.Item is Salarie salarie)
            {
                await UpdateSalarieAsync(salarie);
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
            var nouvelleFenetre = new AddSalarieWindow();

            // Ouvrir la fenêtre
            nouvelleFenetre.ShowDialog();
        }

        private async Task UpdateSalarieAsync(Salarie salarie)
        {
            using HttpClient client = new HttpClient();
            if (RoleManager.IsAdmin())
            {
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
            else
            {
                MessageBox.Show($"Vous n'êtes pas admin");
            }
        }
    }
}
