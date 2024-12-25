using System.Windows;
using System.Windows.Controls;

namespace ChatManager;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        GlobalTabItem.Focus();
    }

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
