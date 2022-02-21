using Wordle.App.Services.WordService;
using Wordle.App.ViewModels;

namespace Wordle.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        return MauiApp.CreateBuilder()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            })
            .RegisterServices()
            .RegisterViewModels()
        .Build();
    }

    public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        // register services
        builder.Services.AddSingleton<IWordService, HardCodedWordService>();

        return builder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        // register viewmodels
        builder.Services.AddTransient<MainViewModel>();

        return builder;
    }
}