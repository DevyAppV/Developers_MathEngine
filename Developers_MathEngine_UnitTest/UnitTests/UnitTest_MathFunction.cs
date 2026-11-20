using Developers_MathEngine_MVVM.Engine.CoreCLR;
using Developers_MathEngine_MVVM.Interpreter.Parser;
using Developers_MathEngine_MVVM.Interpreter.Token;
using Developers_MathEngine_MVVM.Model.Formula;
using Developers_MathEngine_MVVM.Model.Project;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Developers_MathEngine_UnitTest.UnitTests
{
    [TestClass]
    public class UnitTest_MathFunction
    {

        [TestMethod]
        public async Task testFunction() {
            //Token asd;
            //asd.
            UnitTest_Setup.setupFormula();
            MainProject.formulaProject_Collection.Last().formula_Collection.Last().FormulaExec_Script.Script = "return await solve(\"abs(-1-1)\");";

            CoreCLR_API.ScriptGlobals scriptGlobals = new CoreCLR_API.ScriptGlobals(MainProject.formulaProject_Collection.Last().formula_Collection.Last());

            ScriptState<object> scriptState = await CoreCLR.EvaluateScriptAsync(
                MainProject.formulaProject_Collection.Last().formula_Collection.Last().FormulaExec_Script.Script, new List<string>(), scriptGlobals);

            //scriptState = await CoreCLR.EvaluateScriptAsync(
            //    MainProject.formulaProject_Collection.Last().formula_Collection.Last().FormulaExec_Script.Script, new List<string>(), scriptGlobals, scriptState);

            Assert.AreEqual("2", scriptState.ReturnValue, "!2");

            //scriptGlobals.currentFunctionName = "";
            //scriptGlobals.ignoreVarList = null;
            MainProject.formulaProject_Collection.Last().formula_Collection.Last().FormulaExec_Script.Script = "return await solve(\"abs(-1+abs(-1-1)-1)\");";
            scriptState = await CoreCLR.EvaluateScriptAsync(
                MainProject.formulaProject_Collection.Last().formula_Collection.Last().FormulaExec_Script.Script, new List<string>(), scriptGlobals, scriptState);

            Assert.AreEqual("0", scriptState.ReturnValue, "!0");


            //TODO: Solve with var

            MainProject.formulaProject_Collection.Last().formula_Collection.Last().FormulaExec_Script.Script = "return await solve(\"asd+abs(-1+abs(-1-qwe)-1)\");";
            MainProject.formulaProject_Collection.Last().formula_Collection.Last().Variable_Dictionary.Add("asd",new Variable() { VariableName="asd",VariableValue="-1" });
            MainProject.formulaProject_Collection.Last().formula_Collection.Last().Variable_Dictionary.Add("qwe",new Variable() { VariableName="qwe",VariableValue="1" });
            scriptState = await CoreCLR.EvaluateScriptAsync(
                MainProject.formulaProject_Collection.Last().formula_Collection.Last().FormulaExec_Script.Script, new List<string>(), scriptGlobals, scriptState);

            Assert.AreEqual("-1", scriptState.ReturnValue, "!-1");

        }
    }
}
