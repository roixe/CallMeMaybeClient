
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


    public class AddServiceViewModel : BaseViewModel
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


        public Service NewService { get; set; }
        public ICommand CreerServiceCommand { get; }

        public AddServiceViewModel()
        {


            _httpClient = new HttpClient();
            NewService = new Service();
            CreerServiceCommand = new AsyncRelayCommand(CreerService);
            Services = new ObservableCollection<Service>();

            _ = LoadData();
        }


        private async Task LoadData()
        {
            try
            {

                var servicesResponse = await _httpClient.GetStringAsync("http://localhost:5164/api/service/get/all");

                var services = JsonSerializer.Deserialize<List<Service>>(servicesResponse);



                foreach (var service in services)
                {
                    Services.Add(service);

                }
                OnPropertyChanged(nameof(Services));

            }

            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données : {ex.Message}");
            }
        }


        private async Task CreerService()
        {
            try
            {

                var url = "http://localhost:5164/api/service/create";
                var json = JsonSerializer.Serialize(NewService);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);


                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Le service a été créé avec succès !");



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
