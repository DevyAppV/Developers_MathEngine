using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Developers_MathEngine_MVVM.Interpreter.Token
{
    public enum TokenType
    {

        //general
        NONE,
        //CODE,
        //MATH,


        //math
        INFINITE,
        OPERATION,
        STRING_VALUE,
        MATH_VALUE,
        PLUS,
        MINUS,
        MULTIPLY,
        DIVIDE,
        POWER,
        DECIMAL,

        //grouping
        LPAREN,
        RPAREN,
        LBRACKET,
        RBRACKET,



        //keywords
        USING,
        CLASS,
        FUNCTION,
        IDENTIFIER,
        NUMBER,
        STRING,
        BOOL,
        TRUE,
        FALSE,
        RETURN,


        //EQUAL
        EQUAL_ASSIGN,

        //Conditional Compare
        CONDITIONAL_IF,
        CONDITIONAL_ELSEIF,
        CONDITIONAL_ELSE,
        CONDITIONAL_VALUE,
        CONDITIONAL_EQUAL,
        CONDITIONAL_EQUAL_NOT,
        CONDITIONAL_NOT,
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
        IN,
        WHILE,
        DO_WHILE,

        //end of file escape characters
        SPACE,
        SEMICOLON,
        NEWLINE,
        COMMA,
        DOT,
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


        #region Remove [BAD]
        public string identifier = "";
        public string classScope = "";
        public string functionScope = "";
        public int nestedStatementCount = 0;

        #endregion Remove [BAD]

        //public static Dictionary<string, TokenType> tokenKeyword_Dictionary = new Dictionary<string, TokenType>()
        //{
        //    { "true", TokenType.TRUE},
        //    { "false", TokenType.FALSE},
        //};

        public static Dictionary<string, TokenType> token_Dictionary = new Dictionary<string, TokenType>() {


            //{"CODE:", TokenType.CODE},
            //{"MATH:", TokenType.MATH},

            //parameter/body/group

			//math
			{ "infinite", TokenType.INFINITE},
			{ "operation", TokenType.OPERATION},
			{ "string_value", TokenType.STRING_VALUE},
			{ "math_value", TokenType.MATH_VALUE},
			{ "+", TokenType.PLUS},
            { "-", TokenType.MINUS},
            { "*", TokenType.MULTIPLY},
            { "/", TokenType.DIVIDE},
            { "^", TokenType.DIVIDE},

			//grouping
			{ "(", TokenType.LPAREN},
            { ")", TokenType.RPAREN},
			{ "{", TokenType.LBRACKET},
            { "}", TokenType.RBRACKET},

			//keywords
			{ "using", TokenType.USING},
			{ "class", TokenType.CLASS},
            { "function", TokenType.FUNCTION},
            { "number", TokenType.NUMBER},
            { "string", TokenType.STRING},
            { "bool", TokenType.BOOL},
            { "true", TokenType.TRUE},
            { "false", TokenType.FALSE},
            { "return", TokenType.RETURN},

			//equal
			{ "=", TokenType.EQUAL_ASSIGN},

			//conditional compare
			{ "if", TokenType.CONDITIONAL_IF},
			{ "elseif", TokenType.CONDITIONAL_ELSEIF},
			{ "else", TokenType.CONDITIONAL_ELSE},
			{ "conditional_value", TokenType.CONDITIONAL_VALUE},
			{ "==", TokenType.CONDITIONAL_EQUAL},
            { "!=", TokenType.CONDITIONAL_EQUAL_NOT},
            { "!", TokenType.CONDITIONAL_NOT},
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
            { "in", TokenType.IN},
            { "while", TokenType.WHILE},
            { "dowhile", TokenType.DO_WHILE},

			//end of file escape characters
			{ " ", TokenType.SPACE},
            { "\\n", TokenType.NEWLINE},
            { ",", TokenType.COMMA},
            { ".", TokenType.DOT},
            { "\"", TokenType.QUOTATIONMARK},
            { "\\0", TokenType.END_OF_FILE},
            { ";", TokenType.END_OF_CODE},
            { "\\n", TokenType.END_OF_LINE},

			//error
			{ "Exception", TokenType.ERROR },

        };
    }
}
