﻿using System.Timers;

namespace ChatManager.Services
{
    internal enum LogEvent
    {
        Info,
        Warning,
        Error,
        Variable,
        Method,
        Control,
        ExMessage,
        BoxMessage
    }

    internal enum ProgramClass
    {
        MainForm,
        ColorPickerForm,
        FileSelectorForm,
        Checks,
        Converter,
        FileExport,
        FileImport,
        Logging,
        OpenWindows,
        ShowMessageBox,
        Updater
    }

    internal static class Logging
    {
        private static readonly string LogSession = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd_HH-mm-ss");
        private static readonly string LogPath = GetSetSettings.GetLogPath;
        private static StreamWriter? logWriter;
        private static System.Timers.Timer? timer;

        internal static async Task Initialize()
        {
            if (!Directory.Exists(LogPath))
                Directory.CreateDirectory(LogPath);

            string logfilePath = Path.Combine(LogPath, $"ChatManager_{LogSession}.log");
            logWriter = new(logfilePath, true);
            await Write(LogEvent.Info, ProgramClass.Logging, "Logging started");

            // Add Timer to write any second all open entries in the log
            timer = new(5000);
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }

        // Stop logWriter and restart it
        private static void TimerElapsed(object? sender, ElapsedEventArgs e)
        {
            if (logWriter != null)
            {
                lock (logWriter)
                {
                    logWriter.FlushAsync().Wait();
                    logWriter.Close();
                    logWriter = new(Path.Combine(LogPath, $"ChatManager_{LogSession}.log"), true);
                }
            }
        }

        internal static async Task Write(LogEvent Level, ProgramClass programClass, string Message)
        {
            string Event = Level switch
            {
                LogEvent.Info => "INFO",
                LogEvent.Warning => "WARNING",
                LogEvent.Error => "ERROR",
                LogEvent.Variable => "VARIABLE",
                LogEvent.Method => "METHOD",
                LogEvent.Control => "CONTROL",
                LogEvent.ExMessage => "EXECPTION",
                LogEvent.BoxMessage => "MESSAGE-BOX",
                _ => "UNCATEGORIZED"
            };

            if (logWriter != null)
            {
                await logWriter.WriteLineAsync($"[{DateTime.Now:HH:mm:ss}] => {Event} on {programClass}: {Message}");
            } else
            {
                await Initialize();
            }
        }

        internal static async Task Finalize()
        {
            if (logWriter != null)
            {
                await Write(LogEvent.Info, ProgramClass.Logging, "Logging stopped");
                await logWriter.FlushAsync();
                logWriter.Close();
            }

            timer?.Stop();
        }
    }
}