using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ChatManager.Properties;
using CommunityToolkit.Mvvm.Input;

namespace ChatManager.ViewModels;

public sealed partial class SettingsViewModel : INotifyPropertyChanged
{
    private bool _autosave;
    private int _autosaveInterval;
    private int _language;
    private int _logLevel;
    private bool _openLastColors;
    private bool _saveLastColors;
    private int _theme;

    public SettingsViewModel()
    {
        ResetButtonCommand = new RelayCommand(ResetSettings);
        SaveButtonCommand = new RelayCommand(SaveSettings);

        // Initialize properties with current settings
        _autosave = Settings.Default.Autosave;
        _autosaveInterval = Settings.Default.AutosaveInterval;
        _language = Settings.Default.Language;
        _logLevel = Settings.Default.LogLevel;
        _openLastColors = Settings.Default.OpenLastColors;
        _saveLastColors = Settings.Default.SaveLastColors;
        _theme = Settings.Default.Theme;
    }

    public ICommand ResetButtonCommand { get; }
    public ICommand SaveButtonCommand { get; }
    public bool SettingsChanged { get; private set; }

    public int SelectedTheme
    {
        get => _theme;
        set => SetProperty(ref _theme, value);
    }

    public int SelectedLanguage
    {
        get => _language;
        set => SetProperty(ref _language, value);
    }

    public bool SaveLastColorsOnClose
    {
        get => _saveLastColors;
        set => SetProperty(ref _saveLastColors, value);
    }

    public bool OpenLastColorsOnStart
    {
        get => _openLastColors;
        set => SetProperty(ref _openLastColors, value);
    }

    public int SelectedLogLevel
    {
        get => _logLevel;
        set => SetProperty(ref _logLevel, value);
    }

    public bool IsAutosaveEnabled
    {
        get => _autosave;
        set => SetProperty(ref _autosave, value);
    }

    public int AutosaveInterval
    {
        get => _autosaveInterval;
        set => SetProperty(ref _autosaveInterval, value);
    }

    public void ResetSettings()
    {
        Settings.Default.Reset();
        LoadSettings();
        SettingsChanged = true;
    }

    public void SaveSettings()
    {
        Settings.Default.Autosave = _autosave;
        Settings.Default.AutosaveInterval = _autosaveInterval;
        Settings.Default.Language = _language;
        Settings.Default.LogLevel = _logLevel;
        Settings.Default.OpenLastColors = _openLastColors;
        Settings.Default.SaveLastColors = _saveLastColors;
        Settings.Default.Theme = _theme;
        Settings.Default.Save();
        SettingsChanged = false;
    }

    private void LoadSettings()
    {
        _autosave = Settings.Default.Autosave;
        _autosaveInterval = Settings.Default.AutosaveInterval;
        _language = Settings.Default.Language;
        _logLevel = Settings.Default.LogLevel;
        _openLastColors = Settings.Default.OpenLastColors;
        _saveLastColors = Settings.Default.SaveLastColors;
        _theme = Settings.Default.Theme;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null!)
    {
        if (Equals(field, value))
            return;

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        SettingsChanged = true;
    }
}
