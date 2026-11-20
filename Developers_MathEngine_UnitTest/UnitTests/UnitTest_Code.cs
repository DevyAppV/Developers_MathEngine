using Developers_MathEngine_MVVM.Engine.CoreCLR;
using Developers_MathEngine_MVVM.Engine.Math;
using Developers_MathEngine_MVVM.Interpreter.Errors;
using Developers_MathEngine_MVVM.Interpreter.Parser;
using Developers_MathEngine_MVVM.Interpreter.Token;
using Developers_MathEngine_MVVM.Model.Project;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Developers_MathEngine_UnitTest.UnitTests
{
    [TestClass]
    public class UnitTest_Code
    {

        [TestMethod]
        public async Task testAdd() {
            //loops
            //List<Token> result = await Parser_Code.parse(new List<string>()
            //{
            //    "if(1==1){ 123 }"

            //},false);

            UnitTest_Setup.setupFormula();
            MainProject.formulaProject_Collection.Last().formula_Collection.Last().Rules_Collection.Add(new Developers_MathEngine_MVVM.Model.Formula.Rule()
            {
                codeRule =
                "if(hello.ToString() == \"10\")" +
                "throw new Exception(\"hello var == 10\");"
            });
            MainProject.formulaProject_Collection.Last().formula_Collection.Last().Rules_Collection.Add(new Developers_MathEngine_MVVM.Model.Formula.Rule()
            {
                codeRule =
                "if(hello.ToString() == \"10\")" +
                "return true;",
            });




            ErrorHandler.errorMessage = "";

            CoreCLR_API.ScriptGlobals scriptGlobals = new CoreCLR_API.ScriptGlobals(MainProject.formulaProject_Collection.Last().formula_Collection.Last());



            MainProject.formulaProject_Collection.Last().formula_Collection.Last().FormulaExec_Script.Script = "return await solve(\"1+1\");";

            //TEST MATH OPERATION
            MainProject.formulaProject_Collection.Last().formula_Collection.Last().FormulaExec_Script.Script = CoreCLR_Script.createDefaultScript();
            MainProject.formulaProject_Collection.Last().formula_Collection.Last().FormulaExec_Script.Script += "string formula = \"2+2+3\";" +
                "return formula;";
            ScriptState<object> scriptState = await CoreCLR.EvaluateScriptAsync(
                MainProject.formulaProject_Collection.Last().formula_Collection.Last().FormulaExec_Script.Script, new List<string>(), scriptGlobals);
            scriptGlobals.ScriptState = scriptState;
            scriptGlobals.ScriptGlobal = scriptGlobals;
            Assert.AreEqual("2+2+3", scriptState.ReturnValue);


            MainProject.formulaProject_Collection.Last().formula_Collection.Last().FormulaExec_Script.Script = CoreCLR_Script.createDefaultScript();
            MainProject.formulaProject_Collection.Last().formula_Collection.Last().FormulaExec_Script.Script += "string formula = await solve(\"2+2+3\");" +
                "formula";

            scriptState = await CoreCLR.EvaluateScriptAsync(MainProject.formulaProject_Collection.Last().formula_Collection.Last().FormulaExec_Script.Script, new List<string>(), scriptGlobals, scriptState);
            scriptGlobals.ScriptState = scriptState;
            scriptGlobals.ScriptGlobal = scriptGlobals;
            Assert.AreEqual("7", scriptState.ReturnValue);



            //TEST VARIABLE
            MainProject.formulaProject_Collection.Last().formula_Collection.Last().FormulaExec_Script.Script = CoreCLR_Script.createDefaultScript();
            MainProject.formulaProject_Collection.Last().formula_Collection.Last().FormulaExec_Script.Script +=
                "Variable hello = setVariable(nameof(hello), \"10\"); " +
                "Variable world = setVariable(nameof(world), \"123\");" +
                "" +
                "" +
                //"world = \"1\";" +
                //"world = setVariableDefault(nameof(world));" +//needs to get var list first before set
                "";
                //"string formula = await solve(\"2+2+3\") + hello.getValue() + world.getValue();" +
                //"" +
                //"return formula;";


            //scriptGlobals.IsGetVariable = true;
            scriptState = await CoreCLR.EvaluateScriptAsync(MainProject.formulaProject_Collection.Last().formula_Collection.Last().FormulaExec_Script.Script, new List<string>(), scriptGlobals, scriptState);
            scriptGlobals.ScriptState = scriptState;
            scriptGlobals.ScriptGlobal = scriptGlobals;

            MainProject.formulaProject_Collection.Last().formula_Collection.Last().FormulaExec_Script.Script = CoreCLR_Script.createDefaultScript();
            MainProject.formulaProject_Collection.Last().formula_Collection.Last().FormulaExec_Script.Script +=
                "string formula = await solve(\"2+2+3\") + hello.Value + world.Value;" +
                "" +
                "return formula;";
            //scriptGlobals.IsGetVariable = false;
            scriptState = await CoreCLR.EvaluateScriptAsync(MainProject.formulaProject_Collection.Last().formula_Collection.Last().FormulaExec_Script.Script, new List<string>(), scriptGlobals, scriptState);
            scriptGlobals.ScriptState = scriptState;
            scriptGlobals.ScriptGlobal = scriptGlobals;
            Assert.AreEqual("710123", scriptState.ReturnValue);



            //TEST Rule

            MainProject.formulaProject_Collection.Last().formula_Collection.Last().FormulaExec_Script.Script = CoreCLR_Script.createDefaultScript();
            MainProject.formulaProject_Collection.Last().formula_Collection.Last().FormulaExec_Script.Script +=
                //"hello = \"1\";" +
                //"ruleEvaluateAll();" +
                //"ruleEvaluateAt(0);" +
                "return (bool)(await ruleEvaluateAt(1));" +
                "";
            //scriptGlobals.IsGetVariable = false;
            try
            {
                scriptState = await CoreCLR.EvaluateScriptAsync(MainProject.formulaProject_Collection.Last().formula_Collection.Last().FormulaExec_Script.Script, new List<string>(), scriptGlobals, scriptState);
                scriptGlobals.ScriptState = scriptState;
                scriptGlobals.ScriptGlobal = scriptGlobals;
            }
            //catch
            //{
            //    string s = ErrorHandler.errorMessage;
            //}
            finally { }



            //if (!string.IsNullOrEmpty(ErrorHandler.errorMessage))
            //{
            //    //set error msg to tbox
            //}
            //else
            //{
            //}
        }

    }
}
