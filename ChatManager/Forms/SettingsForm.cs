﻿using ChatManager.Services;

namespace ChatManager.Forms
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private static bool languageChanged = false;

        public static bool GetLanguageChanged => languageChanged;

        private void Localize()
        {
            Logging.Write(LogEvent.Method, ProgramClass.SettingsForm, "Localize entered");

            Localization localization = new(GetSetSettings.GetCurrentLocale);

            // Change the Text of the Form
            Text = localization.GetString(Name);
            Logging.Write(LogEvent.Variable, ProgramClass.SettingsForm, $"FormText set to {Text}");

            // Find all Controls of the desired Type and pack them in a Control List
            IEnumerable<Control> GetControls(Control parent, Type type)
            {
                var controls = parent.Controls.Cast<Control>();

                return controls
                    .Where(c => c.GetType() == type)
                    .Concat(controls.SelectMany(c => GetControls(c, type)));
            }

            var groups = GetControls(this, typeof(GroupBox));
            var checkBoxes = GetControls(this, typeof(CheckBox));

            foreach (Control control in groups)
            {
                if (control is GroupBox group)
                {
                    Logging.Write(LogEvent.Variable, ProgramClass.SettingsForm, $"Control is {group.Name}");
                    group.Text = localization.GetString(group.Name);
                    Logging.Write(LogEvent.Variable, ProgramClass.SettingsForm, $"Control.Text set to {group.Text}");
                }
            }

            foreach (Control control in checkBoxes)
            {
                if (control is CheckBox checkBox)
                {
                    Logging.Write(LogEvent.Variable, ProgramClass.SettingsForm, $"Control is {checkBox.Name}");
                    checkBox.Text = localization.GetString(checkBox.Name);
                    Logging.Write(LogEvent.Variable, ProgramClass.SettingsForm, $"Control.Text set to {checkBox.Text}");
                }
            }

            Logging.Write(LogEvent.Variable, ProgramClass.SettingsForm, $"Control is {lblAutosaveInterval.Name}");
            lblAutosaveInterval.Text = localization.GetString(lblAutosaveInterval.Name);
            Logging.Write(LogEvent.Variable, ProgramClass.SettingsForm, $"Control.Text set to {lblAutosaveInterval.Text}");
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            Localize();
            AdjustContentOnForm();
        }

        private void AdjustContentOnForm()
        {
            Logging.Write(LogEvent.Method, ProgramClass.SettingsForm, "AdjustContentOnForm entered");

            switch (GetSetSettings.GetCurrentLocale)
            {
                case "de":
                    cbLanguage.SelectedIndex = 2;
                    break;

                case "en":
                    cbLanguage.SelectedIndex = 0;
                    break;

                case "fr":
                    cbLanguage.SelectedIndex = 1;
                    break;
            }

            if (GetSetSettings.GetSaveOnClose)
            {
                chbSaveOnClose.Checked = true;
            }
            else
            {
                chbSaveOnClose.Checked = false;
            }

            if (GetSetSettings.GetAutosave)
            {
                chbAutosave.Checked = true;
            }
            else
            {
                chbAutosave.Checked = false;
            }

            numberAutosaveInterval.Value = GetSetSettings.GetAutosaveInterval;
        }

        private void SwitchCurrentLocale(object sender, EventArgs e)
        {
            Logging.Write(LogEvent.Method, ProgramClass.SettingsForm, "SwitchCurrentLocale entered");

            string currLocale = GetSetSettings.GetCurrentLocale;
            string newLanguage = cbLanguage.SelectedItem.ToString()!;

            switch (newLanguage)
            {
                case "English":
                    newLanguage = "en";
                    break;

                case "France":
                    newLanguage = "fr";
                    break;

                case "German":
                    newLanguage = "de";
                    break;
            }

            if (currLocale != newLanguage)
            {
                Logging.Write(LogEvent.Setting, ProgramClass.SettingsForm, $"Saving new locale: {newLanguage}");
                GetSetSettings.SaveSettings("_locale", newLanguage);
                Localize();
            }
        }

        private void ChangingCheckBoxes(object sender, EventArgs e)
        {
            Logging.Write(LogEvent.Method, ProgramClass.SettingsForm, "ChangingSettings entered");

            if (sender is CheckBox checkBox)
            {
                if (checkBox.Name == "chbSaveOnClose")
                {
                    if (checkBox.Checked)
                    {
                        GetSetSettings.SaveSettings("_saveOnClose", true);
                        Logging.Write(LogEvent.Setting, ProgramClass.SettingsForm, "chbSaveOnClose = true");
                        return;
                    }
                    else
                    {
                        GetSetSettings.SaveSettings("_saveOnClose", false);
                        Logging.Write(LogEvent.Setting, ProgramClass.SettingsForm, "chbSaveOnClose = false");
                        return;
                    }
                }
                else if (checkBox.Name == "chbAutosave")
                {
                    if (checkBox.Checked)
                    {
                        GetSetSettings.SaveSettings("_autosave", true);
                        Logging.Write(LogEvent.Setting, ProgramClass.SettingsForm, "Autosave = true");
                        return;
                    }
                    else
                    {
                        GetSetSettings.SaveSettings("_autosave", false);
                        Logging.Write(LogEvent.Setting, ProgramClass.SettingsForm, "Autosave = false");
                        return;
                    }
                }
                else
                {
                    Logging.Write(LogEvent.Warning, ProgramClass.SettingsForm, $"CheckBox: {checkBox.Name} is not listed!");
                }
            }
            else
            {
                Logging.Write(LogEvent.Warning, ProgramClass.SettingsForm, $"Sender: {sender} is not a CheckBox!");
            }
        }

        private void SetAutosaveInterval(object sender, EventArgs e)
        {
            if (sender is NumericUpDown)
            {
                GetSetSettings.SaveSettings("_autosaveInterval", numberAutosaveInterval.Value);
                Logging.Write(LogEvent.Setting, ProgramClass.SettingsForm, $"AutosaveInterval = {numberAutosaveInterval.Value}");
            }
            else
            {
                Logging.Write(LogEvent.Warning, ProgramClass.SettingsForm, $"Sender: {sender} is not a NumericUpDown!");
            }
        }
    }
}