
using CallMeMaybeClient.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using CallMeMaybeClient.Services;
using System.Collections.ObjectModel;

namespace CallMeMaybeClient.ViewsModels
{


    public class AddSalarieViewModel: BaseViewModel
    {
        private readonly HttpClient _httpClient;

        private ObservableCollection<Service> _services;
        public ObservableCollection<Service> Services
        {
            get => _services;
            set
            {
                _services = value;
                OnPropertyChanged(nameof(Services));
            }
        }

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
        public Salarie NewSalarie { get; set; }
        public ICommand CreerSalarieCommand { get; }

        public AddSalarieViewModel()
        {
           

            _httpClient = new HttpClient();
            NewSalarie = new Salarie();
            CreerSalarieCommand = new AsyncRelayCommand(CreerSalarie);
            Sites = new ObservableCollection<Site>();
            Services = new ObservableCollection<Service>();

            _=LoadData();
        }


        private async Task LoadData()
        {
            try
            {
                
                var sitesResponse = await _httpClient.GetStringAsync("http://localhost:5164/api/site/get/all");
                var servicesResponse = await _httpClient.GetStringAsync("http://localhost:5164/api/service/get/all");

                var sites = JsonSerializer.Deserialize<List<Site>>(sitesResponse);
                var services = JsonSerializer.Deserialize<List<Service>>(servicesResponse);


                foreach (var site in sites)
                {
                    Sites.Add(site);
                    

                }

                foreach (var service in services)
                {
                    Services.Add(service);

                }
                OnPropertyChanged(nameof(Sites));
                OnPropertyChanged(nameof(Services));

            }

            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données : {ex.Message}");
            }
        }

        private async Task CreerSalarie()
        {
            try
            {

                var url = "http://localhost:5164/api/salarie/create";
                var json = JsonSerializer.Serialize(NewSalarie);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);


                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Le salarié a été créé avec succès !");
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
