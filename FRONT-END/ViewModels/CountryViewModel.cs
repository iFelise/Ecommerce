using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FRONT_END.Service;
using LIBRARY.Shared.Entity;
using System.Collections.ObjectModel;

namespace FRONT_END.ViewModels;

public partial class CountryViewModel : ObservableValidator
{
    private readonly CountryService _apiService;

    [ObservableProperty]
    private ObservableCollection<Country> _countries;

    [ObservableProperty]
    private Country? _selectedCountry;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _newCountry = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public CountryViewModel(CountryService apiService)
    {
        _apiService = apiService;
        _countries = new ObservableCollection<Country>();
    }

    [RelayCommand]
    public async Task LoadCountries()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            var countries = await _apiService.GetCountriesAsync();

            if (countries != null)
            {
                Countries.Clear();
                foreach (var country in countries)
                {
                    Countries.Add(country);
                }
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"No se pudieron cargar los pa�ses: {ex.Message}";
            await Shell.Current.DisplayAlert("Error", ErrorMessage, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void OnCountrySelected()
    {
        if (SelectedCountry != null)
        {
            NewCountry = SelectedCountry.Name;
            ErrorMessage = string.Empty;
        }
    }

    [RelayCommand]
    public async Task AddCountry()
    {
        if (IsBusy) return;

        // Validaciones
        if (string.IsNullOrWhiteSpace(NewCountry))
        {
            ErrorMessage = "El campo es obligatorio";
            return;
        }

        if (Countries.Any(c => c.Name.Equals(NewCountry, StringComparison.OrdinalIgnoreCase)))
        {
            ErrorMessage = "El pa�s ya existe";
            return;
        }

        if (NewCountry.Length > 100)
        {
            ErrorMessage = "El nombre del pa�s no puede tener m�s de 100 caracteres";
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            var newCountry = new Country
            {
                Name = NewCountry.Trim(),
                States = new List<State>()
            };

            var success = await _apiService.AddCountryAsync(newCountry);
            if (success)
            {
                await LoadCountries();
                NewCountry = string.Empty;
                SelectedCountry = null;
            }
            else
            {
                ErrorMessage = "No se pudo agregar el pa�s";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"No se pudo agregar el pa�s: {ex.Message}";
            await Shell.Current.DisplayAlert("Error", ErrorMessage, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public void EditCountry(Country country)
    {
        if (country != null)
        {
            SelectedCountry = country;
            NewCountry = country.Name;
            ErrorMessage = string.Empty;
        }
    }

    [RelayCommand]
    public async Task UpdateCountry()
    {
        if (SelectedCountry == null)
        {
            ErrorMessage = "Seleccione un pa�s para actualizar";
            return;
        }

        if (string.IsNullOrWhiteSpace(NewCountry))
        {
            ErrorMessage = "El campo es obligatorio";
            return;
        }

        if (Countries.Any(c => c.Name.Equals(NewCountry, StringComparison.OrdinalIgnoreCase) && c.Id != SelectedCountry.Id))
        {
            ErrorMessage = "El pa�s ya existe";
            return;
        }

        if (NewCountry.Length > 100)
        {
            ErrorMessage = "El nombre del pa�s no puede tener m�s de 100 caracteres";
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            var updatedCountry = new Country
            {
                Id = SelectedCountry.Id,
                Name = NewCountry.Trim(),
                States = SelectedCountry.States ?? new List<State>()
            };

            var success = await _apiService.UpdateCountryAsync(updatedCountry);
            if (success)
            {
                await LoadCountries();
                NewCountry = string.Empty;
                SelectedCountry = null;
            }
            else
            {
                ErrorMessage = "No se pudo actualizar el pa�s";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"No se pudo actualizar el pa�s: {ex.Message}";
            await Shell.Current.DisplayAlert("Error", ErrorMessage, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task DeleteCountry(Country? country = null)
    {
        var countryToDelete = country ?? SelectedCountry;
        if (countryToDelete == null) return;

        bool confirm = await Shell.Current.DisplayAlert(
            "Confirmar",
            $"�Est� seguro de que desea eliminar el pa�s '{countryToDelete.Name}'?",
            "S�",
            "No");

        if (!confirm) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            var success = await _apiService.DeleteCountryAsync(countryToDelete.Id);
            if (success)
            {
                Countries.Remove(countryToDelete);
                if (SelectedCountry?.Id == countryToDelete.Id)
                {
                    NewCountry = string.Empty;
                    SelectedCountry = null;
                }
            }
            else
            {
                ErrorMessage = "No se pudo eliminar el pa�s";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"No se pudo eliminar el pa�s: {ex.Message}";
            await Shell.Current.DisplayAlert("Error", ErrorMessage, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}