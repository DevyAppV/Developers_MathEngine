using Developers_MathEngine_MVVM.Engine.CoreCLR;
using Developers_MathEngine_MVVM.Model.Formula;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Developers_MathEngine_MVVM.Engine.Math
{
    public class Rules
    {
        //public 
        public static async void evaluateAll(ObservableCollection<Rule> rules_Collection, ScriptState<object> scriptState, CoreCLR_API.ScriptGlobals scriptGlobals)
        {
            if (scriptGlobals.CustomButton != null)
            {
                foreach (Rule rule in scriptGlobals.CustomButton.Rules_Collection)
                {

                    string code = CoreCLR_Script.createDefaultScript();
                    string VarExecScript = scriptGlobals.CustomButton.VariableExec_Script.Script;
                    code += rule.codeRule;
                    ScriptState<object> currentScriptState = await CoreCLR.CoreCLR.EvaluateScriptAsync(VarExecScript, new List<string>(), scriptGlobals);
                    scriptState = await CoreCLR.CoreCLR.EvaluateScriptAsync(code, new List<string>(), scriptGlobals, currentScriptState);

                }
            }
            else
            {
                foreach (Rule rule in rules_Collection)
                {

                    string code = CoreCLR_Script.createDefaultScript();
                    code += rule.codeRule;

                    scriptState = await CoreCLR.CoreCLR.EvaluateScriptAsync(code, new List<string>(), scriptGlobals, scriptState);

                }
            }
        }
        public static async Task<object> evaluateAt(int index, ObservableCollection<Rule> rules_Collection, ScriptState<object> scriptState, CoreCLR_API.ScriptGlobals scriptGlobals)
        {
            //foreach (string rule in rules_Collection) {
            string code = "";
            if (scriptGlobals.CustomButton != null)
            {
                code = CoreCLR_Script.createDefaultScript();
                code += scriptGlobals.CustomButton.Rules_Collection[index].codeRule;
                string VarExecScript = scriptGlobals.CustomButton.VariableExec_Script.Script;


                ScriptState<object> currentScriptState = await CoreCLR.CoreCLR.EvaluateScriptAsync(VarExecScript, new List<string>(), scriptGlobals);
                currentScriptState = await CoreCLR.CoreCLR.EvaluateScriptAsync(code, new List<string>(), scriptGlobals, currentScriptState);

                return (object)currentScriptState.ReturnValue;
            }
            else if (rules_Collection.Count > index)
            {
                code = CoreCLR_Script.createDefaultScript();
                code += rules_Collection[index].codeRule;
                //string VarExecScript = scriptGlobals.Formula.FormulaExec_Script.Script;

                //scriptState = await CoreCLR.CoreCLR.EvaluateScriptAsync(VarExecScript, new List<string>(), scriptGlobals, scriptState);
                scriptState = await CoreCLR.CoreCLR.EvaluateScriptAsync(code, new List<string>(), scriptGlobals, scriptState);

                return (object)scriptState.ReturnValue;
            }

            return null;
        }
    }
}
