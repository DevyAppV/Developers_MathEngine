using Developers_MathEngine_MVVM.Model.Project;
using Microsoft.Data.Sqlite;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Developers_MathEngine_MVVM.Database
{
    public class DatabaseDB
    {
        public static string loadedFilePath = "Formula";
        public static string loadedFileName_WithExt = "";
        public static SqliteConnection DBConnection = null;
        public static SqliteConnectionStringBuilder DBConnectionStringBuilder = null;

        public static void createDBConnectionString(string fileName_WithExt)
        {
            DBConnectionStringBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = fileName_WithExt
            };
        }

        public static void createDBConnection(string fileName_WithExt)
        {
            DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            //Task t = Task.Run(() =>
            //{
            //dispatcherQueue.TryEnqueue(async () =>
            //{
            createDBConnectionString(fileName_WithExt);
            if (DBConnectionStringBuilder != null)
            {
                DBConnection = new SqliteConnection(DBConnectionStringBuilder.ConnectionString);
            }
            //});
            //});
            //t.Wait();
        }







        public static async Task saveDB_FromFilePicker(string filePath)
        {
            if (!string.IsNullOrEmpty(loadedFileName_WithExt))
            {
                if (DBConnection != null)
                {
                    DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

                    //Task t0 = Task.Run(async () =>
                    //{
                    //    dispatcherQueue.TryEnqueue(async () =>
                    //    {
                    //        DatabaseDB.createDBConnectionString(filePath);
                    //        using (SqliteConnection DBConnection = new SqliteConnection(DatabaseDB.DBConnectionStringBuilder.ConnectionString))
                    //        {
                    //            DatabaseDB.DBConnection = DBConnection;
                    //            await DatabaseDB.DBConnection.OpenAsync();
                    //            await Formula.FormulaProjectDB.dropFormulaProjectDB("FormulaProject", "Formula", "FormulaVariable", "FormulaRule");

                    //        }
                    //    });
                    //});
                    Task t = Task.Run(async () =>
                    {
                        dispatcherQueue.TryEnqueue(async () =>
                        {
                            DatabaseDB.createDBConnectionString(filePath);
                            using (SqliteConnection DBConnection = new SqliteConnection(DatabaseDB.DBConnectionStringBuilder.ConnectionString))
                            {
                                DatabaseDB.DBConnection = DBConnection;
                                //createDBConnection(loadedFileName_WithExt);
                                await DatabaseDB.DBConnection.OpenAsync();
                                await Formula.FormulaProjectDB.dropFormulaProjectDB("FormulaProject", "Formula", "FormulaVariable", "FormulaRule");

                                await Formula.FormulaProjectDB.createFormulaProjectDB("FormulaProject", "Formula", "FormulaVariable", "FormulaRule");
                                await Custom_Buttons.CustomButtonDB.createCustomButtonDB("CustomButton", "CustomButtonVariable", "CustomButtonRule");
                            }
                        });
                    });


                    Task t2 = Task.Run(async () =>
                    {
                        dispatcherQueue.TryEnqueue(async () =>
                        {
                            DatabaseDB.createDBConnectionString(filePath);
                            using (SqliteConnection DBConnection = new SqliteConnection(DatabaseDB.DBConnectionStringBuilder.ConnectionString))
                            {
                                DatabaseDB.DBConnection = DBConnection;
                                //createDBConnection(loadedFileName_WithExt);
                                await DatabaseDB.DBConnection.OpenAsync();
                                //TODO: populate db and save to db; alter dynamic update
                                await Formula.FormulaProjectDB.saveFormulaPoject("FormulaProject", "Formula", "FormulaVariable", "FormulaRule");
                                await Custom_Buttons.CustomButtonDB.saveCustomButton("CustomButton", "CustomButtonVariable", "CustomButtonRule");
                            }
                        });
                    });
                    //t0.Wait();
                    //while (t0.Status != TaskStatus.RanToCompletion) { await Task.Delay(200); }
                    t.Wait();
                    while (t.Status != TaskStatus.RanToCompletion) { await Task.Delay(200); }
                    t2.Wait();
                    while (t2.Status != TaskStatus.RanToCompletion) { await Task.Delay(200); }

                    await DBConnection.CloseAsync();
                }
            }
        }

        public static async Task loadDB_FromFilePicker(string filePath)
        {

            //TODO:
            if (!string.IsNullOrEmpty(loadedFileName_WithExt))
            {
                createDBConnection(loadedFileName_WithExt);
                if (DBConnection != null)
                {
                    DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
                    Task t = Task.Run(() =>
                    {
                        dispatcherQueue.TryEnqueue(() =>
                        {
                            MainProject.formulaProject_Collection.Clear();
                            Dependency_Injection.DependencyInjectionShared.formula_ViewModel.formulaListDisplay_Collection.Clear();
                            Custom_Buttons.CustomButtonDB.removeCustomButtonsBeforeLoad();
                            Custom_Buttons.CustomButtonDB.removeCustomButtonsTabsBeforeLoad();
                            Custom_Buttons.CustomButtonDB.addDefaultCustomButtonsTabsBeforeLoad();
                        });
                    });
                    await DBConnection.OpenAsync();

                    Task t2 = Task.Run(async () =>
                    {
                        dispatcherQueue.TryEnqueue(async () =>
                        {
                            DatabaseDB.createDBConnectionString(filePath);
                            using (SqliteConnection DBConnection = new SqliteConnection(DatabaseDB.DBConnectionStringBuilder.ConnectionString))
                            {
                                DatabaseDB.DBConnection = DBConnection;
                                //createDBConnection(loadedFileName_WithExt);
                                await DatabaseDB.DBConnection.OpenAsync();
                                await Formula.FormulaProjectDB.loadFormulaDB("FormulaProject", "Formula", "FormulaVariable", "FormulaRule", dispatcherQueue, filePath);
                            }
                        });
                    });
                    //});
                    Task t3 = Task.Run(async () =>
                    {
                        dispatcherQueue.TryEnqueue(async () =>
                        {
                            DatabaseDB.createDBConnectionString(filePath);
                            using (SqliteConnection DBConnection = new SqliteConnection(DatabaseDB.DBConnectionStringBuilder.ConnectionString))
                            {
                                DatabaseDB.DBConnection = DBConnection;
                                //createDBConnection(loadedFileName_WithExt);
                                await DatabaseDB.DBConnection.OpenAsync();
                                await Custom_Buttons.CustomButtonDB.loadCustomButtonDB("CustomButton", "CustomButtonVariable", "CustomButtonRule", dispatcherQueue, filePath);
                            }
                        });
                    });
                    t.Wait();
                    while (t.Status != TaskStatus.RanToCompletion) { await Task.Delay(200); }
                    t2.Wait();
                    while (t2.Status != TaskStatus.RanToCompletion) { await Task.Delay(200); }
                    t3.Wait();
                    while (t3.Status != TaskStatus.RanToCompletion) { await Task.Delay(200); }
                    //TODO: Load from DB and populate app data; alter dynamic update


                    await DBConnection.CloseAsync();
                }
            }
        }





    }
}
