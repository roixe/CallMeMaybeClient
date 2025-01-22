using CallMeMaybeClient.Models;
using CallMeMaybeClient.ViewsModels;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Input;
using System.Windows;
using System.Net.Http.Json;
using System.Windows.Controls;

public class SalarieViewModel : BaseViewModel
{
    private ObservableCollection<Salarie> _salaries;
    private List<Salarie> _allSalaries;
    private Salarie _selectedSalarie;
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

    public Salarie SelectedSalarie
    {
        get => _selectedSalarie;
        set
        {
            _selectedSalarie = value;
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

    public SalarieViewModel()
    {
        _ = LoadDataAsync();
        DeleteCommand = new RelayCommand(async () => await OnDelete(), CanDelete);
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

    private async Task OnDelete()
    {
        if (SelectedSalarie == null)
            return;

        var result = MessageBox.Show($"Voulez-vous vraiment supprimer {SelectedSalarie.nom} {SelectedSalarie.prenom} ?",
            "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            try
            {
                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync($"http://localhost:5164/api/salarie/delete/{SelectedSalarie.id}");
                if (response.IsSuccessStatusCode)
                {
                    _allSalaries.Remove(SelectedSalarie);
                    _allSalaries = _allSalaries ?? new List<Salarie>();
                    Salaries = new ObservableCollection<Salarie>(_allSalaries);

                    MessageBox.Show("Salarié supprimé avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (SelectedSalarie.id == 0)
                {
                    MessageBox.Show("ID =0","Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

                }

                else
                {
                    MessageBox.Show("Erreur lors de la suppression.", "Erreur ", MessageBoxButton.OK, MessageBoxImage.Error);
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
            Salaries = new ObservableCollection<Salarie>(_allSalaries);
        }
        else
        {
            Salaries = new ObservableCollection<Salarie>(_allSalaries.Where(s =>
                (s.nom != null && s.nom.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                (s.serviceNom != null && s.serviceNom.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                (s.villeNom != null && s.villeNom.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
            ));
        }
    }
    private bool CanDelete()
    {
        return SelectedSalarie != null;
    }
}
