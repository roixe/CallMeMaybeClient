using CallMeMaybeClient.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


namespace CallMeMaybeClient.ViewsModels
{

    public class SalarieViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Salarie> _salaries;
        private string _searchText;
        private bool _isLoading;

        public ObservableCollection<Salarie> Salaries
        {
            get => _salaries;
            set
            {
                _salaries = value;
                OnPropertyChanged();
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

        public SalarieViewModel()
        {
            // Charger les données au démarrage
            _ = LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            IsLoading = true;

            try
            {
                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("http://localhost:5164/api/salarie/get/all");
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                var salaries = JsonSerializer.Deserialize<ObservableCollection<Salarie>>(json);

                Salaries = salaries ?? new ObservableCollection<Salarie>();
            }
            catch
            {
                // Gérer les erreurs (ex : afficher un message d'erreur)
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                _ = LoadDataAsync(); // Recharger toutes les données
            }
            else
            {
                Salaries = new ObservableCollection<Salarie>(
                    _salaries.Where(s => s.nom.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                );
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
