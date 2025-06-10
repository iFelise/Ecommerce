using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FRONT_END.Service;
using LIBRARY.Shared.DTO.CategoryDTO;
using System.Collections.ObjectModel;

namespace FRONT_END.ViewModels
{
    public partial class CategoryViewModel : ObservableObject
    {
        private readonly CategoryService _categoryService;

        public CategoryViewModel(CategoryService categoryService)
        {
            _categoryService = categoryService;
            Categories = new ObservableCollection<CategoryResponseDto>();
        }

        [ObservableProperty]
        private ObservableCollection<CategoryResponseDto> categories;

        [ObservableProperty]
        private CategoryResponseDto selectedCategory;

        [ObservableProperty]
        private string newCategoryName;

        [ObservableProperty]
        private string newCategoryDescription;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private bool isBusy;

        [RelayCommand]
        private void OnCategorySelected()
        {
            if (SelectedCategory != null)
            {
                NewCategoryName = SelectedCategory.Name;
                NewCategoryDescription = SelectedCategory.Description;
                ErrorMessage = string.Empty;
            }
        }

        [RelayCommand]
        public async Task LoadCategories()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                var categories = await _categoryService.GetCategoriesAsync();
                Categories.Clear();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al cargar categorías: {ex.Message}";
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", ErrorMessage, "OK");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task AddCategory()
        {
            if (IsBusy) return;

            // Validaciones
            if (string.IsNullOrWhiteSpace(NewCategoryName))
            {
                ErrorMessage = "El nombre es obligatorio";
                return;
            }

            if (Categories.Any(c => c.Name.Equals(NewCategoryName, StringComparison.OrdinalIgnoreCase)))
            {
                ErrorMessage = "La categoría ya existe";
                return;
            }

            if (NewCategoryName.Length > 100)
            {
                ErrorMessage = "El nombre no puede tener más de 100 caracteres";
                return;
            }

            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                var newCategory = new CreateCategoryDto
                {
                    Name = NewCategoryName.Trim(),
                    Description = NewCategoryDescription?.Trim(),
                    ProdCategories = new List<CreateProdCategoryDto>()
                };

                var success = await _categoryService.AddCategoryAsync(newCategory);
                if (success)
                {
                    await LoadCategories();
                    NewCategoryName = string.Empty;
                    NewCategoryDescription = string.Empty;
                    SelectedCategory = null;
                }
                else
                {
                    ErrorMessage = "No se pudo agregar la categoría";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"No se pudo agregar la categoría: {ex.Message}";
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", ErrorMessage, "OK");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public void EditCategory(CategoryResponseDto category)
        {
            if (category != null)
            {
                SelectedCategory = category;
                NewCategoryName = category.Name;
                NewCategoryDescription = category.Description;
                ErrorMessage = string.Empty;
            }
        }

        [RelayCommand]
        public async Task UpdateCategory()
        {
            if (SelectedCategory == null)
            {
                ErrorMessage = "Seleccione una categoría para actualizar";
                return;
            }

            if (string.IsNullOrWhiteSpace(NewCategoryName))
            {
                ErrorMessage = "El nombre es obligatorio";
                return;
            }

            if (Categories.Any(c => c.Name.Equals(NewCategoryName, StringComparison.OrdinalIgnoreCase) && c.Id != SelectedCategory.Id))
            {
                ErrorMessage = "La categoría ya existe";
                return;
            }

            if (NewCategoryName.Length > 100)
            {
                ErrorMessage = "El nombre no puede tener más de 100 caracteres";
                return;
            }

            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                var updatedCategory = new UpdateCategoryDto
                {
                    Id = SelectedCategory.Id,
                    Name = NewCategoryName.Trim(),
                    Description = NewCategoryDescription?.Trim()
                };

                var success = await _categoryService.UpdateCategoryAsync(updatedCategory);
                if (success)
                {
                    await LoadCategories();
                    NewCategoryName = string.Empty;
                    NewCategoryDescription = string.Empty;
                    SelectedCategory = null;
                }
                else
                {
                    ErrorMessage = "No se pudo actualizar la categoría";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"No se pudo actualizar la categoría: {ex.Message}";
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", ErrorMessage, "OK");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task DeleteCategory(CategoryResponseDto category = null)
        {
            var categoryToDelete = category ?? SelectedCategory;
            if (categoryToDelete == null) return;

            bool confirm = false;
            if (Application.Current?.MainPage != null)
            {
                confirm = await Application.Current.MainPage.DisplayAlert(
                    "Confirmar",
                    $"¿Está seguro de que desea eliminar la categoría '{categoryToDelete.Name}'?",
                    "Sí",
                    "No");
            }

            if (!confirm) return;

            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                var success = await _categoryService.DeleteCategoryAsync(categoryToDelete.Id);
                if (success)
                {
                    Categories.Remove(categoryToDelete);
                    if (SelectedCategory?.Id == categoryToDelete.Id)
                    {
                        NewCategoryName = string.Empty;
                        NewCategoryDescription = string.Empty;
                        SelectedCategory = null;
                    }
                }
                else
                {
                    ErrorMessage = "No se pudo eliminar la categoría";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"No se pudo eliminar la categoría: {ex.Message}";
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", ErrorMessage, "OK");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Agregar comando Cancel faltante
        [RelayCommand]
        public void Cancel()
        {
            SelectedCategory = null;
            NewCategoryName = string.Empty;
            NewCategoryDescription = string.Empty;
            ErrorMessage = string.Empty;
        }
    }
}