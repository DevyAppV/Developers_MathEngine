using Developers_MathEngine_MVVM.Model.Project;
using Microsoft.Data.Sqlite;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Developers_MathEngine_MVVM.Database.Formula
{
    //NOTE:foreign key not used for backward compatibility
    public class FormulaProjectDB
    {
        public static async Task dropFormulaProjectDB(
            string FormulaProject_TableName,
            string FormulaList_TableName,
            string FormulaVariableList_TableName,
            string FormulaRuleList_TableName)
        {
            await dropTables_First(FormulaProject_TableName);
            await dropTables_First(FormulaList_TableName);
            await dropTables_First(FormulaVariableList_TableName);
            await dropTables_First(FormulaRuleList_TableName);
        }
        public static async Task createFormulaProjectDB(
            string FormulaProject_TableName,
            string FormulaList_TableName,
            string FormulaVariableList_TableName,
            string FormulaRuleList_TableName)
        {

            await create_FormulaProject_Table(FormulaProject_TableName);
            await create_FormulaList_Table(FormulaList_TableName);
            await create_FormulaVariableList_Table(FormulaVariableList_TableName, false);
            await create_FormulaRulesList_Table(FormulaRuleList_TableName, false);

        }

        protected private static async Task dropTables_First(string tableName)
        {
            SqliteCommand command = DatabaseDB.DBConnection.CreateCommand();
            command.CommandText = $"DROP TABLE IF EXISTS \"{tableName}\";";
            await command.ExecuteNonQueryAsync();
        }

        #region Create Data Tables
        protected private static async Task create_FormulaProject_Table(string tableName, string indexId_Name = "idx_formulaProject__Project_index")
        {
            SqliteCommand command = DatabaseDB.DBConnection.CreateCommand();
            command.CommandText = $"CREATE TABLE IF NOT EXISTS {tableName}(" +
                $"[index] TEXT," +
                $"project_name TEXT" +
                $");";
            await command.ExecuteNonQueryAsync();
            command.CommandText = $"CREATE INDEX IF NOT EXISTS {indexId_Name} ON {tableName}([index]);";
            await command.ExecuteNonQueryAsync();
        }

        protected private static async Task create_FormulaList_Table(string tableName/*, string pkID_Name = "pk_formulaList"*/, string fkID_Name = "fk_formulaProject_formulaList", string indexId_Name = "idx_formulaList__Project_id")
        {
            SqliteCommand command = DatabaseDB.DBConnection.CreateCommand();
            command.CommandText = $"CREATE TABLE IF NOT EXISTS {tableName}(" +
                $"project_index TEXT," +
                $"[index] TEXT," +
                $"name TEXT," +
                $"variable_script TEXT," +
                $"formula_script TEXT," +
                $"description_script TEXT" +
                $");";


            await command.ExecuteNonQueryAsync();
            command.CommandText = $"CREATE INDEX IF NOT EXISTS {indexId_Name} ON {tableName}(project_index);";
            await command.ExecuteNonQueryAsync();

        }


        protected private static async Task create_FormulaVariableList_Table(string tableName, bool isCustomButtonTable/*, string pkID_Name = "pk_formulaList_VarList"*/, string fkID_Name = "fk_formulaList_VarList", string indexId_Name = "idx_formulaList_VarList__List_id")
        {
            SqliteCommand command = DatabaseDB.DBConnection.CreateCommand();
            if (!isCustomButtonTable)
            {
                command.CommandText = $"CREATE TABLE IF NOT EXISTS {tableName}(" +
                    $"list_index TEXT," +
                    $"variable_index TEXT," +
                    $"variable_name TEXT," +
                    $"variable_value TEXT," +
                    $"variable_description TEXT" +
                    $");";

                await command.ExecuteNonQueryAsync();
                command.CommandText = $"CREATE INDEX IF NOT EXISTS {indexId_Name} ON {tableName}(list_index);";
                await command.ExecuteNonQueryAsync();
            }
            else
            {
                command.CommandText = $"CREATE TABLE IF NOT EXISTS {tableName}(" +
                    $"buttonName TEXT," +
                    $"variable_index TEXT," +
                    $"variable_name TEXT," +
                    $"variable_value TEXT," +
                    $"variable_description TEXT" +
                    $");";

                await command.ExecuteNonQueryAsync();
                command.CommandText = $"CREATE INDEX IF NOT EXISTS {indexId_Name} ON {tableName}(buttonName);";

                await command.ExecuteNonQueryAsync();
            }
            await command.ExecuteNonQueryAsync();
        }

        //setup rules scriptGlobals on load
        protected private static async Task create_FormulaRulesList_Table(string tableName, bool isCustomButtonTable/*, string pkID_Name = "pk_formulaList_RuleList"*/, string fkID_Name = "fk_formulaList_RuleList", string indexId_Name = "idx_List_RuleList__List_id")
        {
            SqliteCommand command = DatabaseDB.DBConnection.CreateCommand();
            if (!isCustomButtonTable)
            {
                command.CommandText = $"CREATE TABLE IF NOT EXISTS {tableName}(" +
                $"list_index TEXT," +
                $"rule_index TEXT," +
                $"code_rule TEXT" +

                $");";

                await command.ExecuteNonQueryAsync();
                command.CommandText = $"CREATE INDEX IF NOT EXISTS {indexId_Name} ON {tableName}(list_index);";
                await command.ExecuteNonQueryAsync();
            }
            else
            {
                command.CommandText = $"CREATE TABLE IF NOT EXISTS {tableName}(" +
                $"buttonName TEXT," +
                $"rule_index TEXT," +
                $"code_rule TEXT" +

                $");";
                await command.ExecuteNonQueryAsync();
                command.CommandText = $"CREATE INDEX IF NOT EXISTS {indexId_Name} ON {tableName}(buttonName);";
                await command.ExecuteNonQueryAsync();
            }
            await command.ExecuteNonQueryAsync();

        }

        #endregion Create Data Tables



        #region Save Data Tables

        public static async Task saveFormulaPoject(
            string FormulaProject_TableName,
            string FormulaList_TableName,
            string FormulaVariableList_TableName,
            string FormulaRuleList_TableName)
        {
            SqliteCommand command = DatabaseDB.DBConnection.CreateCommand();
            foreach (FormulaProject project in MainProject.formulaProject_Collection)
            {
                command.CommandText = $"INSERT INTO {FormulaProject_TableName} VALUES (\'{project.ProjectIndex}\',\'{project.ProjectName}\');";

                await command.ExecuteNonQueryAsync();
                foreach (Model.Formula.Formula formula in project.formula_Collection)
                {

                    command.CommandText = $"INSERT INTO {FormulaList_TableName} VALUES (" +
                        $"\'{project.ProjectIndex}\'," +
                        $"\'{formula.FormulaIndex}\'," +
                        $"\'{formula.FormulaName}\'," +
                        $"\'{formula.VariableExec_Script.Script}\'," +
                        $"\'{formula.FormulaExec_Script.Script}\'," +
                        $"\'{formula.Description}\'" +
                        $");";
                    await command.ExecuteNonQueryAsync();

                    foreach (Model.Formula.Rule rule in formula.Rules_Collection)
                    {
                        command.CommandText = $"INSERT INTO {FormulaRuleList_TableName} VALUES (" +
                        $"\'{formula.FormulaIndex}\'," +
                        $"\'{rule.ruleIndex}\'," +
                        $"\'{rule.codeRule}\'" +
                        $");";
                        await command.ExecuteNonQueryAsync();
                    }
                    foreach (Model.Formula.Variable variable in formula.Variable_Collection)
                    {
                        command.CommandText = $"INSERT INTO {FormulaVariableList_TableName} VALUES (" +
                        $"\'{formula.FormulaIndex}\'," +
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
        #endregion Populate Data Tables



        #region Load Data Tables
        public static async Task loadFormulaDB(
            string FormulaProject_TableName,
            string FormulaList_TableName,
            string FormulaVariableList_TableName,
            string FormulaRuleList_TableName, DispatcherQueue dispatcherQueue, string filePath)
        {
            //dispatcherQueue.TryEnqueue(async() => { 
            //DatabaseDB.createDBConnectionString(filePath);

                //using (SqliteConnection DBConnection = new SqliteConnection())
                //{
                //    await DBConnection.OpenAsync();
                    SqliteCommand commandProject = DatabaseDB.DBConnection.CreateCommand();
                    SqliteCommand commandFormula = DatabaseDB.DBConnection.CreateCommand();
                    SqliteCommand commandRule = DatabaseDB.DBConnection.CreateCommand();
                    SqliteCommand commandVariable = DatabaseDB.DBConnection.CreateCommand();

                    commandProject.CommandText = $"SELECT * FROM {FormulaProject_TableName};";
                    SqliteDataReader formulaProject_DataReader = null;
                    SqliteDataReader formulaList_DataReader = null;
                    SqliteDataReader formulaList_Rules_DataReader = null;
                    SqliteDataReader formulaList_Variables_DataReader = null;
                    try
                    {
                        formulaProject_DataReader = await commandProject.ExecuteReaderAsync();
                    }
                    catch { }
                    if (formulaProject_DataReader != null)
                    {
                        while (formulaProject_DataReader.Read())
                        {
                            MainProject.formulaProject_Collection.Add(new FormulaProject()
                            {
                                ProjectIndex = formulaProject_DataReader.GetString(0),
                                ProjectName = formulaProject_DataReader.GetString(1),
                            });

                        }





                        foreach (var project in MainProject.formulaProject_Collection)
                        {
                            try
                            {
                                commandFormula.CommandText = $"SELECT * FROM {FormulaList_TableName} WHERE project_index = \'{project.ProjectIndex}\' ORDER BY 'index' ASC;";
                                formulaList_DataReader = await commandFormula.ExecuteReaderAsync();

                            }
                            catch { }

                            if (formulaList_DataReader != null)
                            {
                                while (formulaList_DataReader.Read())
                                {

                                    project.formula_Collection.Add(new Model.Formula.Formula()
                                    {
                                        FormulaIndex = formulaList_DataReader.GetString(1),
                                        FormulaName = formulaList_DataReader.GetString(2),
                                        VariableExec_Script = new Model.Script.Scripting() { ScriptName = "ExecVariable", Script = formulaList_DataReader.GetString(3) },
                                        FormulaExec_Script = new Model.Script.Scripting() { Script = formulaList_DataReader.GetString(4) },
                                        Description = formulaList_DataReader.GetString(5),
                                        Variable_Collection = new System.Collections.ObjectModel.ObservableCollection<Model.Formula.Variable>(),
                                        Rules_Collection = new System.Collections.ObjectModel.ObservableCollection<Model.Formula.Rule>()

                                    });
                                }


                                foreach (var collection in project.formula_Collection)
                                {
                                    try
                                    {
                                        commandRule.CommandText = $"SELECT * FROM {FormulaRuleList_TableName} WHERE list_index = \'{collection.FormulaIndex}\' ORDER BY rule_index ASC;";
                                        commandVariable.CommandText = $"SELECT * FROM {FormulaVariableList_TableName} WHERE list_index = \'{collection.FormulaIndex}\' ORDER BY variable_index ASC;";

                                        formulaList_Rules_DataReader = await commandRule.ExecuteReaderAsync();
                                        formulaList_Variables_DataReader = await commandVariable.ExecuteReaderAsync();
                                    }
                                    catch
                                    {

                                    }

                                    if (formulaList_Rules_DataReader != null)
                                    {
                                        while (formulaList_Rules_DataReader.Read())
                                        {
                                            if (collection.Rules_Collection == null)
                                            {
                                                collection.Rules_Collection = new System.Collections.ObjectModel.ObservableCollection<Model.Formula.Rule>();
                                            }
                                            collection.Rules_Collection.Add(new Model.Formula.Rule()
                                            {
                                                ruleIndex = formulaList_Rules_DataReader.GetString(1),
                                                codeRule = formulaList_Rules_DataReader.GetString(2)
                                            });
                                        }
                                        await formulaList_Rules_DataReader.CloseAsync();

                                    }
                                    if (formulaList_Variables_DataReader != null)
                                    {
                                        while (formulaList_Variables_DataReader.Read())
                                        {
                                            if (collection.Variable_Collection == null)
                                            {
                                                collection.Variable_Collection = new System.Collections.ObjectModel.ObservableCollection<Model.Formula.Variable>();
                                            }

                                            collection.Variable_Collection.Add(new Model.Formula.Variable()
                                            {
                                                VariableIndex = formulaList_Variables_DataReader.GetString(1),
                                                VariableName = formulaList_Variables_DataReader.GetString(2),
                                                VariableValue = formulaList_Variables_DataReader.GetString(3),
                                                VariableDescription = formulaList_Variables_DataReader.GetString(4),
                                            });
                                            collection.Variable_Dictionary.Add(
                                                formulaList_Variables_DataReader.GetString(2), new Model.Formula.Variable()
                                                {
                                                    VariableIndex = formulaList_Variables_DataReader.GetString(1),
                                                    VariableName = formulaList_Variables_DataReader.GetString(2),
                                                    VariableValue = formulaList_Variables_DataReader.GetString(3),
                                                    VariableDescription = formulaList_Variables_DataReader.GetString(4),
                                                });
                                        }
                                        await formulaList_Variables_DataReader.CloseAsync();

                                    }
                                }








                                await formulaList_DataReader.CloseAsync();
                            }
                        }


                            await formulaProject_DataReader.CloseAsync();
                    }

                //}
            //});



        }
        #endregion Load Data Tables
    }
}
