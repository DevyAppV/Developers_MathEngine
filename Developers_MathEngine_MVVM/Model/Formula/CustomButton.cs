using CommunityToolkit.Mvvm.ComponentModel;
using Developers_MathEngine_MVVM.Model.Script;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using static Developers_MathEngine_MVVM.Engine.CoreCLR.CoreCLR_API;

namespace Developers_MathEngine_MVVM.Model.Formula
{
    public partial class CustomButton : Formula
    {
        public CustomButton() { }
        public CustomButton(
            ScriptState<object> ScriptState,
            ScriptGlobals ScriptGlobals,
            string FormulaName,
            Scripting VariableExec_Script,
            Scripting FormulaExec_Script,
            string Description,
            string Solution,
            string Use,
            string TabName,
            ObservableCollection<Scripting> LocalScript_Collection,
            ObservableCollection<Rule> Rules_Collection,
            //ObservableCollection<RuntimeVariable> RuntimeVariable_Collection,
            ObservableCollection<Variable> Variable_Collection) : base(ScriptState, ScriptGlobals, FormulaName, VariableExec_Script, FormulaExec_Script, Description, Solution, LocalScript_Collection, Rules_Collection, Variable_Collection)
        {
            this.ScriptState = ScriptState;
            this.FormulaName = FormulaName;
            this.VariableExec_Script = VariableExec_Script;
            this.FormulaExec_Script = FormulaExec_Script;
            this.Description = Description;
            this.Solution = Solution;
            this.LocalScript_Collection = LocalScript_Collection;
            this.Rules_Collection = Rules_Collection;
            //this.RuntimeVariable_Collection = RuntimeVariable_Collection;
            this.Variable_Collection = Variable_Collection;

            this.Use = Use;
            this.TabName = TabName;
        }
        //public CustomButton(
        //    Formula formula,
        //    string Use)
        //{
        //    this.ScriptState = formula.ScriptState;
        //    this.FormulaName = formula.FormulaName;
        //    this.VariableExec_Script = formula.VariableExec_Script;
        //    this.FormulaExec_Script = formula.FormulaExec_Script;
        //    this.Description = formula.Description;
        //    this.Solution = formula.Solution;
        //    this.LocalScript_Collection = formula.LocalScript_Collection;
        //    this.Rules_Collection = formula.Rules_Collection;
        //    //this.RuntimeVariable_Collection = RuntimeVariable_Collection;
        //    this.Variable_Collection = formula.Variable_Collection;

        //    this.Use = Use;
        //}
        public CustomButton(
            Formula formula,
            string Use,
            string TabName)
        {
            this.ScriptState = formula.ScriptState;
            this.FormulaName = formula.FormulaName;
            this.VariableExec_Script = formula.VariableExec_Script;
            this.FormulaExec_Script = formula.FormulaExec_Script;
            this.Description = formula.Description;
            this.Solution = formula.Solution;
            this.LocalScript_Collection = formula.LocalScript_Collection;
            this.Rules_Collection = formula.Rules_Collection;
            //this.RuntimeVariable_Collection = RuntimeVariable_Collection;
            this.Variable_Collection = formula.Variable_Collection;

            this.Use = Use;
            this.TabName = TabName;
        }


        [ObservableProperty]
        public partial string Use { get; set; } = "";
        [ObservableProperty]
        public partial string TabName { get; set; } = "";

        //ObservableCollection<CustomButton> CustomButton_Collection = new ObservableCollection<CustomButton>();




    }
}
