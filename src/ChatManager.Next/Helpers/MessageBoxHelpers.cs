using System;
using ChatManager.Views;

namespace ChatManager.Helpers;

public static class MessageBoxHelpers
{
    public static bool? ShowMessage(string message, string title, string buttonOneText, string? buttonTwoText = null)
    {
        MessageBoxView messageBox = new(message, title, buttonOneText, buttonTwoText);

        return messageBox.ShowDialog();
    }

    public static void ShowUnhandledException()
    {
        MessageBoxView messageBox = new("An unhandled exception occurred. Please check the logs for more information.", "Unhandeld Exception", "Exit Application");
        messageBox.ShowDialog();

        Environment.Exit(1);
    }
}
