using CallMeMaybeClient.Models;
using CallMeMaybeClient.ViewsModels;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Input;
using System.Windows;
using System.Net.Http.Json;
using System.Windows.Controls;

public class ServiceViewModel : BaseViewModel
{
    private ObservableCollection<Service> _services;
    private List<Service> _servicesList;
    private Service _selectedService;
    private string _searchText;
    private bool _isLoading;



    public ObservableCollection<Service> Services
    {
        get => _services;
        set
        {
            _services = value;
            OnPropertyChanged();
        }
    }


    public Service SelectedService
    {
        get => _selectedService;
        set
        {
            _selectedService = value;
            OnPropertyChanged();
            (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();

        }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            PerformSearch();
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    public ICommand DeleteCommand { get; }

    public ServiceViewModel()
    {
        Services = new ObservableCollection<Service>();
        _ = LoadDataAsync();
        DeleteCommand = new RelayCommand(async () => await OnDelete(), CanDelete);
    }

    public async Task LoadDataAsync()
    {
        IsLoading = true;
        try
        {
            using HttpClient client = new HttpClient();

            // Charger les services
            HttpResponseMessage servicesResponse = await client.GetAsync("http://localhost:5164/api/service/get/all");
            servicesResponse.EnsureSuccessStatusCode();
            string servicesJson = await servicesResponse.Content.ReadAsStringAsync();
            var services = JsonSerializer.Deserialize<List<Service>>(servicesJson);
            _servicesList = services ?? new List<Service>();
            Services = new ObservableCollection<Service>(services ?? new List<Service>());

        }
        catch (Exception ex)
        {
            // Gérer les erreurs (ajoutez éventuellement des logs ou affichez un message d'erreur)
            MessageBox.Show($"Erreur lors du chargement des données : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task OnDelete()
    {
        if (SelectedService == null)
            return;

        var result = MessageBox.Show($"Voulez-vous vraiment supprimer {SelectedService.nom} ?, tous les employés affecté à ce service n'auront plus  de service associé",
            "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            try
            {
                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync($"http://localhost:5164/api/site/delete/{SelectedService.id}");
                if (response.IsSuccessStatusCode)
                {
                    _servicesList.Remove(SelectedService);
                    _servicesList = _servicesList ?? new List<Service>();
                    Services = new ObservableCollection<Service>(_servicesList);

                    MessageBox.Show("Site supprimé avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Lire le contenu de la réponse en cas d'échec
                    var errorMessage = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(errorMessage) && errorMessage.Contains("Impossible de supprimer le service"))
                    {
                        MessageBox.Show(errorMessage, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la suppression du site.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }



    private void PerformSearch()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            Services = new ObservableCollection<Service>(_servicesList);
        }
        else
        {
            Services = new ObservableCollection<Service>(_servicesList.Where(s =>
                (s.nom != null && s.nom.Contains(SearchText, StringComparison.OrdinalIgnoreCase))));
        }
    }
    private bool CanDelete()
    {
        return SelectedService != null;
    }
}
