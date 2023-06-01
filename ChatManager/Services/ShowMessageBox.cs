﻿namespace ChatManager.Services
{
    internal class ShowMessageBox
    {
        public static void Show(string caption, string message)
        {
            Localization localization = new(GetSetSettings.GetCurrentLocale);

            MessageBoxIcon icon = MessageBoxIcon.None;

            switch (caption)
            {
                case var value when value == localization.GetString("MessageBoxError"):
                    icon = MessageBoxIcon.Error;
                    break;

                case var value when value == localization.GetString("MessageBoxInfo"):
                    icon = MessageBoxIcon.Information;
                    break;

                case var value when value == localization.GetString("MessageBoxWarn"):
                    icon = MessageBoxIcon.Warning;
                    break;

                case var value when value == localization.GetString("MessageBoxUpdate"):
                    icon = MessageBoxIcon.Information;
                    break;

                case var value when value == localization.GetString("MessageBoxNoUpdate"):
                    icon = MessageBoxIcon.Information;
                    break;
            }

            Logging.Write(LogEvent.BoxMessage, ProgramClass.ShowMessageBox, "MessageBox shown");

            MessageBox.Show(message, caption, MessageBoxButtons.OK, icon);

            Logging.Write(LogEvent.BoxMessage, ProgramClass.ShowMessageBox, "MessageBox accepted");
        }

        public static bool ShowUpdate(string version)
        {
            Localization localization = new(GetSetSettings.GetCurrentLocale);
            DialogResult result = MessageBox.Show(localization.GetString("Update_IsAvailable") + $" {version}", localization.GetString("MessageBoxUpdate"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            Logging.Write(LogEvent.BoxMessage, ProgramClass.ShowMessageBox, "MessageBox shown");

            if (result == DialogResult.Yes)
            {
                Logging.Write(LogEvent.Info, ProgramClass.ShowMessageBox, "DialogResult is yes");
                return true;
            } else
            {
                Logging.Write(LogEvent.Info, ProgramClass.ShowMessageBox, "DialogResult is no");
                return false;
            }
        }

        public static void ShowBug()
        {
            Localization localization = new(GetSetSettings.GetCurrentLocale);
            DialogResult result = MessageBox.Show(localization.GetString("Error_IsDetected"), localization.GetString("MessageBoxError"), MessageBoxButtons.YesNo, MessageBoxIcon.Error);

            Logging.Write(LogEvent.BoxMessage, ProgramClass.ShowMessageBox, "MessageBox shown");

            if (result == DialogResult.Yes)
            {
                Logging.Write(LogEvent.Info, ProgramClass.ShowMessageBox, "DialogResult is yes");
                OpenWindows.OpenLinksInBrowser(GetSetSettings.GetBugPath);
            }
            else
            {
                Logging.Write(LogEvent.Info, ProgramClass.ShowMessageBox, "DialogResult is no");
            }
        }
    }
}