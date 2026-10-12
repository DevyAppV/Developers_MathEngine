using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Developers_MathEngine_Library.Interpreter.Token
{
    public enum TokenType
    {

        //general
        NONE,
        CODE,
        MATH,


        //math
        PLUS,
        MINUS,
        MULTIPLY,
        DIVIDE,
        POWER,
        DECIMAL,

        //grouping
        LPAREN,
        RPAREN,

        //keywords
        IDENTIFIER,
        NUMBER,
        STRING,
        FUNCTION,
        TRUE,
        FALSE,

        //EQUAL
        EQUAL_ASSIGN,

        //Conditional Compare
        CONDITIONAL_EQUAL,
        CONDITIONAL_EQUAL_NOT,
        GREATER_THAN,
        GREATER_THAN_OR_EQUAL,
        LESS_THAN,
        LESS_THAN_OR_EQUAL,

        //Conditional Logical
        AND,
        OR,

        //Loops
        FOR,
        FOREACH,
        WHILE,
        DO_WHILE,

        //end of file escape characters
        SPACE,
        NEWLINE,
        COMMA,
        QUOTATIONMARK,
        END_OF_FILE,
        END_OF_CODE,
        END_OF_LINE,

        //error
        ERROR,
    };


    //static Dictionary<char, TokenType> tokenMap;

    public class Token
    {

        public TokenType tokenType;
        public string value = "";

        public static Dictionary<string, TokenType> token_Dictionary = new Dictionary<string, TokenType>() {


            {"CODE:", TokenType.CODE},
            //{"MATH:", TokenType.MATH},

			//math
			{ "+", TokenType.PLUS},
            { "-", TokenType.MINUS},
            { "*", TokenType.MULTIPLY},
            { "/", TokenType.DIVIDE},
            { "^", TokenType.DIVIDE},

			//grouping
			{ "(", TokenType.LPAREN},
            { ")", TokenType.RPAREN},

			//keywords
			{ "number", TokenType.NUMBER},
            { "string", TokenType.STRING},
            { "function", TokenType.FUNCTION},
            { "true", TokenType.TRUE},
            { "false", TokenType.FALSE},

			//equal
			{ "=", TokenType.EQUAL_ASSIGN},

			//conditional compare
			{ "==", TokenType.CONDITIONAL_EQUAL},
            { "!=", TokenType.CONDITIONAL_EQUAL_NOT},
            { ">", TokenType.GREATER_THAN},
            { ">=", TokenType.GREATER_THAN_OR_EQUAL},
            { "<", TokenType.LESS_THAN},
            { "<=", TokenType.LESS_THAN_OR_EQUAL},

			//conditional logical
			{ "&&", TokenType.AND},
            { "||", TokenType.OR},

			//loops
			{ "for", TokenType.FOR},
            { "foreach", TokenType.FOREACH},
            { "while", TokenType.WHILE},
            { "do_while", TokenType.DO_WHILE},

			//end of file escape characters
			{ " ", TokenType.SPACE},
            { "\\n", TokenType.NEWLINE},
            { ",", TokenType.COMMA},
            { "\"", TokenType.QUOTATIONMARK},
            { "\\0", TokenType.END_OF_FILE},
            { ";", TokenType.END_OF_CODE},
            { "\\n", TokenType.END_OF_LINE},

			//error
			{ "throw", TokenType.ERROR },

        };
    }
}
