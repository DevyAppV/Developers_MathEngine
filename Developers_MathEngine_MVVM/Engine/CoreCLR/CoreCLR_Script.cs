using System;
using System.Collections.Generic;
using System.Text;

namespace Developers_MathEngine_MVVM.Engine.CoreCLR
{
    public class CoreCLR_Script
    {
        public static string createDefaultScript()
        {
            return
                "using Developers_MathEngine_MVVM.Engine.Math;\r" +
                "using Developers_MathEngine_MVVM.Engine.CoreCLR;\r" +
                "using System;\r" +
                "using System.Linq;\r" +
                "using System.Collections.Generic;\r" +
                "using System.Threading.Tasks;\r" +
                "using Microsoft.UI.Dispatching;\r" +
                "\r" +
                "\r";
        }
        public static string createDefaultScript_ForDefaultCustomButton()
        {
            return
                "using Developers_MathEngine_MVVM.Engine.Math;\r" +
                "using Developers_MathEngine_MVVM.Engine.CoreCLR;\r" +
                "using System;\r" +
                "using System.Linq;\r" +
                "using System.Collections.Generic;\r" +
                "using System.Threading.Tasks;\r" +
                "using Microsoft.UI.Dispatching;\r" +
                "\r" +
                "\r";
        }
        public static string createDefaultScript_ForFormula()
        {
            return
                "using Developers_MathEngine_MVVM.Engine.Math;\r" +
                "using Developers_MathEngine_MVVM.Engine.CoreCLR;\r" + 
                "using System;\r" +
                "using System.Linq;\r" +
                "using System.Collections.Generic;\r" +
                "using System.Threading.Tasks;\r" +
                "using Microsoft.UI.Dispatching;\r" +
                "\r" +
                "//string example1 = getVariable_FromList(nameof(example));\r" +
                "//return await solve($@\"{example1}+example\");\r" +
                "//RuleEvaluateAll(Formula);\r" +
                "//await RuleEvaluateAt(0,Formula);\r" +
                "\r" +
                "return await solve($@\"\r\r" +
                "" +
                "\", \"100\");\r\r" +
                "" +
                "";
        }
        public static string createDefaultScript_ForVariable()
        {
            return
                "using System;\r" +
                "using Developers_MathEngine_MVVM.Engine.Math;\r" +
                "using Developers_MathEngine_MVVM.Engine.CoreCLR;\r" +
                "using System.Collections.Generic;\r" +
                "using Microsoft.UI.Dispatching;\r" +
                "\r" +
                "//Variable example = new Variable(nameof(example), \"1\");\r" +
                "//getVariables_ToList(new List<Variable>(){example}, Formula);\r\r" +
                "" +
                "";
        }
    }
}
