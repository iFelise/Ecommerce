using FRONT_END.View;

namespace FRONT_END
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Registra las rutas
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(CategoriesPage), typeof(CategoriesPage));

            // Crea el Shell como página principal
            MainPage = new MainShell();
        }
    }

    // Cambia el nombre a MainShell para evitar conflictos
    public class MainShell : Shell
    {
        public MainShell()
        {
            // Configuración del Shell con pestañas
            Items.Add(new TabBar
            {
                Items =
                {
                    new Tab
                    {
                        Title = "Países",
                        Icon = "globe.png",
                        Items = { new ShellContent { Route = "MainPage", ContentTemplate = new DataTemplate(typeof(MainPage)) } }
                    },
                    new Tab
                    {
                        Title = "Categorías",
                        Icon = "category.png",
                        Items = { new ShellContent { Route = "CategoriesPage", ContentTemplate = new DataTemplate(typeof(CategoriesPage)) } }
                    }
                }
            });
        }
    }
}