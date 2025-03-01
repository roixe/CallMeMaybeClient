using CallMeMaybeClient.Models;
using CallMeMaybeClient.ViewsModels;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Input;
using System.Windows;
using System.Net.Http.Json;
using System.Windows.Controls;

public class SiteViewModel : BaseViewModel
{
    private ObservableCollection<Site> _sites;
    private List<Site> _sitesList;
    private Site _selectedSite;
    private string _searchText;
    private bool _isLoading;



    public ObservableCollection<Site> Sites
    {
        get => _sites;
        set
        {
            _sites = value;
            OnPropertyChanged();
        }
    }


    public Site SelectedSite
    {
        get => _selectedSite;
        set
        {
            _selectedSite = value;
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

    public SiteViewModel()
    {
        Sites = new ObservableCollection<Site>();
        _ = LoadDataAsync();
        DeleteCommand = new RelayCommand(async () => await OnDelete(), CanDelete);
    }

    public async Task LoadDataAsync()
    {
        IsLoading = true;
        try
        {
            using HttpClient client = new HttpClient();
            string customHeaderValue = "CallMeMaybe";
            client.DefaultRequestHeaders.Add("X-App-Identifier", customHeaderValue);
            // Charger les Sites
            HttpResponseMessage SitesResponse = await client.GetAsync("http://localhost:5164/api/site/get/all");
            SitesResponse.EnsureSuccessStatusCode();
            string SitesJson = await SitesResponse.Content.ReadAsStringAsync();
            var sites = JsonSerializer.Deserialize<List<Site>>(SitesJson);
            _sitesList = sites ?? new List<Site>();
            Sites = new ObservableCollection<Site>(sites ?? new List<Site>());

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
        if (SelectedSite == null)
            return;

        var result = MessageBox.Show(
            $"Voulez-vous vraiment supprimer {SelectedSite.ville} ? Tous les employés affectés à ce site n'auront plus de site associé.",
            "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            try
            {
                using HttpClient client = new HttpClient();
                string customHeaderValue = "CallMeMaybe";
                client.DefaultRequestHeaders.Add("X-App-Identifier", customHeaderValue);
                HttpResponseMessage response = await client.DeleteAsync($"http://localhost:5164/api/site/delete/{SelectedSite.id}");

                if (response.IsSuccessStatusCode)
                {
                    // Supprimer le site de la liste et mettre à jour l'ObservableCollection
                    _sitesList.Remove(SelectedSite);
                    _sitesList ??= new List<Site>();
                    Sites = new ObservableCollection<Site>(_sitesList);

                    MessageBox.Show("Site supprimé avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Lire le contenu de la réponse en cas d'échec
                    var errorMessage = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(errorMessage) && errorMessage.Contains("Impossible de supprimer le site"))
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
            Sites = new ObservableCollection<Site>(_sitesList);
        }
        else
        {
            Sites = new ObservableCollection<Site>(_sitesList.Where(s =>
                (s.ville != null && s.ville.Contains(SearchText, StringComparison.OrdinalIgnoreCase))));
        }
    }
    private bool CanDelete()
    {
        return SelectedSite != null;
    }
}
