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
        private List<Salarie> _allSalaries;
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
                var salaries = JsonSerializer.Deserialize<List<Salarie>>(json);

                _allSalaries = salaries ?? new List<Salarie>();
                Salaries = new ObservableCollection<Salarie>(_allSalaries);
            }
            catch
            {
               
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
                // Si aucune recherche, restaurer toutes les données
                Salaries = new ObservableCollection<Salarie>(_allSalaries);
            }
            else
            {
                // Filtrer par nom, service ou ville
                Salaries = new ObservableCollection<Salarie>(
                    _allSalaries.Where(s =>
                        (s.nom != null && s.nom.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                        (s.serviceNom != null && s.serviceNom.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                        (s.villeNom != null && s.villeNom.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    )
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
