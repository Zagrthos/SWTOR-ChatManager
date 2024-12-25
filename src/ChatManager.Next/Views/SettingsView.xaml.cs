using System.Windows;
using ChatManager.ViewModels;

namespace ChatManager.Views;

/// <summary>
/// Interaction logic for SettingsView.xaml
/// </summary>
public partial class SettingsView : Window
{
    public SettingsView(SettingsViewModel settingsViewModel)
    {
        InitializeComponent();
        DataContext = settingsViewModel;
    }

    public SettingsViewModel ViewModel => (SettingsViewModel)DataContext;
}
