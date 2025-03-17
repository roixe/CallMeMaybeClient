
using CallMeMaybeClient.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using CallMeMaybeClient.Services;
using System.Collections.ObjectModel;
using CallMeMaybeClient.ViewsModels;
using CallMeMaybeClient.Views;

namespace CallMeMaybeClient.ViewsModels
{


    public class AddSiteViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;

        public event Action<Site> SiteAdded;


        private ObservableCollection<Site> _sites;
        public ObservableCollection<Site> Sites
        {
            get => _sites;
            set
            {
                _sites = value;
                OnPropertyChanged(nameof(Sites));
            }
        }


        public Site NewSite { get; set; }
        public ICommand CreerSiteCommand { get; }

        public AddSiteViewModel()
        {


            _httpClient = new HttpClient();
            NewSite = new Site();
            CreerSiteCommand = new AsyncRelayCommand(CreerSite);
            Sites = new ObservableCollection<Site>();

            _ = LoadData();
        }


        private async Task LoadData()
        {
            try
            {
                HttpClient client = new HttpClient();
                string customHeaderValue = "CallMeMaybe";
                client.DefaultRequestHeaders.Add("X-App-Identifier", customHeaderValue);
                var sitesResponse = await client.GetStringAsync("http://localhost:5164/api/site/get/all");

                var sites = JsonSerializer.Deserialize<List<Site>>(sitesResponse);



                foreach (var site in Sites)
                {
                    sites.Add(site);

                }
                OnPropertyChanged(nameof(sites));

            }

            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données : {ex.Message}");
            }
        }


        private async Task CreerSite()
        {
            try
            {
                HttpClient client = new HttpClient();
                string customHeaderValue = "CallMeMaybe";
                client.DefaultRequestHeaders.Add("X-App-Identifier", customHeaderValue);
                var url = "http://localhost:5164/api/Site/create";
                var json = JsonSerializer.Serialize(NewSite);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);


                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Le Site a été créé avec succès !");

                    SiteAdded?.Invoke(NewSite);



                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erreur : {error}" + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'envoi des données : {ex.Message}");
            }
        }

    }
}
