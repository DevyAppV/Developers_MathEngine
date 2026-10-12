using Developers_MathEngine_Library.Interpreter.Token;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.SmartCards;

namespace Developers_MathEngine_Library.Interpreter.Utilities
{
    public class Utilities_Parser_Math
    {
        public static List<(string functionName, int startIndex_ParFunc, int endIndex_ParFunc, List<Token.Token> parExpression_Tokens)> getParentParFunc_Nested(List<Token.Token> lexer_List)
        {
            //Loop, using main parent, see if need substitute var in nest
            //if yes substitute var in nest for current loop calc
            //if not get answer
            //repeat loop for nest
            //
            List<(string functionName, int startIndex_ParFunc, int endIndex_ParFunc, List<Token.Token> parExpression_Tokens)> result = new List<(string functionName, int startIndex_ParFunc, int endIndex_ParFunc, List<Token.Token> parExpression_Tokens)>();
            StringBuilder functionName = new StringBuilder();
            int firstPar = -1;
            int lastPar = -1;

            int firstPar_Count = 0;
            int lastPar_Count = 0;

            int index = lexer_List.Count() - 1;
            while (index >= 0)
            {
                if (lexer_List[index].tokenType.Equals(Token.TokenType.RPAREN))
                {
                    if (lastPar_Count == 0)
                        lastPar = index;
                    lastPar_Count++;
                }
                else if (lexer_List[index].tokenType.Equals(Token.TokenType.LPAREN))
                {
                    firstPar_Count++;

                    if (lastPar_Count - firstPar_Count == 0)
                    {
                        //reset
                        lastPar_Count = 0;
                        firstPar_Count = 0;

                        if (lastPar > -1)
                            firstPar = index;

                        while (index - 1 >= 0)
                        {
                            --index;
                            if (!Utilities_Lexer_Math.isMathOperator(lexer_List[index].value[0]) &&
                                !lexer_List[index].tokenType.Equals(Token.TokenType.LPAREN) &&
                                !lexer_List[index].tokenType.Equals(Token.TokenType.RPAREN) &&
                                !lexer_List[index].tokenType.Equals(Token.TokenType.COMMA))
                            {
                                functionName = functionName.Insert(0, lexer_List[index].value[0]);
                            }
                            else
                            {
                                index = lastPar;
                                break;
                            }
                        }

                        result.Add((functionName.ToString(), firstPar, lastPar, lexer_List.GetRange(firstPar + 1, lastPar - (firstPar + 1))));

                    }
                    else if (lastPar_Count == 0) { firstPar_Count = 0; }
                }
                index--;
            }

            return result;

        }

        //public class ParFuncList
        //{
        //    public (string functionName, int startIndex_ParFunc, int endIndex_ParFunc, List<Token.Token> parExpression_Tokens) parFunc_Result;
        //    public List<ParFuncList> parFunc_InOrder_List;

        //    public ParFuncList getParFunc_InOrder_List_AtNestedIndex(List<int> nestedList_Index)
        //    {
        //        ParFuncList result = null;
        //        foreach (int i in nestedList_Index)
        //        {
        //            result = parFunc_InOrder_List[i];
        //        }
        //        return result;
        //    }

        //    public void setAdd_ParFunc_InOrder_List_AtNestedIndex(
        //        List<ParFuncList> parFunc_InOrder_List_Obj,
        //        List<int> nestedList_Index,
        //        ParFuncList parFunc_ADD,
        //        (string functionName, int startIndex_ParFunc, int endIndex_ParFunc, List<Token.Token> parExpression_Tokens) parFunc_Result_Value_SET,
        //        bool add,
        //        bool set)
        //    {
        //        List<ParFuncList> chk = parFunc_InOrder_List_Obj[nestedList_Index[0]].parFunc_InOrder_List;
        //        nestedList_Index.RemoveAt(0);
        //        if (nestedList_Index.Count == 0)
        //        {
        //            if (add)
        //            {
        //                parFunc_InOrder_List_Obj[nestedList_Index[0]].parFunc_InOrder_List.Add(parFunc_ADD);
        //            }
        //            if (set)
        //            {
        //                parFunc_InOrder_List_Obj[nestedList_Index[0]].parFunc_Result = parFunc_Result_Value_SET;
        //            }
        //        }
        //        else
        //        {
        //            setAdd_ParFunc_InOrder_List_AtNestedIndex(chk, nestedList_Index, parFunc_ADD, parFunc_Result_Value_SET, add, set);
        //        }

        //    }
        //}

        //public static List<(string functionName, int startIndex_ParFunc, int endIndex_ParFunc, List<Token.Token> parExpression_Tokens)> getParentParFunc_ChildPar_InOrder(List<Token.Token> lexer_List, List<int> nestedList_Index)
        //{
        //    //dictionary result array
        //    //add child to parent
        //    //from right to left

        //    List<ParFuncList> ParFunc_List = new List<ParFuncList>();

        //    List<(string functionName, int startIndex_ParFunc, int endIndex_ParFunc, List<Token.Token> parExpression_Tokens)> result_List = getParentParFunc(lexer_List);
        //    //added parent

        //    int count = 0;
        //    if (nestedList_Index == null || nestedList_Index.Count <= 0)
        //    {
        //        nestedList_Index = new List<int>() { 0 };
        //    }
        //    foreach ((string functionName, int startIndex_ParFunc, int endIndex_ParFunc, List<Token.Token> parExpression_Tokens) result in result_List)
        //    {
        //        ParFunc_List.Add(new ParFuncList() { parFunc_Result = result });

        //        if (result.parExpression_Tokens.Any(x => x.value == "(" && x.tokenType == TokenType.LPAREN))
        //        {

        //            nestedList_Index.Add(0);
        //            List<(string functionName, int startIndex_ParFunc, int endIndex_ParFunc, List<Token.Token> parExpression_Tokens)> resultPar_List = getParentParFunc_ChildPar_InOrder(result.parExpression_Tokens, nestedList_Index);
        //            whi
        //            //int childCount = -1;
        //            foreach ((string functionName, int startIndex_ParFunc, int endIndex_ParFunc, List<Token.Token> parExpression_Tokens) result_Child in resultPar_List)
        //            {
        //                ParFunc_List[nestedList_Index[0]].parFunc_InOrder_List.Add(new ParFuncList());

        //                ParFunc_List[nestedList_Index[0]].setAdd_ParFunc_InOrder_List_AtNestedIndex(ParFunc_List[count].parFunc_InOrder_List, nestedList_Index, null, result_Child, false, true);
        //                nestedList_Index[nestedList_Index.Last()]++;
        //            }
        //        }
        //        nestedList_Index.Clear();
        //        count++;
        //        nestedList_Index.Add(count);
        //    }


        //}


        public static List<(string functionName, int startIndex_ParFunc, int endIndex_ParFunc, List<Token.Token> parExpression_Tokens)> getParentParFunc(List<Token.Token> lexer_List)
        {
            //Loop, using main parent, see if need substitute var in nest
            //if yes substitute var in nest for current loop calc
            //if not get answer
            //repeat loop for nest
            //
            List<(string functionName, int startIndex_ParFunc, int endIndex_ParFunc, List<Token.Token> parExpression_Tokens)> result = new List<(string functionName, int startIndex_ParFunc, int endIndex_ParFunc, List<Token.Token> parExpression_Tokens)>();
            StringBuilder functionName = new StringBuilder();
            int firstPar = -1;
            int lastPar = -1;

            int firstPar_Count = 0;
            int lastPar_Count = 0;

            int index = lexer_List.Count() - 1;
            while (index >= 0)
            {
                if (lexer_List[index].tokenType.Equals(Token.TokenType.RPAREN))
                {
                    if (lastPar_Count == 0)
                        lastPar = index;
                    lastPar_Count++;
                }
                else if (lexer_List[index].tokenType.Equals(Token.TokenType.LPAREN))
                {
                    firstPar_Count++;

                    if (lastPar_Count - firstPar_Count == 0)
                    {
                        //reset
                        lastPar_Count = 0;
                        firstPar_Count = 0;

                        if (lastPar > -1)
                            firstPar = index;

                        while (index - 1 >= 0)
                        {
                            --index;
                            if (!Utilities_Lexer_Math.isMathOperator(lexer_List[index].value[0]) &&
                                !lexer_List[index].tokenType.Equals(Token.TokenType.LPAREN) &&
                                !lexer_List[index].tokenType.Equals(Token.TokenType.RPAREN) &&
                                !lexer_List[index].tokenType.Equals(Token.TokenType.COMMA))
                            {
                                functionName = functionName.Insert(0, lexer_List[index].value[0]);
                            }
                            else
                            {
                                ++index;
                                break;
                            }
                        }

                        result.Add((functionName.ToString(), firstPar, lastPar, lexer_List.GetRange(firstPar + 1, lastPar - (firstPar + 1))));

                    }
                }
                index--;
            }

            return result;
        }













































        public static Token.Token basicParse(List<Token.Token> parExpression_LexedTokens, ref int index)
        {
            if (Utilities_String.countStr<Token.Token>(parExpression_LexedTokens) == "1")
            {
                return parExpression_LexedTokens[0];
            }
            else if (Utilities_String.countStr<Token.Token>(parExpression_LexedTokens) == "2")
            {
                if (parExpression_LexedTokens[0].value.Equals("-") && parExpression_LexedTokens[1].tokenType == TokenType.NUMBER)
                    return new Token.Token() { value = "-" + parExpression_LexedTokens[1].value, tokenType = TokenType.NUMBER };
            }
            return parseAddSubtract(ref parExpression_LexedTokens, ref index);

        }


        public static Token.Token parseAddSubtract(ref List<Token.Token> parExpression_LexedTokens, ref int index)
        {

            int tempIndex = index;
            Token.Token token_L = parseDivideMultiply(ref parExpression_LexedTokens, ref index);
            while (index < parExpression_LexedTokens.Count)
            {
                bool isParsedContinue = false;
                if (parExpression_LexedTokens[index].value.Equals("+"))
                {
                    int replaceStart_Index = index - 1;
                    if (token_L != null && token_L.tokenType == TokenType.NUMBER &&
                        parExpression_LexedTokens[index].tokenType == TokenType.PLUS)
                    {
                        index++;
                        Token.Token token_R = parseNumber(parExpression_LexedTokens, ref index);
                        //string solution = (double.Parse(token_L.value) + double.Parse(token_R.value)).ToString();
                        //string solution = (double.Parse(token_L.value) + double.Parse(token_R.value)).ToString();
                        string solution = Utilities_StringMath.evaluate_BasicMath(
                            (TokenType.NUMBER, token_L.value),
                            (TokenType.PLUS, "+"),
                            (TokenType.NUMBER, token_R.value));

                        parExpression_LexedTokens.RemoveRange(replaceStart_Index, index - replaceStart_Index);
                        parExpression_LexedTokens.Insert(replaceStart_Index, new Token.Token() { value = solution, tokenType = TokenType.NUMBER });
                        index = replaceStart_Index;

                        token_L = parseNumber(parExpression_LexedTokens, ref index);
                        isParsedContinue = true;
                    }
                    //return token_L;
                }

                if (index >= parExpression_LexedTokens.Count) break;

                if (parExpression_LexedTokens[index].value.Equals("-"))
                {
                    int replaceStart_Index = index - 1;
                    if (token_L != null && token_L.tokenType == TokenType.NUMBER &&
                        parExpression_LexedTokens[index].tokenType == TokenType.MINUS)
                    {
                        index++;
                        Token.Token token_R = parseNumber(parExpression_LexedTokens, ref index);
                        //string solution = (double.Parse(token_L.value) - double.Parse(token_R.value)).ToString();
                        string solution = Utilities_StringMath.evaluate_BasicMath(
                            (TokenType.NUMBER, token_L.value),
                            (TokenType.MINUS, "-"),
                            (TokenType.NUMBER, token_R.value));

                        parExpression_LexedTokens.RemoveRange(replaceStart_Index, index - replaceStart_Index);
                        parExpression_LexedTokens.Insert(replaceStart_Index, new Token.Token() { value = solution, tokenType = TokenType.NUMBER });
                        index = replaceStart_Index;

                        token_L = parseNumber(parExpression_LexedTokens, ref index);
                        isParsedContinue = true;
                    }
                    //return token_L;
                }
                if (!isParsedContinue)
                {
                    index++;
                    if (index >= parExpression_LexedTokens.Count) break;
                    token_L = parseNumber(parExpression_LexedTokens, ref index);
                }
            }
            index = tempIndex;
            return parseNumber(parExpression_LexedTokens, ref index);
        }

        public static Token.Token parseDivideMultiply(ref List<Token.Token> parExpression_LexedTokens, ref int index)
        {

            int tempIndex = index;
            Token.Token token_L = parseExponent(ref parExpression_LexedTokens, ref index);
            while (index < parExpression_LexedTokens.Count)
            {
                bool isParsedContinue = false;

                if (parExpression_LexedTokens[index].value.Equals("*"))
                {

                    int replaceStart_Index = index - 1;
                    if (token_L != null && token_L.tokenType == TokenType.NUMBER &&
                        parExpression_LexedTokens[index].tokenType == TokenType.MULTIPLY)
                    {
                        index++;
                        Token.Token token_R = parseNumber(parExpression_LexedTokens, ref index);
                        //string solution = (double.Parse(token_L.value) * double.Parse(token_R.value)).ToString();
                        string solution = Utilities_StringMath.evaluate_BasicMath(
                            (TokenType.NUMBER, token_L.value),
                            (TokenType.MULTIPLY, "*"),
                            (TokenType.NUMBER, token_R.value));

                        parExpression_LexedTokens.RemoveRange(replaceStart_Index, index - replaceStart_Index);
                        parExpression_LexedTokens.Insert(replaceStart_Index, new Token.Token() { value = solution, tokenType = TokenType.NUMBER });
                        index = replaceStart_Index;

                        token_L = parseNumber(parExpression_LexedTokens, ref index);
                        isParsedContinue = true;
                    }
                    //return token_L;
                }
                if (index >= parExpression_LexedTokens.Count) break;

                if (parExpression_LexedTokens[index].value.Equals("/"))
                {
                    int replaceStart_Index = index - 1;
                    if (token_L != null && token_L.tokenType == TokenType.NUMBER &&
                        parExpression_LexedTokens[index].tokenType == TokenType.DIVIDE)
                    {
                        index++;
                        Token.Token token_R = parseNumber(parExpression_LexedTokens, ref index);
                        //string solution = (double.Parse(token_L.value) / double.Parse(token_R.value)).ToString();
                        string solution = Utilities_StringMath.evaluate_BasicMath(
                            (TokenType.NUMBER, token_L.value),
                            (TokenType.DIVIDE, "/"),
                            (TokenType.NUMBER, token_R.value));

                        parExpression_LexedTokens.RemoveRange(replaceStart_Index, index - replaceStart_Index);
                        parExpression_LexedTokens.Insert(replaceStart_Index, new Token.Token() { value = solution, tokenType = TokenType.NUMBER });
                        index = replaceStart_Index;

                        token_L = parseNumber(parExpression_LexedTokens, ref index);
                        isParsedContinue = true;
                    }
                    //return token_L;
                }

                if (!isParsedContinue)
                {
                    index++;
                    if (index >= parExpression_LexedTokens.Count) break;
                    token_L = parseNumber(parExpression_LexedTokens, ref index);
                }
            }
            index = tempIndex;
            return parseNumber(parExpression_LexedTokens, ref index);
        }


        public static Token.Token parseExponent(ref List<Token.Token> parExpression_LexedTokens, ref int index)
        {
            int tempIndex = index;
            Token.Token token_L = parseNumber(parExpression_LexedTokens, ref index);
            while (index < parExpression_LexedTokens.Count)
            {
                bool isParsedContinue = false;

                if (parExpression_LexedTokens[index].value.Equals("^"))
                {
                    int replaceStart_Index = index - 1;
                    if (token_L != null && token_L.tokenType == TokenType.NUMBER &&
                        parExpression_LexedTokens[index].tokenType == TokenType.POWER)
                    {
                        index++;
                        Token.Token token_R = parseNumber(parExpression_LexedTokens, ref index);
                        //string solution = Math.Pow(double.Parse(token_L.value), double.Parse(token_R.value)).ToString();
                        string solution = Utilities_StringMath.evaluate_BasicMath(
                            (TokenType.NUMBER, token_L.value),
                            (TokenType.POWER, "^"),
                            (TokenType.NUMBER, token_R.value));

                        parExpression_LexedTokens.RemoveRange(replaceStart_Index, index - replaceStart_Index);
                        parExpression_LexedTokens.Insert(replaceStart_Index, new Token.Token() { value = solution, tokenType = TokenType.NUMBER });
                        index = replaceStart_Index;

                        token_L = parseNumber(parExpression_LexedTokens, ref index);
                        isParsedContinue = true;
                    }
                    //return token_L;
                }

                if (!isParsedContinue)
                {
                    index++;
                    if (index >= parExpression_LexedTokens.Count) break;
                    token_L = parseNumber(parExpression_LexedTokens, ref index);
                }
            }
            index = tempIndex;
            return parseNumber(parExpression_LexedTokens, ref index);
        }


        public static Token.Token parseNumber(List<Token.Token> parExpression_LexedTokens, ref int index)
        {
            bool isNegNum = false;
            int index_Start = index;
            while ((index < parExpression_LexedTokens.Count)
                ||
                (parExpression_LexedTokens[index_Start].tokenType == TokenType.MINUS)

                )
            {
                if (parExpression_LexedTokens[index_Start].tokenType == TokenType.MINUS)
                {
                    isNegNum = true;
                    index++;
                    continue;
                }

                if (parExpression_LexedTokens[index].tokenType == TokenType.NUMBER)
                {
                    if (isNegNum)
                    {
                        index++;
                        return new Token.Token() { value = "-" + parExpression_LexedTokens[index - 1].value, tokenType = parExpression_LexedTokens[index].tokenType };
                    }
                    else
                    {
                        index++;
                        return parExpression_LexedTokens[index - 1];
                    }
                }
                break;
            }
            throw new Exception("Error: Performed math operation with A non-number");

        }


















    }

}
