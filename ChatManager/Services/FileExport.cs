﻿using ChatManager.Forms;
using ChatManager.Properties;

namespace ChatManager.Services
{
    internal class FileExport
    {
        public FileExport()
        {
            Logging.Write(LogEvent.Info, ProgramClass.FileExport, "FileExport Constructor created").ConfigureAwait(false);
        }

        private static readonly bool backupAvailability = GetSetSettings.GetBackupAvailability;
        private static readonly string backupPath = GetSetSettings.GetBackupPath;

        private static readonly string[] selectedServers = FileSelectorForm.GetSelectedServers;
        private static readonly string[] fileNames = FileSelectorForm.GetListBoxMulti.ToArray();
        
        // Is used for positioning the characters in the array
        private static int arrayCounter = 0;

        public async Task BackupFilesAndWrite(string[] content)
        {
            await Logging.Write(LogEvent.Method, ProgramClass.FileExport, "BackupFilesAndWrite entered");

            // Check if backupDir exists and if not show a warning Box
            if (!backupAvailability)
            {
                await ShowMessageBox.Show(Resources.MessageBoxWarn, Resources.Warn_BackupDirMissing);
            }

            // Check if the user selected any characters
            if (fileNames.Length != 0)
            {
                await Logging.Write(LogEvent.Info, ProgramClass.FileExport, "fileNames Array selected");

                string timestamp = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd_HH-mm-ss");

                string deeperBackup = Path.Combine(backupPath, timestamp);
                await Logging.Write(LogEvent.Variable, ProgramClass.FileExport, $"Create new backup Folder with timestamp: {deeperBackup}");

                Directory.CreateDirectory(deeperBackup);

                if (Directory.Exists(deeperBackup))
                {
                    await Logging.Write(LogEvent.Info, ProgramClass.FileExport, "Backup Folder created");
                }
                else
                {
                    await Logging.Write(LogEvent.Warning, ProgramClass.FileExport, "Backup Folder could NOT be created!");
                    await ShowMessageBox.ShowBug();
                }

                // Get the Array from the association
                string[,] name = AssociateFileWithServer();

                // Debug Purposes only
                // Log every entry if it's not null or empty
                for (int i = 0; i < 100; i++)
                {
                    if (!string.IsNullOrEmpty(name[i, 0]) && !string.IsNullOrEmpty(name[i, 1]))
                    {
                        await Logging.Write(LogEvent.Variable, ProgramClass.FileExport, $"Current name[{i}, 0] is: {name[i, 0]}");
                        await Logging.Write(LogEvent.Variable, ProgramClass.FileExport, $"Current name[{i}, 1] is: {name[i, 1]}");
                    }
                    else
                    {
                        break;
                    }
                }

                // Loop through the arrayCounter and Copy all files in the array to the backup position
                for (int i = 0; i < arrayCounter; i++)
                {
                    await Logging.Write(LogEvent.Variable, ProgramClass.FileExport, $"Current i is: {i}");

                    if (!string.IsNullOrEmpty(name[i, 0]) && !string.IsNullOrEmpty(name[i, 1]))
                    {
                        string path = name[i, 1];
                        await Logging.Write(LogEvent.Variable, ProgramClass.FileExport, $"Current path is: {path}");

                        string fileName = Path.GetFileName(name[i, 1]);
                        await Logging.Write(LogEvent.Variable, ProgramClass.FileExport, $"Current fileName is: {fileName}");

                        string newPath = $"{deeperBackup}\\{fileName}";
                        await Logging.Write(LogEvent.Variable, ProgramClass.FileExport, $"Current newPath is: {newPath}");

                        // Copy only if the dir is present
                        if (backupAvailability)
                        {
                            File.Copy(path, newPath, true);
                            await Logging.Write(LogEvent.Variable, ProgramClass.FileExport, $"File {name[i, 0]} copied to: {newPath}");

                            string[] lines = File.ReadAllLines(path);

                            string searchLine = "ChatColors = ";
                            int lineNumber = 0;

                            // Search the correct line in the file
                            for (int line = 0; line < lines.Length; line++)
                            {
                                if (lines[line].StartsWith(searchLine))
                                {
                                    lineNumber = line;
                                    break;
                                }
                            }

                            // Split the string to only get the wanted numbers
                            // It assumes it starts with a "ChatColors = "
                            string colorLine = lines[lineNumber].Split("=")[1].TrimStart();

                            await Logging.Write(LogEvent.Variable, ProgramClass.FileExport, $"Current ChatColors: {colorLine}");

                            // Split it again to the get colors in an array
                            string[] colorLines = colorLine.Split(";");

                            // Loop through the changed array and check if there's empty colors
                            // Check if the array is big enough else break
                            // If yes check if the old array has any value that can fill it
                            // If yes fill it with the old value
                            for (int color = 0; color < content.Length; color++)
                            {
                                if (content[color] == "")
                                {
                                    if (color >= colorLines.Length)
                                    {
                                        break;
                                    }

                                    if (colorLines[color] != "")
                                    {
                                        content[color] = colorLines[color];
                                    }
                                }
                            }

                            // Put the array into a string
                            string colorIndexes = string.Join(";", content);

                            // Change the line to the new Array of colors
                            lines[lineNumber] = $"ChatColors = {colorIndexes}";

                            await Logging.Write(LogEvent.Variable, ProgramClass.FileExport, $"New ChatColors: {lines[lineNumber]}");

                            // Write it all back
                            File.WriteAllLines(path, lines);

                            await Logging.Write(LogEvent.Variable, ProgramClass.FileExport, $"File {name[i, 0]} written back");
                        }
                        else
                        {
                            await Logging.Write(LogEvent.Warning, ProgramClass.FileExport, $"File {name[i, 0]} NOT copied to: {newPath}!");
                            await ShowMessageBox.ShowBug();
                        }
                    }
                    else
                    {
                        await Logging.Write(LogEvent.Variable, ProgramClass.FileExport, $"Current i: {i} is null or empty");
                    }
                }
            }
            else
            {
                await Logging.Write(LogEvent.Warning, ProgramClass.FileExport, "fileNames Array is empty!");
                await ShowMessageBox.ShowBug();
            }
        }

        // Create a multidimensional Array that has all files associated with the servers
        private static string[,] AssociateFileWithServer()
        {
            Logging.Write(LogEvent.Method, ProgramClass.FileExport, "AssociateFileWithServer entered").ConfigureAwait(false);

            FileImport fileImport = new();

            string[,] namesWithServers = new string[1000, 3];

            // Loop through all servers
            foreach (string server in selectedServers)
            {
                // If server is not filled stop it
                if (string.IsNullOrEmpty(server))
                {
                    break;
                }

                Logging.Write(LogEvent.Variable, ProgramClass.FileExport, $"Current server is: {server}").ConfigureAwait(false);

                // Get the names from all characters on this server
                string[,] name = fileImport.GetArray(server);

                // Loop through and check them
                for (int i = 0; i < fileNames.Length; i++)
                {
                    // i is used to identify the name of the available characters

                    // Loop through all fileNames
                    for (int j = 0; j < name.Length / name.Rank; j++)
                    {
                        // j is used to identify the selected filename by the user

                        if (arrayCounter > fileNames.Length || arrayCounter == fileNames.Length)
                        {
                            Logging.Write(LogEvent.Variable, ProgramClass.FileExport, $"arrayCounter is: {arrayCounter}").ConfigureAwait(false);
                            Logging.Write(LogEvent.Variable, ProgramClass.FileExport, $"fileNames.Length is: {fileNames.Length}").ConfigureAwait(false);

                            break;
                        }

                        // Get the path
                        string file = fileNames[arrayCounter];
                        Logging.Write(LogEvent.Variable, ProgramClass.FileExport, $"Current file is: {file}").ConfigureAwait(false);

                        // Get the filename
                        string fileName = Path.GetFileName(name[j, 1]);
                        Logging.Write(LogEvent.Variable, ProgramClass.FileExport, $"Current fileName is: {fileName}").ConfigureAwait(false);

                        // If file or fileName is null or empty stop it
                        if (string.IsNullOrEmpty(name[j, 0]) && string.IsNullOrEmpty(fileName))
                        {
                            Logging.Write(LogEvent.Variable, ProgramClass.FileExport, "Current name and fileName is empty!").ConfigureAwait(false);
                            break;
                        }

                        // If the current file or fileName are equally with the ones in the array, skip this iteration
                        if (namesWithServers[j, 0] == file && namesWithServers[j, 1] == name[j, 1])
                        {
                            continue;
                        }
                        // Check if the name of the character is the same as the one that was selected
                        // and check if the fileName starts with the server prefix
                        if (name[j, 0] == file && fileName.StartsWith(Checks.ServerNameIdentifier(server)))
                        {
                            Logging.Write(LogEvent.Variable, ProgramClass.FileExport, $"arrayCounter is: {arrayCounter}").ConfigureAwait(false);

                            // If the entry in the array is not null or empty do it,
                            // insert the data in the array, set the counter one up
                            // and then stop if it was inserted in the array
                            if (!string.IsNullOrEmpty(name[j, 1]))
                            {
                                // the fileName
                                namesWithServers[arrayCounter, 0] = fileNames[arrayCounter];

                                // the filePath
                                namesWithServers[arrayCounter, 1] = name[j, 1];

                                // the server
                                namesWithServers[arrayCounter, 2] = server;

                                Logging.Write(LogEvent.Variable, ProgramClass.FileExport, $"{fileNames[arrayCounter]}").ConfigureAwait(false);

                                arrayCounter++;

                                break;
                            }
                            else
                            {
                                Logging.Write(LogEvent.Variable, ProgramClass.FileExport, "Already done or null").ConfigureAwait(false);
                            }
                        }
                    }
                }
            }
            return namesWithServers;
        }
    }
}