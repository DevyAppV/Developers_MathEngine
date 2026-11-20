using CommunityToolkit.Mvvm.ComponentModel;
using Developers_MathEngine_MVVM.Model.Script;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using static Developers_MathEngine_MVVM.Engine.CoreCLR.CoreCLR_API;

namespace Developers_MathEngine_MVVM.Model.Formula
{
    public partial class Formula : ObservableObject
    {

        public Formula(
            ScriptState<object> ScriptState = null,
            ScriptGlobals ScriptGlobals = null,
            string FormulaName = "",
            Scripting VariableExec_Script = null,
            Scripting FormulaExec_Script = null,
            string Description = "",
            string Solution = "",
            ObservableCollection<Scripting> LocalScript_Collection = null,
            ObservableCollection<Rule> Rules_Collection = null,
            //ObservableCollection<RuntimeVariable> RuntimeVariable_Collection = null,
            ObservableCollection<Variable> Variable_Collection = null)
        {
            this.ScriptState = ScriptState;
            this.ScriptGlobals = ScriptGlobals;
            this.FormulaName = FormulaName;
            this.VariableExec_Script = VariableExec_Script;
            this.FormulaExec_Script = FormulaExec_Script;
            this.Description = Description;
            this.Solution = Solution;
            this.LocalScript_Collection = LocalScript_Collection;
            this.Rules_Collection = Rules_Collection;
            //RuntimeVariable_Collection = this.RuntimeVariable_Collection;
            this.Variable_Collection = Variable_Collection;

        }

        //public Formula(string formulaName, string formulaEx, string description, string solution, ObservableCollection<Scripting> LocalScript_Collection, ObservableCollection<Rule> Rules_Collection, ObservableCollection<Variable> Variable_Collection)
        //{
        //    FormulaName = formulaName;
        //    FormulaExec_Script = FormulaExec_Script;
        //    Description = description;
        //    Solution = solution;
        //    Rules_Collection = this.Rules_Collection;
        //    Variable_Collection = this.Variable_Collection;
        //}
        //public int ruleToDelete_Index = 0;
        [ObservableProperty]
        public partial DispatcherQueue dispatcherQueue { get; set; }
        public bool isQuickSolve = false;

        public bool beginSolve { get; set; } = true;
        public string beginSolve_CurrentFormula { get; set; } = "";
        public HashSet<string> beginSolve_VarHashSet { get; set; } = new HashSet<string>();

        public bool isSolvingNestedFunc { get; set; } = false;


        public bool setUseOnce { get; set; } = false;

        public ScriptState<object> ScriptState { get; set; }

        public ScriptGlobals ScriptGlobals { get; set; }

        [ObservableProperty]
        public partial string FormulaIndex
        {
            get;
            set;
        }

        [ObservableProperty]
        public partial string FormulaName
        {
            get;
            set;
        }

        [ObservableProperty]
        public partial Scripting VariableExec_Script
        {
            get;
            set;
        }

        [ObservableProperty]
        public partial Scripting FormulaExec_Script
        {
            get;
            set;
        }

        [ObservableProperty]
        public partial string Description
        {
            get;
            set;
        }

        [ObservableProperty]
        public partial string Solution
        {
            get;
            set;
        }


        [ObservableProperty]
        public partial ObservableCollection<Scripting> LocalScript_Collection { get; set; } = new ObservableCollection<Scripting>();

        [ObservableProperty]
        public partial ObservableCollection<Rule> Rules_Collection { get; set; } = new ObservableCollection<Rule>();
        //public ObservableCollection<RuntimeVariable> RuntimeVariable_Collection = new ObservableCollection<RuntimeVariable>();

        [ObservableProperty]
        public partial ObservableCollection<Variable> Variable_Collection { get; set; } = new ObservableCollection<Variable>();


        //dictionary, varname, var
        public Dictionary<string, Variable> Variable_Dictionary = new Dictionary<string, Variable>();
        //public Dictionary<int,HashSet<string>> RuntimeVariable_Dictionary = new Dictionary<int, HashSet<string>>();

        //public void setRuntimeVarDicitonary_ToDictionary()
        //{
        //    foreach(var runtimeVar in RuntimeVariable_Collection)
        //    {
        //        RuntimeVariable_Dictionary.Add(int.Parse(runtimeVar.ColumnIndex), runtimeVar.variable_Collection.ToHashSet());
        //    }
        //}

        //public void setRuntimeVarDicitonary_ToOtherDictionary(ref Dictionary<int, HashSet<string>> RuntimeVariable_Dictionary_Other)
        //{
        //    foreach(var runtimeVar in RuntimeVariable_Collection)
        //    {
        //        RuntimeVariable_Dictionary_Other.Add(int.Parse(runtimeVar.ColumnIndex), runtimeVar.variable_Collection.ToHashSet());
        //    }
        //}
        public void addVariable_Collect_Diction(string varIndex, string varName, string varValue, string varDescription)
        {
            if (!Variable_Dictionary.ContainsKey(varName))
            {
                bool added = false;
                var collection = Variable_Collection.FirstOrDefault(x => x.VariableName.Equals(varName));
                if (string.IsNullOrEmpty(varDescription) &&
                    collection == null)
                {
                    added = true;
                    Variable_Collection.Add(new Variable() { VariableIndex= varIndex, VariableName = varName, VariableValue = varValue });
                }
                else if (collection == null)
                {
                    added = true;
                    Variable_Collection.Add(new Variable() { VariableIndex = varIndex, VariableName = varName, VariableValue = varValue, VariableDescription = varDescription });
                }
                if (added)
                    Variable_Dictionary.Add(varName, Variable_Collection.Last());
                else if(!added && collection != null)
                {
                    Variable_Dictionary.Add(varName, collection);
                }
            }
        }
        public void deleteVariable_Collect_Diction(string varName)
        {
            if (Variable_Dictionary.ContainsKey(varName))
            {
                Variable_Collection.Remove(Variable_Collection.FirstOrDefault(x => x.VariableName == varName));
                Variable_Dictionary.Remove(varName);
            }
        }


    }
}
