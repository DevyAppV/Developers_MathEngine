using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Developers_MathEngine_Library.Interpreter.Utilities;

namespace Developers_MathEngine_Library.Interpreter.Lexer
{
    public class Lexer_Math
    {

        public static async Task<List<Token.Token>> lex(string _input, int _index)
        {
            string input = _input;
            int index = _index;


            Queue<Token.Token> result = new Queue<Token.Token>();

            //Utilities_Lexer_Math utilities_Math = new Utilities_Lexer_Math();
            Utilities_Lexer_Math.remove_NewLines_Spaces(ref input);

            StringBuilder var_FuncStr_Input = new StringBuilder();

            while (index < input.Length)
            {
                switch (input[index])
                {

                    case '+':
                        result.Enqueue(new Token.Token() { tokenType = Token.TokenType.PLUS, value = input[index].ToString() });

                        index++;
                        break;
                    case '-':
                        result.Enqueue(new Token.Token() { tokenType = Token.TokenType.MINUS, value = input[index].ToString() });

                        index++;
                        break;
                    case '/':
                        result.Enqueue(new Token.Token() { tokenType = Token.TokenType.DIVIDE, value = input[index].ToString() });

                        index++;
                        break;
                    case '*':
                        result.Enqueue(new Token.Token() { tokenType = Token.TokenType.MULTIPLY, value = input[index].ToString() });

                        index++;
                        break;

                    case '^':
                        result.Enqueue(new Token.Token() { tokenType = Token.TokenType.POWER, value = input[index].ToString() });

                        index++;
                        break;

                    case '(':
                        result.Enqueue(new Token.Token() { tokenType = Token.TokenType.LPAREN, value = input[index].ToString() });


                        index++;
                        break;

                    case ')':
                        result.Enqueue(new Token.Token() { tokenType = Token.TokenType.RPAREN, value = input[index].ToString() });

                        index++;
                        break;

                    case '=':

                        result.Enqueue(new Token.Token() { tokenType = Token.TokenType.EQUAL_ASSIGN, value = input[index].ToString() });

                        index++;
                        break;

                    case '>':
                        if (index + 1 < input.Length)
                        {
                            if ((int)input[index + 1] == (int)'=')
                            {
                                result.Enqueue(new Token.Token() { tokenType = Token.TokenType.GREATER_THAN_OR_EQUAL, value = ">=" });
                                break;
                            }
                        }
                        result.Enqueue(new Token.Token() { tokenType = Token.TokenType.GREATER_THAN, value = input[index].ToString() });

                        index++;
                        break;

                    case '<':
                        if (index + 1 < input.Length)
                        {
                            if ((int)input[index + 1] == (int)'=')
                            {
                                result.Enqueue(new Token.Token() { tokenType = Token.TokenType.GREATER_THAN_OR_EQUAL, value = "<=" });
                                break;
                            }
                        }
                        result.Enqueue(new Token.Token() { tokenType = Token.TokenType.EQUAL_ASSIGN, value = input[index].ToString() });

                        index++;
                        break;

                    default:
                        bool TIsStringVar_FIsNum = false;
                        int numOfDecimal = 0;
                        while (index < input.Length)
                        {
                            while (index < input.Length && Utilities_Lexer_Math.isNum(input[index]))
                            {
                                if (input[index].Equals('.')) numOfDecimal++;
                                var_FuncStr_Input = var_FuncStr_Input.Append(input[index]);
                                index++;
                            }
                            while (index < input.Length && !Utilities_Lexer_Math.isMathOperator(input[index]))
                            {
                                if (!TIsStringVar_FIsNum)
                                {
                                    TIsStringVar_FIsNum = true;
                                }
                                var_FuncStr_Input = var_FuncStr_Input.Append(input[index]);
                                index++;
                            }
                            if (index < input.Length && (!(Utilities_Lexer_Math.isNum(input[index]) || !Utilities_Lexer_Math.isMathOperator(input[index]))))
                            {
                                break;
                            }
                        }

                        if (index < input.Length && input[index].Equals('('))
                        {
                            result.Enqueue(new Token.Token() { tokenType = Token.TokenType.FUNCTION, value = var_FuncStr_Input.ToString() + input[index].ToString() });

                            index++;
                        }

                        else if (!TIsStringVar_FIsNum && numOfDecimal <= 1)
                        {
                            result.Enqueue(new Token.Token() { tokenType = Token.TokenType.NUMBER, value = var_FuncStr_Input.ToString() });

                        }
                        else
                        {
                            result.Enqueue(new Token.Token() { tokenType = Token.TokenType.STRING, value = var_FuncStr_Input.ToString() });
                        }
                        var_FuncStr_Input = var_FuncStr_Input.Clear();
                        break;

                }

            }

            return result.ToList();
        }
    }
}
