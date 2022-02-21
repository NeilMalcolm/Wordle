using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Wordle.App.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    #region OnPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    #endregion
}