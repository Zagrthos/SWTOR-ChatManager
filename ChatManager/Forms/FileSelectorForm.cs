﻿using ChatManager.Services;

namespace ChatManager.Forms
{
    public partial class FileSelectorForm : Form
    {
        public FileSelectorForm(List<string> servers, bool save)
        {
            if (save)
            {
                isSave = true;
            }
            InitializeComponent();
            SetTabs(servers);
        }

        private readonly bool isSave = false;
        private static string listBoxString = string.Empty;
        private static string listBoxName = string.Empty;
        private static readonly string[] selectedServers = new string[5];
        private static readonly List<string> listBoxMulti = new();

        public static string GetListBoxString => listBoxString;
        public static string GetListBoxName => listBoxName;
        public static string[] GetSelectedServers => selectedServers;
        public static List<string> GetListBoxMulti => listBoxMulti;

        // Remove the not needed servers from the List
        private void SetTabs(List<string> servers)
        {
            Logging.Write(LogEvent.Method, ProgramClass.FileSelectorForm, "SetTabs entered");

            // Create a list of all current TabPages from the tabsFileSelector
            foreach (TabPage tabPage in tabsFileSelector.TabPages.Cast<TabPage>().ToList())
            {
                // Compare only the TabName without the "tb" prefix
                string tabServerName = tabPage.Name.Substring(2);

                // If Server is not in List, kick it
                if (!servers.Contains(tabServerName))
                {
                    tabsFileSelector.TabPages.Remove(tabPage);
                    tabPage.Dispose();
                    Logging.Write(LogEvent.Variable, ProgramClass.FileSelectorForm, $"Server: {tabPage.Text} removed");
                }
                else
                {
                    Logging.Write(LogEvent.Variable, ProgramClass.FileSelectorForm, $"Server: {tabPage.Text} found");
                }
            }
        }

        // Set the correct ListBox for the correct use-case
        private void SetListBox(bool isSave)
        {
            Logging.Write(LogEvent.Method, ProgramClass.FileSelectorForm, "SetListBox entered");

            FileImport fileImport = new();

            // Create a list of all current TabPages from the tabsFileSelector
            foreach (TabPage tabPage in tabsFileSelector.TabPages.Cast<TabPage>().ToList())
            {
                // Get the TableLayoutPanel on the TabPage
                TableLayoutPanel? tlp = tabPage.Controls.OfType<TableLayoutPanel>().FirstOrDefault();

                // If there's a tlp go on
                if (tlp != null)
                {
                    Logging.Write(LogEvent.Control, ProgramClass.FileSelectorForm, $"Selected tlp is: {tlp.Name}");

                    // If the user wants to save it's config, use a different ListBox
                    // ListBox
                    if (!isSave)
                    {
                        Logging.Write(LogEvent.Info, ProgramClass.FileSelectorForm, "ListBox is ListBox");

                        // Set the name of the ListBox
                        string name = $"lbx{tabPage.Name.Substring(2)}";
                        Logging.Write(LogEvent.Control, ProgramClass.FileSelectorForm, $"ListBoxName is: {name}");

                        // Converting the MultiDimensionalArray into a List but remove every entry that is null
                        string[,] charactersMulti = fileImport.GetArray($"{name.Substring(3)}");
                        List<string> characters = new();
                        for (int i = 0; i < 100; i++)
                        {
                            if (charactersMulti[i, 0] != null)
                            {
                                // TODO: Decide if logging to be removed or not
                                characters.Add(charactersMulti[i, 0]);
                                //await Logging.Write(LogEvent.Variable, ProgramClass.FileSelectorForm, $"charakter {i+1} on {name.Substring(3)}: {characters[i]}");
                            }
                        }

                        // Create the new ListBox
                        ListBox listBox = new()
                        {
                            Name = name,
                            Location = new Point(3, 3),
                            Dock = DockStyle.Fill,
                            DataSource = characters
                        };

                        Logging.Write(LogEvent.Control, ProgramClass.FileSelectorForm, $"ListBox: {listBox.Name} created");

                        // Add it to the tlp and adjust the position
                        tlp.Controls.Add(listBox);
                        tlp.SetCellPosition(listBox, new(0, 0));
                        tlp.SetColumnSpan(listBox, 2);

                        // Remove all useless Controls because we don't need them in this case
                        Logging.Write(LogEvent.Info, ProgramClass.FileSelectorForm, "Start to remove useless Controls");
                        for (int columnIndex = 0; columnIndex < tlp.ColumnCount; columnIndex++)
                        {
                            Control? control = tlp.GetControlFromPosition(columnIndex, 1);

                            if (control != null && control.Name != $"btn{name.Substring(3)}Select")
                            {
                                Logging.Write(LogEvent.Control, ProgramClass.FileSelectorForm, $"Control: {control.Name} removed");
                                tlp.Controls.Remove(control);
                                control.Dispose();
                            }
                        }

                        // Set the btnSelect to Column 1 and so in the middle of the Window
                        Control? btnSelect = tlp.GetControlFromPosition(0, 1);
                        if (btnSelect != null)
                        {
                            tlp.SetColumn(btnSelect, 1);
                        }
                    }
                    // CheckedListBox
                    else
                    {
                        Logging.Write(LogEvent.Info, ProgramClass.FileSelectorForm, "ListBox is CheckedListBox");

                        // Set the name of the CheckedListBox
                        string name = $"clbx{tabPage.Name.Substring(2)}";
                        Logging.Write(LogEvent.Control, ProgramClass.FileSelectorForm, $"CheckedListBoxName is: {name}");

                        // Converting the MultiDimensionalArray into a List but remove every entry that is null
                        string[,] charactersMulti = fileImport.GetArray($"{name.Substring(4)}");
                        List<string> characters = new();
                        for (int i = 0; i < 100; i++)
                        {
                            if (charactersMulti[i, 0] != null)
                            {
                                // TODO: Decide if logging to be removed or not
                                characters.Add(charactersMulti[i, 0]);
                                //await Logging.Write(LogEvent.Variable, ProgramClass.FileSelectorForm, $"charakter {i} on {name.Substring(3)}: {characters[i]}");
                            }
                        }

                        // Create the new CheckedListBox
                        CheckedListBox listBox = new()
                        {
                            Name = name,
                            Location = new Point(3, 3),
                            Dock = DockStyle.Fill,
                            CheckOnClick = true,
                            DataSource = characters
                        };

                        Logging.Write(LogEvent.Control, ProgramClass.FileSelectorForm, $"CheckedListBox: {listBox.Name} created");

                        // Add it to the tlp and adjust the position
                        tlp.Controls.Add(listBox);
                        tlp.SetCellPosition(listBox, new(0, 0));
                        tlp.SetColumnSpan(listBox, 2);

                        // Set the btnSelect to Column 1 and so in the middle of the Window
                        // Set the btnSelectAll to Column 1 and so at the left of the Window
                        // Set the btnDeselectAll to Column 2 and so in the right of the Window
                        Control? btnSelect = tlp.GetControlFromPosition(0, 1);
                        Control? btnSelectAll = tlp.GetControlFromPosition(1, 1);
                        Control? btnDeselectAll = tlp.GetControlFromPosition(2, 1);
                        if (btnSelect != null && btnSelectAll != null && btnDeselectAll != null)
                        {
                            tlp.SetColumn(btnSelectAll, 0);
                            tlp.SetColumn(btnSelect, 1);
                            tlp.SetColumn(btnDeselectAll, 2);
                        }
                    }
                }
                else
                {
                    // If there's no tlp, log it
                    Logging.Write(LogEvent.Warning, ProgramClass.FileSelectorForm, $"No tlp found in TabPage: {tabPage.Name}!");
                    ShowMessageBox.ShowBug();
                }
            }
        }

        // On Click of the Button "Select"
        private void ListBoxClick(object sender, EventArgs e)
        {
            Logging.Write(LogEvent.Method, ProgramClass.FileSelectorForm, "ListBoxClick entered");
            Logging.Write(LogEvent.Variable, ProgramClass.FileSelectorForm, $"Sender is: {sender}");

            // If the sender is a Button initialize it as button
            if (sender is Button button)
            {
                Logging.Write(LogEvent.Variable, ProgramClass.FileSelectorForm, $"Button is: {button.Name}");

                // If the button has a Tag initialize it as targetTextBox
                if (button.Tag is string targetListBox)
                {
                    Logging.Write(LogEvent.Variable, ProgramClass.FileSelectorForm, $"Button Tag is: {button.Tag}");

                    // Find the Control...
                    Control? control = Controls.Find(targetListBox, true).FirstOrDefault();
                    Logging.Write(LogEvent.Variable, ProgramClass.FileSelectorForm, $"Control is: {control!.GetType()}");

                    // ... and if it is a CheckedListBox search for the correct panel
                    if (control is CheckedListBox)
                    {
                        // Get all CheckedListBox Controls
                        var checkedListBoxes = GetControls(this, typeof(CheckedListBox));

                        // Set counter to 0
                        byte counter = 0;

                        // Loop all Controls and get the SelectedItems from them
                        foreach (Control getControl in checkedListBoxes)
                        {
                            // Convert them to a CheckListBox
                            CheckedListBox? checkedListBox = getControl as CheckedListBox;
                            Logging.Write(LogEvent.Variable, ProgramClass.FileSelectorForm, $"CheckedListBox is: {checkedListBox!.Name}");

                            // Check if the Controls have ANY checkedItem
                            if (checkedListBox.CheckedItems.Count > 0)
                            {
                                // Set the selectedServers to the correct name
                                selectedServers[counter] = checkedListBox.Name.Substring(4);
                                Logging.Write(LogEvent.Variable, ProgramClass.FileSelectorForm, $"selectedServers[{counter}] is: {selectedServers[counter]}");

                                // Count the CheckedListBoxes
                                counter++;

                                // If yes get them all
                                foreach (var item in checkedListBox.CheckedItems)
                                {
                                    Logging.Write(LogEvent.Variable, ProgramClass.FileSelectorForm, $"Current item is: {item}");

                                    if (item != null)
                                    {
                                        listBoxMulti.Add(item.ToString()!);
                                    }
                                }
                            }
                        }
                    }
                    // ... and if it is a ListBox initialize it as listBox
                    else if (control is ListBox listBox)
                    {
                        Logging.Write(LogEvent.Variable, ProgramClass.FileSelectorForm, $"ListBox is: {listBox.Name}");

                        string charName = listBox.SelectedItem!.ToString()!;
                        string listBoxNaming = listBox.Name;
                        if (!string.IsNullOrEmpty(charName) && !string.IsNullOrEmpty(listBoxNaming))
                        {
                            listBoxString = charName;
                            listBoxName = listBoxNaming;
                        }
                    }
                }
                else
                {
                    Logging.Write(LogEvent.Warning, ProgramClass.MainForm, $"Button: {button.Name} has no Tag!");
                    ShowMessageBox.ShowBug();
                }
            }
            else
            {
                Logging.Write(LogEvent.Warning, ProgramClass.FileSelectorForm, $"Sender: {sender} is not a Button!");
            }

            Close();
        }

        private void SelectClick(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                string tabName = button.Name.Substring(2);

                Control[] controls = Array.Empty<Control>();

                CheckedListBox? checkedListBox;

                if (tabName.Contains("StarForge"))
                {
                    controls = Controls.Find("clbxStarForge", true);
                }
                else if (tabName.Contains("SateleShan"))
                {
                    controls = Controls.Find("clbxSateleShan", true);
                }
                else if (tabName.Contains("DarthMalgus"))
                {
                    controls = Controls.Find("clbxDarthMalgus", true);
                }
                else if (tabName.Contains("TulakHord"))
                {
                    controls = Controls.Find("clbxTulakHord", true);
                }
                else if (tabName.Contains("TheLeviathan"))
                {
                    controls = Controls.Find("clbxTheLeviathan", true);
                }

                if (controls.Length > 0)
                {
                    checkedListBox = controls[0] as CheckedListBox;

                    if (checkedListBox != null)
                    {
                        bool isChecked = !button.Name.Contains("Deselect");
                        for (int i = 0; i < checkedListBox.Items.Count; i++)
                        {
                            checkedListBox.SetItemChecked(i, isChecked);
                        }
                    }
                }
            }
        }

        // Find all Controls of the desired Type and pack them in a Control List
        private IEnumerable<Control> GetControls(Control parent, Type type)
        {
            var controls = parent.Controls.Cast<Control>();

            return controls
                .Where(c => c.GetType() == type)
                .Concat(controls.SelectMany(c => GetControls(c, type)));
        }

        // Change the Tags of the Buttons if the Form is opened in the Save Context
        private void FileSelectorForm_Load(object sender, EventArgs e)
        {
            SetListBox(isSave);

            if (isSave)
            {
                var buttons = GetControls(this, typeof(Button));

                foreach (Control control in buttons)
                {
                    if (control is Button button)
                    {
                        string parent = button.Parent!.Name.Substring(3);

                        if (button.Name == $"btn{parent}Select")
                        {
                            button.Tag = $"c{button.Tag}";
                            Logging.Write(LogEvent.Method, ProgramClass.FileSelectorForm, $"New Tag of {button.Name}: {button.Tag}");
                        }
                    }
                }
            }

            Localize();
        }

        private void Localize()
        {
            Logging.Write(LogEvent.Method, ProgramClass.FileSelectorForm, "Localize entered");

            Localization localization = new(GetSetSettings.GetCurrentLocale);

            // Change the Text of the Form
            Text = localization.GetString(Name);

            var buttons = GetControls(this, typeof(Button));

            foreach (Control control in buttons)
            {
                if (control is Button button)
                {
                    button.Text = localization.GetString(button.Name);
                }
            }
        }
    }
}
