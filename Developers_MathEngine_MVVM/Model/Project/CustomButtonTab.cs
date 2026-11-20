using CommunityToolkit.Mvvm.ComponentModel;
using Developers_MathEngine_MVVM.Engine.CoreCLR;
using Developers_MathEngine_MVVM.Model.Formula;
using Developers_MathEngine_MVVM.Model.Script;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Developers_MathEngine_MVVM.Model.Project
{
    public partial class CustomButtonTab : ObservableObject
    {

        [ObservableProperty]
        public partial string SubTabName { get; set; }

        [ObservableProperty]
        public partial string Name { get; set; }

        [ObservableProperty]
        public partial int row { get; set; } = 0;

        [ObservableProperty]
        public partial ObservableCollection<CustomButton> customButtons_Collection { get; set; } = new ObservableCollection<CustomButton>();

        [ObservableProperty]
        public partial ObservableCollection<CustomButtonTab> customButtonsSubTab_Collection { get; set; } = new ObservableCollection<CustomButtonTab>();
        //public CustomButtonTab selectedSubTab = null;
        //public CustomButtonTab selectedSubTab_Previous = null;

        public static Dictionary<string, CustomButton> customButtons_Dictionary = new Dictionary<string, CustomButton>();

        //public void reset_CustomButtons_Dictionary()
        //{
        //    foreach (CustomButton button in customButtons_Collection)
        //    {
        //        if (string.IsNullOrEmpty(button.FormulaName)) continue;
        //        if (customButtons_Dictionary.ContainsKey(button.FormulaName))
        //        {
        //            if (customButtons_Dictionary[button.FormulaName] != button)
        //            {
        //                customButtons_Dictionary[button.FormulaName] = button;
        //            }
        //        }
        //    }
        //}

        public void CreateSubTab(string subTabName)
        {
            //selectedSubTab_Previous = customButtonsSubTab_Collection.Last();
            customButtonsSubTab_Collection.Add(new CustomButtonTab() { Name = subTabName, row = this.row + 1 });
            //MainProject.selectedCustomButtonTab = customButtonsSubTab_Collection.Last();
            //selectedSubTab = customButtonsSubTab_Collection.Last();
        }

        public void CreateDefaultSubTab_FromLastCollection(string subTabName, string parentSubTabName)
        {
            //selectedSubTab_Previous = customButtonsSubTab_Collection.Last();
            MainProject.CustomButtonTab_Collection.Last().customButtonsSubTab_Collection.
                Add(new CustomButtonTab() { Name = subTabName, SubTabName = parentSubTabName, row = this.row + 1 });
            //MainProject.selectedCustomButtonTab = customButtonsSubTab_Collection.Last();
            //selectedSubTab = customButtonsSubTab_Collection.Last();
        }


        public void addCustomButtonToDictionary()
        {
            foreach (CustomButton button in customButtonsSubTab_Collection.Last().customButtons_Collection)
            {
                if (string.IsNullOrEmpty(button.FormulaName)) continue;
                if (!customButtons_Dictionary.ContainsKey(button.FormulaName))
                {
                    customButtons_Dictionary.Add(button.FormulaName, button);
                }
            }
        }



        public void setUpDefaultCustomButton()
        {

            MainProject.CustomButtonTab_Collection.Last().Name = "Default";
            MainProject.CustomButtonTab_Collection.Last().row = 0;
            MainProject.selectedCustomButtonTab = MainProject.CustomButtonTab_Collection.Last();
            CreateDefaultSubTab_FromLastCollection("Symbols", "Default");
            MainProject.selectedCustomButtonTab.addSetDefaultCustomButtons_Symbols();
            MainProject.selectedCustomButtonTab.addCustomButtonToDictionary();
            CreateDefaultSubTab_FromLastCollection("Algebra", "Default");
            MainProject.selectedCustomButtonTab.addSetDefaultCustomButtons_Algebra();
            MainProject.selectedCustomButtonTab.addCustomButtonToDictionary();
            CreateDefaultSubTab_FromLastCollection("Trig", "Default");
            MainProject.selectedCustomButtonTab.addSetDefaultCustomButtons_Trig();
            MainProject.selectedCustomButtonTab.addCustomButtonToDictionary();

        }


        public void addSetDefaultCustomButtons_Symbols()
        {

            MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals("Default")).
                   customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals("Symbols")).
                   customButtons_Collection.Add(new CustomButton(
                   ScriptState: null,
                   ScriptGlobals: null,
                   FormulaName: "π",
                   VariableExec_Script: new Scripting()
                   {
                       ScriptName = "ExecVariable",
                       Script = CoreCLR_Script.createDefaultScript_ForVariable() +
                       ""

                   },
                   FormulaExec_Script: new Scripting()
                   {
                       Script = CoreCLR_Script.createDefaultScript_ForDefaultCustomButton() +
                       //"string result = await solve(\"x\");\r\r" +
                       "return \"" + Math.PI + "\";"
                   },
                   Description: "Returns PI.",
                   Solution: "",
                   Use: "π",
                   TabName: "Symbols",
                   LocalScript_Collection: new ObservableCollection<Scripting>(),
                   Rules_Collection: new ObservableCollection<Rule>(),
                   //RuntimeVariable_Collection: new ObservableCollection<RuntimeVariable>(),
                   Variable_Collection: new ObservableCollection<Variable>()
                   {

                   }
                   ));


            CustomButton button = MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals("Default")).
                customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals("Symbols")).
                customButtons_Collection.Last();
            CustomButtonTab.customButtons_Dictionary.Add(button.FormulaName, new CustomButton(button, button.Use, "Symbols"));




            MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals("Default")).
                   customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals("Symbols")).
                   customButtons_Collection.Add(new CustomButton(
                   ScriptState: null,
                   ScriptGlobals: null,
                   FormulaName: "e",
                   VariableExec_Script: new Scripting()
                   {
                       ScriptName = "ExecVariable",
                       Script = CoreCLR_Script.createDefaultScript_ForVariable() +
                       ""

                   },
                   FormulaExec_Script: new Scripting()
                   {
                       Script = CoreCLR_Script.createDefaultScript_ForDefaultCustomButton() +
                       //"string result = await solve(\"x\");\r\r" +
                       "return \"" + Math.E + "\";"
                   },
                   Description: "Returns the natural log base e.",
                   Solution: "",
                   Use: "e",
                   TabName: "Symbols",
                   LocalScript_Collection: new ObservableCollection<Scripting>(),
                   Rules_Collection: new ObservableCollection<Rule>(),
                   //RuntimeVariable_Collection: new ObservableCollection<RuntimeVariable>(),
                   Variable_Collection: new ObservableCollection<Variable>()
                   {

                   }
                   ));


            button = MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals("Default")).
               customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals("Symbols")).
               customButtons_Collection.Last();
            CustomButtonTab.customButtons_Dictionary.Add(button.FormulaName, new CustomButton(button, button.Use, "Symbols"));
        }



        public void addSetDefaultCustomButtons_Algebra()
        {


            //modulus

            MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals("Default")).
                customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals("Algebra")).
                customButtons_Collection.Add(new CustomButton(
                ScriptState: null,
                ScriptGlobals: null,
                FormulaName: "factor",
                VariableExec_Script: new Scripting()
                {
                    ScriptName = "ExecVariable",
                    Script = CoreCLR_Script.createDefaultScript_ForVariable() +
                "\rVariable x = new Variable(nameof(x), \"x\");\rgetVariables_ToList(new List<Variable>(){x}, Formula);\r\r"

                },
                FormulaExec_Script: new Scripting()
                {
                    Script = CoreCLR_Script.createDefaultScript_ForDefaultCustomButton() +
                    "\r" +
                    "\r" +
                    "\rstring x = await solve($@\"{getVariable_FromList(nameof(x))}\");\rstring halfX = await solve($@\"\rx/2\r\");\rif(x.Equals(\"1\"))halfX=\"1\";\rstring result = \"\";\rstring returnResult = \"\";\rstring cutResult = x;\rfor(string i = \"1\"; isLeftStrLessOrEqual(i,halfX); i=increment(i,'1')){\r\tstring chk = await solve($@\"{i}*{cutResult}\");\r\tif(chk.Equals(x)){\r\t\tif(isLeftStrGreater(i,cutResult)) break;\r\t\tresult+=$@\"{i}, {cutResult}\"+'\\r';\r\r\t}\r\tcutResult =getSubString(await solve($@\"({x})/({i}+1)\"),\"0\",\"infinite\", true);\r\tif(i.Equals(cutResult)) break;\r}\rreturn result;\r" +
                    "" +
                    ""
                },
                Description: "return factors of a number",
                Solution: "",
                Use: "factor(x)",
                   TabName: "Algebra",
                LocalScript_Collection: new ObservableCollection<Scripting>(),
                Rules_Collection: new ObservableCollection<Rule>(),
                //RuntimeVariable_Collection: new ObservableCollection<RuntimeVariable>(),
                Variable_Collection: new ObservableCollection<Variable>()
                {
                    new Variable()
                    {
                        VariableIndex = "0",
                        VariableName = "x",
                        VariableValue = "x",
                        VariableDescription = "a math expression; Value to be factored"
                    },
                }
                ));
            CustomButton button = MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals("Default")).
                customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals("Algebra")).
                customButtons_Collection.Last();
            CustomButtonTab.customButtons_Dictionary.Add(button.FormulaName, new CustomButton(button, button.Use, "Algebra"));


            //absolute

            MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals("Default")).
                customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals("Algebra")).
                customButtons_Collection.Add(new CustomButton(
                ScriptState: null,
                ScriptGlobals: null,
                FormulaName: "nRoot",
                VariableExec_Script: new Scripting()
                {
                    ScriptName = "ExecVariable",
                    Script = CoreCLR_Script.createDefaultScript_ForVariable() +
                    "Variable A = new Variable(nameof(A), \"A\");\r" +
                    "Variable n = new Variable(nameof(n), \"n\");\r" +
                    "Variable precision = new Variable(nameof(precision), \"precision\");\r" +
                    "getVariables_ToList(new List<Variable>(){A,n,precision}, Formula);"

                },
                FormulaExec_Script: new Scripting()
                {
                    Script = CoreCLR_Script.createDefaultScript_ForDefaultCustomButton() +
                    //"string result = await solve(\"x\");\r\r" +
                    "string A = getVariable_FromList(nameof(A));\r" +
                    "string n = getVariable_FromList(nameof(n));\r" +
                    "string precision  = getVariable_FromList(nameof(precision));\r\r" +
                    "string x = \"0\";\r\r\r" +
                    "x = getSubString(await solve( $\"A/n+1\", \"25\"),\"0\",\"infinite\", true);\r" +
                    "string lowerX = \"0\";\r\r" +
                    "string subX = x;\r" +
                    "string powerXResult = \"0\";\rstring powerLowerXResult = \"0\";\r" +
                    "for(;isLeftStrGreater(subX ,\"0\");subX = decrement(subX ,'1')){\r" +
                    "\tpowerXResult = await solve( $@\"{subX}^{n}\", precision);\r" +
                    "\tpowerLowerXResult  = await solve( $@\"({subX} -1) ^ {n}\", precision);\r" +
                    "\tif(isLeftStrGreater(powerXResult, A) && !isLeftStrGreater(powerLowerXResult , A)  )\r" +
                    "\t{\r\t\tx = await solve( $\"({subX}+{subX}-1)/2\", \"25\");\r\t\t\r\t\tbreak;\r" +
                    "\t}\r\r}\r\r\r" +
                    "//return x+\"::\"+powerLowerXResult  +\"::\"+powerXResult ;\rstring result = \"\";\r" +
                    "for(int i =1;i<=5;i++){\r\t//string appliedPrecision = await solve( $\"{precision}*{i.ToString()}\",\"infinite\");\r" +
                    "\tx = await solve($@\"\r\t\t1/{n}*(({n}-1)*{x}+({A})/({x}^({n}-1)))\r" +
                    "\t\t\", precision);\r" +
                    "\t//result+=x+'';\r" +
                    "}\r" +
                    "return x;\r" +
                    "//return result;"
                },
                Description: "Returns the nth root of A.",
                Solution: "",
                Use: "nRoot(A,n)",
                   TabName: "Algebra",
                LocalScript_Collection: new ObservableCollection<Scripting>(),
                Rules_Collection: new ObservableCollection<Rule>(),
                //RuntimeVariable_Collection: new ObservableCollection<RuntimeVariable>(),
                Variable_Collection: new ObservableCollection<Variable>() {
                    new Variable()
                    {
                        VariableIndex = "0",
                        VariableName = "A",
                        VariableValue = "A",
                        VariableDescription = "an expression"
                    },
                    new Variable()
                    {
                        VariableIndex = "1",
                        VariableName = "n",
                        VariableValue = "n",
                        VariableDescription = "nth Root"
                    },
                    new Variable()
                    {
                        VariableIndex = "2",
                        VariableName = "precision",
                        VariableValue = "25",
                        VariableDescription = "set a default length for divisions"
                    },

                }
                ));

            button = MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals("Default")).
                 customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals("Algebra")).
                 customButtons_Collection.Last();
            CustomButtonTab.customButtons_Dictionary.Add(button.FormulaName, new CustomButton(button, button.Use, "Algebra"));



            MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals("Default")).
                customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals("Algebra")).
                customButtons_Collection.Add(new CustomButton(
                ScriptState: null,
                ScriptGlobals: null,
                FormulaName: "abs",
                VariableExec_Script: new Scripting()
                {
                    ScriptName = "ExecVariable",
                    Script = CoreCLR_Script.createDefaultScript_ForVariable()
                },
                FormulaExec_Script: new Scripting()
                {
                    Script = CoreCLR_Script.createDefaultScript_ForDefaultCustomButton() +
                    //"string result = await solve(\"x\");\r\r" +
                    "string result = await solve(\"x\", \"1000\");" +
                    "\r" +
                    "return (result.TrimStart(\' \').StartsWith(\'-\')) ? result.TrimStart(\' \').TrimStart(\'-\') : result.TrimStart(\' \');\r"

                },
                Description: "Returns x as a positive number.",
                Solution: "",
                Use: "abs(x)",
                   TabName: "Algebra",
                LocalScript_Collection: new ObservableCollection<Scripting>(),
                Rules_Collection: new ObservableCollection<Rule>(),
                //RuntimeVariable_Collection: new ObservableCollection<RuntimeVariable>(),
                Variable_Collection: new ObservableCollection<Variable>() {
                    new Variable()
                    {
                        VariableIndex = "0",
                        VariableName = "x",
                        VariableValue = "x",
                        VariableDescription = "a math expression"
                    }

                }
                ));

            button = MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals("Default")).
                 customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals("Algebra")).
                 customButtons_Collection.Last();
            CustomButtonTab.customButtons_Dictionary.Add(button.FormulaName, new CustomButton(button, button.Use, "Algebra"));



            MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals("Default")).
                customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals("Algebra")).
                customButtons_Collection.Add(new CustomButton(
                ScriptState: null,
                ScriptGlobals: null,
                FormulaName: "mod",
                VariableExec_Script: new Scripting()
                {
                    ScriptName = "ExecVariable",
                    Script = CoreCLR_Script.createDefaultScript_ForVariable() +
                    "Variable x = new Variable(nameof(x), \"x\");\r" +
                    "Variable m = new Variable(nameof(m), \"m\");\r" +
                    "getVariables_ToList(new List<Variable>(){x, m}, Formula);"
                },
                FormulaExec_Script: new Scripting()
                {
                    Script = CoreCLR_Script.createDefaultScript_ForDefaultCustomButton() +

                    "string x = await solve($@\"{getVariable_FromList(nameof(x))}\");\rstring m = await solve($@\"{getVariable_FromList(nameof(m))}\");\r\rstring divideFloor =\r\tgetSubString(await solve($@\"{x}/{m}\", \"1000\"),\r\t\"0\",\"infinite\",true);\rif(string.IsNullOrEmpty(divideFloor))divideFloor = \"0\";\r\rstring result = await solve($@\"\r{x}-({divideFloor}*{m})\r\", \"100\");\r\rif(x.StartsWith(\"-\") && !m.StartsWith(\"-\"))\r\tresult = await solve($@\"\r\t\t{result}+{m}\r\t\t\", \"100\");\relse if(!x.StartsWith(\"-\") && m.StartsWith(\"-\"))\r\tresult = await solve($@\"\r\t\t{result}+{m}\r\t\t\", \"100\");\r\rreturn result;"

                },
                Description: "Returns the remainder of a number (x). Not the remainder of a division.",
                Solution: "",
                Use: "mod(x,m)",
                   TabName: "Algebra",
                LocalScript_Collection: new ObservableCollection<Scripting>(),
                Rules_Collection: new ObservableCollection<Rule>(),
                //RuntimeVariable_Collection: new ObservableCollection<RuntimeVariable>(),
                Variable_Collection: new ObservableCollection<Variable>() {
                    new Variable()
                    {
                        VariableIndex = "0",
                        VariableName = "x",
                        VariableValue = "x",
                        VariableDescription = "a math expression"
                    },
                    new Variable()
                    {
                        VariableIndex = "1",
                        VariableName = "m",
                        VariableValue = "m",
                        VariableDescription = "the modulus"
                    },

                }
                ));

            button = MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals("Default")).
                 customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals("Algebra")).
                 customButtons_Collection.Last();
            CustomButtonTab.customButtons_Dictionary.Add(button.FormulaName, new CustomButton(button, button.Use, "Algebra"));










            MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals("Default")).
                customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals("Algebra")).
                customButtons_Collection.Add(new CustomButton(
                ScriptState: null,
                ScriptGlobals: null,
                FormulaName: "fact",
                VariableExec_Script: new Scripting()
                {
                    ScriptName = "ExecVariable",
                    Script = CoreCLR_Script.createDefaultScript_ForVariable() +
                    "Variable x = new Variable(nameof(x), \"1\");\rgetVariables_ToList(new List<Variable>(){x}, Formula);"
                },
                FormulaExec_Script: new Scripting()
                {
                    Script = CoreCLR_Script.createDefaultScript_ForDefaultCustomButton() +

                    "string x = await solve($@\"{getVariable_FromList(nameof(x))}\");\r" +
                    "string result = x;\r" +
                    "x=decrement(x, '1');\r" +
                    "for(;isLeftStrGreater(x,\"0\");x=decrement(x, '1'))\r" +
                    "{\r" +
                    "\tresult = await solve($@\"\r" +
                    "\t{result} * {x}\r" +
                    "\t\", \"1000\");\r" +
                    "}\r" +
                    "return result;"

                },
                Description: "Returns the factorial of a number (x).",
                Solution: "",
                Use: "fact(x)",
                   TabName: "Algebra",
                LocalScript_Collection: new ObservableCollection<Scripting>(),
                Rules_Collection: new ObservableCollection<Rule>(),
                //RuntimeVariable_Collection: new ObservableCollection<RuntimeVariable>(),
                Variable_Collection: new ObservableCollection<Variable>() {
                    new Variable()
                    {
                        VariableIndex = "0",
                        VariableName = "x",
                        VariableValue = "x",
                        VariableDescription = "a math expression"
                    }

                }
                ));

            button = MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals("Default")).
                 customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals("Algebra")).
                 customButtons_Collection.Last();
            CustomButtonTab.customButtons_Dictionary.Add(button.FormulaName, new CustomButton(button, button.Use, "Algebra"));







            //MainProject.CustomButtonTab_Collection.FirstOrDefault(x=>x.Name.Equals("Default")).
            //    customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals("Algebra")).
            //    customButtons_Collection.Add(new CustomButton(
            //    ScriptState: null,
            //    ScriptGlobals: null,
            //    FormulaName: "round",
            //    VariableExec_Script: new Scripting() { ScriptName = "ExecVariable",
            //    Script= CoreCLR_Script.createDefaultScript_ForVariable()
            //    },
            //    FormulaExec_Script: new Scripting()
            //    {
            //        Script = CoreCLR_Script.createDefaultScript_ForDefaultCustomButton() +

            //        "Variable x;" +
            //        "Variable isRoundInteger;" +
            //        "setVarValue(x, nameof(x), \"\")" +

            //        "string result : x;"

            //    },
            //    Description: "Returns x rounded to nearest number at integer or decimal length.",
            //    Solution: "",
            //    Use: "round(x,isRoundInteger)",
            //    LocalScript_Collection: new ObservableCollection<Scripting>(),
            //    Rules_Collection: new ObservableCollection<Rule>(),
            //    //RuntimeVariable_Collection: new ObservableCollection<RuntimeVariable>(),
            //    Variable_Collection: new ObservableCollection<Variable>() {
            //        new Variable()
            //        {
            //            VariableName = "x",
            //            VariableValue = "x",
            //            VariableDescription = "a math expression"
            //        },
            //        new Variable()
            //        {
            //            VariableName = "isRoundInteger",
            //            VariableValue = "isRoundInteger",
            //            VariableDescription = "True = round to integer places\r" +
            //            "False = round to decimal places"
            //        },

            //    }
            //    ));

            //button = MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals("Default")).
            //     customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals("Algebra")).
            //     customButtons_Collection.Last();
            //CustomButtonTab.customButtons_Dictionary.Add(button.FormulaName, new CustomButton(button, button.Use));

        }
        public void addSetDefaultCustomButtons_Trig()
        {












            //trig func
            MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals("Default")).
                customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals("Trig")).
                customButtons_Collection.Add(new CustomButton(
                ScriptState: null,
                ScriptGlobals: null,
                FormulaName: "sin",
                VariableExec_Script: new Scripting()
                {
                    ScriptName = "ExecVariable",
                    Script = CoreCLR_Script.createDefaultScript_ForVariable() +
                    "\rVariable x = new Variable(nameof(x), \"x\");\rVariable isDegree = new Variable(nameof(isDegree), \"isDegree\");\rgetVariables_ToList(new List<Variable>(){x,isDegree}, Formula);"

                },
                FormulaExec_Script: new Scripting()
                {
                    Script = CoreCLR_Script.createDefaultScript_ForDefaultCustomButton() +
                    "\rbool isDeg = getVariable_FromList(nameof(isDegree)).Equals(\"true\") ? true : false;\r\rdouble angle = (isDeg) ? double.Parse(getVariable_FromList(nameof(x)))*Math.PI/180 : double.Parse(getVariable_FromList(nameof(x)));\r\rif(angle%(Math.PI)==0)return \"0\";\rdouble result = Math.Sin(angle%(Math.PI*2));\r\rreturn result.ToString();"


                },
                Description: "C# sine of an angle.",
                Solution: "",
                Use: "sin(x,isDegree)",
                   TabName: "Trig",
                LocalScript_Collection: new ObservableCollection<Scripting>(),
                Rules_Collection: new ObservableCollection<Rule>(),
                //RuntimeVariable_Collection: new ObservableCollection<RuntimeVariable>(),
                Variable_Collection: new ObservableCollection<Variable>()
                {
                    new Variable(){
                    VariableIndex="0",
                    VariableName = "x",
                    VariableValue = "x",
                    VariableDescription = "a math expression",
                    },
                    new Variable(){
                    VariableIndex="1",
                    VariableName = "isDegree",
                    VariableValue = "isDegree",
                    VariableDescription = "true if x is in degree, else false",
                    },
                }
                ));


            CustomButton button = MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals("Default")).
                 customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals("Trig")).
                 customButtons_Collection.Last();
            CustomButtonTab.customButtons_Dictionary.Add(button.FormulaName, new CustomButton(button, button.Use, "Trig"));






















            //trig func
            MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals("Default")).
                customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals("Trig")).
                customButtons_Collection.Add(new CustomButton(
                ScriptState: null,
                ScriptGlobals: null,
                FormulaName: "cos",
                VariableExec_Script: new Scripting()
                {
                    ScriptName = "ExecVariable",
                    Script = CoreCLR_Script.createDefaultScript_ForVariable() +
                    "\rVariable x = new Variable(nameof(x), \"x\");\rVariable isDegree = new Variable(nameof(isDegree), \"isDegree\");\rgetVariables_ToList(new List<Variable>(){x,isDegree}, Formula);"

                },
                FormulaExec_Script: new Scripting()
                {
                    Script = CoreCLR_Script.createDefaultScript_ForDefaultCustomButton() +
                    "\r\r\rbool isDeg = getVariable_FromList(nameof(isDegree)).Equals(\"true\") ? true : false;\r\rdouble angle = (isDeg) ? double.Parse(getVariable_FromList(nameof(x)))*Math.PI/180 : double.Parse(getVariable_FromList(nameof(x)));\r\rif(angle%(Math.PI/2)==0 && angle%(Math.PI)!=0)return \"0\";\rdouble result = Math.Cos(angle%(Math.PI*2));\r\rreturn result.ToString();\r\r\r"


                },
                Description: "C# cosine of an angle.",
                Solution: "",
                Use: "cos(x,isDegree)",
                   TabName: "Trig",
                LocalScript_Collection: new ObservableCollection<Scripting>(),
                Rules_Collection: new ObservableCollection<Rule>(),
                //RuntimeVariable_Collection: new ObservableCollection<RuntimeVariable>(),
                Variable_Collection: new ObservableCollection<Variable>()
                {
                    new Variable(){
                    VariableIndex="0",
                    VariableName = "x",
                    VariableValue = "x",
                    VariableDescription = "a math expression",
                    },
                    new Variable(){
                    VariableIndex="1",
                    VariableName = "isDegree",
                    VariableValue = "isDegree",
                    VariableDescription = "true if x is in degree, else false",
                    },
                }
                ));


            button = MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals("Default")).
                 customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals("Trig")).
                 customButtons_Collection.Last();
            CustomButtonTab.customButtons_Dictionary.Add(button.FormulaName, new CustomButton(button, button.Use, "Trig"));



















            //trig func
            MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals("Default")).
                customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals("Trig")).
                customButtons_Collection.Add(new CustomButton(
                ScriptState: null,
                ScriptGlobals: null,
                FormulaName: "tan",
                VariableExec_Script: new Scripting()
                {
                    ScriptName = "ExecVariable",
                    Script = CoreCLR_Script.createDefaultScript_ForVariable() +
                    "\rVariable x = new Variable(nameof(x), \"x\");\rVariable isDegree = new Variable(nameof(isDegree), \"isDegree\");\rgetVariables_ToList(new List<Variable>(){x,isDegree}, Formula);"

                },
                FormulaExec_Script: new Scripting()
                {
                    Script = CoreCLR_Script.createDefaultScript_ForDefaultCustomButton() +
                    "\rbool isDeg = getVariable_FromList(nameof(isDegree)).Equals(\"true\") ? true : false;\r\rdouble angle = (isDeg) ? double.Parse(getVariable_FromList(nameof(x)))*Math.PI/180 : double.Parse(getVariable_FromList(nameof(x)));\r\r\r\rdouble result = Math.Tan(angle%(Math.PI*2));\r\rreturn result.ToString();\r\r"

                },
                Description: "C# tangent of an angle.",
                Solution: "",
                Use: "tan(x,isDegree)",
                   TabName: "Trig",
                LocalScript_Collection: new ObservableCollection<Scripting>(),
                Rules_Collection: new ObservableCollection<Rule>(),
                //RuntimeVariable_Collection: new ObservableCollection<RuntimeVariable>(),
                Variable_Collection: new ObservableCollection<Variable>()
                {
                    new Variable(){
                    VariableIndex="0",
                    VariableName = "x",
                    VariableValue = "x",
                    VariableDescription = "a math expression",
                    },
                    new Variable(){
                    VariableIndex="1",
                    VariableName = "isDegree",
                    VariableValue = "isDegree",
                    VariableDescription = "true if x is in degree, else false",
                    },
                }
                ));


            button = MainProject.CustomButtonTab_Collection.FirstOrDefault(x => x.Name.Equals("Default")).
                 customButtonsSubTab_Collection.FirstOrDefault(x => x.Name.Equals("Trig")).
                 customButtons_Collection.Last();
            CustomButtonTab.customButtons_Dictionary.Add(button.FormulaName, new CustomButton(button, button.Use, "Trig"));





































        }

    }
}
