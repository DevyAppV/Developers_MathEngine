using Developers_MathEngine_MVVM.Interpreter.Lexer;
using Developers_MathEngine_MVVM.Interpreter.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Developers_MathEngine_MVVM.Interpreter.Parser
{
    public class Parser_Code
    {
        public static async Task<List<Token.Token>> parse(List<string> inputs, bool isGetVar)
        {
            Task<List<Token.Token>> taskResult = Task.Run(() =>
            {
                return Enumerable.Repeat(new Token.Token(), inputs.Count).AsParallel().ToList();
            });

            List<Token.Token> result = await Task.Run(async () =>
            {
                List<Token.Token> Result = (await Task.WhenAll(taskResult))[0].ToList();
                Parallel.For(0, inputs.Count, async i =>
                //for (int i = 0; i < inputs.Count; i++)
                {
                    (Dictionary<string, SortedDictionary<string, List<Token.Token>>> operationDictionary, Dictionary<string, Dictionary<string, Dictionary<string, Token.Token>>> varDictionary) result = Lexer_Code.lex(inputs[i], 0);


                    Result[i] = parseTokens(result, isGetVar);

                    //}
                });
                return Result;
            });


            return result;
        }

        //result = class/func     ,     varDictionary == class/func/var
        private static Token.Token parseTokens((Dictionary<string, SortedDictionary<string, List<Token.Token>>> operationDictionary, Dictionary<string, Dictionary<string, Dictionary<string, Token.Token>>> varDictionary) result, bool isGetVar)
        {

            //TODO: setup global var first



            if (!isGetVar)
            {
                //foreach(var globalOp in result.operationDictionary[""][""])
                //for (int i = 0; i < result.operationDictionary[""][""].Count; i++)
                int index = 0;
                while (index < result.operationDictionary[""][""].Count)
                {

                    switch (result.operationDictionary[""][""][index].tokenType)
                    {
                        case TokenType.CONDITIONAL_IF:
                            compareIf_ElseIf(ref result, TokenType.CONDITIONAL_IF, ref index);


                            break;
                        case TokenType.CONDITIONAL_ELSEIF:
                            break;
                        case TokenType.CONDITIONAL_ELSE:
                            break;
                        default:
                            index++;
                            break;
                    }
                }
            }
            else
            {

            }

            return new Token.Token();
        }


        private static bool compareIf_ElseIf(ref (Dictionary<string, SortedDictionary<string, List<Token.Token>>> operationDictionary, Dictionary<string, Dictionary<string, Dictionary<string, Token.Token>>> varDictionary) result,
            TokenType tokenComparisonType,
            ref int index)
        {
            string lTokenValue = "";
            string rTokenValue = "";
            TokenType comparisonLogicType = TokenType.NONE;
            while (index + 1 < result.operationDictionary[""][""].Count)
            {
                index++;
                if (string.IsNullOrEmpty(lTokenValue) && result.operationDictionary[""][""][index].tokenType.Equals("conditional_value"))
                {
                    lTokenValue = result.operationDictionary[""][""][index].value;
                }
                else if (string.IsNullOrEmpty(rTokenValue) && result.operationDictionary[""][""][index].tokenType.Equals("conditional_value"))
                {
                    rTokenValue = result.operationDictionary[""][""][index].value;
                }
                else if (string.IsNullOrEmpty(rTokenValue) &&
                    (
                    result.operationDictionary[""][""][index].tokenType.Equals(">") ||
                    result.operationDictionary[""][""][index].tokenType.Equals(">=") ||
                    result.operationDictionary[""][""][index].tokenType.Equals("<") ||
                    result.operationDictionary[""][""][index].tokenType.Equals("<=") ||
                    result.operationDictionary[""][""][index].tokenType.Equals("==") ||
                    result.operationDictionary[""][""][index].tokenType.Equals("!=")
                    ))
                {
                    comparisonLogicType = result.operationDictionary[""][""][index].tokenType;
                }
                else
                {
                    //concat condition value
                    //solve condition value
                }

            }




            string isBoolStringNumber = "";

            if (lTokenValue.StartsWith("\"") && lTokenValue.EndsWith("\""))
            {
                if (rTokenValue.StartsWith("\"") && rTokenValue.EndsWith("\""))
                {
                    isBoolStringNumber = "string";
                }
                else return false;
            }
            if (!lTokenValue.StartsWith("\"") && !lTokenValue.EndsWith("\""))
            {
                if (!rTokenValue.StartsWith("\"") && !rTokenValue.EndsWith("\""))
                {
                    isBoolStringNumber = "number";
                }
                else return false;
            }
            if (lTokenValue.Equals("true") || lTokenValue.Equals("false"))
            {
                if (!rTokenValue.Equals("true") || !rTokenValue.Equals("false"))
                {
                    isBoolStringNumber = "bool";
                }
                else return false;
            }

            //global statement &&
            //class function
            switch (tokenComparisonType)
            {
                case TokenType.CONDITIONAL_IF:
                    return setCondition(lTokenValue, rTokenValue, comparisonLogicType, isBoolStringNumber);
                case TokenType.CONDITIONAL_ELSEIF:
                    return setCondition(lTokenValue, rTokenValue, comparisonLogicType, isBoolStringNumber);
                case TokenType.CONDITIONAL_ELSE:
                    return setCondition(lTokenValue, rTokenValue, comparisonLogicType, isBoolStringNumber);
                default:
                    return false;

            }

        }

        private static bool setCondition(string lTokenValue, string rTokenValue, TokenType comparisonLogicType, string isBoolStringNumber)
        {
            if (isBoolStringNumber.Equals("number"))
            {
                if (comparisonLogicType == TokenType.CONDITIONAL_EQUAL && lTokenValue.Equals(rTokenValue))
                {
                    return true;
                }
                else if (comparisonLogicType == TokenType.CONDITIONAL_EQUAL_NOT && !lTokenValue.Equals(rTokenValue))
                {
                    return true;
                }
                else if (comparisonLogicType == TokenType.GREATER_THAN && Utilities.Utilities_String.isLeftStr_LargerThenRightStr_Number(lTokenValue, rTokenValue))
                {
                    return true;
                }
                else if (comparisonLogicType == TokenType.GREATER_THAN_OR_EQUAL && Utilities.Utilities_String.isLeftStr_LargerThenEqualToRightStr_Number(lTokenValue, rTokenValue))
                {
                    return true;
                }
                else if (comparisonLogicType == TokenType.LESS_THAN && !Utilities.Utilities_String.isLeftStr_LargerThenRightStr_Number(lTokenValue, rTokenValue))
                {
                    return true;
                }
                else if (comparisonLogicType == TokenType.LESS_THAN_OR_EQUAL && !Utilities.Utilities_String.isLeftStr_LargerThenEqualToRightStr_Number(lTokenValue, rTokenValue))
                {
                    return true;
                }
            }
            else if (isBoolStringNumber.Equals("string"))
            {
                if (comparisonLogicType == TokenType.CONDITIONAL_EQUAL && lTokenValue.Equals(rTokenValue))
                {
                    return true;
                }
                else if (comparisonLogicType == TokenType.CONDITIONAL_EQUAL_NOT && !lTokenValue.Equals(rTokenValue))
                {
                    return true;
                }
            }
            else if (isBoolStringNumber.Equals("bool"))
            {
                if (comparisonLogicType == TokenType.CONDITIONAL_EQUAL && lTokenValue.Equals(rTokenValue))
                {
                    return true;
                }
                else if (comparisonLogicType == TokenType.CONDITIONAL_EQUAL_NOT && !lTokenValue.Equals(rTokenValue))
                {
                    return true;
                }
            }



            return false;



        }

    }
}
