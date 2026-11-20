using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Developers_MathEngine_MVVM.Dependency_Injection;
using Developers_MathEngine_MVVM.Engine.CoreCLR;
using Developers_MathEngine_MVVM.Model.Formula;
using Developers_MathEngine_MVVM.Model.Project;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Developers_MathEngine_MVVM.Engine.CoreCLR.CoreCLR_API;
//using Windows.System;

namespace Developers_MathEngine_MVVM.ViewModel.Formula
{
    public partial class Formula_ViewModel : ObservableObject
    {

        public TextBox selectedTextBox { get; set; }
        public Model.Formula.Formula selectedFormula { get; set; }
        public Model.Formula.Variable selectedVariable { get; set; }
        public Model.Formula.Rule selectedRule { get; set; }
        public int selectedTextBox_SelectionStart = 0;
        public int selectedTextBox_SelectionCount = 0;

        [ObservableProperty]
        public partial string formulaProjectName { get; set; }

        [ObservableProperty]
        public partial ObservableCollection<Developers_MathEngine_MVVM.Model.Formula.Formula> formulaListDisplay_Collection { get; set; } = new ObservableCollection<Model.Formula.Formula>();

        //Formula Project
        [RelayCommand]
        private void CreateFormulaProject(string projectName)
        {
            if (!string.IsNullOrEmpty(projectName))
            {
                MainProject.formulaProject_Collection.Add(new FormulaProject()
                {
                    ProjectIndex = MainProject.formulaProject_Collection.Count.ToString(),
                    ProjectName = projectName,
                    formula_Collection = new System.Collections.ObjectModel.ObservableCollection<Model.Formula.Formula>(),
                });
            }
        }

        [RelayCommand]
        private void selectFormulaProject(FormulaProject formulaProject)
        {
            formulaProjectName = formulaProject.ProjectName;
            formulaListDisplay_Collection = formulaProject.formula_Collection;
        }

        [RelayCommand]
        private async Task deleteFormulaProject(FormulaProject formulaProject)
        {
            int i = int.Parse(formulaProject.ProjectIndex);
            MainProject.formulaProject_Collection.RemoveAt(i);
            DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            dispatcherQueue.TryEnqueue(async () =>
            {
                Parallel.For(i, MainProject.formulaProject_Collection.Count, (x) =>
                {
                    dispatcherQueue.TryEnqueue(async () =>
                    {
                        MainProject.formulaProject_Collection.ElementAt(x).ProjectIndex = (x).ToString();
                    });
                });

            });
        }












        //formula list
        [RelayCommand]
        private void expandFormulaSection(StackPanel stackPanel)
        {
            stackPanel.Visibility = (stackPanel.Visibility == Microsoft.UI.Xaml.Visibility.Visible) ? Microsoft.UI.Xaml.Visibility.Collapsed : Microsoft.UI.Xaml.Visibility.Visible;
        }

        [RelayCommand]
        private void addFormula()
        {
            if (formulaListDisplay_Collection != null)
            {
                formulaListDisplay_Collection.Add(
                    new Model.Formula.Formula()
                    {
                        VariableExec_Script = new Model.Script.Scripting() { Script = CoreCLR_Script.createDefaultScript_ForVariable() },
                        FormulaExec_Script = new Model.Script.Scripting() { Script = CoreCLR_Script.createDefaultScript_ForFormula() },
                        FormulaIndex = formulaListDisplay_Collection.Count.ToString(),
                        Rules_Collection = new ObservableCollection<Rule>(),
                        Variable_Collection = new ObservableCollection<Variable>(),

                    });
                formulaListDisplay_Collection.Last().ScriptGlobals = new ScriptGlobals(formulaListDisplay_Collection.Last());
            }
        }

        [RelayCommand]
        private void clearFormulaList()
        {
            if (formulaListDisplay_Collection != null)
            {
                formulaListDisplay_Collection.Clear();
            }
        }

        [RelayCommand]
        private void deleteFormula(Developers_MathEngine_MVVM.Model.Formula.Formula formula)
        {
            int i = int.Parse(formula.FormulaIndex);
            formulaListDisplay_Collection.RemoveAt(i);

            DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            dispatcherQueue.TryEnqueue(async () =>
            {
                Parallel.For(i, formulaListDisplay_Collection.Count, (x) =>
                {
                    dispatcherQueue.TryEnqueue(async () =>
                    {
                        formulaListDisplay_Collection.ElementAt(x).FormulaIndex = (x).ToString();
                    });
                });

            });
        }

        [RelayCommand]
        private void textboxSelected(TextBox tBox)
        {
            selectedTextBox = tBox;
            try
            {
                selectedFormula = (Model.Formula.Formula)tBox.Tag;
            }
            catch
            {
                try
                {
                    selectedVariable = (Model.Formula.Variable)tBox.Tag;
                }
                catch
                {

                    try
                    {
                        selectedRule = (Model.Formula.Rule)tBox.Tag;
                    }
                    catch
                    {

                    }
                }
            }
            selectedTextBox_SelectionStart = tBox.SelectionStart;
            selectedTextBox_SelectionCount = tBox.SelectedText.Length;
        }












        [RelayCommand]

        private async Task solve(Model.Formula.Formula formula)
        {
            try
            {
                formula.isQuickSolve = true;
                await getVariable(formula);
                await quickSolveFormula(formula);
                formula.isQuickSolve = false;
            }
            catch (Exception e)
            {
                formula.Solution = e.Message;
            }
        }
        //formula calc
        [RelayCommand]
        private async Task getVariable(Model.Formula.Formula formula)
        {
            try
            {
                //TODO: Evaluate LocalScripts
                formula.dispatcherQueue = DispatcherQueue.GetForCurrentThread();
                formula.beginSolve = true;
                formula.beginSolve_VarHashSet.Clear();
                formula.ScriptGlobals = new ScriptGlobals(formula);

                formula.ScriptGlobals.CustomButton = null;
                //formula.dispatcherQueue.TryEnqueue(async() =>
                //{
                formula.ScriptState = await CoreCLR.EvaluateScriptAsync(formula.VariableExec_Script.Script, new List<string>(), formula.ScriptGlobals, formula.ScriptState);
                //});
                formula.beginSolve = false;
                //});
                //Formula.dispatcherQueue.TryEnqueue(() =>
                //{
                if (!formula.isQuickSolve)
                {
                    int i = 0;
                    while (i < formula.Variable_Dictionary.Count)
                    {

                        if (formula.beginSolve_VarHashSet.Count <= 0 || !formula.beginSolve_VarHashSet.Contains(formula.Variable_Dictionary.ElementAt(i).Key))
                        {
                            if (i < formula.Variable_Collection.Count &&
                                i < formula.Variable_Dictionary.Count &&
                                formula.Variable_Collection.FirstOrDefault(x => x.VariableName.Equals(formula.Variable_Dictionary.ElementAt(i).Key)) != null)
                            {
                                formula.Variable_Collection.Remove(formula.Variable_Collection.FirstOrDefault(x => x.VariableName.Equals(formula.Variable_Dictionary.ElementAt(i).Key)));

                                formula.Variable_Dictionary.Remove(formula.Variable_Dictionary.ElementAt(i).Key);
                            }
                            else
                            {
                                i++;
                            }
                        }
                        else
                        {
                            i++;
                        }
                    }
                    i = 0;
                    while (i < formula.Variable_Collection.Count)
                    {

                        if (!formula.Variable_Dictionary.ContainsKey(formula.Variable_Collection[i].VariableName))
                        {
                            //var chk = formula.beginSolve_VarHashSet.FirstOrDefault(formula.Variable_Collection[i].VariableName);
                            var col = formula.Variable_Collection.FirstOrDefault(x => x.VariableName.Equals(formula.Variable_Collection[i].VariableName));
                            if (col == null)
                                i++;
                            else
                                formula.Variable_Collection.Remove(col);
                        }
                        else
                        {
                            i++;
                        }

                    }
                }

            }
            catch (Exception e)
            {
                formula.Solution = e.Message;
            }
        }
        [RelayCommand]
        private async Task quickSolveFormula(Model.Formula.Formula formula)
        {

            try
            {
                DispatcherQueue dispatcher = DispatcherQueue.GetForCurrentThread();
                if (formula != null && formula.dispatcherQueue != null) dispatcher = formula.dispatcherQueue;
                await Task.Run(async () =>
                {
                    formula.beginSolve = true;
                    formula.dispatcherQueue = dispatcher;
                    //formula.beginSolve_VarHashSet = new HashSet<string>();
                    //if (formula.ScriptGlobals == null)
                    //{
                    formula.ScriptGlobals = new ScriptGlobals(formula);
                    formula.ScriptGlobals.CustomButton = null;
                    //}


                    foreach (Variable v in formula.Variable_Collection)
                    {
                        if (formula.Variable_Dictionary.ContainsKey(v.VariableName))
                        {
                            formula.Variable_Dictionary[v.VariableName].VariableValue = v.VariableValue;
                        }
                    }








                    formula.ScriptState = await CoreCLR.EvaluateScriptAsync(formula.FormulaExec_Script.Script, new List<string>(), formula.ScriptGlobals, formula.ScriptState);



                });

                //await Task.Run(() =>
                //{

                    int i = 0;
                    //dispatcher.TryEnqueue(() =>
                    //{
                        while (i < formula.Variable_Dictionary.Count)
                        {
                            if (formula.beginSolve_VarHashSet.Count <= 0 || !formula.beginSolve_VarHashSet.Contains(formula.Variable_Dictionary.ElementAt(i).Key))
                            {
                                if (i < formula.Variable_Collection.Count &&
                                    i < formula.Variable_Dictionary.Count &&
                                    formula.Variable_Collection.FirstOrDefault(x => x.VariableName.Equals(formula.Variable_Dictionary.ElementAt(i).Key)) != null)
                                {
                                    formula.Variable_Collection.Remove(formula.Variable_Collection.FirstOrDefault(x => x.VariableName.Equals(formula.Variable_Dictionary.ElementAt(i).Key)));

                                    formula.Variable_Dictionary.Remove(formula.Variable_Dictionary.ElementAt(i).Key);
                                }
                                else
                                {
                                    i++;
                                }
                            }
                            else
                            {
                                i++;
                            }
                        }
                        formula.beginSolve_VarHashSet.Clear();
                    //});
                //});

                for (int j = 0; j < formula.Variable_Collection.Count; j++)
                {
                    formula.Variable_Collection[j].VariableIndex = j.ToString();
                }

                //dispatcher.TryEnqueue(() =>
                //{
                formula.Solution = (string)(formula.ScriptState.ReturnValue);
                //});
                formula.ScriptState = null;
                formula.beginSolve = false;

            }
            catch (Exception e)
            {
                formula.Solution = e.Message;
            }
        }
        [RelayCommand]
        private void addRule(Model.Formula.Formula formula)
        {
            formula.Rules_Collection.Add(new Rule() { ScriptGlobals = formula.ScriptGlobals, codeRule = "", ruleIndex = formula.Rules_Collection.Count.ToString() });
        }
        [RelayCommand]
        private void clearRule(Model.Formula.Formula formula)
        {
            formula.Rules_Collection.Clear();
        }
        //[RelayCommand]
        public Tuple<Model.Formula.Formula, string> formulaRuleDeleteTuple(Model.Formula.Formula formula, string ruleIndex)
        {
            return Tuple.Create(formula, ruleIndex);
        }


        [RelayCommand]
        private void deleteRule(Rule rule)
        {
            rule.ScriptGlobals.Formula.Rules_Collection.RemoveAt(int.Parse(rule.ruleIndex));
            DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            dispatcherQueue.TryEnqueue(async () =>
            {
                int i = 0;
                Parallel.For(i, rule.ScriptGlobals.Formula.Rules_Collection.Count, (x) =>
                {
                    dispatcherQueue.TryEnqueue(async () =>
                    {
                        rule.ScriptGlobals.Formula.Rules_Collection.ElementAt(x).ruleIndex = (x).ToString();
                    });
                });

            });
        }

        [RelayCommand]
        private void createCustomButton(Model.Formula.Formula formula)
        {
            //DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            //MainProject.CustomButtonTab_Collection

            if (Dependency_Injection.DependencyInjectionShared.customButton_ViewModel.selectedCustomButtonTab != null)
            {
                if (!string.IsNullOrEmpty(formula.FormulaName) &&
                    DependencyInjectionShared.customButton_ViewModel.selectedCustomButtonTab != null &&
                    !CustomButtonTab.customButtons_Dictionary.ContainsKey(formula.FormulaName))
                {
                    DependencyInjectionShared.customButton_ViewModel.selectedCustomButtonTab.customButtons_Collection.Add(
                        new CustomButton(formula, "", DependencyInjectionShared.customButton_ViewModel.selectedCustomButtonTab.Name));

                    if (formula.Variable_Collection.Count > 0)
                    {
                        DependencyInjectionShared.customButton_ViewModel.selectedCustomButtonTab.customButtons_Collection.Last().Use +=
                            formula.FormulaName + "(";
                        int addedVarToUse = 0;
                        foreach (Variable v in formula.Variable_Collection)
                        {
                            if (v.VariableName.Equals(v.VariableValue))
                            {
                                DependencyInjectionShared.customButton_ViewModel.selectedCustomButtonTab.customButtons_Collection.Last().Use += v.VariableName + ",";
                                addedVarToUse++;
                            }
                        }

                        if (addedVarToUse > 0)
                        {
                            int lastComma = DependencyInjectionShared.customButton_ViewModel.selectedCustomButtonTab.customButtons_Collection.Last().Use.LastIndexOf(',');

                            if (lastComma > 0)
                            {
                                DependencyInjectionShared.customButton_ViewModel.selectedCustomButtonTab.customButtons_Collection.Last().Use =
                                DependencyInjectionShared.customButton_ViewModel.selectedCustomButtonTab.customButtons_Collection.Last().Use.Remove(
                                    lastComma
                                    ).Insert(lastComma, ")");
                            }
                        }
                        else
                        {
                            DependencyInjectionShared.customButton_ViewModel.selectedCustomButtonTab.customButtons_Collection.Last().Use = formula.FormulaName;
                        }
                    }
                    else
                    {
                        DependencyInjectionShared.customButton_ViewModel.selectedCustomButtonTab.customButtons_Collection.Last().Use = formula.FormulaName;
                    }
                    CustomButtonTab.customButtons_Dictionary.Add(formula.FormulaName,
                        new CustomButton(DependencyInjectionShared.customButton_ViewModel.selectedCustomButtonTab.customButtons_Collection.Last(), DependencyInjectionShared.customButton_ViewModel.selectedCustomButtonTab.customButtons_Collection.Last().Use, DependencyInjectionShared.customButton_ViewModel.selectedCustomButtonTab.Name));
                }
            }
        }


        ////[RelayCommand]
        //public void variableValueTextChanged(Variable variable) {
        //    formula.Variable_Dictionary[variable.VariableName].VariableValue = variable.VariableValue;
        //}

        ////[RelayCommand]
        //public void variableDescriptionTextChanged(Model.Formula.Formula formula, Variable variable)
        //{
        //    formula.Variable_Dictionary[variable.VariableName].VariableDescription = variable.VariableValue;

        //}



    }
}
