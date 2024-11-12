using System.Windows;
using ChatManager.ViewModels;

namespace ChatManager.Views;

/// <summary>
/// Interaction logic for MessageBoxView.xaml
/// </summary>
public partial class MessageBoxView : Window
{
    public MessageBoxView(string message, string title, string buttonOneText = "Yes", string? buttonTwoText = "No")
    {
        InitializeComponent();
        MessageBoxViewModel viewModel = new(message, title, buttonOneText, buttonTwoText);
        DataContext = viewModel;

        viewModel.RequestClose += (sender, e) => DialogResult = viewModel.Result;
    }

    public static bool Show(string message, string title, string buttonOneText = "Yes", string? buttonTwoText = "No")
    {
        MessageBoxView messageBox = new(message, title, buttonOneText, buttonTwoText);

        return messageBox.ShowDialog() is true;
    }
}
