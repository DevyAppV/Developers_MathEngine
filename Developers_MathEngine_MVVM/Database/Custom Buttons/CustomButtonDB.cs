
using Developers_MathEngine_MVVM.Database.Formula;
using Developers_MathEngine_MVVM.Model.Formula;
using Developers_MathEngine_MVVM.Model.Project;
using Microsoft.Data.Sqlite;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage.Streams;

namespace Developers_MathEngine_MVVM.Database.Custom_Buttons
{
    public class CustomButtonDB : FormulaProjectDB
    {

        public static async Task createCustomButtonDB(
            string CustomButton_List_TableName,
            string CustomButton_VariableList_TableName,
            string CustomButton_RuleList_TableName)
        {
            await dropTables_First(CustomButton_List_TableName);
            await dropTables_First(CustomButton_VariableList_TableName);
            await dropTables_First(CustomButton_RuleList_TableName);

            await create_CustomButtonList_Table(CustomButton_List_TableName);
            await create_FormulaVariableList_Table(CustomButton_VariableList_TableName, true/*,"pk_customButtonList_VarList"*/, "fk_customButtonName_VarList", "idx_customButtonList_VarList__List_id");
            await create_FormulaRulesList_Table(CustomButton_RuleList_TableName, true/*, "pk_customButtonList_RuleList"*/, "fk_customButtonName_RuleList", "idx_customButtonList_RuleList__List_id");

        }

        #region create data tables

        protected private static async Task create_CustomButtonList_Table(string tableName, string indexId_Name = "idx_CustomButton__CustomButton_Name")
        {
            SqliteCommand command = DatabaseDB.DBConnection.CreateCommand();
            command.CommandText = $"CREATE TABLE IF NOT EXISTS {tableName}(" +
                $"subTabName TEXT," +
                $"tabName TEXT," +
                $"tabRow0Index TEXT," +
                $"tabRow1Index TEXT," +
                $"tabRow2Index TEXT," +
                $"tabRow_ListIndex TEXT," +
                $"name TEXT," +
                $"variable_script TEXT," +
                $"formula_script TEXT," +
                $"description_script TEXT," +
                $"use TEXT" +
                $");";

            await command.ExecuteNonQueryAsync();

            command.CommandText = $"CREATE INDEX IF NOT EXISTS {indexId_Name} ON {tableName}(name);";


            await command.ExecuteNonQueryAsync();

        }
        #endregion create data tables


        #region save data tables
        private static async Task buildCommand_ForSaveCustomButton(string CustomButton_List_TableName, CustomButtonTab customBtnTab,
            string CustomButton_VariableList_TableName,
            string CustomButton_RuleList_TableName, string tabName, int tabRow_ListIndex,
            int row0_Index, int row1_Index, int row2_Index)
        {
            SqliteCommand command = DatabaseDB.DBConnection.CreateCommand();

            //int tabRow_ListIndex = 0;
            if (customBtnTab.customButtons_Collection.Count <= 0)
            {
                command.CommandText = $"INSERT INTO {CustomButton_List_TableName} VALUES (" +
                    $"\'{customBtnTab.SubTabName}\'," +
                    $"\'{tabName}\'," +
                    $"\'{row0_Index}\'," +
                    $"\'{row1_Index}\'," +
                    $"\'{row2_Index}\'," +
                    $"\'{tabRow_ListIndex.ToString()}\'," +
                    $"\'{""}\'," +
                    $"\'{""}\'," +
                    $"\'{""}\'," +
                    $"\'{""}\'," +
                    $"\'{""}\'" +
                    $");";

                await command.ExecuteNonQueryAsync();
            }
            else
            {
                foreach (Model.Formula.CustomButton customButton in customBtnTab.customButtons_Collection)
                {

                    command.CommandText = $"INSERT INTO {CustomButton_List_TableName} VALUES (" +
                        $"\'{customBtnTab.SubTabName}\'," +
                        $"\'{tabName}\'," +
                        $"\'{row0_Index}\'," +
                        $"\'{row1_Index}\'," +
                        $"\'{row2_Index}\'," +
                        $"\'{tabRow_ListIndex.ToString()}\'," +
                        $"\'{customButton.FormulaName}\'," +
                        $"\'{customButton.VariableExec_Script.Script}\'," +
                        $"\'{customButton.FormulaExec_Script.Script}\'," +
                        $"\'{customButton.Description}\'," +
                        $"\'{customButton.Use}\'" +
                        $");";

                    await command.ExecuteNonQueryAsync();

                    if (customButton.Rules_Collection != null)
                    {
                        foreach (Model.Formula.Rule rule in customButton.Rules_Collection)
                        {
                            command.CommandText = $"INSERT INTO {CustomButton_RuleList_TableName} VALUES (" +
                            $"\'{customButton.FormulaName}\'," +
                            $"\'{rule.ruleIndex}\'," +
                            $"\'{rule.codeRule}\'" +
                            $");";
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                    if (customButton.Variable_Collection != null)
                    {
                        foreach (Model.Formula.Variable variable in customButton.Variable_Collection)
                        {
                            command.CommandText = $"INSERT INTO {CustomButton_VariableList_TableName} VALUES (" +
                            $"\'{customButton.FormulaName}\'," +
                            $"\'{variable.VariableIndex}\'," +
                            $"\'{variable.VariableName}\'," +
                            $"\'{variable.VariableValue}\'," +
                            $"\'{variable.VariableDescription}\'" +
                            $");";
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
        }

        public static async Task saveCustomButton(string CustomButton_List_TableName,
            string CustomButton_VariableList_TableName,
            string CustomButton_RuleList_TableName)
        {




            //SqliteCommand command = DBConnection.CreateCommand();

            int tabRow0_ListIndex = 0;
            foreach (CustomButtonTab tabRow0 in MainProject.CustomButtonTab_Collection)
            //while(tabRow0_ListIndex < MainProject.CustomButtonTab_Collection.Count)
            {
                if (tabRow0_ListIndex == 0)
                {
                    //skip defaultTab
                    tabRow0_ListIndex++;
                    continue;
                }
                await buildCommand_ForSaveCustomButton(CustomButton_List_TableName, tabRow0,
                    CustomButton_VariableList_TableName, CustomButton_RuleList_TableName, tabRow0.Name, tabRow0_ListIndex,
                    0, -1, -1);
                int subTab1_ListIndex = 0;
                foreach (CustomButtonTab tabRow1 in tabRow0.customButtonsSubTab_Collection)
                {
                    await buildCommand_ForSaveCustomButton(CustomButton_List_TableName, tabRow1,
                    CustomButton_VariableList_TableName, CustomButton_RuleList_TableName, tabRow1.Name, subTab1_ListIndex,
                    0, 0, -1);
                    int subTab2_ListIndex = 0;
                    foreach (CustomButtonTab tabRow2 in tabRow1.customButtonsSubTab_Collection)
                    {

                        await buildCommand_ForSaveCustomButton(CustomButton_List_TableName, tabRow2,
                    CustomButton_VariableList_TableName, CustomButton_RuleList_TableName, tabRow2.Name, subTab2_ListIndex,
                    0, 0, 0);

                        subTab2_ListIndex++;
                    }
                    subTab1_ListIndex++;
                }
                tabRow0_ListIndex++;

            }


            //await command.ExecuteNonQueryAsync();
        }

        #endregion save data tables

        #region Load Data Tables
        public static void addDefaultCustomButtonsTabsBeforeLoad()
        {

            //DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            //Task addDefaultCustomButtons = Task.Run(() =>
            //{
            //    dispatcherQueue.TryEnqueue(() =>
            //    {
            MainProject.CustomButtonTab_Collection.Add(new CustomButtonTab());

            MainProject.CustomButtonTab_Collection.Last().setUpDefaultCustomButton();
            //    });
            //});
            //addDefaultCustomButtons.Wait();

        }
        public static void removeCustomButtonsTabsBeforeLoad()
        {
            //DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            //Task clearTabs = Task.Run(() =>
            //{
            //    dispatcherQueue.TryEnqueue(() =>
            //    {
            int t0 = 0;
            while (t0 < MainProject.CustomButtonTab_Collection.Count)
            //foreach (var tab0 in MainProject.CustomButtonTab_Collection)
            {
                int t1 = 0;
                while (t1 < MainProject.CustomButtonTab_Collection[t0].customButtonsSubTab_Collection.Count)
                {
                    MainProject.CustomButtonTab_Collection[t0].customButtonsSubTab_Collection[t1].customButtonsSubTab_Collection.Clear();


                    t1++;
                }
                MainProject.CustomButtonTab_Collection[t0].customButtonsSubTab_Collection.Clear();
                t0++;
            }

            MainProject.CustomButtonTab_Collection.Clear();

            //MainProject.CustomButtonTab_Collection = null;
            //MainProject.CustomButtonTab_Collection = new ObservableCollection<CustomButtonTab>() { new CustomButtonTab() };
            if (Dependency_Injection.DependencyInjectionShared.customButton_ViewModel.selectedCustomButtonTab_ToDisplayButtons != null)
                Dependency_Injection.DependencyInjectionShared.customButton_ViewModel.selectedCustomButtonTab_ToDisplayButtons.Clear();

            //    });
            //});

            //clearTabs.Wait();

        }

        public static void removeCustomButtonsBeforeLoad()
        {

            //DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();


            //Task clearButtons = Task.Run(() =>
            //{
            //dispatcherQueue.TryEnqueue(() =>
            //{
            CustomButtonTab.customButtons_Dictionary.Clear();
            int t0 = 0;
            while (t0 < MainProject.CustomButtonTab_Collection.Count)
            //foreach (var tab0 in MainProject.CustomButtonTab_Collection)
            {
                int t1 = 0;
                while (t1 < MainProject.CustomButtonTab_Collection[t0].customButtonsSubTab_Collection.Count)
                {
                    int t2 = 0;
                    while (t2 < MainProject.CustomButtonTab_Collection[t0].customButtonsSubTab_Collection[t1].customButtonsSubTab_Collection.Count)
                    {
                        MainProject.CustomButtonTab_Collection[t0].customButtonsSubTab_Collection[t1].customButtonsSubTab_Collection[t2].customButtons_Collection.Clear();


                        t2++;
                    }

                    MainProject.CustomButtonTab_Collection[t0].customButtonsSubTab_Collection[t1].customButtons_Collection.Clear();

                    t1++;
                }

                MainProject.CustomButtonTab_Collection[t0].customButtons_Collection.Clear();
                t0++;
            }
            //    });
            //});
            ////dispatcherQueue.TryEnqueue(async() =>
            ////{
            //clearButtons.Wait();
            //});
        }







        private static void initialLoadBtnFormula(
            SqliteDataReader customButtonDataReader, ObservableCollection<CustomButtonTab> tabToSearch)
        {

            string subTabName = customButtonDataReader.GetString(0);
            string tabName = customButtonDataReader.GetString(1);
            string row0 = customButtonDataReader.GetString(2);
            string row1 = customButtonDataReader.GetString(3);
            string row2 = customButtonDataReader.GetString(4);
            string _FormulaName = customButtonDataReader.GetString(6);
            string _VariableExec_Script = customButtonDataReader.GetString(7);
            string _FormulaExec_Script = customButtonDataReader.GetString(8);
            string _Description = customButtonDataReader.GetString(9);
            string _Use = customButtonDataReader.GetString(10);
            //string listIndex = customButton1_DataReader.GetString(3);
            //MainProject.CustomButtonTab_Collection.
            int row = 0;
            if (!row1.Equals("-1") && row2.Equals("-1")) row = 1;
            else if (!row1.Equals("-1") && !row2.Equals("-1")) row = 2;
            if (
                string.IsNullOrEmpty(_FormulaName) &&
                string.IsNullOrEmpty(_VariableExec_Script) &&
                string.IsNullOrEmpty(_FormulaExec_Script) &&
                string.IsNullOrEmpty(_Description) &&
                string.IsNullOrEmpty(_Use)
                )
            {
                if (!string.IsNullOrEmpty(subTabName))
                {
                    var tab = tabToSearch.FirstOrDefault(x => x.Name.Equals(subTabName));
                    if (tab != null)
                    {
                        CustomButtonTab tabCollection = null;
                        tabCollection = tab.customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals(tabName));
                        if (tabCollection == null)
                        {
                            tab.customButtonsSubTab_Collection.Add(new CustomButtonTab()
                            {
                                //TODO ADD ROW TO TAB
                                Name = tabName,
                                SubTabName = tab.Name,
                                row = row,
                                customButtons_Collection = new System.Collections.ObjectModel.ObservableCollection<CustomButton>(),
                                customButtonsSubTab_Collection = new System.Collections.ObjectModel.ObservableCollection<CustomButtonTab>()
                            });
                        }
                    }
                    //else
                    //{

                    //    tabToSearch.Add(new CustomButtonTab()
                    //    {
                    //        Name = tabName,
                    //        SubTabName = "",
                    //        row = row,
                    //        customButtons_Collection = new System.Collections.ObjectModel.ObservableCollection<CustomButton>(),
                    //        customButtonsSubTab_Collection = new System.Collections.ObjectModel.ObservableCollection<CustomButtonTab>()
                    //    });
                    //}
                }
                else
                {
                    tabToSearch.Add(new CustomButtonTab()
                    {
                        Name = tabName,
                        SubTabName = "",
                        row = row,
                        customButtons_Collection = new System.Collections.ObjectModel.ObservableCollection<CustomButton>(),
                        customButtonsSubTab_Collection = new System.Collections.ObjectModel.ObservableCollection<CustomButtonTab>()
                    });

                }
            }

            else if (!string.IsNullOrEmpty(subTabName))
            {
                var tab = tabToSearch.FirstOrDefault(x => x.Name.Equals(subTabName));
                if (tab != null)
                {
                    CustomButtonTab tabCollection = null;
                    tabCollection = tab.customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals(tabName));
                    if (tabCollection == null)
                    {
                        tab.customButtonsSubTab_Collection.Add(new CustomButtonTab()
                        {
                            Name = tabName,
                            SubTabName = tab.Name,
                            row = row,
                            customButtons_Collection = new System.Collections.ObjectModel.ObservableCollection<CustomButton>(),
                            customButtonsSubTab_Collection = new System.Collections.ObjectModel.ObservableCollection<CustomButtonTab>()
                        });
                        tab.customButtonsSubTab_Collection.Last().customButtons_Collection.Add(new CustomButton()
                        {
                            TabName = tabName,
                            FormulaName = _FormulaName,
                            VariableExec_Script = new Model.Script.Scripting()
                            {
                                ScriptName = "ExecVariable",
                                Script = _VariableExec_Script
                            },
                            FormulaExec_Script = new Model.Script.Scripting() { Script = _FormulaExec_Script },
                            Description = _Description,
                            Use = _Use,
                            Variable_Collection = new ObservableCollection<Variable>(),
                            Rules_Collection = new ObservableCollection<Rule>()
                        });
                    }
                    else
                    {


                        tabCollection.customButtons_Collection.Add(new CustomButton()
                        {
                            TabName = tabName,
                            FormulaName = _FormulaName,
                            VariableExec_Script = new Model.Script.Scripting()
                            {
                                ScriptName = "ExecVariable",
                                Script = _VariableExec_Script
                            },
                            FormulaExec_Script = new Model.Script.Scripting() { Script = _FormulaExec_Script },
                            Description = _Description,
                            Use = _Use,
                            Variable_Collection = new ObservableCollection<Variable>(),
                            Rules_Collection = new ObservableCollection<Rule>()
                        });

                    }
                }
            }
            else
            {
                var tabCollection = tabToSearch.FirstOrDefault(x => x.Name.Equals(tabName));
                if (tabCollection == null)
                {
                    tabToSearch.Add(new CustomButtonTab()
                    {
                        Name = tabName,
                        SubTabName = "",
                        row = row,
                        customButtons_Collection = new System.Collections.ObjectModel.ObservableCollection<CustomButton>(),
                        customButtonsSubTab_Collection = new System.Collections.ObjectModel.ObservableCollection<CustomButtonTab>()
                    });
                    //var tab0 = MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals(tabName));
                    if (tabToSearch.Last() != null)
                    {
                        tabToSearch.Last().customButtons_Collection.Add(new CustomButton()
                        {
                            TabName = tabName,
                            FormulaName = _FormulaName,
                            VariableExec_Script = new Model.Script.Scripting()
                            {
                                ScriptName = "ExecVariable",
                                Script = _VariableExec_Script
                            },
                            FormulaExec_Script = new Model.Script.Scripting() { Script = _FormulaExec_Script },
                            Description = _Description,
                            Use = _Use,
                            Variable_Collection = new ObservableCollection<Variable>(),
                            Rules_Collection = new ObservableCollection<Rule>()

                        });
                    }
                }
                else
                {

                    //tabToSearch.Add(new CustomButtonTab()
                    //{
                    //    Name = tabName,
                    //    SubTabName = "",
                    //    customButtons_Collection = new System.Collections.ObjectModel.ObservableCollection<CustomButton>(),
                    //    customButtonsSubTab_Collection = new System.Collections.ObjectModel.ObservableCollection<CustomButtonTab>()
                    //});
                    //var tab0 = MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals(tabName));
                    //if (tabToSearch.Last() != null)
                    //{
                    tabCollection.customButtons_Collection.Add(new CustomButton()
                    {
                        TabName = tabName,
                        FormulaName = _FormulaName,
                        VariableExec_Script = new Model.Script.Scripting()
                        {
                            ScriptName = "ExecVariable",
                            Script = _VariableExec_Script
                        },
                        FormulaExec_Script = new Model.Script.Scripting() { Script = _FormulaExec_Script },
                        Description = _Description,
                        Use = _Use,
                        Variable_Collection = new ObservableCollection<Variable>(),
                        Rules_Collection = new ObservableCollection<Rule>()

                    });
                    //}
                }
            }
        }



        //private static void loadBtnRule(
        //    SqliteDataReader customButtonDataReader, ObservableCollection<CustomButtonTab> tabToSearch)
        //{

        //}

        //private static void loadBtnVariable(
        //    SqliteDataReader customButtonDataReader, ObservableCollection<CustomButtonTab> tabToSearch)
        //{

        //}

        public static async Task loadCustomButtonDB(
            string CustomButton_List_TableName,
            string CustomButton_VariableList_TableName,
            string CustomButton_RuleList_TableName, DispatcherQueue dispatcherQueue, string filePath)
        {
            //DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            SqliteCommand command0CustomButton = null;
            SqliteCommand command1CustomButton = null;
            SqliteCommand command2CustomButton = null;
            SqliteCommand commandVariable = null;
            SqliteCommand commandRule = null;
            //Task t = Task.Run(() => { 
            //dispatcherQueue.TryEnqueue(async () =>
            //{
            SqliteDataReader customButton0_DataReader = null;
            SqliteDataReader customButton1_DataReader = null;
            SqliteDataReader customButton2_DataReader = null;
            SqliteDataReader customButton_Rules_DataReader = null;
            SqliteDataReader customButton_Variables_DataReader = null;



            dispatcherQueue.TryEnqueue(async () =>
            {
                DatabaseDB.createDBConnectionString(filePath);
                using (SqliteConnection DBConnection = new SqliteConnection(DatabaseDB.DBConnectionStringBuilder.ConnectionString))
                {
                    DatabaseDB.createDBConnection(filePath);
                    await DBConnection.OpenAsync();
                    command0CustomButton = DBConnection.CreateCommand();
                    command1CustomButton = DBConnection.CreateCommand();
                    command2CustomButton = DBConnection.CreateCommand();

                    CustomButtonTab btnTab = null;
                    //DatabaseDB.createDBConnectionString(filePath);
                    //using (SqliteConnection connection = new SqliteConnection(DBConnectionStringBuilder.ConnectionString))
                    //{
                    try
                    {
                        command0CustomButton.CommandText = $"SELECT * FROM {CustomButton_List_TableName} WHERE tabRow0Index != -1 AND tabRow1Index == -1 AND tabRow2Index == -1 ORDER BY tabRow0Index ASC, tabRow_ListIndex ASC;";

                        customButton0_DataReader = await command0CustomButton.ExecuteReaderAsync();
                    }
                    catch { }
                    if (customButton0_DataReader != null)
                    {
                        while (customButton0_DataReader.Read())
                        {



                            initialLoadBtnFormula(customButton0_DataReader, MainProject.CustomButtonTab_Collection);


                        }
                        //if (customButton0_DataReader != null)
                        await customButton0_DataReader.CloseAsync();

                    }

                    //await command0CustomButton.Connection.CloseAsync();





                    try
                    {
                        command1CustomButton.CommandText = $"SELECT * FROM {CustomButton_List_TableName} WHERE tabRow0Index != -1 AND tabRow1Index != -1 AND tabRow2Index == -1 ORDER BY tabRow0Index ASC, tabRow_ListIndex ASC;";

                        customButton1_DataReader = await command1CustomButton.ExecuteReaderAsync();
                    }
                    catch { }
                    if (customButton1_DataReader != null)
                    {
                        while (customButton1_DataReader.Read())
                        {

                            //string subTabName = customButton1_DataReader.GetString(0);
                            //string tabName = customButton1_DataReader.GetString(1);
                            //var tab = MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals(subTabName));
                            //btnTab = tab.customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals(tabName));
                            initialLoadBtnFormula(customButton1_DataReader, MainProject.CustomButtonTab_Collection);



                        }
                        await customButton1_DataReader.CloseAsync();
                        //if (customButton1_DataReader != null)
                        //await command1CustomButton.Connection.CloseAsync();

                    }






























                    try
                    {
                        command2CustomButton.CommandText = $"SELECT * FROM {CustomButton_List_TableName} WHERE tabRow0Index != -1 AND tabRow1Index != -1 AND tabRow2Index != -1 ORDER BY tabRow0Index ASC, tabRow_ListIndex ASC;";

                        customButton2_DataReader = await command2CustomButton.ExecuteReaderAsync();
                    }
                    catch { }
                    if (customButton2_DataReader != null)
                    {
                        while (customButton2_DataReader.Read())
                        {




                            //string subTabName = customButton2_DataReader.GetString(0);
                            //string tabName = customButton2_DataReader.GetString(1);
                            //string row0 = customButton2_DataReader.GetString(2);
                            //string row1 = customButton2_DataReader.GetString(3);
                            //string row2 = customButton2_DataReader.GetString(4);
                            //string _FormulaName = customButton2_DataReader.GetString(6);
                            //string _VariableExec_Script = customButton2_DataReader.GetString(7);
                            //string _FormulaExec_Script = customButton2_DataReader.GetString(8);
                            //string _Description = customButton2_DataReader.GetString(9);
                            //string _Use = customButton2_DataReader.GetString(10);
                            //string listIndex = customButton2_DataReader.GetString(3);
                            //MainProject.CustomButtonTab_Collection.

                            //if (!row0.Equals("-1") && !row1.Equals("-1") && !row2.Equals("-1"))
                            //{

                            bool skipedDefault = false;
                            foreach (var baseCol in MainProject.CustomButtonTab_Collection)
                            {
                                if (!skipedDefault)
                                {
                                    skipedDefault = true;
                                    continue;
                                }
                                //foreach (var col1 in baseCol.customButtonsSubTab_Collection)
                                //{

                                    initialLoadBtnFormula(customButton2_DataReader, baseCol.customButtonsSubTab_Collection);

                                //}
                            }
                            //}


                        }

                        //if (customButton2_DataReader != null)
                        //await customButton2_DataReader.CloseAsync();
                        await command2CustomButton.Connection.CloseAsync();
                    }
                }
            });


































            bool skipedDefault = false;
            //bool doOnce = false;
            //bool doOnce1 = false;
            //bool doOnce2 = false;
            dispatcherQueue.TryEnqueue(async () =>
            {
                DatabaseDB.createDBConnectionString(filePath);
                using (SqliteConnection DBConnection = new SqliteConnection(DatabaseDB.DBConnectionStringBuilder.ConnectionString))
                {
                    commandVariable = DBConnection.CreateCommand();
                    commandRule = DBConnection.CreateCommand();
                    customButton_Rules_DataReader = null;
                    customButton_Variables_DataReader = null;
                    await DBConnection.OpenAsync();
                    foreach (var tab0 in MainProject.CustomButtonTab_Collection)
                    {
                        //if (!doOnce)
                        //{
                        //    tab0.customButtons_Collection.Clear();
                        //    tab0.customButtonsSubTab_Collection.Clear();
                        //    doOnce = true;
                        //}
                        if (!skipedDefault)
                        {
                            skipedDefault = true;
                            continue;
                        }
                        int col = 0;
                        //foreach (var col in tab0.customButtons_Collection)
                        while (col < tab0.customButtons_Collection.Count)
                        {
                            if (tab0.customButtons_Collection[col].TabName.Equals(tab0.Name))
                            {
                                //tab0.customButtons_Collection.Add(
                                //    new CustomButton()
                                //    {
                                //        TabName = tab0.customButtons_Collection[col].TabName,
                                //        FormulaName = tab0.customButtons_Collection[col].FormulaName,
                                //        VariableExec_Script = new Model.Script.Scripting()
                                //        {
                                //            ScriptName = "ExecVariable",
                                //            Script = tab0.customButtons_Collection[col].VariableExec_Script.Script
                                //        },
                                //        FormulaExec_Script = new Model.Script.Scripting() { Script = tab0.customButtons_Collection[col].FormulaExec_Script.Script },
                                //        Description = tab0.customButtons_Collection[col].Description,
                                //        Use = tab0.customButtons_Collection[col].Use,
                                //    }
                                //);

                                try
                                {
                                    commandRule.CommandText = $"SELECT * FROM {CustomButton_RuleList_TableName} WHERE buttonName = \'{tab0.customButtons_Collection[col].FormulaName}\' ORDER BY rule_index ASC;";
                                    customButton_Rules_DataReader = await commandRule.ExecuteReaderAsync();
                                }
                                catch { }
                                if (customButton_Rules_DataReader != null)
                                {
                                    while (customButton_Rules_DataReader.Read())
                                    {
                                        tab0.customButtons_Collection[col].Rules_Collection.Add(new Model.Formula.Rule()
                                        {
                                            ruleIndex = customButton_Rules_DataReader.GetString(1),
                                            codeRule = customButton_Rules_DataReader.GetString(2)
                                        });
                                    }
                                    await customButton_Rules_DataReader.CloseAsync();
                                }
                                try
                                {
                                    commandVariable.CommandText = $"SELECT * FROM {CustomButton_VariableList_TableName} WHERE buttonName = \'{tab0.customButtons_Collection[col].FormulaName}\' ORDER BY variable_index ASC;";
                                    customButton_Variables_DataReader = await commandVariable.ExecuteReaderAsync();
                                }
                                catch { }
                                if (customButton_Variables_DataReader != null)
                                {
                                    while (customButton_Variables_DataReader.Read())
                                    {

                                        tab0.customButtons_Collection[col].Variable_Collection.Add(new Model.Formula.Variable()
                                        {
                                            VariableIndex = customButton_Variables_DataReader.GetString(1),
                                            VariableName = customButton_Variables_DataReader.GetString(2),
                                            VariableValue = customButton_Variables_DataReader.GetString(3),
                                            VariableDescription = customButton_Variables_DataReader.GetString(4),
                                        });
                                        //MainProject.formulaProject_Collection.Last().formula_Collection.Last().Variable_Dictionary.Add(
                                        //    customButton_Variables_DataReader.GetString(2), new Model.Formula.Variable()
                                        //    {
                                        //        VariableIndex = customButton_Variables_DataReader.GetString(1),
                                        //        VariableName = customButton_Variables_DataReader.GetString(2),
                                        //        VariableValue = customButton_Variables_DataReader.GetString(3),
                                        //        VariableDescription = customButton_Variables_DataReader.GetString(4),
                                        //    });
                                    }
                                    await customButton_Variables_DataReader.CloseAsync();
                                }
                                if (!CustomButtonTab.customButtons_Dictionary.ContainsKey(tab0.customButtons_Collection[col].FormulaName))
                                    CustomButtonTab.customButtons_Dictionary.Add(
                                        tab0.customButtons_Collection[col].FormulaName,
                                        tab0.customButtons_Collection[col]);
                                //col++;
                            }
                            col++;
                        }

















                        //SqliteCommand col1Command = DBConnection.CreateCommand();
                        //SqliteCommand col2Command = DBConnection.CreateCommand();

                        //SqliteDataReader col1DataReader = null;
                        //SqliteDataReader col2DataReader = null;
                        //customButton_Rules_DataReader = null;
                        //customButton_Variables_DataReader = null;




                        foreach (var tab1 in tab0.customButtonsSubTab_Collection)
                        {
                            //if (!doOnce1)
                            //{
                            //    tab1.customButtons_Collection.Clear();
                            //    tab1.customButtonsSubTab_Collection.Clear();
                            //    doOnce1 = true;
                            //}
                            //int col = 0;
                            col = 0;
                            //foreach (var col in tab1.customButtons_Collection)
                            while (col < tab1.customButtons_Collection.Count)
                            {
                                if (tab1.customButtons_Collection[col].TabName.Equals(tab1.Name))
                                {

                                    try
                                    {
                                        commandRule.CommandText = $"SELECT * FROM {CustomButton_RuleList_TableName} WHERE buttonName = \'{tab1.customButtons_Collection[col].FormulaName}\' ORDER BY rule_index ASC;";
                                        customButton_Rules_DataReader = await commandRule.ExecuteReaderAsync();
                                    }
                                    catch { }
                                    if (customButton_Rules_DataReader != null)
                                    {
                                        while (customButton_Rules_DataReader.Read())
                                        {
                                            tab1.customButtons_Collection[col].Rules_Collection.Add(new Model.Formula.Rule()
                                            {
                                                ruleIndex = customButton_Rules_DataReader.GetString(1),
                                                codeRule = customButton_Rules_DataReader.GetString(2)
                                            });
                                        }
                                        await customButton_Rules_DataReader.CloseAsync();
                                    }
                                    try
                                    {
                                        commandVariable.CommandText = $"SELECT * FROM {CustomButton_VariableList_TableName} WHERE buttonName = \'{tab1.customButtons_Collection[col].FormulaName}\' ORDER BY variable_index ASC;";
                                        customButton_Variables_DataReader = await commandVariable.ExecuteReaderAsync();
                                    }
                                    catch { }
                                    if (customButton_Variables_DataReader != null)
                                    {
                                        while (customButton_Variables_DataReader.Read())
                                        {

                                            tab1.customButtons_Collection[col].Variable_Collection.Add(new Model.Formula.Variable()
                                            {
                                                VariableIndex = customButton_Variables_DataReader.GetString(1),
                                                VariableName = customButton_Variables_DataReader.GetString(2),
                                                VariableValue = customButton_Variables_DataReader.GetString(3),
                                                VariableDescription = customButton_Variables_DataReader.GetString(4),
                                            });
                                            //MainProject.formulaProject_Collection.Last().formula_Collection.Last().Variable_Dictionary.Add(
                                            //    customButton_Variables_DataReader.GetString(2), new Model.Formula.Variable()
                                            //    {
                                            //        VariableIndex = customButton_Variables_DataReader.GetString(1),
                                            //        VariableName = customButton_Variables_DataReader.GetString(2),
                                            //        VariableValue = customButton_Variables_DataReader.GetString(3),
                                            //        VariableDescription = customButton_Variables_DataReader.GetString(4),
                                            //    });
                                        }
                                        await customButton_Variables_DataReader.CloseAsync();
                                    }

                                    if (!CustomButtonTab.customButtons_Dictionary.ContainsKey(tab1.customButtons_Collection[col].FormulaName))
                                        CustomButtonTab.customButtons_Dictionary.Add(
                                        tab1.customButtons_Collection[col].FormulaName,
                                        tab1.customButtons_Collection[col]);
                                    //col++;
                                }
                                col++;
                            }





















                            foreach (var tab2 in tab1.customButtonsSubTab_Collection)
                            {
                                //if (!doOnce2)
                                //{
                                //    tab2.customButtons_Collection.Clear();
                                //    tab2.customButtonsSubTab_Collection.Clear();
                                //    doOnce2 = true;
                                //}
                                col = 0;
                                //foreach (var col in tab2.customButtons_Collection)
                                while (col < tab2.customButtons_Collection.Count)
                                {
                                    if (tab2.customButtons_Collection[col].TabName.Equals(tab2.Name))
                                    {


                                        try
                                        {
                                            commandRule.CommandText = $"SELECT * FROM {CustomButton_RuleList_TableName} WHERE buttonName = \'{tab2.customButtons_Collection[col].FormulaName}\' ORDER BY rule_index ASC;";
                                            customButton_Rules_DataReader = await commandRule.ExecuteReaderAsync();
                                        }
                                        catch { }
                                        if (customButton_Rules_DataReader != null)
                                        {
                                            while (customButton_Rules_DataReader.Read())
                                            {
                                                tab2.customButtons_Collection[col].Rules_Collection.Add(new Model.Formula.Rule()
                                                {
                                                    ruleIndex = customButton_Rules_DataReader.GetString(1),
                                                    codeRule = customButton_Rules_DataReader.GetString(2)
                                                });
                                            }
                                            await customButton_Rules_DataReader.CloseAsync();
                                        }
                                        try
                                        {
                                            commandVariable.CommandText = $"SELECT * FROM {CustomButton_VariableList_TableName} WHERE buttonName = \'{tab2.customButtons_Collection[col].FormulaName}\' ORDER BY variable_index ASC;";
                                            customButton_Variables_DataReader = await commandVariable.ExecuteReaderAsync();
                                        }
                                        catch { }
                                        if (customButton_Variables_DataReader != null)
                                        {
                                            while (customButton_Variables_DataReader.Read())
                                            {

                                                tab2.customButtons_Collection[col].Variable_Collection.Add(new Model.Formula.Variable()
                                                {
                                                    VariableIndex = customButton_Variables_DataReader.GetString(1),
                                                    VariableName = customButton_Variables_DataReader.GetString(2),
                                                    VariableValue = customButton_Variables_DataReader.GetString(3),
                                                    VariableDescription = customButton_Variables_DataReader.GetString(4),
                                                });
                                                //MainProject.formulaProject_Collection.Last().formula_Collection.Last().Variable_Dictionary.Add(
                                                //    customButton_Variables_DataReader.GetString(2), new Model.Formula.Variable()
                                                //    {
                                                //        VariableIndex = customButton_Variables_DataReader.GetString(1),
                                                //        VariableName = customButton_Variables_DataReader.GetString(2),
                                                //        VariableValue = customButton_Variables_DataReader.GetString(3),
                                                //        VariableDescription = customButton_Variables_DataReader.GetString(4),
                                                //    });
                                            }
                                            await customButton_Variables_DataReader.CloseAsync();
                                        }

                                        if (!CustomButtonTab.customButtons_Dictionary.ContainsKey(tab2.customButtons_Collection[col].FormulaName))
                                            CustomButtonTab.customButtons_Dictionary.Add(
                                            tab2.customButtons_Collection[col].FormulaName,
                                            tab2.customButtons_Collection[col]);
                                        //col++;
                                    }
                                    col++;
                                }
                            }


                        }

                    }
                }
            });

            //if (customButton2_DataReader != null)
            //});
            //});
            //t.Wait();
        }
        #endregion Load Data Tables
    }
}
