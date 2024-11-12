using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.Logging;

namespace ChatManager;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
[SuppressMessage("Minor Code Smell", "S2325:Methods and properties that don't access instance data should be static", Justification = "These are event methods.")]
public sealed partial class MainWindow : Window
{
    private readonly ILogger<MainWindow> _logger;

    public MainWindow(ILogger<MainWindow> logger)
    {
        _logger = logger;

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
