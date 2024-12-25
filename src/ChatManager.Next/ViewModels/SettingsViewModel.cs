using ChatManager.Properties;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChatManager.ViewModels;

public sealed partial class SettingsViewModel : ObservableRecipient
{
    [ObservableProperty]
    private bool _autosave;

    [ObservableProperty]
    private int _autosaveInterval;

    [ObservableProperty]
    private int _language;

    [ObservableProperty]
    private int _logLevel;

    [ObservableProperty]
    private bool _openLastColors;

    [ObservableProperty]
    private bool _saveLastColors;

    [ObservableProperty]
    private int _theme;

    [ObservableProperty]
    private bool _settingsChanged;

    public SettingsViewModel()
    {
        // Initialize properties with current settings
        Autosave = Settings.Default.Autosave;
        AutosaveInterval = Settings.Default.AutosaveInterval;
        Language = Settings.Default.Language;
        LogLevel = Settings.Default.LogLevel;
        OpenLastColors = Settings.Default.OpenLastColors;
        SaveLastColors = Settings.Default.SaveLastColors;
        Theme = Settings.Default.Theme;
    }

    [RelayCommand]
    private void ResetSettings()
    {
        Settings.Default.Reset();
        LoadSettings();
        SettingsChanged = true;
    }

    [RelayCommand]
    private void SaveSettings()
    {
        Settings.Default.Autosave = Autosave;
        Settings.Default.AutosaveInterval = AutosaveInterval;
        Settings.Default.Language = Language;
        Settings.Default.LogLevel = LogLevel;
        Settings.Default.OpenLastColors = OpenLastColors;
        Settings.Default.SaveLastColors = SaveLastColors;
        Settings.Default.Theme = Theme;
        Settings.Default.Save();
        SettingsChanged = false;
    }

    private void LoadSettings()
    {
        Autosave = Settings.Default.Autosave;
        AutosaveInterval = Settings.Default.AutosaveInterval;
        Language = Settings.Default.Language;
        LogLevel = Settings.Default.LogLevel;
        OpenLastColors = Settings.Default.OpenLastColors;
        SaveLastColors = Settings.Default.SaveLastColors;
        Theme = Settings.Default.Theme;
    }
}
