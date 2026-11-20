using Developers_MathEngine_MVVM.Interpreter.Errors;
using Developers_MathEngine_MVVM.Interpreter.Utilities;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Developers_MathEngine_MVVM.Interpreter.Lexer
{
    public class Lexer_Code
    {

        //dictionary class+function+var
        public static (Dictionary<string, SortedDictionary<string, List<Token.Token>>> operationDictionary, Dictionary<string, Dictionary<string, Dictionary<string, Token.Token>>> varDictionary) lex(string input, int index)
        {
            (Dictionary<string, SortedDictionary<string, List<Token.Token>>> operationDictionary, Dictionary<string, Dictionary<string, Dictionary<string, Token.Token>>> varDictionary) result =
            (new Dictionary<string, SortedDictionary<string, List<Token.Token>>>(), new Dictionary<string, Dictionary<string, Dictionary<string, Token.Token>>>());

            //Queue<Token.Token> result = new Queue<Token.Token>();

            StringBuilder scanner = new StringBuilder();

            StringBuilder tokenIdentifierType = new StringBuilder();
            StringBuilder tokenIdentifier = new StringBuilder();
            StringBuilder tokenIdentifierValue = new StringBuilder();
            StringBuilder tokenClassScope = new StringBuilder();
            StringBuilder tokenFunctionScope = new StringBuilder();

            //int classBracketCount = 0;
            //int classFunctionCount = 0;
            int nestedStatementCount = 0;
            int quotationCount = 0;


            while (index < input.Length)
            {
                switch (input[index])
                {
                    case ' ':
                        if (index + 1 < input.Length)
                        {
                            //skip spaces
                            while (index + 1 < input.Length && input[index + 1].Equals(' '))
                            {
                                index++;
                            }
                        }

                        if (!isConditionalValue(tokenIdentifierType.ToString()))
                        {
                            checkFor_SetIdentifierType(ref scanner, ref tokenIdentifierType, ref tokenIdentifier, ref nestedStatementCount);
                            checkFor_SetIdentifier_FromType(ref scanner, ref tokenIdentifierType, ref tokenIdentifier, ref tokenIdentifierValue, ref tokenClassScope, ref tokenFunctionScope);
                        }
                        break;
                    case '!':
                        //check for conditional
                        setCondition(ref result, ref scanner, ref tokenIdentifier, ref tokenIdentifier, ref tokenIdentifierValue, ref tokenClassScope,
                            ref tokenFunctionScope, ref input, ref index, "!=", true, ref nestedStatementCount);
                        break;
                    case '>':
                        //check for conditional
                        setCondition(ref result, ref scanner, ref tokenIdentifier, ref tokenIdentifier, ref tokenIdentifierValue, ref tokenClassScope,
                            ref tokenFunctionScope, ref input, ref index, ">=", true, ref nestedStatementCount);
                        break;
                    case '<':
                        //check for conditional
                        setCondition(ref result, ref scanner, ref tokenIdentifier, ref tokenIdentifier, ref tokenIdentifierValue, ref tokenClassScope,
                            ref tokenFunctionScope, ref input, ref index, "<=", true, ref nestedStatementCount);
                        break;
                    case '=':
                        //check for conditional

                        if (index + 1 < input.Length)
                        {
                            //skip spaces
                            while (index + 1 < input.Length && input[index + 1].Equals(' '))
                            {
                                index++;
                            }
                            if (!input[index + 1].Equals('='))
                            {
                                checkFor_SetIdentifier_FromType(ref scanner, ref tokenIdentifierType, ref tokenIdentifier, ref tokenIdentifierValue, ref tokenClassScope, ref tokenFunctionScope);
                            }
                            else
                            {
                                setCondition(ref result, ref scanner, ref tokenIdentifier, ref tokenIdentifier, ref tokenIdentifierValue, ref tokenClassScope,
                                ref tokenFunctionScope, ref input, ref index, "==", false, ref nestedStatementCount);
                            }
                        }

                        //chk for identidier
                        break;
                    case ';':
                        if (!string.IsNullOrEmpty(tokenIdentifier.ToString()))
                        {
                            setResult(ref result, ref tokenIdentifierType, ref tokenIdentifier, ref tokenIdentifierValue, ref tokenClassScope, ref tokenFunctionScope, ref nestedStatementCount);
                        }
                        clearStrBuilder(ref tokenIdentifierType, ref tokenIdentifier, ref tokenIdentifierValue, ref tokenClassScope, ref tokenFunctionScope, false, false, false, false);

                        scanner.Clear();

                        break;


                    case '(':
                        //function loop condition api parameters

                        break;
                    case ')':
                        //function loop condition api parameters

                        break;

                    case ',':
                        //function loop condition api parameters

                        if (!string.IsNullOrEmpty(tokenIdentifier.ToString()))
                        {
                            setResult(ref result, ref tokenIdentifierType, ref tokenIdentifier, ref tokenIdentifierValue, ref tokenClassScope, ref tokenFunctionScope, ref nestedStatementCount);
                        }
                        clearStrBuilder(ref tokenIdentifierType, ref tokenIdentifier, ref tokenIdentifierValue, ref tokenClassScope, ref tokenFunctionScope, false, false, false, false);
                        scanner.Clear();
                        break;





                    case '{':
                        //function class body

                        break;
                    case '}':
                        //"asdasd){asdasd"

                        if (!isConditionalValue(tokenIdentifierType.ToString()))
                        {
                            if (input.Length >= index - 1)
                            {
                                if (input[index - 1].Equals(')'))
                                {

                                }
                            }
                        }

                        //function class
                        //tokenIdentifierType.Append(input[index]);
                        switch (tokenIdentifierType.ToString())
                        {
                            case "function":
                                removeLast_ClassFunctionScope(ref tokenFunctionScope, "0");
                                break;
                            case "class":

                                removeLast_ClassFunctionScope(ref tokenClassScope, "1");
                                break;
                            default:
                                if (scanner.Length > 0)
                                {
                                    tokenIdentifierType.Append("formula");
                                    tokenIdentifierValue.Append(scanner.ToString());
                                    setResult(ref result, ref tokenIdentifierType, ref tokenIdentifier, ref tokenIdentifierValue, ref tokenClassScope, ref tokenFunctionScope, ref nestedStatementCount);
                                }
                                break;
                        }
                        break;
                    default:
                        //append word
                        getScannerValue(ref scanner, ref input, ref index, ref quotationCount, tokenIdentifierType.ToString());
                        break;
                }
                index++;
            }

            if (!string.IsNullOrWhiteSpace(scanner.ToString()) || !string.IsNullOrEmpty(scanner.ToString()))
            {
                tokenIdentifierType.Append("formula");
                tokenIdentifierValue.Append(scanner.ToString());
                setResult(ref result, ref tokenIdentifierType, ref tokenIdentifier, ref tokenIdentifierValue, ref tokenClassScope, ref tokenFunctionScope, ref nestedStatementCount);


            }

            return result;
        }



        private static bool isConditionalValue(string tokenIdentifierType)
        {

            if (
                tokenIdentifierType.Equals("if") ||
                tokenIdentifierType.Equals("elseif") ||
                tokenIdentifierType.Equals("else") ||
                tokenIdentifierType.Equals("conditional_value")

                )
            {
                return true;
            }
            return false;
        }

        private static void getScannerValue(ref StringBuilder scanner, ref string input, ref int index, ref int quotationCount, string tokenIdentifierType)
        {
            //start with " false
            if (!input[index].Equals("\""))
            {
                scanner.Append(input[index]);
                while (index + 1 < input.Length)
                {
                    index++;
                    if (
                        !input[index].Equals('=') &&
                        !input[index].Equals(';') &&
                        !input[index].Equals(' ') &&
                        !input[index].Equals('>') &&
                        !input[index].Equals('<') &&
                        !input[index].Equals('!')
                        )
                    {
                        //if (isConditionalValue(tokenIdentifierType)) { }
                        if (scanner.Length == 1 && scanner.ToString().Equals("\"") && quotationCount <= 0) quotationCount++;
                        else if (scanner.ToString().Last().Equals("\"") &&
                            (
                            index + 1 < input.Length &&
                            input[index + 1].Equals('+') &&
                            input[index + 1].Equals('=') &&
                            input[index + 1].Equals('>') &&
                            input[index + 1].Equals('<') &&
                            input[index + 1].Equals(')') &&
                            input[index + 1].Equals('!')
                            ) &&
                            quotationCount > 0)
                        {
                            quotationCount--;
                            if (input[index + 1].Equals(')'))
                            {
                                int indexCheck = index;
                                while (indexCheck+1< input.Length && input[indexCheck + 1].Equals(' '))
                                {
                                    indexCheck++;
                                }
                                if (input[indexCheck].Equals('{'))
                                {
                                }
                            }
                        }


                        if (quotationCount > 0)
                        {
                            index--;
                            break;
                        }

                        scanner.Append(input[index]);
                    }
                    else
                    {
                        index--;
                        break;
                    }

                }
            }//start with " true
            else
            {
                scanner.Append(input[index]);
                while (index + 1 < input.Length)
                {
                    index++;
                    if (!input[index].Equals('\"'))
                    {
                        scanner.Append(input[index]);
                    }
                    else
                    {
                        scanner.Append(input[index]);
                        break;
                    }

                }
            }


        }



        private static void setCondition(
            ref (Dictionary<string, SortedDictionary<string, List<Token.Token>>> operationDictionary, Dictionary<string, Dictionary<string, Dictionary<string, Token.Token>>> varDictionary) result,
            ref StringBuilder scanner, ref StringBuilder tokenIdentifierType, ref StringBuilder tokenIdentifier,
            ref StringBuilder tokenIdentifierValue, ref StringBuilder tokenClassScope, ref StringBuilder tokenFunctionScope, ref string input, ref int index,
            string checkForConditionType, bool setConditionalLogic_BySelf, ref int nestedStatementCount)
        {

            while (index + 1 < input.Length && input[index + 1].Equals(' '))
            {
                index++;
            }

            if (index + 1 < input.Length)
            {
                if (input[index + 1].Equals('='))
                {
                    //set conditional value L
                    tokenIdentifierType.Append("conditional_value");
                    tokenIdentifierValue.Append(scanner.ToString());
                    scanner.Clear();

                    setResult(ref result, ref tokenIdentifierType, ref tokenIdentifier, ref tokenIdentifierValue, ref tokenClassScope, ref tokenFunctionScope, ref nestedStatementCount);
                    clearStrBuilder(ref tokenIdentifierType, ref tokenIdentifier, ref tokenIdentifierValue, ref tokenClassScope, ref tokenFunctionScope, false, false, false, false);

                    //set conditional Logic
                    index++;

                    tokenIdentifierType.Clear();
                    tokenIdentifierValue.Clear();
                    scanner.Append(checkForConditionType);
                    tokenIdentifierType.Append(scanner.ToString());
                    tokenIdentifierValue.Append(scanner.ToString());
                    setResult(ref result, ref tokenIdentifierType, ref tokenIdentifier, ref tokenIdentifierValue, ref tokenClassScope, ref tokenFunctionScope, ref nestedStatementCount);
                    clearStrBuilder(ref tokenIdentifierType, ref tokenIdentifier, ref tokenIdentifierValue, ref tokenClassScope, ref tokenFunctionScope, false, false, false, false);
                    scanner.Clear();


                    tokenIdentifierType.Clear();
                    tokenIdentifierValue.Clear();
                    tokenIdentifierType.Append("conditional_value");
                    //set conditional value R
                    while (index + 1 < input.Length)
                    {

                        index++;

                        // " ){
                        if (input[index].Equals(')'))

                            if (
                                !input[index].Equals('>') ||
                                !input[index].Equals('<') ||
                                !(input[index].Equals('=') && index + 1 < input.Length && input[index + 1].Equals('=')) ||
                                !(input[index].Equals('!') && index + 1 < input.Length && input[index + 1].Equals('='))
                                )
                            {
                                scanner.Append(input[index]);
                            }
                            else
                            {
                                index--;
                                tokenIdentifierValue.Append(scanner.ToString());
                                scanner.Clear();

                                setResult(ref result, ref tokenIdentifierType, ref tokenIdentifier, ref tokenIdentifierValue, ref tokenClassScope, ref tokenFunctionScope, ref nestedStatementCount);
                                clearStrBuilder(ref tokenIdentifierType, ref tokenIdentifier, ref tokenIdentifierValue, ref tokenClassScope, ref tokenFunctionScope, false, false, false, false);

                                break;
                            }

                    }
                }
                else if (setConditionalLogic_BySelf)
                {
                    //set conditional Logic
                    scanner.Append(input[index]);
                    tokenIdentifierType.Append(scanner.ToString());
                    tokenIdentifierValue.Append(scanner.ToString());
                    setResult(ref result, ref tokenIdentifierType, ref tokenIdentifier, ref tokenIdentifierValue, ref tokenClassScope, ref tokenFunctionScope, ref nestedStatementCount);
                    clearStrBuilder(ref tokenIdentifierType, ref tokenIdentifier, ref tokenIdentifierValue, ref tokenClassScope, ref tokenFunctionScope, false, false, false, false);
                    scanner.Clear();
                }
            }


        }








































        //class,function,variable,operation
        private static void setResult(ref (Dictionary<string, SortedDictionary<string, List<Token.Token>>> operationDictionary, Dictionary<string, Dictionary<string, Dictionary<string, Token.Token>>> varDictionary) result,
            ref StringBuilder tokenIdentifierType, ref StringBuilder tokenIdentifier,
            ref StringBuilder tokenIdentifierValue, ref StringBuilder tokenClassScope, ref StringBuilder tokenFunctionScope, ref int nestedStatementCount)
        {
            string idType = tokenIdentifierType.ToString();
            if (idType.Equals("number") ||
                idType.Equals("bool") ||
                idType.Equals("string")
                )
            {
                if (!result.varDictionary.ContainsKey(tokenClassScope.ToString()))
                {
                    result.varDictionary.Add(tokenClassScope.ToString(), new Dictionary<string, Dictionary<string, Token.Token>>() {
                        { tokenFunctionScope.ToString(),new Dictionary<string, Token.Token>()
                        {{ tokenIdentifier.ToString(), new Token.Token(){

                            identifier = tokenIdentifier.ToString(),
                            tokenType = Token.Token.token_Dictionary[tokenIdentifierType.ToString()],
                            value = tokenIdentifierValue.ToString(),
                            classScope = tokenClassScope.ToString(),
                            functionScope = tokenFunctionScope.ToString(),
                            nestedStatementCount = nestedStatementCount

                        } } } }
                    });
                }
                else if (result.varDictionary.ContainsKey(tokenClassScope.ToString()))
                {
                    //result.tokenDictionary[tokenClassScope.ToString()] = new Dictionary<string, Dictionary<string, Token.Token>>();
                    if (result.varDictionary[tokenClassScope.ToString()].ContainsKey(tokenFunctionScope.ToString()))
                    {
                        if (result.varDictionary[tokenClassScope.ToString()][tokenFunctionScope.ToString()].ContainsKey(tokenIdentifier.ToString()))
                        {
                            //throw error
                            ErrorHandler.errorMessage = tokenIdentifier.ToString() + "already exists";
                        }
                    }
                    else
                    {
                        result.varDictionary[tokenClassScope.ToString()].Add(tokenFunctionScope.ToString(), new Dictionary<string, Token.Token>());
                    }

                    result.varDictionary[tokenClassScope.ToString()][tokenFunctionScope.ToString()].Add(tokenIdentifier.ToString(), new Token.Token()
                    {
                        identifier = tokenIdentifier.ToString(),
                        tokenType = Token.Token.token_Dictionary[tokenIdentifierType.ToString()],
                        value = tokenIdentifierValue.ToString(),
                        classScope = tokenClassScope.ToString(),
                        functionScope = tokenFunctionScope.ToString(),
                        nestedStatementCount = nestedStatementCount
                    });

                }
            }
            else
            {

                if (!result.operationDictionary.ContainsKey(tokenClassScope.ToString()))
                {
                    result.operationDictionary.Add(tokenClassScope.ToString(), new SortedDictionary<string, List<Token.Token>>() {
                        { tokenFunctionScope.ToString(),new List<Token.Token>()
                        { new Token.Token(){

                            identifier = tokenIdentifier.ToString(),
                            tokenType = Token.Token.token_Dictionary[tokenIdentifierType.ToString()],
                            value = tokenIdentifierValue.ToString(),
                            classScope = tokenClassScope.ToString(),
                            functionScope = tokenFunctionScope.ToString()

                        } } }
                    });
                }
                else if (result.operationDictionary.ContainsKey(tokenClassScope.ToString()))
                {
                    //result.tokenDictionary[tokenClassScope.ToString()] = new Dictionary<string, Dictionary<string, Token.Token>>();
                    if (!result.operationDictionary[tokenClassScope.ToString()].ContainsKey(tokenFunctionScope.ToString()))
                    {
                        result.operationDictionary[tokenClassScope.ToString()].Add(tokenFunctionScope.ToString(), new List<Token.Token>());
                    }

                    result.operationDictionary[tokenClassScope.ToString()][tokenFunctionScope.ToString()].Add(new Token.Token()
                    {
                        identifier = tokenIdentifier.ToString(),
                        tokenType = Token.Token.token_Dictionary[tokenIdentifierType.ToString()],
                        value = tokenIdentifierValue.ToString(),
                        classScope = tokenClassScope.ToString(),
                        functionScope = tokenFunctionScope.ToString()
                    });

                }
            }
        }

        private static void clearStrBuilder(ref StringBuilder tokenIdentifierType, ref StringBuilder tokenIdentifier,
            ref StringBuilder tokenIdentifierValue, ref StringBuilder tokenClassScope, ref StringBuilder tokenFunctionScope,
            bool removeLast_TokenClassScope, bool completelyRemove_TokenClassScop, bool removeLast_TokenFunctionScope, bool completelyRemove_TokenFunctionScope)
        {
            if (Utilities.Utilities_String.isStringLength_GreaterThan0(tokenIdentifierType.ToString()))
                tokenIdentifierType.Clear();
            if (Utilities.Utilities_String.isStringLength_GreaterThan0(tokenIdentifier.ToString()))
                tokenIdentifier.Clear();
            if (Utilities.Utilities_String.isStringLength_GreaterThan0(tokenIdentifierValue.ToString()))
                tokenIdentifierValue.Clear();
            if (completelyRemove_TokenClassScop && Utilities.Utilities_String.isStringLength_GreaterThan0(tokenClassScope.ToString()))
                tokenClassScope.Clear();
            if (removeLast_TokenClassScope && Utilities.Utilities_String.isStringLength_GreaterThan0(tokenClassScope.ToString()))
                removeLast_ClassFunctionScope(ref tokenClassScope, "1");


            if (completelyRemove_TokenFunctionScope && Utilities.Utilities_String.isStringLength_GreaterThan0(tokenFunctionScope.ToString()))
                tokenFunctionScope.Clear();
            if (removeLast_TokenFunctionScope && Utilities.Utilities_String.isStringLength_GreaterThan0(tokenFunctionScope.ToString()))
                removeLast_ClassFunctionScope(ref tokenFunctionScope, "0");


            //if (Utilities.Utilities_String.isStringLength_GreaterThan0(tokenFunctionScope.ToString()))
            //    tokenFunctionScope.Clear();
        }


        private static void removeLast_ClassFunctionScope(ref StringBuilder tokenClassScope, string dotsToSkip)
        {
            string tokenString = tokenClassScope.ToString();
            tokenClassScope.Clear();

            string indexOfLastDot = Utilities.Utilities_String.indexOfChar_Last(tokenString, '.', dotsToSkip);
            if (!indexOfLastDot.Equals("-1"))
            {
                tokenClassScope.Append(Utilities.Utilities_String.removeString(tokenString, Utilities_String.increment(indexOfLastDot)));
            }

        }


        private static void checkFor_SetIdentifierType(ref StringBuilder scanner, ref StringBuilder tokenIdentifierType, ref StringBuilder tokenIdentifier, ref int nestedStatementCount)
        {
            if (scanner.Length > 0 && tokenIdentifierType.Length <= 0)
            {
                switch (scanner.ToString())
                {
                    case "using":
                        tokenIdentifierType.Append(scanner.ToString());
                        break;
                    case "class":
                        tokenIdentifierType.Append(scanner.ToString());
                        break;
                    case "function":
                        tokenIdentifierType.Append(scanner.ToString());
                        break;
                    case "bool":
                        tokenIdentifierType.Append(scanner.ToString());
                        break;
                    case "string":
                        tokenIdentifierType.Append(scanner.ToString());
                        break;
                    case "number":
                        tokenIdentifierType.Append(scanner.ToString());
                        break;



                    //loops
                    case "for":
                        nestedStatementCount++;
                        tokenIdentifierType.Append(scanner.ToString());
                        break;
                    case "foreach":
                        nestedStatementCount++;
                        tokenIdentifierType.Append(scanner.ToString());
                        break;
                    case "in":
                        tokenIdentifierType.Append(scanner.ToString());
                        break;
                    case "while":
                        nestedStatementCount++;
                        tokenIdentifierType.Append(scanner.ToString());
                        break;
                    case "dowhile":
                        nestedStatementCount++;
                        tokenIdentifierType.Append(scanner.ToString());
                        break;

                    //conditional
                    case "if":
                        nestedStatementCount++;
                        tokenIdentifierType.Append(scanner.ToString());
                        break;
                    case "elseif":
                        nestedStatementCount++;
                        tokenIdentifierType.Append(scanner.ToString());
                        break;
                    case "else":
                        nestedStatementCount++;
                        tokenIdentifierType.Append(scanner.ToString());
                        break;


                    //return
                    case "return":
                        tokenIdentifierType.Append(scanner.ToString());
                        break;
                    default:
                        //op on var
                        tokenIdentifierType.Append("operation");
                        tokenIdentifier.Append("operation");
                        break;
                }
            }
        }


        private static void checkFor_SetIdentifier_FromType(ref StringBuilder scanner, ref StringBuilder tokenIdentifierType, ref StringBuilder tokenIdentifier,
            ref StringBuilder tokenIdentifierValue,
            ref StringBuilder tokenClassScope, ref StringBuilder tokenFunctionScope)
        {
            if (scanner.Length > 0 && tokenIdentifier.Length <= 0)
            {
                switch (tokenIdentifierType.ToString())
                {
                    case "using":
                        //tokenIdentifier.Clear();
                        //tokenScope.Clear();
                        tokenIdentifier.Append(scanner.ToString());
                        //tokenClassScope.Append(scanner.ToString());
                        break;
                    case "class":
                        //tokenIdentifier.Clear();
                        //tokenScope.Clear();
                        tokenIdentifier.Append(scanner.ToString());
                        tokenClassScope.Append(scanner.ToString() + ".");
                        break;
                    case "function":
                        //tokenIdentifier.Clear();
                        //tokenScope.Clear();
                        tokenIdentifier.Append(scanner.ToString());
                        tokenFunctionScope.Append(scanner.ToString());
                        break;
                    case "bool":
                        //tokenIdentifier.Clear();
                        tokenIdentifier.Append(scanner.ToString());
                        break;
                    case "string":
                        //tokenIdentifier.Clear();
                        tokenIdentifier.Append(scanner.ToString());
                        break;
                    case "number":
                        //tokenIdentifier.Clear();
                        tokenIdentifier.Append(scanner.ToString());
                        break;


                    //loops

                    case "in":
                        tokenIdentifierValue.Append(scanner.ToString());
                        break;

                    ////conditional
                    //case "if":

                    //    break;
                    //case "elseif":

                    //    break;


                    //return
                    case "return":
                        tokenIdentifierValue.Append(scanner.ToString());
                        break;
                    default:
                        //op on var
                        tokenIdentifierValue.Append(scanner.ToString());
                        break;
                }

            }
        }


    }
}
