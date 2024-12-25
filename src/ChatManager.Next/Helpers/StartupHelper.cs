using System;
using System.Globalization;
using System.IO;
using ChatManager.Converter;
using ChatManager.Properties;
using ChatManager.ViewModels;
using ChatManager.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NReco.Logging.File;

namespace ChatManager.Helpers;

public static class StartupHelper
{
    public static IHost CreateApplicationHost()
    {
        HostApplicationBuilderSettings appSettings = new()
        {
            ContentRootPath = Directory.GetCurrentDirectory(),
            EnvironmentName = MetadataHelper.GetAppEnvironment
        };

        HostApplicationBuilder appBuilder = Host.CreateEmptyApplicationBuilder(appSettings);

        appBuilder.Logging.AddAppLogging();
        appBuilder.Services.AddAppServices();

        return appBuilder.Build();
    }

    private static void AddAppLogging(this ILoggingBuilder builder)
    {
        if (!Directory.Exists(MetadataHelper.GetAppDirectoryLogs))
            Directory.CreateDirectory(MetadataHelper.GetAppDirectoryLogs);

        builder.AddFile(Path.Combine(MetadataHelper.GetAppDirectoryLogs, $"ChatManager_{DateTimeOffset.Now:yyyy-MM-dd}.log"), static c =>
        {
            c.Append = true;
            c.MaxRollingFiles = 7;
            c.RollingFilesConvention = FileLoggerOptions.FileRollingConvention.Descending;
            c.UseUtcTimestamp = false;
            c.FormatLogFileName = static (logTime) => string.Format(CultureInfo.InvariantCulture, logTime, $"{DateTimeOffset.Now:yyyy-MM-dd}");
            c.FormatLogEntry = static (message) =>
            {
                string logMessage = $"[{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss}] {message.LogLevel}: {message.LogName}[{message.EventId}] {message.Message}";
                if (message.Exception is not null)
                    logMessage += Environment.NewLine + message.Exception;

                return logMessage;
            };
        });
        builder.SetMinimumLevel((Enum.IsDefined(typeof(LogLevel), Settings.Default.LogLevel)) ? Enum.Parse<LogLevel>(Settings.Default.LogLevel.ToString(CultureInfo.InvariantCulture)) : LogLevel.Information);
    }

    private static void AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<ThemeConverter>();

        services.AddSingleton<SettingsView>();
        services.AddSingleton<SettingsViewModel>();

        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MainWindow>();

        services.AddSingleton<MessageBoxView>();
        services.AddSingleton<MessageBoxViewModel>();
    }
}
