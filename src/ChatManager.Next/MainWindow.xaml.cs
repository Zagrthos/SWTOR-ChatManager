using System.Windows;
using System.Windows.Controls;
using ChatManager.ViewModels;

namespace ChatManager;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel mainWindowViewModel)
    {
        InitializeComponent();
        DataContext = mainWindowViewModel;
        GlobalTabItem.Focus();
    }

    public MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;

    private void TabItem_GotFocus(object sender, RoutedEventArgs e)
    {
        if (sender is not TabItem tabItem)
            return;

        tabItem.FontWeight = FontWeights.Bold;
    }

    private void TabItem_LostFocus(object sender, RoutedEventArgs e)
    {
        if (sender is not TabItem tabItem)
            return;

        tabItem.FontWeight = FontWeights.Normal;
    }
}
