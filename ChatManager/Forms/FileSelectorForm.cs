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

            // Attach an event handler to handle the Load Event
            //Load += (sender, e) => SetListBox(isSave);
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
        private async void SetTabs(List<string> servers)
        {
            await Logging.Write(LogEvent.Method, ProgramClass.FileSelectorForm, "SetTabs entered").ConfigureAwait(false);
            
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
                    await Logging.Write(LogEvent.Variable, ProgramClass.FileSelectorForm, $"Server: {tabPage.Text} removed").ConfigureAwait(false);
                }
                else
                {
                    await Logging.Write(LogEvent.Variable, ProgramClass.FileSelectorForm, $"Server: {tabPage.Text} found").ConfigureAwait(false);
                }
            }
        }

        // Set the correct ListBox for the correct use-case
        private async void SetListBox(bool isSave)
        {
            await Logging.Write(LogEvent.Method, ProgramClass.FileSelectorForm, "SetListBox entered").ConfigureAwait(false);

            FileImport fileImport = new();

            // Create a list of all current TabPages from the tabsFileSelector
            foreach (TabPage tabPage in tabsFileSelector.TabPages.Cast<TabPage>().ToList())
            {
                // Get the TableLayoutPanel on the TabPage
                TableLayoutPanel? tlp = tabPage.Controls.OfType<TableLayoutPanel>().FirstOrDefault();

                // If there's a tlp go on
                if (tlp != null)
                {
                    await Logging.Write(LogEvent.Control, ProgramClass.FileSelectorForm, $"Selected tlp is: {tlp.Name}").ConfigureAwait(false);
                                        
                    // If the user wants to save it's config, use a different ListBox
                    // ListBox
                    if (!isSave)
                    {
                        await Logging.Write(LogEvent.Info, ProgramClass.FileSelectorForm, "ListBox is ListBox").ConfigureAwait(false);

                        // Set the name of the ListBox
                        string name = $"lbx{tabPage.Name.Substring(2)}";
                        await Logging.Write(LogEvent.Control, ProgramClass.FileSelectorForm, $"ListBoxName is: {name}").ConfigureAwait(false);

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

                        await Logging.Write(LogEvent.Control, ProgramClass.FileSelectorForm, $"ListBox: {listBox.Name} created").ConfigureAwait(false);

                        // Add it to the tlp and adjust the position
                        tlp.Controls.Add(listBox);
                        tlp.SetCellPosition(listBox, new(0, 0));
                        tlp.SetColumnSpan(listBox, 2);

                        // Remove all useless Controls because we don't need them in this case
                        await Logging.Write(LogEvent.Info, ProgramClass.FileSelectorForm, "Start to remove useless Controls");
                        for (int columnIndex = 0; columnIndex < tlp.ColumnCount; columnIndex++)
                        {
                            Control? control = tlp.GetControlFromPosition(columnIndex, 1);

                            if (control != null && control.Name != $"btn{name.Substring(3)}Select")
                            {
                                await Logging.Write(LogEvent.Control, ProgramClass.FileSelectorForm, $"Control: {control.Name} removed").ConfigureAwait(false);
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
                        await Logging.Write(LogEvent.Info, ProgramClass.FileSelectorForm, "ListBox is CheckedListBox").ConfigureAwait(false);

                        // Set the name of the CheckedListBox
                        string name = $"clbx{tabPage.Name.Substring(2)}";
                        await Logging.Write(LogEvent.Control, ProgramClass.FileSelectorForm, $"CheckedListBoxName is: {name}").ConfigureAwait(false);

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

                        await Logging.Write(LogEvent.Control, ProgramClass.FileSelectorForm, $"CheckedListBox: {listBox.Name} created").ConfigureAwait(false);

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
                    await Logging.Write(LogEvent.Warning, ProgramClass.FileSelectorForm, $"No tlp found in TabPage: {tabPage.Name}!").ConfigureAwait(false);
                }
            }
        }

        // On Click of the Button "Select"
        private void ListBoxClick(object sender, EventArgs e)
        {
            Logging.Write(LogEvent.Method, ProgramClass.FileSelectorForm, "ListBoxClick entered").ConfigureAwait(false);
            Logging.Write(LogEvent.Variable, ProgramClass.FileSelectorForm, $"Sender is: {sender}").ConfigureAwait(false);

            // If the sender is a Button initialize it as button
            if (sender is Button button)
            {
                Logging.Write(LogEvent.Variable, ProgramClass.FileSelectorForm, $"Button is: {button.Name}").ConfigureAwait(false);

                // If the button has a Tag initialize it as targetTextBox
                if (button.Tag is string targetListBox)
                {
                    Logging.Write(LogEvent.Variable, ProgramClass.FileSelectorForm, $"Button Tag is: {button.Tag}").ConfigureAwait(false);

                    // Find the Control...
                    Control? control = Controls.Find(targetListBox, true).FirstOrDefault();
                    Logging.Write(LogEvent.Variable, ProgramClass.FileSelectorForm, $"Control is: {control!.GetType()}").ConfigureAwait(false);

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
                            Logging.Write(LogEvent.Variable, ProgramClass.FileSelectorForm, $"CheckedListBox is: {checkedListBox!.Name}").ConfigureAwait(false);

                            // Check if the Controls have ANY checkedItem
                            if (checkedListBox.CheckedItems.Count > 0)
                            {
                                // Set the selectedServers to the correct name
                                selectedServers[counter] = checkedListBox.Name.Substring(4);
                                Logging.Write(LogEvent.Variable, ProgramClass.FileSelectorForm, $"selectedServers[{counter}] is: {selectedServers[counter]}").ConfigureAwait(false);

                                // Count the CheckedListBoxes
                                counter++;

                                // If yes get them all
                                foreach (var item in checkedListBox.CheckedItems)
                                {
                                    Logging.Write(LogEvent.Variable, ProgramClass.FileSelectorForm, $"Current item is: {item}").ConfigureAwait(false);

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
                        Logging.Write(LogEvent.Variable, ProgramClass.FileSelectorForm, $"ListBox is: {listBox.Name}").ConfigureAwait(false);

                        string charName = listBox.SelectedItem!.ToString()!;
                        string listBoxNaming = listBox.Name;
                        if (!string.IsNullOrEmpty(charName) && !string.IsNullOrEmpty(listBoxNaming))
                        {
                            listBoxString = charName;
                            listBoxName = listBoxNaming;
                        }
                    }
                }
            }

            Close();
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
                foreach (TabControl tabControl in Controls)
                {
                    foreach (TabPage tab in tabControl.Controls)
                    {
                        foreach (TableLayoutPanel tlp in tab.Controls)
                        {
                            foreach (Control control in tlp.Controls)
                            {
                                if (control is Button button)
                                {
                                    button.Tag = $"c{button.Tag}";
                                    Logging.Write(LogEvent.Method, ProgramClass.FileSelectorForm, $"New Tag of {button.Name}: {button.Tag}").ConfigureAwait(false);
                                }
                            }
                        }
                    }
                }
            }
        }

        // Delete all Controls which are NOT part of the Initialization
        private void FileSelectorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logging.Write(LogEvent.Method, ProgramClass.FileSelectorForm, "Form Closed").ConfigureAwait(false);
            Logging.Write(LogEvent.Info, ProgramClass.FileSelectorForm, "Start to remove Controls").ConfigureAwait(false);

            foreach (TabControl tabControl in Controls)
            {
                foreach (TabPage tab in tabControl.Controls)
                {
                    Logging.Write(LogEvent.Info, ProgramClass.FileSelectorForm, $"tabPage is: {tab.Name}").ConfigureAwait(false);

                    foreach (TableLayoutPanel tlp in tab.Controls)
                    {
                        Logging.Write(LogEvent.Info, ProgramClass.FileSelectorForm, $"tlp is: {tlp.Name}").ConfigureAwait(false);

                        foreach (Control control in tlp.Controls)
                        {
                            if (control is ListBox)
                            {
                                Logging.Write(LogEvent.Info, ProgramClass.FileSelectorForm, $"control name: {control.Name}").ConfigureAwait(false);
                                Logging.Write(LogEvent.Info, ProgramClass.FileSelectorForm, $"control is: {control.GetType()}").ConfigureAwait(false);

                                control.Dispose();
                                tlp.Controls.Remove(control);

                                Logging.Write(LogEvent.Info, ProgramClass.FileSelectorForm, $"control removed").ConfigureAwait(false);
                            }
                            else if (control is CheckedListBox)
                            {
                                Logging.Write(LogEvent.Info, ProgramClass.FileSelectorForm, $"control name: {control.Name}").ConfigureAwait(false);
                                Logging.Write(LogEvent.Info, ProgramClass.FileSelectorForm, $"control is: {control.GetType()}").ConfigureAwait(false);

                                control.Dispose();
                                tlp.Controls.Remove(control);

                                Logging.Write(LogEvent.Info, ProgramClass.FileSelectorForm, $"control removed").ConfigureAwait(false);
                            }
                        }
                    }
                }
            }
        }
    }
}
