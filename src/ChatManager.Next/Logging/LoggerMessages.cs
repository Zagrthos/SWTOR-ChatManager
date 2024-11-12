using System;
using Microsoft.Extensions.Logging;

namespace ChatManager.Logging;

public static partial class LoggerMessages
{
    [LoggerMessage(0, LogLevel.Trace, "{windowName} initialized")]
    public static partial void WindowInitialized(this ILogger logger, string windowName);

    [LoggerMessage(1, LogLevel.Trace, "{windowName} activated")]
    public static partial void WindowActivated(this ILogger logger, string windowName);

    [LoggerMessage(2, LogLevel.Trace, "{windowName} loaded")]
    public static partial void WindowLoaded(this ILogger logger, string windowName);

    [LoggerMessage(3, LogLevel.Trace, "{windowName} unloaded")]
    public static partial void WindowUnloaded(this ILogger logger, string windowName);

    [LoggerMessage(4, LogLevel.Trace, "{windowName} closing")]
    public static partial void WindowClosing(this ILogger logger, string windowName);

    [LoggerMessage(5, LogLevel.Trace, "{windowName} closed")]
    public static partial void WindowClosed(this ILogger logger, string windowName);

    [LoggerMessage(6, LogLevel.Trace, "{windowName} deactivated")]
    public static partial void WindowDeactivated(this ILogger logger, string windowName);

    [LoggerMessage(10, LogLevel.Trace, "{control} initialized")]
    public static partial void ControlInitialized(this ILogger logger, string control);

    [LoggerMessage(11, LogLevel.Trace, "{control} loaded")]
    public static partial void ControlLoaded(this ILogger logger, string control);

    [LoggerMessage(12, LogLevel.Trace, "{control} unloaded")]
    public static partial void ControlUnloaded(this ILogger logger, string control);

    [LoggerMessage(200, LogLevel.Information, "Starting application in version {version} on {os}-{arch} using .NET {dotnet}")]
    public static partial void StartingApplication(this ILogger logger, string version, string os, string arch, string dotnet);

    [LoggerMessage(299, LogLevel.Information, "Stopping application in version {version} on {os}-{arch} using .NET {dotnet}")]
    public static partial void StoppingApplication(this ILogger logger, string version, string os, string arch, string dotnet);

    [LoggerMessage(500, LogLevel.Critical, "Unhandled exception in {source} occurred:")]
    public static partial void UnhandledException(this ILogger logger, string source, Exception exception);
}
