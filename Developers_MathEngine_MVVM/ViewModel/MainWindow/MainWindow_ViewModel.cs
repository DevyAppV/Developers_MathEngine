using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Developers_MathEngine_MVVM.Database;
using Developers_MathEngine_MVVM.WindowHandle;
using Microsoft.Data.Sqlite;
using Microsoft.UI;
using Microsoft.Windows.Storage.Pickers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Developers_MathEngine_MVVM.ViewModel.MainWindow
{
    public partial class MainWindow_ViewModel : ObservableObject
    {

        private bool busy = false;
        [RelayCommand]
        public async Task saveFile()
        {
            StorageFolder documentsFolder = KnownFolders.DocumentsLibrary;
            try
            {
                documentsFolder = await documentsFolder.CreateFolderAsync("Developers Math Engine", CreationCollisionOption.FailIfExists);
            }
            catch { }

            //if (File.create)
            // Assuming this code is executed inside your Window class (e.g., MainWindow.xaml.cs)
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(WindowHandler.activeWindow);

            // 1. Get the WindowId associated with the current WinUI 3 Window
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);

            // 2. Initialize the modern FileSavePicker with the WindowId
            FileSavePicker savePicker = new FileSavePicker(windowId);

            // 3. Configure picker properties
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.SuggestedStartFolder = (await documentsFolder.GetFolderAsync("Developers Math Engine")).Path;
            savePicker.SuggestedFileName = DatabaseDB.loadedFilePath;

            // 4. Define allowed extensions (Key = Description, Value = List of extensions)
            savePicker.FileTypeChoices.Add("SQLite Databse", new List<string> { ".db" });

            // 5. Open the picker and retrieve the path-based result
            PickFileResult result = await savePicker.PickSaveFileAsync();

            if (result != null && !string.IsNullOrWhiteSpace(result.Path))
            {
                if (!busy)
                {
                    try
                    {
                        busy = true;
                        //string filePath = result.Path;
                        DatabaseDB.loadedFileName_WithExt = result.Path;
                        DatabaseDB.loadedFilePath = result.Path;

                        //string fileName = Path.GetFileName(result.Path);

                        if (File.Exists(result.Path))
                        {
                            //if (DatabaseDB.DBConnection != null)
                            //{
                            //    // Clear the connection pooling mechanism explicitly
                            //    SqliteConnection.ClearAllPools();

                            //    await DatabaseDB.DBConnection.CloseAsync();
                            //    await DatabaseDB.DBConnection.DisposeAsync();
                            //    // Force release of native file handles 
                            //    GC.Collect();
                            //    GC.WaitForPendingFinalizers();
                            //}

                            File.Copy(result.Path, result.Path + "1", true);
                            //File.Delete(result.Path);

                        }

                        DatabaseDB.createDBConnection(result.Path);
                        await DatabaseDB.DBConnection.OpenAsync();

                        await DatabaseDB.saveDB_FromFilePicker(result.Path);

                        // Clear the connection pooling mechanism explicitly
                        SqliteConnection.ClearAllPools();

                        // Force release of native file handles 

                        GC.WaitForPendingFinalizers();
                        // Force release of native file handles 
                        if (DatabaseDB.DBConnection != null)
                        {
                            await DatabaseDB.DBConnection.CloseAsync();
                            await DatabaseDB.DBConnection.DisposeAsync();
                            DatabaseDB.DBConnection = null;
                        }
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                    catch
                    {
                        // Clear the connection pooling mechanism explicitly
                        SqliteConnection.ClearAllPools();

                        // Force release of native file handles 

                        GC.WaitForPendingFinalizers();
                        // Force release of native file handles 
                        if (DatabaseDB.DBConnection != null)
                        {
                            await DatabaseDB.DBConnection.CloseAsync();
                            await DatabaseDB.DBConnection.DisposeAsync();
                            DatabaseDB.DBConnection = null;
                        }
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                    finally
                    {
                        busy = false;
                    }
                }
                //string contentToSave = "Hello, WinUI 3 World!";

                // 6. Write out using standard modern .NET file streams
                //await File.WriteAllTextAsync(filePath, contentToSave);
            }
        }


        [RelayCommand]
        public async Task loadFile()
        {
            StorageFolder documentsFolder = null;
            try
            {
                documentsFolder = await KnownFolders.DocumentsLibrary.CreateFolderAsync("Developers Math Engine", CreationCollisionOption.FailIfExists);

            }
            catch { }
            // Assuming this code is executed inside your Window class (e.g., MainWindow.xaml.cs)
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(WindowHandler.activeWindow);

            // 1. Get the WindowId associated with the current WinUI 3 Window
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);

            // 2. Initialize FileOpenPicker with the WindowId
            FileOpenPicker openPicker = new FileOpenPicker(windowId);

            // 3. Configure starting criteria
            //openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            if (documentsFolder != null)
            {
                openPicker.SuggestedStartFolder = (await documentsFolder.GetFolderAsync("Developers Math Engine")).Path;
            }
            openPicker.ViewMode = PickerViewMode.List;

            // 4. Specify which file types to look for
            openPicker.FileTypeFilter.Add(".db");

            //openPicker.FileTypeFilter.Add(".json");

            // 5. Show the picker and process the string result
            /*IReadOnlyList<*/
            PickFileResult/*>*/ result = await openPicker.PickSingleFileAsync();

            //foreach (PickFileResult resultItem in result)
            //{
            if (result != null && !string.IsNullOrWhiteSpace(result.Path))
            {
                if (!busy)
                {
                    try
                    {
                        busy = true;
                        //string filePath = resultItem.Path;
                        DatabaseDB.loadedFileName_WithExt = result.Path;
                        DatabaseDB.loadedFilePath = result.Path;
                        Task t = Task.Run(() =>
                        {
                            DatabaseDB.createDBConnection(result.Path);
                        });
                        t.Wait();

                        await DatabaseDB.DBConnection.OpenAsync();
                        await DatabaseDB.loadDB_FromFilePicker(result.Path);

                        // Clear the connection pooling mechanism explicitly
                        SqliteConnection.ClearAllPools();

                        // Force release of native file handles 

                        GC.WaitForPendingFinalizers();
                        // Force release of native file handles 
                        if (DatabaseDB.DBConnection != null)
                        {
                            await DatabaseDB.DBConnection.CloseAsync();
                            await DatabaseDB.DBConnection.DisposeAsync();
                            DatabaseDB.DBConnection = null;
                        }
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        // 6. Read the file contents directly using .NET I/O
                        //string fileContent = await File.ReadAllTextAsync(filePath);

                        // Process your file content here
                    }
                    catch
                    {
                        //busy = false;
                        // Clear the connection pooling mechanism explicitly
                        SqliteConnection.ClearAllPools();

                        // Force release of native file handles 

                        GC.WaitForPendingFinalizers();
                        // Force release of native file handles 
                        if (DatabaseDB.DBConnection != null)
                        {
                            await DatabaseDB.DBConnection.CloseAsync();
                            await DatabaseDB.DBConnection.DisposeAsync();
                            DatabaseDB.DBConnection = null;
                        }
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                    finally
                    {
                        busy = false;
                    }
                }
            }
            //}
        }
    }
}
