using ChatManager.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChatManager.ViewModels;

public partial class MainWindowViewModel(SettingsViewModel settingsViewModel) : ObservableObject
{
    private readonly SettingsViewModel _settingsViewModel = settingsViewModel;

    [RelayCommand]
    private void OpenSettings()
    {
        SettingsView settings = new(_settingsViewModel);
        settings.ShowDialog();
    }
}
