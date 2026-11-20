using Developers_MathEngine_MVVM.Engine.CoreCLR;
using Developers_MathEngine_MVVM.Engine.Math;
using Developers_MathEngine_MVVM.Interpreter.Lexer;
using Developers_MathEngine_MVVM.Interpreter.Token;
using Developers_MathEngine_MVVM.Interpreter.Utilities;
using Developers_MathEngine_MVVM.Model.Formula;
using Developers_MathEngine_MVVM.Model.Project;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Developers_MathEngine_MVVM.Engine.CoreCLR.CoreCLR_API;


namespace Developers_MathEngine_MVVM.Interpreter.Parser
{
    public class Parser_Math
    {
        //use parse: this is for graphing
        public static async Task<List<string>> parse(List<string> inputs, ScriptGlobals scriptGlobals, string precision)
        {

            Task<List<string>> taskResult = Task.Run(() =>
            {
                return Enumerable.Repeat("", inputs.Count).AsParallel().ToList();
            });

            List<string> result = await Task.Run(async () =>
            {
                List<string> Result = taskResult.Result;
                Parallel.For(0, inputs.Count, async i =>
                //for (int i = 0; i < inputs.Count; i++)
                {
                    List<Token.Token> lexer_List = Lexer.Lexer_Math.lex(inputs[i], 0);


                    Result[i] = await parse(lexer_List, scriptGlobals, precision);
                    //}
                    //}
                });
                return Result;
            });
            return result;

        }


        //public static async Task<string> parse(string inputs, ScriptGlobals scriptGlobals)
        //{
        //    string result = "";
        //    List<Token.Token> lexer_List = Lexer.Lexer_Math.lex(inputs, 0);

        //    List<Token.Token> lexerParseButtonFunc_List_Result = await parseEvaluateButtons(lexer_List, scriptGlobals);
        //    List<Token.Token> parsedButtonVar = setVariableToken_FromCustomButton(lexer_List, scriptGlobals);
        //    List<Token.Token> parsedFormulaVar = setVariableToken_FromFormula(parsedButtonVar, scriptGlobals);
        //    List<Token.Token> lexerParsePar_List_Result = parseParToken(parsedFormulaVar);
        //    int parseIndex = 0;
        //    if (lexerParsePar_List_Result.FirstOrDefault(x => x.tokenType == TokenType.STRING || x.tokenType == TokenType.FUNCTION) != null)
        //    {
        //        //Dictionary<string,Model.Formula.Variable> varDictionary = new Dictionary<string,Model.Formula.Variable>();
        //        foreach (Token.Token token in lexerParsePar_List_Result)
        //        {
        //            result += token.value;
        //        }
        //    }
        //    else if (lexerParsePar_List_Result.Count > 0)
        //    {
        //        result = Utilities.Utilities_Parser_Math.basicParse(lexerParsePar_List_Result, ref parseIndex).value;
        //    }
        //    //}
        //    return result;
        //}

        public static async Task<string> parse(List<Token.Token> lexer_List, ScriptGlobals scriptGlobals, string precision)
        {

            string result = "";

            List<Token.Token> lexerParseButtonFunc_List_Result = await parseEvaluateButtons(lexer_List, scriptGlobals, precision);
            //List<Token.Token> parsedButtonVar_FromFormula = setVariableToken_FromFormula(lexerParseButtonFunc_List_Result,scriptGlobals);
            List<Token.Token> parsedButtonVar_FromButton = await setVariableToken_FromCustomButton(lexerParseButtonFunc_List_Result, scriptGlobals);




            List<Token.Token> lexerParsePar_List_Result = parseParToken(parsedButtonVar_FromButton, precision);
            int parseIndex = 0;
            if (lexerParsePar_List_Result.FirstOrDefault(x => x.tokenType == TokenType.STRING || x.tokenType == TokenType.FUNCTION) != null)
            {
                //List<Token.Token> lexerParseNonPar_List_Result = Lexer_Math.lex(Utilities.Utilities_Parser_Math.basicParse(lexerParsePar_List_Result, ref parseIndex, precision).value, 0);

                foreach (Token.Token token in lexerParsePar_List_Result)
                {
                    result += token.value;
                }
                //scriptGlobals.Formula.isSolvingNestedFunc = true;
            }
            else
            {
                //List<Token.Token> lexerParsePar_List_Result = parseParToken(parsedButtonVar_FromButton, precision);
                result = Utilities.Utilities_Parser_Math.basicParse(lexerParsePar_List_Result, ref parseIndex, precision).value;
            }
            //}
            if (scriptGlobals.Formula.isSolvingNestedFunc)
            {
                scriptGlobals.Formula.isSolvingNestedFunc = false;
                return result;
            }
            else
            {
                //scriptGlobals.Formula.beginSolve = true;
                return result;
            }
        }

        //public static async Task<string> parse(List<Token.Token> lexer_List, CoreCLR_API.ScriptGlobals scriptGlobals, ObservableCollection<RuntimeVariable> RuntimeVariable_Collection = null, string functionName = "", Dictionary<string, Developers_MathEngine_MVVM.Model.Formula.Variable> variableDictionary = null)
        //{
        //    string result = "";
        //    List<Token.Token> lexerParseButtonFunc_List_Result = await parseEvaluateButtons(lexer_List, scriptGlobals, RuntimeVariable_Collection, variableDictionary);
        //    List<Token.Token> lexerParsePar_List_Result = parseParToken(lexerParseButtonFunc_List_Result);
        //    int parseIndex = 0;
        //    result = Utilities.Utilities_Parser_Math.basicParse(lexerParsePar_List_Result, ref parseIndex).value;
        //    //}
        //    return result;
        //}


        private static List<Token.Token> parseParToken(List<Token.Token> _lexer_List, string precision)
        {
            List<Token.Token> lexer_List = new List<Token.Token>(_lexer_List);
            int parseIndex = 0;
            //if (_lexer_List.Count == 1&& _lexer_List[0].value.StartsWith('-'))
            //{
            //    List<Token.Token> tokenList = Lexer_Math.lex(_lexer_List[0].value,0);
            //    _lexer_List.RemoveAt(0);
            //    _lexer_List.InsertRange(0, tokenList);
            //}

            if (lexer_List.Any(x => x.value == "(" && x.tokenType == Token.TokenType.LPAREN))
            {



                List<(string functionName, int startIndex_ParFunc, int endIndex_ParFunc, List<Token.Token> parExpression_Tokens)> result_GetPar = Utilities_Parser_Math.getParentParFunc(lexer_List);


                for (int i = 0; i < result_GetPar.Count; i++)
                {

                    List<Token.Token> parExpression_Tokens_RecursiveResult = parseParToken(result_GetPar[i].parExpression_Tokens, precision);
                    //int j = 0;
                    //while (j < parExpression_Tokens_RecursiveResult.Count)
                    //{
                    //    if (parExpression_Tokens_RecursiveResult[j].value.Contains("."))
                    //    {
                    //        parExpression_Tokens_RecursiveResult.RemoveAt(j);
                    //        parExpression_Tokens_RecursiveResult.InsertRange(j, Lexer_Math.lex(parExpression_Tokens_RecursiveResult[j].value, 0));
                    //    }
                    //    j++;
                    //}

                    parseIndex = 0;
                    Token.Token parsedToken = Utilities.Utilities_Parser_Math.basicParse(parExpression_Tokens_RecursiveResult, ref parseIndex, precision);
                    //List<Token.Token> tokenList = null;
                    //if (parsedToken.value.StartsWith('-')) { tokenList = Lexer_Math.lex(parsedToken.value,0); }
                    //if (tokenList != null)
                    //{
                    //    lexer_List.RemoveRange(result_GetPar[i].startIndex_ParFunc, result_GetPar[i].endIndex_ParFunc + 1 - result_GetPar[i].startIndex_ParFunc);
                    //    lexer_List.InsertRange(result_GetPar[i].startIndex_ParFunc, tokenList);
                    //}
                    /*else */
                    if (parsedToken.tokenType != Token.TokenType.STRING)
                    {
                        lexer_List.RemoveRange(result_GetPar[i].startIndex_ParFunc, result_GetPar[i].endIndex_ParFunc + 1 - result_GetPar[i].startIndex_ParFunc);
                        lexer_List.Insert(result_GetPar[i].startIndex_ParFunc, parsedToken);
                    }
                }


            }

            return lexer_List;
        }


        public static async Task<List<Token.Token>> parseEvaluateButtons(List<Token.Token> lexer_List, ScriptGlobals scriptGlobals, string precision)
        {
            //for(int i = 0;i< lexer_List.Count;i++)
            int lParen = 0;
            int rParen = 0;
            string buttonName = "";
            string buttonValue = "";
            int buttonIndex = 0;
            int i = 0;
            while (i < lexer_List.Count)
            {
                if (lexer_List[i].tokenType == TokenType.FUNCTION)
                {
                    buttonName = lexer_List[i].value;
                    string buttonName_New = buttonName.Remove(buttonName.LastIndexOf('('));
                    buttonIndex = i;
                    lParen++;
                    //i++;

                    int j = i + 1;
                    while (j < lexer_List.Count)
                    {
                        if (lexer_List[j].tokenType == TokenType.FUNCTION)
                        {
                            lParen++;
                        }
                        //else if (lexer_List[j].tokenType == TokenType.STRING)
                        //{
                        //    //TODO: set variable from formula varlist

                        //}
                        else if (lexer_List[j].tokenType == TokenType.LPAREN)
                        {
                            lParen++;
                        }
                        else if (lexer_List[j].tokenType == TokenType.RPAREN)
                        {
                            rParen++;
                        }
                        if ((rParen != 0 && lParen != 0 && rParen - lParen == 0))
                        {

                            rParen = 0;
                            lParen = 0;

                            //List<Token.Token> parsedButtonVar_FromButton = setVariableToken_FromCustomButton(lexer_List.GetRange(buttonIndex + 1, j - buttonIndex - 1), buttonName_New, scriptGlobals);

                            //foreach (var v in parsedButtonVar_FromButton)
                            //{
                            //    //lexVal.Add(v);
                            //    buttonValue += v.value;
                            //}

                            foreach (var v in lexer_List.GetRange(buttonIndex + 1, j - buttonIndex - 1))
                            {
                                //lexVal.Add(v);
                                buttonValue += v.value;
                            }

                            List<Token.Token> buttonToChk = await evaluateButton(buttonName_New, buttonValue, scriptGlobals, precision);


                            if (buttonToChk != null)
                            {
                                //List<Token.Token> lexVal = new List<Token.Token>();
                                lexer_List.RemoveRange(buttonIndex, j - buttonIndex + 1);

                                //int q = 0;
                                //while (q < buttonToChk.Count)
                                //{

                                //    if (buttonToChk[q].value.StartsWith('-'))
                                //    {
                                //        List<Token.Token> lexList = Lexer_Math.lex(buttonToChk[q].value, 0);
                                //        buttonToChk.RemoveAt(q);
                                //        buttonToChk.InsertRange(q, lexer_List);
                                //    }
                                //    lexer_List.Insert(buttonIndex, buttonToChk[q]);
                                //    buttonIndex++;
                                //    q++;
                                //}

                                foreach (Token.Token v in buttonToChk)
                                {
                                    lexer_List.Insert(buttonIndex, v);
                                    buttonIndex++;
                                }
                                buttonValue = "";
                            }
                            else
                            {
                                i++;
                            }
                            //i--;
                            break;
                        }
                        j++;
                    }
                }
                else
                {
                    i++;
                }
            }
            return lexer_List;
        }


        private static void addToUseDictionary(string buttonName, ScriptGlobals scriptGlobals)
        {

            foreach (Model.Formula.Variable variable in CustomButtonTab.customButtons_Dictionary[buttonName].Variable_Collection)
            {
                if (!variable.VariableValue.Equals(variable.VariableName) &&
                    !scriptGlobals.CustomButtonUse_TokensArg_Dictionary.ContainsKey(variable.VariableName)
                    )
                {
                    scriptGlobals.CustomButtonUse_TokensArg_Dictionary.Add(variable.VariableName, Lexer_Math.lex(variable.VariableValue, 0));
                }
                else if (!variable.VariableValue.Equals(variable.VariableName) &&
                    scriptGlobals.CustomButtonUse_TokensArg_Dictionary.ContainsKey(variable.VariableName)
                    )
                {
                    scriptGlobals.CustomButtonUse_TokensArg_Dictionary[variable.VariableName] = Lexer_Math.lex(variable.VariableValue, 0);
                }
            }
        }

        public static async Task<List<Token.Token>> evaluateButton(string buttonName, string buttonValue, ScriptGlobals scriptGlobals, string precision,
            List<(int nestedIndex, Dictionary<int, HashSet<string>> runtimeVar)> CustomButtonRuntimeVariable_TokensArg_Dictionary_Arg = null)
        {
            if (!CustomButtonTab.customButtons_Dictionary.ContainsKey(buttonName))
            {
                return null;
            }

            //scriptGlobals.CustomButtonUse_TokensArg_Dictionary.Clear();
            //scriptGlobals.CustomButtonRuntimeVariable_TokensArg_Dictionary.Clear();
            string use = CustomButtonTab.customButtons_Dictionary[buttonName].Use;

            List<Token.Token> tokenList_Use = Lexer_Math.lex(use, 0, false);
            List<Token.Token> tokenList_buttonValue = Lexer_Math.lex(buttonValue, 0, false);
            OrderedDictionary<string, Token.Token> useOrderedDictionary = Parser_Math.getCustomButtonUseToken_ArgDictionary(tokenList_Use);
            //split to dictionary
            (bool resultContainsFunction, OrderedDictionary<string, List<Token.Token>> result) useDictionary =
                Parser_Math.setCustomButtonUseToken_Dictionary(buttonName, tokenList_buttonValue, useOrderedDictionary);

            //CustomButtonTab.customButtons_Dictionary[buttonName].setRuntimeVarDicitonary_ToOtherDictionary(ref scriptGlobals.CustomButtonRuntimeVariable_TokensArg_Dictionary);
            //set var
            //asdasdasdasd
            //how to pass before custom button evaluation
            ScriptGlobals newScriptGlobals = null;
            if (useDictionary.resultContainsFunction)
            {
                //foreach (var v in useDictionary.result) {
                int i = 0;
                while (i < useDictionary.result.Count)
                {
                    if (useDictionary.result[useDictionary.result.ElementAt(i).Key].FirstOrDefault(x => x.tokenType == TokenType.FUNCTION) != null)
                    {
                        //useDictionary.result[useDictionary.result.ElementAt(i).Key] = Lexer_Math.lex(await parse(useDictionary.result[useDictionary.result.ElementAt(i).Key], scriptGlobals), 0);
                        useDictionary.result[useDictionary.result.ElementAt(i).Key] = await parseEvaluateButtons(useDictionary.result[useDictionary.result.ElementAt(i).Key], scriptGlobals, precision);

                    }
                    else
                    {
                        i++;
                    }
                }
                addToUseDictionary(buttonName, scriptGlobals);
                //scriptGlobals.CustomButtonUse_TokensArg_Dictionary = new OrderedDictionary<string, List<Token.Token>>(useDictionary.result);
            }
            //if (scriptGlobals.Formula.setUseOnce)
            //{
            //    scriptGlobals.CustomButtonUse_TokensArg_Dictionary.Clear();
            //    scriptGlobals.Formula.setUseOnce = true;
            //    //foreach (KeyValuePair<string, List<Token.Token>> token in useDictionary.result)
            //    int i = 0;
            //    while (i < useDictionary.result.Count)
            //    {
            //        scriptGlobals.CustomButtonUse_TokensArg_Dictionary.Add(useDictionary.result.ElementAt(i).Key, Lexer_Math.lex(await parse(useDictionary.result[useDictionary.result.ElementAt(i).Key], scriptGlobals, precision), 0));

            //        //foreach (var v in useDictionary.result[token.Key])
            //        //{
            //        //    scriptGlobals.CustomButtonUse_TokensArg_Dictionary[token.Key].Add(new Token.Token() { value = v.value, tokenType = v.tokenType });
            //        //}
            //        i++;
            //    }
            //    addToUseDictionary(buttonName, scriptGlobals);
            //    //scriptGlobals.CustomButtonUse_TokensArg_Dictionary = new OrderedDictionary<string, List<Token.Token>>(useDictionary.result);
            //}
            //else

            if (scriptGlobals.Formula.setUseOnce)
            {
                newScriptGlobals = new ScriptGlobals(scriptGlobals.Formula);

                int i = 0;
                while (i < useDictionary.result.Count)
                {
                    if (!newScriptGlobals.CustomButtonUse_TokensArg_Dictionary.ContainsKey(useDictionary.result.ElementAt(i).Key))
                    {
                        newScriptGlobals.CustomButtonUse_TokensArg_Dictionary.Add(useDictionary.result.ElementAt(i).Key, Lexer_Math.lex(await parse(useDictionary.result[useDictionary.result.ElementAt(i).Key], scriptGlobals, precision), 0));

                    }
                    else
                    {
                        newScriptGlobals.CustomButtonUse_TokensArg_Dictionary[useDictionary.result.ElementAt(i).Key].Clear();
                        newScriptGlobals.CustomButtonUse_TokensArg_Dictionary[useDictionary.result.ElementAt(i).Key] = Lexer_Math.lex(await parse(useDictionary.result[useDictionary.result.ElementAt(i).Key], scriptGlobals, precision), 0);


                    }
                    i++;
                }
                addToUseDictionary(buttonName, newScriptGlobals);

            }
            if (newScriptGlobals != null)
            {
                //TODO: Globals and LocalScripts
                ScriptState<object> scriptState1 = null;
                if (!string.IsNullOrEmpty(CustomButtonTab.customButtons_Dictionary[buttonName].VariableExec_Script.Script))
                {

                    setCustomButtons(scriptGlobals, buttonName, precision).Wait();
                    try
                    {
                        scriptState1 = await CoreCLR.EvaluateScriptAsync(CustomButtonTab.customButtons_Dictionary[buttonName].VariableExec_Script.Script, new List<string>(), newScriptGlobals, scriptGlobals.ScriptState);
                        scriptState1 = await CoreCLR.EvaluateScriptAsync(CustomButtonTab.customButtons_Dictionary[buttonName].FormulaExec_Script.Script, new List<string>(), newScriptGlobals, scriptState1);
                    }
                    catch { }
                }
                else
                {

                    setCustomButtons(scriptGlobals, buttonName, precision).Wait();
                    try
                    {
                        scriptState1 = await CoreCLR.EvaluateScriptAsync(CustomButtonTab.customButtons_Dictionary[buttonName].FormulaExec_Script.Script, new List<string>(), newScriptGlobals, scriptState1);
                    }
                    catch { }
                }
                return Lexer_Math.lex((string)scriptState1.ReturnValue, 0, true, true);
            }
            else
            {
                scriptGlobals.CustomButtonUse_TokensArg_Dictionary = new OrderedDictionary<string, List<Token.Token>>();
                scriptGlobals.Formula.setUseOnce = true;

                int i = 0;
                while (i < useDictionary.result.Count)
                {
                    scriptGlobals.CustomButtonUse_TokensArg_Dictionary.Add(useDictionary.result.ElementAt(i).Key, Lexer_Math.lex(await parse(useDictionary.result[useDictionary.result.ElementAt(i).Key], scriptGlobals, precision), 0));

                    i++;
                }
                addToUseDictionary(buttonName, scriptGlobals);
                //TODO: Globals and LocalScripts
                ScriptState<object> scriptState2 = null;

                if (!string.IsNullOrEmpty(CustomButtonTab.customButtons_Dictionary[buttonName].VariableExec_Script.Script))
                {

                    setCustomButtons(scriptGlobals, buttonName, precision).Wait();
                    try
                    {
                        scriptState2 = await CoreCLR.EvaluateScriptAsync(CustomButtonTab.customButtons_Dictionary[buttonName].VariableExec_Script.Script, new List<string>(), scriptGlobals, scriptGlobals.ScriptState);
                        scriptState2 = await CoreCLR.EvaluateScriptAsync(CustomButtonTab.customButtons_Dictionary[buttonName].FormulaExec_Script.Script, new List<string>(), scriptGlobals, scriptState2);
                    }
                    catch { }
                }
                else
                {

                    setCustomButtons(scriptGlobals, buttonName, precision).Wait();
                    try
                    {
                        scriptState2 = await CoreCLR.EvaluateScriptAsync(CustomButtonTab.customButtons_Dictionary[buttonName].FormulaExec_Script.Script, new List<string>(), scriptGlobals, scriptState2);
                    }
                    catch { }
                }
                //scriptGlobals.Formula.setUseOnce = false;
                return Lexer_Math.lex((string)scriptState2.ReturnValue, 0, true, true);
            }



        }


        private static async Task setCustomButtons(ScriptGlobals scriptGlobals, string buttonName, string precision)
        {

            if (scriptGlobals.CustomButton == null) scriptGlobals.CustomButton = new CustomButton();
            if (scriptGlobals.CustomButton.Variable_Collection == null) scriptGlobals.CustomButton.Variable_Collection = new ObservableCollection<Model.Formula.Variable>();

            else
            {

                scriptGlobals.CustomButton.Variable_Collection.Clear();

            }
            if (scriptGlobals.CustomButton.Rules_Collection == null) scriptGlobals.CustomButton.Rules_Collection = new ObservableCollection<Rule>();

            else
            {

                scriptGlobals.CustomButton.Rules_Collection.Clear();

            }
            //scriptGlobals.CustomButton.FormulaName = buttonName;

            for (int i = 0; i < scriptGlobals.CustomButtonUse_TokensArg_Dictionary.Count; i++)
            {
                scriptGlobals.CustomButton.Variable_Collection.Add(
                new Model.Formula.Variable()
                {
                    VariableName = scriptGlobals.CustomButtonUse_TokensArg_Dictionary.ElementAt(i).Key,
                    VariableValue = await parse(scriptGlobals.CustomButtonUse_TokensArg_Dictionary.ElementAt(i).Value, scriptGlobals, precision)
                });
            }

            if (CustomButtonTab.customButtons_Dictionary.ContainsKey(buttonName))
            {
                if (scriptGlobals.CustomButton.VariableExec_Script == null) scriptGlobals.CustomButton.VariableExec_Script = new Model.Script.Scripting()
                {
                    ScriptName = "ExecVariable",
                    Script = CustomButtonTab.customButtons_Dictionary[buttonName].VariableExec_Script.Script
                };
                //scriptGlobals.CustomButton.VariableExec_Script.Script = CustomButtonTab.customButtons_Dictionary[buttonName].VariableExec_Script.Script;
                for (int i = 0; i < CustomButtonTab.customButtons_Dictionary[buttonName].Rules_Collection.Count; i++)
                {
                    scriptGlobals.CustomButton.Rules_Collection.Add(
                    new Model.Formula.Rule()
                    {
                        ruleIndex = i.ToString(),
                        codeRule = CustomButtonTab.customButtons_Dictionary[buttonName].Rules_Collection[i].codeRule
                    });

                }
            }
            //return scriptGlobals;
        }
















        public static List<Token.Token> setVariableToken_FromFormula(List<Token.Token> _lexer_List, ScriptGlobals scriptGlobals)
        {
            //for (int i = _lexer_List.Count - 1; i >= 0; i--)
            int i = 0;
            while (i < _lexer_List.Count)
            {
                if (_lexer_List[i].tokenType == TokenType.STRING)
                {
                    if (scriptGlobals.Formula.Variable_Dictionary.ContainsKey(_lexer_List[i].value))
                    {
                        List<Token.Token> lexedResult = Lexer_Math.lex(scriptGlobals.Formula.Variable_Dictionary[_lexer_List[i].value].VariableValue, 0);
                        _lexer_List.RemoveAt(i);
                        _lexer_List.InsertRange(i, lexedResult);
                        i += lexedResult.Count;
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
            return _lexer_List;
        }




        public static async Task<List<Token.Token>> setVariableToken_FromCustomButton(List<Token.Token> _lexer_List, ScriptGlobals scriptGlobals)
        {
            //List<Token.Token> result = new List<Token.Token>();
            int lParen = 0;
            int rParen = 0;
            int columnCount = 0;

            StringBuilder funcName = new StringBuilder();
            for (int i = _lexer_List.Count - 1; i >= 0; i--)
            {
                if (_lexer_List[i].tokenType == TokenType.STRING && funcName.Length == 0)
                {
                    //if (scriptGlobals.CustomButtonRuntimeVariable_TokensArg_Dictionary.Count == 0 ||
                    //    (scriptGlobals.CustomButtonRuntimeVariable_TokensArg_Dictionary.ContainsKey(columnCount) &&
                    //    !scriptGlobals.CustomButtonRuntimeVariable_TokensArg_Dictionary[columnCount].Contains(_lexer_List[i].value)))
                    //{
                    if (CustomButtonTab.customButtons_Dictionary.ContainsKey(_lexer_List[i].value) &&
                        !CustomButtonTab.customButtons_Dictionary[_lexer_List[i].value].Use.EndsWith(')'))
                    {
                        try
                        {
                            ScriptState<object> scriptState = await CoreCLR.EvaluateScriptAsync(CustomButtonTab.customButtons_Dictionary[_lexer_List[i].value].VariableExec_Script.Script, new List<string>(), scriptGlobals);
                            scriptState = await CoreCLR.EvaluateScriptAsync(CustomButtonTab.customButtons_Dictionary[_lexer_List[i].value].FormulaExec_Script.Script, new List<string>(), scriptGlobals, scriptState);

                            string tokenVal = (string)scriptState.ReturnValue;

                            TokenType tokenType = TokenType.NUMBER;
                            foreach (Token.Token quickLexToken in Lexer_Math.lex((string)scriptState.ReturnValue, 0))
                            {
                                if (quickLexToken.tokenType == TokenType.STRING || quickLexToken.tokenType == TokenType.FUNCTION)
                                {
                                    tokenType = TokenType.STRING;
                                    break;
                                }
                            }
                            if (!string.IsNullOrEmpty(tokenVal))
                            {
                                _lexer_List[i].tokenType = tokenType;
                                _lexer_List[i].value = tokenVal;
                            }
                        }
                        catch { }
                    }
                    else if (scriptGlobals.CustomButtonUse_TokensArg_Dictionary.ContainsKey(_lexer_List[i].value))
                    {
                        //string varName = _lexer_List[i].value;
                        //_lexer_List[i].value = "";
                        //_lexer_List[i].tokenType = TokenType.NONE;
                        string tokenVal = "";
                        foreach (Token.Token token in scriptGlobals.CustomButtonUse_TokensArg_Dictionary[_lexer_List[i].value])
                        {
                            tokenVal += token.value;
                        }

                        TokenType tokenType = TokenType.NUMBER;
                        foreach (Token.Token quickLexToken in Lexer_Math.lex(tokenVal, 0))
                        {
                            if (quickLexToken.tokenType == TokenType.STRING || quickLexToken.tokenType == TokenType.FUNCTION)
                            {
                                tokenType = TokenType.STRING;
                                break;
                            }
                        }
                        if (!string.IsNullOrEmpty(tokenVal))
                        {
                            _lexer_List[i].tokenType = tokenType;
                            _lexer_List[i].value = tokenVal;
                        }
                        //_lexer_List[i].value = scriptGlobals.CustomButtonUse_TokensArg_Dictionary[_lexer_List[i].value][0].value;
                    }
                    //else if (!string.IsNullOrEmpty(buttonName) &&
                    //    CustomButtonTab.customButtons_Dictionary.ContainsKey(buttonName))
                    //{
                    //    string varName = _lexer_List[i].value;
                    //    foreach (Model.Formula.Variable variable in
                    //        CustomButtonTab.customButtons_Dictionary[buttonName].Variable_Collection)
                    //    {
                    //        if (
                    //            variable.VariableName.Equals(varName) &&
                    //            !variable.VariableValue.Equals(varName) &&
                    //            !scriptGlobals.CustomButtonUse_TokensArg_Dictionary.ContainsKey(varName))
                    //        {
                    //            _lexer_List[i].value = variable.VariableValue;
                    //            break;
                    //        }
                    //    }
                    //}
                    //}
                }
                if (_lexer_List[i].tokenType == TokenType.FUNCTION)
                {
                    funcName.Clear();
                    funcName.Append(_lexer_List[i].value.Remove(_lexer_List[i].value.LastIndexOf("(")));
                    lParen++;
                    for (int j = i; j < _lexer_List.Count; j++)
                    {
                        if (_lexer_List[j].tokenType == TokenType.LPAREN)
                        {
                            lParen++;
                        }
                        else if (_lexer_List[j].tokenType == TokenType.RPAREN)
                        {
                            rParen++;
                        }
                        if ((rParen != 0 && lParen != 0 && rParen - lParen == 0) || _lexer_List[j].tokenType == TokenType.COMMA)
                        {
                            columnCount++;
                            //result.Add(_lexer_List[j - 1]);
                        }
                        if (rParen - lParen == 0)
                        {
                            if (lParen != 0)
                            {
                                lParen = 0;
                                rParen = 0;
                                columnCount = 0;
                                //i = j;
                                funcName.Clear();
                                break;
                            }
                        }

                        if (_lexer_List[j].tokenType == TokenType.STRING)
                        {
                            //if (scriptGlobals.CustomButtonRuntimeVariable_TokensArg_Dictionary.Count == 0 ||
                            //    (scriptGlobals.CustomButtonRuntimeVariable_TokensArg_Dictionary.ContainsKey(columnCount) &&
                            //    !scriptGlobals.CustomButtonRuntimeVariable_TokensArg_Dictionary[columnCount].Contains(_lexer_List[j].value)))
                            //{
                            if (scriptGlobals.CustomButtonUse_TokensArg_Dictionary.ContainsKey(_lexer_List[j].value))
                            {
                                string varName = _lexer_List[i].value;
                                _lexer_List[i].value = "";
                                _lexer_List[i].tokenType = TokenType.NONE;
                                foreach (Token.Token token in scriptGlobals.CustomButtonUse_TokensArg_Dictionary[varName])
                                {
                                    _lexer_List[j].value += token.value;
                                }
                                //_lexer_List[i].value = scriptGlobals.CustomButtonUse_TokensArg_Dictionary[_lexer_List[i].value][0].value;
                            }
                            //else if (CustomButtonTab.customButtons_Dictionary.ContainsKey(funcName.ToString()))
                            //{
                            //    string varName = _lexer_List[i].value;
                            //    foreach (Model.Formula.Variable variable in
                            //        CustomButtonTab.customButtons_Dictionary[funcName.ToString()].Variable_Collection)
                            //    {
                            //        if(
                            //            variable.VariableName.Equals(varName) &&
                            //            !variable.VariableValue.Equals(varName) &&
                            //            !scriptGlobals.CustomButtonUse_TokensArg_Dictionary.ContainsKey(varName))
                            //        {
                            //            _lexer_List[i].value = variable.VariableValue;
                            //        }
                            //    }
                            //}
                            //}
                        }
                    }
                }

            }
            int q = _lexer_List.Count - 1;
            while (q >= 0)
            {
                if (_lexer_List[q].tokenType == TokenType.NONE)
                {
                    List<Token.Token> lexedVal = Lexer_Math.lex(_lexer_List[q].value, 0);
                    _lexer_List.RemoveAt(q);
                    _lexer_List.InsertRange(q, lexedVal);
                }
                q--;
            }

            return _lexer_List;
        }










        //public static bool getVariableToken(List<Token.Token> _lexer_List)
        //{
        //    bool isGetVariable = false;
        //    for (int i = 0; i < _lexer_List.Count; i++)
        //    {
        //        if (_lexer_List[i].tokenType == Token.TokenType.STRING)
        //        {
        //            if (!MainProject.selectedFormulaProject.selectedFormula.Variable_Dictionary.ContainsKey(_lexer_List[i].value))
        //            {
        //                isGetVariable = true;
        //                MainProject.selectedFormulaProject.selectedFormula.Variable_Collection.Add(
        //                    new Developers_MathEngine_MVVM.Model.Formula.Variable()
        //                    {
        //                        VariableName = _lexer_List[i].value
        //                    });
        //                MainProject.selectedFormulaProject.selectedFormula.Variable_Dictionary.Add(_lexer_List[i].value,
        //                new Developers_MathEngine_MVVM.Model.Formula.Variable()
        //                {
        //                    VariableName = _lexer_List[i].value
        //                });
        //            }

        //            foreach (Developers_MathEngine_MVVM.Model.Formula.Variable colData in MainProject.selectedFormulaProject.selectedFormula.Variable_Collection)
        //            {
        //                if (!MainProject.selectedFormulaProject.selectedFormula.Variable_Dictionary.ContainsKey(colData.VariableName))
        //                {
        //                    MainProject.selectedFormulaProject.selectedFormula.Variable_Collection.Remove(colData);
        //                }
        //            }
        //        }
        //    }
        //    return isGetVariable;
        //}
















        //simple get Use Token Only
        public static OrderedDictionary<string, Token.Token> getCustomButtonUseToken_ArgDictionary(List<Token.Token> _lexer_List)
        {
            OrderedDictionary<string, Token.Token> result = new OrderedDictionary<string, Token.Token>();
            int lParen = 0;
            int rParen = 0;

            StringBuilder funcName = new StringBuilder();

            for (int i = _lexer_List.Count - 1; i >= 0; i--)
            {
                if (_lexer_List[i].tokenType == TokenType.FUNCTION)
                {
                    funcName.Append(_lexer_List[i].value.Remove(_lexer_List[i].value.LastIndexOf('(')));
                    lParen++;
                    for (int j = i; j < _lexer_List.Count; j++)
                    {
                        if (_lexer_List[j].tokenType == TokenType.LPAREN)
                        {
                            lParen++;
                        }
                        else if (_lexer_List[j].tokenType == TokenType.RPAREN)
                        {
                            rParen++;
                        }
                        if ((rParen != 0 && lParen != 0 && rParen - lParen == 0) || _lexer_List[j].tokenType == TokenType.COMMA)
                        {
                            result.Add(_lexer_List[j - 1].value, _lexer_List[j - 1]);
                            if (rParen != 0 && lParen != 0 && rParen - lParen == 0)
                            {
                                rParen = 0;
                                lParen = 0;
                                break;
                            }
                        }
                    }
                }

            }
            return result;
        }


        public static (bool resultContainsFunction, OrderedDictionary<string, List<Token.Token>> result) setCustomButtonUseToken_Dictionary(string buttonName, List<Token.Token> _lexer_List_ButtonValue, OrderedDictionary<string, Token.Token> useOrderedDictionary)
        {
            (bool resultContainsFunction, OrderedDictionary<string, List<Token.Token>> result) result = (false, new OrderedDictionary<string, List<Token.Token>>());
            foreach (var v in useOrderedDictionary)
            {
                result.result.Add(v.Key, new List<Token.Token>());

            }
            if (_lexer_List_ButtonValue == null) return result;

            int nestedFunc = 0;
            List<int> lParen = new List<int>();
            List<int> rParen = new List<int>();

            //StringBuilder funcName = new StringBuilder();
            //funcName.Append(buttonName);
            //int funcNameIndex = 0;
            //int lastCommaIndex = 0;
            int column = 0;
            //get only comma
            for (int i = 0; i < _lexer_List_ButtonValue.Count; i++)
            {
                if (_lexer_List_ButtonValue[i].tokenType == TokenType.FUNCTION)
                {
                    result.resultContainsFunction = true;
                    //if (!string.IsNullOrEmpty(funcName.ToString()))
                    //{
                    //    nestedFunc++;
                    //}
                    //else
                    //{
                    //    //funcNameIndex = i + 1;
                    //    funcName = funcName.Append(_lexer_List_ButtonValue[i].value.Remove(_lexer_List_ButtonValue[i].value.LastIndexOf('(')));
                    //}
                    if (lParen.Count <= nestedFunc)
                    {
                        lParen.Add(1);
                        rParen.Add(0);
                        if (result.result.ContainsKey(result.result.ElementAt(column).Key))
                        {
                            result.result[result.result.ElementAt(column).Key].Add(_lexer_List_ButtonValue[i]);
                        }
                        continue;
                    }
                    else
                    {
                        lParen[nestedFunc]++;
                    }

                }
                if (result.result.ContainsKey(result.result.ElementAt(column).Key))
                {
                    result.result[result.result.ElementAt(column).Key].Add(_lexer_List_ButtonValue[i]);
                }
                if (lParen.Count > 0)
                {
                    if (_lexer_List_ButtonValue[i].tokenType == TokenType.LPAREN)
                    {
                        lParen[nestedFunc]++;
                    }
                    else if (_lexer_List_ButtonValue[i].tokenType == TokenType.RPAREN)
                    {
                        rParen[nestedFunc]++;
                    }
                    if (nestedFunc != 0 && rParen[nestedFunc] - lParen[nestedFunc] == 0)
                    {
                        nestedFunc--;
                    }
                    else if (nestedFunc == 0 && rParen.Count > 0 && lParen.Count > 0 && ((rParen[nestedFunc] != 0 && lParen[nestedFunc] != 0 && rParen[nestedFunc] - lParen[nestedFunc] == 0) || _lexer_List_ButtonValue[i].tokenType == TokenType.COMMA))
                    {


                        if (rParen[nestedFunc] - lParen[nestedFunc] == 0)
                        {
                            nestedFunc = 0;
                            //funcName.Clear();
                            rParen.Clear();
                            lParen.Clear();
                            //column = 0;
                        }
                        //else
                        //{
                        //    column++;
                        //}
                    }
                }
                //else
                //{
                if (rParen.Count <= 0 && lParen.Count <= 0 && _lexer_List_ButtonValue[i].tokenType == TokenType.COMMA)
                {
                    result.result.ElementAt(column).Value.RemoveAt(result.result.ElementAt(column).Value.LastIndexOf(_lexer_List_ButtonValue[i]));
                    //result[result.ElementAt(column).Key] = _lexer_List_ButtonValue.GetRange(lastCommaIndex + 1, i - (lastCommaIndex + 1) - 1);
                    column++;
                    //lastCommaIndex = i;
                }

                //}

            }
            ////get last column par
            //if (lastCommaIndex > 0)
            //    result[result.ElementAt(column).Key] = _lexer_List_ButtonValue.GetRange(lastCommaIndex + 1, _lexer_List_ButtonValue.Count - (lastCommaIndex + 1));
            //else
            //    result[result.ElementAt(column).Key] = _lexer_List_ButtonValue.GetRange(lastCommaIndex, _lexer_List_ButtonValue.Count - lastCommaIndex);

            return result;
        }


        //public static List<Token.Token> parseFunctionToken(List<Token.Token> _lexer_List)
        //{

        //    List<Token.Token> lexer_List = new List<Token.Token>(_lexer_List);
        //    //int parseIndex = 0;
        //    if (lexer_List.Any(x => x.tokenType == Token.TokenType.FUNCTION))
        //    {
        //    }
        //    return null;
        //}




    }
}
