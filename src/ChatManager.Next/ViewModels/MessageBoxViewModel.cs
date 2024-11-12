using System;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace ChatManager.ViewModels;

public sealed class MessageBoxViewModel
{
    public MessageBoxViewModel(string message, string title, string buttonOneText, string? buttonTwoText)
    {
        Message = message;
        Title = title;
        ButtonOneText = buttonOneText;
        ButtonTwoText = buttonTwoText;

        ButtonOneCommand = new RelayCommand(OnButtonOneClicked);
        ButtonTwoCommand = new RelayCommand(OnButtonTwoClicked);
    }

    public string Message { get; }
    public string Title { get; }
    public string ButtonOneText { get; }
    public string? ButtonTwoText { get; }
    public Visibility ButtonTwoVisibility => (ButtonTwoText is null) ? Visibility.Collapsed : Visibility.Visible;
    public bool? Result { get; private set; }
    public ICommand ButtonOneCommand { get; }
    public ICommand ButtonTwoCommand { get; }

    public event EventHandler? RequestClose;

    private void OnButtonOneClicked()
    {
        Result = true;
        RequestClose?.Invoke(this, EventArgs.Empty);
    }

    private void OnButtonTwoClicked()
    {
        Result = false;
        RequestClose?.Invoke(this, EventArgs.Empty);
    }
}
