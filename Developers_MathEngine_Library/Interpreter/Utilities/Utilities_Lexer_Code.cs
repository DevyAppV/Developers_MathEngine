using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Developers_MathEngine_Library.Interpreter.Token;

namespace Developers_MathEngine_Library.Interpreter.Utilities
{
    internal class Utilities_Lexer_Code
    {


        //private helper function to ignore escape characters and reset input string
        public void isEscapeCharacters_GetNewInput_Token(string input, int index)
        {

            while (Token.Token.token_Dictionary[input] == TokenType.SPACE ||
                Token.Token.token_Dictionary[input] == TokenType.NEWLINE ||
                Token.Token.token_Dictionary[input] == TokenType.END_OF_CODE
        		) {
                if (!input.Equals(""))
                {
                    input = "";
                }
                index++;
            }

        }

    }
}
