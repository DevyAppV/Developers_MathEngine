using Developers_MathEngine_MVVM.Engine.Math;
using Developers_MathEngine_MVVM.Interpreter.Errors;
using Developers_MathEngine_MVVM.Interpreter.Lexer;
using Developers_MathEngine_MVVM.Interpreter.Parser;
using Developers_MathEngine_MVVM.Interpreter.Token;
using Developers_MathEngine_MVVM.Interpreter.Utilities;
//using Developers_MathEngine_MVVM.Model.Formula;
using Developers_MathEngine_MVVM.Model.Project;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.UI.Dispatching;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Developers_MathEngine_MVVM.Engine.CoreCLR.CoreCLR_API;


namespace Developers_MathEngine_MVVM.Engine.CoreCLR
{
    public partial class CoreCLR_API
    {

        //api
        public partial class ScriptGlobals
        {


            public OrderedDictionary<string, List<Token>> CustomButtonUse_TokensArg_Dictionary = new OrderedDictionary<string, List<Token>>();
            //public Dictionary<int, HashSet<string>> CustomButtonRuntimeVariable_TokensArg_Dictionary = new Dictionary<int, HashSet<string>>();
            //public Dictionary<int, HashSet<string>> RuntimeVariable_Dictionary_CustomButton_Arg = new Dictionary<int, HashSet<string>>();
            public string solveCurrentButton_Name = "";
            public Developers_MathEngine_MVVM.Model.Formula.Formula Formula;
            ////empty except for variables and rules - use for rules evaluation
            public Developers_MathEngine_MVVM.Model.Formula.CustomButton CustomButton = null;

            public ScriptGlobals(Developers_MathEngine_MVVM.Model.Formula.Formula Formula)
            {
                this.Formula = Formula;
            }
            //for testing
            public ScriptGlobals() { }



            public Variable Variable { get; set; }

            //public async void getVariable(string nameOfVariable)
            //{
            //    variableDictionary = Formula.Variable_Dictionary;

            //    if (!Formula.Variable_Dictionary.ContainsKey(nameOfVariable))
            //    {
            //        Formula.addVariable_Collect_Diction(nameOfVariable, nameOfVariable, "");
            //    }
            //}


            public string getVariable_FromList(string nameOfVariable)
            {
                if (CustomButtonUse_TokensArg_Dictionary.ContainsKey(nameOfVariable))
                {
                    StringBuilder returnToken = new StringBuilder();
                    foreach (Token token in CustomButtonUse_TokensArg_Dictionary[nameOfVariable])
                    {
                        returnToken.Append(token.value);
                    }
                    if (!returnToken.ToString().Equals(nameOfVariable))
                        return returnToken.ToString();
                }
                if (Formula.Variable_Dictionary.ContainsKey(nameOfVariable) && !Formula.Variable_Dictionary[nameOfVariable].VariableValue.Equals(nameOfVariable))
                {
                    return Formula.Variable_Dictionary[nameOfVariable].VariableValue;
                }
                return nameOfVariable;
            }

            public async void getVariables_ToList(List<Variable> variables, Model.Formula.Formula formula)
            {

                //if (formula != null && formula.dispatcherQueue != null)
                //{
                //formula.dispatcherQueue.TryEnqueue(() =>
                //{
                if (formula.beginSolve)
                {
                    foreach (Variable v in variables)
                    {
                        if (!formula.Variable_Dictionary.ContainsKey(v.Name))
                        {
                            //formula.dispatcherQueue.TryEnqueue(() =>
                            //{
                            formula.addVariable_Collect_Diction(formula.Variable_Collection.Count.ToString(), v.Name, v.Value, "");
                            //});
                            formula.beginSolve_VarHashSet.Add(v.Name);
                        }
                        else if (formula.Variable_Collection.FirstOrDefault(x => x.VariableName.Equals(v.Name)) != null)
                        {
                            formula.Variable_Collection.FirstOrDefault(x => x.VariableName.Equals(v.Name)).VariableIndex = formula.Variable_Dictionary[v.Name].VariableIndex;
                            formula.Variable_Collection.FirstOrDefault(x => x.VariableName.Equals(v.Name)).VariableName = formula.Variable_Dictionary[v.Name].VariableName;
                            formula.Variable_Collection.FirstOrDefault(x => x.VariableName.Equals(v.Name)).VariableValue = formula.Variable_Dictionary[v.Name].VariableValue;
                            formula.Variable_Collection.FirstOrDefault(x => x.VariableName.Equals(v.Name)).VariableDescription = formula.Variable_Dictionary[v.Name].VariableDescription;
                        }
                        else
                        {
                            //formula.dispatcherQueue.TryEnqueue(() =>
                            //{
                            formula.addVariable_Collect_Diction(formula.Variable_Collection.Count.ToString(), v.Name, v.Value, formula.Variable_Dictionary[v.Name].VariableValue);
                            //});
                        }
                        if (!formula.beginSolve_VarHashSet.Contains(v.Name))
                        {
                            formula.beginSolve_VarHashSet.Add(v.Name);
                        }
                        if (formula.Variable_Dictionary.ContainsKey(v.Name))
                        {
                            v.Value = formula.Variable_Dictionary[v.Name].VariableValue;
                        }

                    }

                    int i = 0;
                    //foreach (KeyValuePair<string, Model.formula.Variable> v in formula.Variable_Dictionary)
                    while (i < formula.Variable_Dictionary.Count)
                    {
                        if (variables.FirstOrDefault(x => x.Name.Equals(formula.Variable_Dictionary.ElementAt(i).Value.VariableName)) == null)
                        {
                            formula.beginSolve_VarHashSet.Remove(formula.Variable_Dictionary.ElementAt(i).Key);
                            formula.deleteVariable_Collect_Diction(formula.Variable_Dictionary.ElementAt(i).Key);
                        }
                        i++;
                    }
                }
                //});
                //}
            }

            ////set variable from edit list
            //public void setVariableDefault(Variable variable, string nameOfVariable)
            //{

            //    if (Formula != null)
            //    {
            //        Developers_MathEngine_MVVM.Model.Formula.Variable selectVariable = Formula.Variable_Collection.FirstOrDefault(x => x.VariableName == nameOfVariable);
            //        if (selectVariable != null)
            //        {
            //            variable.Value = selectVariable.VariableValue;
            //        }
            //    }
            //}

            ////set variable during runtime
            //public Variable setVariable(Variable variable, string value = "0")
            //{
            //    //Variable Value = value;
            //    if (Formula != null)
            //    {

            //        if (Formula.Variable_Dictionary.ContainsKey(nameOfVariable))
            //        {
            //            Formula.Variable_Dictionary[nameOfVariable].VariableName = nameOfVariable;
            //            Formula.Variable_Dictionary[nameOfVariable].VariableValue = value;
            //            Formula.Variable_Dictionary[nameOfVariable].VariableDescription = description;

            //        }
            //        else
            //        {
            //            Formula.Variable_Dictionary.Add(nameOfVariable,
            //                new Developers_MathEngine_MVVM.Model.Formula.Variable()
            //                {
            //                    VariableName = nameOfVariable,
            //                    VariableValue = value,
            //                    VariableDescription = description
            //                });
            //        }
            //        return new Variable(value);
            //    }
            //    return new Variable("");
            //}
            public ScriptGlobals ScriptGlobal { get; set; }
            public ScriptState<object> ScriptState { get; set; }



            //rules
            public void RuleEvaluateAll(Model.Formula.Formula formula)
            {
                if (Formula != null)
                {
                    Rules.evaluateAll(Formula.Rules_Collection, formula.ScriptState, formula.ScriptGlobals);
                }
            }

            public async Task<object> RuleEvaluateAt(int index, Model.Formula.Formula formula)
            {
                return await Rules.evaluateAt(index, formula.Rules_Collection, formula.ScriptState, formula.ScriptGlobals);

            }



            //length

            public string getLengthToDecimal(string refStr)
            {
                return Utilities_String.countStr(Utilities_String.getSubString(refStr, "0", "0", true));
            }
            public string getLengthFromDecimal(string refStr)
            {
                return Utilities_String.countStr(Utilities_String.getSubString(refStr, getLengthToDecimal(refStr), "infinite"));
            }
            public string getLength(string refStr, bool excludeDecimal)
            {
                return Utilities_String.countStr(refStr, false, excludeDecimal);
            }
            public string getSubString(string refStr, string start_Inclusive, string endOrInfinite_Exclusive, bool isGetToDecimal_Exclusive = false)
            {
                return Utilities_String.getSubString(refStr, start_Inclusive, endOrInfinite_Exclusive, isGetToDecimal_Exclusive);
            }

            public string increment(string refStrValue, char incrementBy_SingleChar)
            {
                //if (Lexer_Math.lex(refStrValue, 0).Last().tokenType == TokenType.NUMBER)
                return Utilities_String.increment(refStrValue, incrementBy_SingleChar);
                //else return refStrValue;
            }

            public string decrement(string refStrValue, char incrementBy_SingleChar)
            {
                //if (Lexer_Math.lex(refStrValue, 0).Last().tokenType == TokenType.NUMBER)
                return Utilities_String.decrement(refStrValue, incrementBy_SingleChar);
                //else return refStrValue;
            }


            //conditional
            public bool isLeftStrGreater(string left, string right)
            {
                return Utilities_String.isLeftStr_LargerThenRightStr_Number(left, right);
            }

            public bool isLeftStrGreaterOrEqual(string left, string right)
            {
                return Utilities_String.isLeftStr_LargerThenEqualToRightStr_Number(left, right);
            }
            public bool isLeftStrLess(string left, string right)
            {
                return !Utilities_String.isLeftStr_LargerThenEqualToRightStr_Number(left, right);
            }
            public bool isLeftStrLessOrEqual(string left, string right)
            {
                return !Utilities_String.isLeftStr_LargerThenRightStr_Number(left, right);
            }


            //Solve
            public async Task<string> solve(string input, string precision = "30")
            {
                //DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

                if (input.Equals(Formula.beginSolve_CurrentFormula) || string.IsNullOrEmpty(Formula.beginSolve_CurrentFormula))
                {
                    Formula.beginSolve = true;
                }

                List<Token> lexerToken = null;
                List<Token> parsedFormulaVar = null;
                if (Formula.beginSolve)
                {
                    Formula.ScriptGlobals.CustomButton = null;
                    solveCurrentButton_Name = "";
                    Formula.beginSolve_CurrentFormula = input;
                    lexerToken = Lexer_Math.lex(input, 0, false, false, true);
                    Formula.beginSolve = false;
                    Formula.setUseOnce = false;
                    Formula.isSolvingNestedFunc = false;
                    CustomButtonUse_TokensArg_Dictionary.Clear();
                    bool skipCalc = false;
                    //HashSet<string> varHashSet = new HashSet<string>();
                    foreach (Token token in lexerToken)
                    {
                        switch (token.value)
                        {
                            case "true":
                                continue;
                            case "false":
                                continue;
                            case "infinite":
                                continue;
                            default:
                                break;
                        }
                        if (token.tokenType == TokenType.STRING)
                        {
                            if (CustomButtonTab.customButtons_Dictionary.ContainsKey(token.value) &&
                                !CustomButtonTab.customButtons_Dictionary[token.value].Use.EndsWith(')'))
                            {
                                //Formula.dispatcherQueue.TryEnqueue(async() =>
                                //{
                                try
                                {
                                    ScriptState<object> scriptState = await CoreCLR.EvaluateScriptAsync(CustomButtonTab.customButtons_Dictionary[token.value].VariableExec_Script.Script, new List<string>(), Formula.ScriptGlobals);
                                    scriptState = await CoreCLR.EvaluateScriptAsync(CustomButtonTab.customButtons_Dictionary[token.value].FormulaExec_Script.Script, new List<string>(), Formula.ScriptGlobals, scriptState);
                                    token.value = (string)scriptState.ReturnValue;
                                    token.tokenType = TokenType.NUMBER;
                                    foreach (Token quickLexToken in Lexer_Math.lex((string)scriptState.ReturnValue, 0))
                                    {
                                        if (quickLexToken.tokenType == TokenType.STRING || quickLexToken.tokenType == TokenType.FUNCTION)
                                        {
                                            token.tokenType = TokenType.STRING;
                                            break;
                                        }
                                    }
                                }
                                catch { }
                                //});
                            }
                            else if (!Formula.Variable_Dictionary.ContainsKey(token.value))
                            {
                                Formula.Variable_Dictionary.Add(token.value, new Model.Formula.Variable()
                                {
                                    VariableValue = token.value,
                                    VariableName = token.value,
                                    //VariableDescription = ""
                                });
                                Formula.dispatcherQueue.TryEnqueue(() =>
                                {
                                    Formula.Variable_Collection.Add(new Model.Formula.Variable()
                                    {
                                        VariableValue = token.value,
                                        VariableName = token.value,
                                        //VariableDescription = ""
                                    });
                                });
                                if (!skipCalc)
                                    skipCalc = true;
                            }
                            Formula.beginSolve_VarHashSet.Add(token.value);
                        }
                    }


                    foreach (Model.Formula.Variable variable in Formula.Variable_Collection)
                    {
                        if (!Formula.Variable_Dictionary[variable.VariableName].VariableValue.Equals(variable.VariableValue))
                        {
                            Formula.Variable_Dictionary[variable.VariableName].VariableValue = variable.VariableValue;
                        }
                    }
                    parsedFormulaVar = Parser_Math.setVariableToken_FromFormula(lexerToken, this);
                    if (skipCalc)
                    {
                        Formula.beginSolve = true;
                        return input;
                    }
                }
                else
                {
                    lexerToken = Lexer_Math.lex(input, 0, false, false, true);
                }
                if (lexerToken.Count > 0)
                {
                    string result = "";
                    if (input.Equals(Formula.beginSolve_CurrentFormula))
                    {
                        if (parsedFormulaVar != null)
                            result = await Parser_Math.parse(parsedFormulaVar, this, precision);
                        else
                            result = await Parser_Math.parse(lexerToken, this, precision);

                        Formula.beginSolve = true;
                        Formula.beginSolve_CurrentFormula = "";
                    }
                    else
                    {
                        if (parsedFormulaVar != null)
                            result = await Parser_Math.parse(parsedFormulaVar, this, precision);
                        else
                            result = await Parser_Math.parse(lexerToken, this, precision);
                    }
                    return result;
                }
                else return input;
            }

            public async Task<List<string>> solve(List<string> input_List, string precision) { return await Parser_Math.parse(input_List, this, precision); }
            //public async Task<string> solve(List<string> str_List) { return string.Join(", ", await Parser_Math.parseBasic(str_List)); }
            //public async Task<string> solve(string[] str_List) { return string.Join(", ", await Parser_Math.parseBasic(str_List.ToList())); }
            //public async Task<string> solve(string str) { return (await Parser_Math.parseBasic(new List<string>() { str }))[0].value; }




        }
    }
}
