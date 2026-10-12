using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Developers_MathEngine_Library.Interpreter.Utilities
{
    internal class Utilities_Lexer_Math
    {

        public static void remove_NewLines_Spaces(ref string input) {
            input = input.Replace("\r","").Replace("\n","").Replace(" ","");
        }
        public static bool isNum(char input) {

            for (int i = 0; i < 10; i++)
            {
                if (input.Equals(i.ToString()[0]))
                {
                    return true;
                }
            }
            if (input.Equals('.'))
            {
                return true;
            }
            return false;
        }

        public static bool isMathOperator(char input) {
            switch (input)
            {
                case '+':
                    return true;
                case '-':
                    return true;
                case '/':
                    return true;
                case '*':
                    return true;
                case '^':
                    return true;

                case '=':
                    return true;
                case '>':
                    return true;
                case '<':
                    return true;

                case '(':
                    return true;
                case ')':
                    return true;
                default:
                    return false;
            }
        }

    }
}
