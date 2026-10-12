using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Developers_MathEngine_Library.Interpreter.Utilities;

namespace Developers_MathEngine_Library.Interpreter.Parser
{
    public class Parser_Math
    {

        public static async Task<List<Token.Token>> parse(List<string> inputs)
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
                    List<Token.Token> lexer_List = (await Task.WhenAll(Lexer.Lexer_Math.lex(inputs[i], 0)))[0].ToList();
                    List<Token.Token> lexerParsePar_List_Result = parseParToken(lexer_List);
                    int parseIndex = 0;
                    Result[i] = Utilities.Utilities_Parser_Math.basicParse(lexerParsePar_List_Result, ref parseIndex);
                    //}
                });
                return Result;
            });


            return result;
        }

        private static List<Token.Token> parseParToken(List<Token.Token> _lexer_List)
        {
            List<Token.Token> lexer_List = new List<Token.Token>(_lexer_List);
            int parseIndex = 0;
            if (lexer_List.Any(x => x.value == "(" && x.tokenType == Token.TokenType.LPAREN))
            {



                List<(string functionName, int startIndex_ParFunc, int endIndex_ParFunc, List<Token.Token> parExpression_Tokens)> result_GetPar = Utilities_Parser_Math.getParentParFunc(lexer_List);


                for (int i = 0; i < result_GetPar.Count; i++)
                {

                    List<Token.Token> parExpression_Tokens_RecursiveResult = parseParToken(result_GetPar[i].parExpression_Tokens);

                    lexer_List.RemoveRange(result_GetPar[i].startIndex_ParFunc, result_GetPar[i].endIndex_ParFunc + 1 - result_GetPar[i].startIndex_ParFunc);

                    parseIndex = 0;
                    lexer_List.Insert(result_GetPar[i].startIndex_ParFunc, Utilities.Utilities_Parser_Math.basicParse(parExpression_Tokens_RecursiveResult, ref parseIndex));

                }


            }

            return lexer_List;
        }

    }
}
