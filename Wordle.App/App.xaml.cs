using Wordle.App.ViewModels;

namespace Wordle.App;

public partial class App : Application
{
    public App(MainViewModel mainViewModel)
    {
        InitializeComponent();

        MainPage = new MainPage()
        {
            // Update to inject this all
            BindingContext = mainViewModel
        };
    }
}