using FRONT_END.ViewModels;

namespace FRONT_END.View;

public partial class CategoriesPage : ContentPage
{
	private readonly CategoryViewModel _categoryViewModel;
    public CategoriesPage(CategoryViewModel categoryViewModel)
	{
		InitializeComponent();
		_categoryViewModel = categoryViewModel;
		BindingContext = _categoryViewModel;
	}
	protected override async void OnAppearing()
	{
		base.OnAppearing();
        if (_categoryViewModel.Categories.Count == 0) // Solo cargar si está vacío
        {
            await _categoryViewModel.LoadCategories();
        }
    }
}