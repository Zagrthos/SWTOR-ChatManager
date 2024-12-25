using System;
using System.Threading.Tasks;
using System.Windows;
using ChatManager.Helpers;
using ChatManager.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ChatManager;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    [STAThread]
    public static void Main()
    {
        using IHost host = StartupHelper.CreateApplicationHost();
        ILogger<App> logger = host.Services.GetRequiredService<ILogger<App>>();

        App app = new();

        AppDomain.CurrentDomain.UnhandledException += (sender, args)
            => logger.UnhandledException("AppDomain.CurrentDomain", (Exception)args.ExceptionObject);

        app.DispatcherUnhandledException += (sender, args) =>
        {
            logger.UnhandledException("App", args.Exception);
            args.Handled = true;
        };

        TaskScheduler.UnobservedTaskException += (sender, args)
            => logger.UnhandledException("TaskScheduler", args.Exception);

        app.InitializeComponent();
        app.MainWindow = host.Services.GetRequiredService<MainWindow>();
        app.MainWindow.Visibility = Visibility.Visible;
        app.Run();
    }
}
