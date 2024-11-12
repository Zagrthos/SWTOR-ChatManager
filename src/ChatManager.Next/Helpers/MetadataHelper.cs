using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Hosting;

namespace ChatManager.Helpers;

public static class MetadataHelper
{
    public static string GetAppAuthors
        => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).CompanyName ?? "Bot authors not found";

    public static string GetAppDirectory
        => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Zagrthos", "SWTOR-ChatManager");

    public static string GetAppDirectoryAutosave
        => Path.Combine(GetAppDirectory, "Autosave");

    public static string GetAppDirectoryLogs
        => Path.Combine(GetAppDirectory, "Logs");

    public static string GetAppDirectorySwtor
        => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SWTOR", "swtor", "settings");

    public static string GetAppDirectorySwtorBackups
        => Path.Combine(GetAppDirectorySwtor, "Backups");

    public static string GetAppDotNetVersion
        => Environment.Version.ToString() ?? ".NET version not found";

    public static string GetAppEnvironment
        => (GetAppName.EndsWith("Dev", StringComparison.OrdinalIgnoreCase)) ? Environments.Development : Environments.Production;

    public static string GetAppName
        => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName?.Split('.')[0] ?? "Bot name not found";

    public static string GetAppVersion
        => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion ?? "Bot version not found";
}
