using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ChatManager.Views;
using CommunityToolkit.Mvvm.Input;

namespace ChatManager.ViewModels;

public partial class MainWindowViewModel : INotifyPropertyChanged
{
    public MainWindowViewModel()
        => OpenSettingsCommand = new RelayCommand(OpenSettings);

    public ICommand OpenSettingsCommand { get; }

    private static void OpenSettings()
    {
        SettingsView settings = new();
        settings.ShowDialog();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
