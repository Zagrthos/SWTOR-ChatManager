﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ChatManager.Enums;
using ChatManager.Services;

namespace ChatManager.Forms;

internal partial class BackupSelectorForm : Form
{
    internal BackupSelectorForm()
    {
        InitializeComponent();
        Localize();
        DisplayBackupDirs();
    }

    private static readonly string backupPath = GetSetSettings.GetBackupPath;
    private string[,] filesInDir = new string[1000, 2];

    private void DisplayBackupDirs()
    {
        Logging.Write(LogEventEnum.Method, ProgramClassEnum.BackupSelector, "DisplayBackupDirs entered");

        // Search the given Path for Directories
        string[] backupDirs = Directory.GetDirectories(backupPath);
        Logging.Write(LogEventEnum.Variable, ProgramClassEnum.BackupSelector, $"backupDirs: {backupDirs.Length}");

        // Create new Array with the size of the found Paths
        string[] backupDirsName = new string[backupDirs.Length];
        Logging.Write(LogEventEnum.Variable, ProgramClassEnum.BackupSelector, $"backupDirsName: {backupDirsName.Length}");

        // Fill the Array with the Directory names
        for (int i = 0; i < backupDirs.Length; i++)
        {
            backupDirsName[i] = Path.GetFileName(backupDirs[i]);
            //Logging.Write(LogEventEnum.Variable, ProgramClassEnum.BackupSelector, backupDirsName[i]);
        }

        // Reverse the names so the newest is first
        Array.Reverse(backupDirsName);

        // Set the DataSource
        lbxBackupDir.DataSource = backupDirsName;

        if (backupDirsName.Length == 0)
        {
            btnDeleteDir.Enabled = false;
        }
        else
        {
            btnDeleteDir.Enabled = true;
        }
    }

    private void DisplayBackupFiles(string dirName)
    {
        Logging.Write(LogEventEnum.Method, ProgramClassEnum.BackupSelector, "DisplayBackupFiles entered");

        // Clear the DataSource so the checked Items get unchecked
        clbxBackupFiles.DataSource = null;

        // Search the given Path for Files
        string[] dirContent = Directory.GetFiles(dirName);
        Logging.Write(LogEventEnum.Variable, ProgramClassEnum.BackupSelector, $"fileNames: {dirContent.Length}");

        if (dirContent.Length == 0)
        {
            btnDeleteFiles.Enabled = false;
            btnBackupDeselectAll.Enabled = false;
            btnBackupSelectAll.Enabled = false;
            btnRestore.Enabled = false;
            return;
        }
        else
        {
            btnDeleteFiles.Enabled = true;
            btnBackupDeselectAll.Enabled = true;
            btnBackupSelectAll.Enabled = true;
            btnRestore.Enabled = true;
        }

        // Associate the Files with their Paths
        AssociateFilesWithPaths(dirContent);

        // Create new Array with the size of the found Files
        string[] files = new string[dirContent.Length];

        for (int i = 0; i < filesInDir.Length / 2; i++)
        {
            //Logging.Write(LogEventEnum.Variable, ProgramClassEnum.BackupSelector, $"{i}");

            // Check if all parts in array are NOT empty or null
            if (!string.IsNullOrEmpty(filesInDir[i, 0]) && !string.IsNullOrEmpty(filesInDir[i, 1]) && !string.IsNullOrEmpty(filesInDir[i, 2]))
            {
                //Logging.Write(LogEventEnum.Variable, ProgramClassEnum.BackupSelector, $"{filesInDir[i, 0]} & {filesInDir[i, 1]} & {filesInDir[i, 2]}");
                //Logging.Write(LogEventEnum.Variable, ProgramClassEnum.BackupSelector, $"file[{i}]");

                // If not null or empty add the name and the server to the list
                files[i] = $"{filesInDir[i, 0]} - {Converter.AddWhitespace(Converter.ServerNameIdentifier(filesInDir[i, 1], false))}";
                Logging.Write(LogEventEnum.Variable, ProgramClassEnum.BackupSelector, files[i]);
            }
            else
            {
                Logging.Write(LogEventEnum.Variable, ProgramClassEnum.BackupSelector, "Already done or null");
                break;
            }
        }

        // Set the DataSource
        clbxBackupFiles.DataSource = files;
    }

    private void AssociateFilesWithPaths(string[] paths)
    {
        Logging.Write(LogEventEnum.Method, ProgramClassEnum.BackupSelector, "AssociateFilesWithPaths entered");

        // Clear the Array
        filesInDir = new string[1000, 3];

        // Now get the FileName, split it into parts and then
        // Set the name to pos 0
        // Set the server to pos 1
        // Set the path to pos 2
        for (int i = 0; i < paths.Length; i++)
        {
            string fileName = Path.GetFileName(paths[i]);
            string[] parts = fileName.Split("_");
            filesInDir[i, 0] = parts[1];
            filesInDir[i, 1] = parts[0];
            filesInDir[i, 2] = paths[i];
            Logging.Write(LogEventEnum.Variable, ProgramClassEnum.BackupSelector, filesInDir[i, 0]);
            Logging.Write(LogEventEnum.Variable, ProgramClassEnum.BackupSelector, filesInDir[i, 1]);
            Logging.Write(LogEventEnum.Variable, ProgramClassEnum.BackupSelector, filesInDir[i, 2]);
        }
    }

    private void SelectBackupDir(object sender, EventArgs e)
    {
        Logging.Write(LogEventEnum.Method, ProgramClassEnum.BackupSelector, "SelectBackupDir entered");

        if (sender is ListBox listBox)
        {
            string? dirName = listBox.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(dirName))
            {
                DisplayBackupFiles(Path.Combine(backupPath, dirName));
            }
            else
            {
                Logging.Write(LogEventEnum.Warning, ProgramClassEnum.BackupSelector, "dirName is null or empty!");
            }
        }
        else
        {
            Logging.Write(LogEventEnum.Warning, ProgramClassEnum.BackupSelector, $"Sender: {sender} is not a ListBox!");
        }
    }

    private void SelectClick(object sender, EventArgs e)
    {
        Logging.Write(LogEventEnum.Method, ProgramClassEnum.BackupSelector, "SelectClick entered");

        if (sender is Button button)
        {
            bool isChecked = !button.Name.Contains("Deselect");
            for (int i = 0; i < clbxBackupFiles.Items.Count; i++)
            {
                clbxBackupFiles.SetItemChecked(i, isChecked);
            }
            Logging.Write(LogEventEnum.Info, ProgramClassEnum.BackupSelector, $"All Checked items are set to: {isChecked}");
        }
        else
        {
            Logging.Write(LogEventEnum.Warning, ProgramClassEnum.BackupSelector, $"Sender: {sender} is not a Button!");
        }
    }

    private void DeleteClick(object sender, EventArgs e)
    {
        Logging.Write(LogEventEnum.Method, ProgramClassEnum.BackupSelector, "DeleteClick entered");

        if (sender is Button button)
        {
            Logging.Write(LogEventEnum.Variable, ProgramClassEnum.BackupSelector, $"Button is: {button.Name}");

            switch (button.Name)
            {
                case "btnDeleteDir":
                    if (!string.IsNullOrEmpty(lbxBackupDir.SelectedItem!.ToString()))
                    {
                        Logging.Write(LogEventEnum.Variable, ProgramClassEnum.BackupSelector, $"DirToDelete: {lbxBackupDir.SelectedItem}");

                        // Convert DataSource to List if not null
                        if (lbxBackupDir.DataSource != null)
                        {
                            string[] array = (string[])lbxBackupDir.DataSource;
                            List<string> dataSource = array.ToList();

                            // Delete the Directory
                            Directory.Delete(Path.Combine(backupPath, lbxBackupDir.SelectedItem.ToString()!), true);

                            // Remove item from DataSource
                            dataSource.Remove(lbxBackupDir.SelectedItem.ToString()!);

                            // Set List as DataSource back in
                            lbxBackupDir.DataSource = dataSource.ToArray();

                            // If no items left disable buttons and clear ListBox
                            if (dataSource.Count == 0)
                            {
                                btnDeleteDir.Enabled = false;
                                btnDeleteFiles.Enabled = false;
                                clbxBackupFiles.DataSource = null;
                                btnBackupDeselectAll.Enabled = false;
                                btnBackupSelectAll.Enabled = false;
                                btnRestore.Enabled = false;
                            }
                        }
                        else
                        {
                            Logging.Write(LogEventEnum.Error, ProgramClassEnum.BackupSelector, $"DataSource is missing from {lbxBackupDir.Name}!");
                            ShowMessageBox.ShowBug();
                            return;
                        }
                    }
                    else
                    {
                        Logging.Write(LogEventEnum.Warning, ProgramClassEnum.BackupSelector, "SelectedItem is null or empty!");
                        ShowMessageBox.ShowBug();
                        return;
                    }
                    break;

                case "btnDeleteFiles":
                    if (clbxBackupFiles.CheckedItems.Count > 0)
                    {
                        if (clbxBackupFiles.DataSource != null)
                        {
                            string[] array = (string[])clbxBackupFiles.DataSource;
                            List<string> dataSource = array.ToList();

                            foreach (string item in clbxBackupFiles.CheckedItems)
                            {
                                Logging.Write(LogEventEnum.Variable, ProgramClassEnum.BackupSelector, $"FileToDelete: {item}");

                                // Split the name in two parts
                                string[] parts = item.Split(" - ");

                                // Generate the fileName
                                string fileName = Converter.ServerNameIdentifier(Converter.RemoveWhitespace(parts[1]), true) + $"_{parts[0]}_PlayerGUIState.ini";

                                // Generate the filePath
                                string path = Path.Combine(backupPath, lbxBackupDir.SelectedItem!.ToString()!, fileName);

                                // Remove item from DataSource
                                dataSource.Remove(item);

                                // Delete the file
                                File.Delete(path);
                            }

                            // Set List as DataSource back in
                            clbxBackupFiles.DataSource = dataSource.ToArray();

                            // Deselect everything else
                            btnBackupDeselectAll.PerformClick();

                            // If no items left disable buttons
                            if (dataSource.Count == 0)
                            {
                                btnDeleteFiles.Enabled = false;
                                btnBackupDeselectAll.Enabled = false;
                                btnBackupSelectAll.Enabled = false;
                                btnRestore.Enabled = false;
                            }
                        }
                    }
                    else
                    {
                        Localization localization = new(GetSetSettings.GetCurrentLocale);

                        ShowMessageBox.Show(localization.GetString(LocalizationEnum.MessageBoxError), localization.GetString(LocalizationEnum.Err_NoFileToDeleteSelected));
                        return;
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
        else
        {
            Logging.Write(LogEventEnum.Warning, ProgramClassEnum.BackupSelector, $"Sender: {sender} is not a Button!");
        }
    }

    private void Restore(object sender, EventArgs e)
    {
        Logging.Write(LogEventEnum.Method, ProgramClassEnum.BackupSelector, "Restore entered");

        if (sender is Button button && button.Name == btnRestore.Name)
        {
            Localization localization = new(GetSetSettings.GetCurrentLocale);

            if (clbxBackupFiles.CheckedItems.Count == 0)
            {
                ShowMessageBox.Show(localization.GetString(LocalizationEnum.MessageBoxError), localization.GetString(LocalizationEnum.Err_NoExportFileSelected));
                return;
            }

            // Get all the checked items in an array
            string[] checkedItems = clbxBackupFiles.CheckedItems.Cast<string>().ToArray();
            Logging.Write(LogEventEnum.Variable, ProgramClassEnum.BackupSelector, $"checkedItems: {checkedItems.Length}");

            string localPath = GetSetSettings.GetLocalPath;

            // Now loop the array
            for (int i = 0; i < checkedItems.Length; i++)
            {
                // Split the name in two parts
                string[] parts = checkedItems[i].Split(" - ");

                // Generate the fileName
                string fileName = Converter.ServerNameIdentifier(Converter.RemoveWhitespace(parts[1]), true) + $"_{parts[0]}_PlayerGUIState.ini";

                // Generate the filePath
                string path = Path.Combine(backupPath, lbxBackupDir.SelectedItem!.ToString()!, fileName);

                // Replace the file
                File.Copy(path, Path.Combine(localPath, fileName), true);
            }

            string changedFiles = localization.GetString(LocalizationEnum.Inf_ExportedFiles);
            changedFiles = changedFiles.Replace("FILECOUNT", checkedItems.Length.ToString());

            ShowMessageBox.Show(localization.GetString(LocalizationEnum.MessageBoxInfo), changedFiles);
        }
        else
        {
            Logging.Write(LogEventEnum.Warning, ProgramClassEnum.BackupSelector, $"Sender: {sender} is not a Button or {btnRestore.Name}!");
        }
    }

    private void Localize()
    {
        Logging.Write(LogEventEnum.Method, ProgramClassEnum.BackupSelector, "Localize entered");

        Localization localization = new(GetSetSettings.GetCurrentLocale);

        Text = localization.GetString(Name);

        IEnumerable<Control> GetControls(Control parent, Type type)
        {
            var controls = parent.Controls.Cast<Control>();

            return controls
                .Where(c => c.GetType() == type)
                .Concat(controls.SelectMany(c => GetControls(c, type)));
        }

        var buttons = GetControls(this, typeof(Button));
        var labels = GetControls(this, typeof(Label));

        foreach (Control control in buttons)
        {
            if (control is Button button)
            {
                button.Text = localization.GetString(button.Name);
            }
        }

        foreach (Control control in labels)
        {
            if (control is Label label && label.Name != lblDateConvertion.Name)
            {
                label.Text = localization.GetString(label.Name);
            }
        }

        (string date, string time) = localization.GetLocalDateTime();

        lblDateConvertion.Text = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss} = {date} - {time}";
    }
}
