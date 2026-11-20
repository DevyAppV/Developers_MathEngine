using Developers_MathEngine_MVVM.Engine.CoreCLR;
using Developers_MathEngine_MVVM.Interpreter.Lexer;
using Developers_MathEngine_MVVM.Interpreter.Parser;
using Developers_MathEngine_MVVM.Interpreter.Token;
using Developers_MathEngine_MVVM.Interpreter.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Developers_MathEngine_UnitTest.UnitTests
{

    [TestClass]
    public class UnitTest_Math
    {
        [TestMethod]
        public async Task testBasicParse_OnDifExpressions()
        {

            //no par
            Lexer_Math mathLex = new Lexer_Math();
            Utilities_Parser_Math utilities_ParserMath = new Utilities_Parser_Math();
            //(string functionName, int startIndex_ParFunc, List<Token> result) resultPar = utilities_ParserMath.getParFunc(ref result);

            CoreCLR_API.ScriptGlobals scriptGlobals = new CoreCLR_API.ScriptGlobals();
            Assert.AreEqual("0.01518435", await Parser_Math.parse("0.123*0.12345", scriptGlobals), "!0.01518435");
            Assert.AreEqual("0.01518435", await Parser_Math.parse(".123*.12345", scriptGlobals), "!0.01518435");
            Assert.AreEqual("0.01518435", await Parser_Math.parse(".12345*.123", scriptGlobals), "!0.01518435");
            Assert.AreEqual("0.01518435", await Parser_Math.parse("-.12345*-.123", scriptGlobals), "--!0.01518435");
            Assert.AreEqual("-0.01518435",await  Parser_Math.parse(".12345*-.123", scriptGlobals), "!-0.01518435");

            Assert.AreEqual("-152402067.44043435",await  Parser_Math.parse("-12345.123*12345.12345", scriptGlobals), "!-152402067.44043435");
            Assert.AreEqual("152402067.44043435",await  Parser_Math.parse("-12345.123*-12345.12345", scriptGlobals), "--!152402067.44043435");
            Assert.AreEqual("152402067.44043435",await  Parser_Math.parse("12345.123*12345.12345", scriptGlobals), "!152402067.44043435");

            Assert.AreEqual("152400259.5", await Parser_Math.parse("12345*12345.1", scriptGlobals), "!152400259.5");
            Assert.AreEqual("-152400259.5",await  Parser_Math.parse("12345*-12345.1", scriptGlobals), "!-152400259.5");
            Assert.AreEqual("152400259.5",await  Parser_Math.parse("-12345*-12345.1", scriptGlobals), "--!152400259.5");

            Assert.AreEqual("152399025", await Parser_Math.parse("-12345*-12345", scriptGlobals), "--!152399025");
            Assert.AreEqual("-152399025",await  Parser_Math.parse("12345*-12345", scriptGlobals), "!-152399025");
            Assert.AreEqual("152399025",await  Parser_Math.parse("(12345)*(12345)", scriptGlobals), "!152399025");
            Assert.AreEqual("24690", await Parser_Math.parse("(12345)+(12345)", scriptGlobals), "!24690");
            Assert.AreEqual("-2",await Parser_Math.parse("-1-1", scriptGlobals),"!-2");
            Assert.AreEqual("0",await Parser_Math.parse("-1--1", scriptGlobals),"!0");


            List<string> TokenResult = await Parser_Math.parse(new List<string>() {

                "1",
                "-2",
                "1+1*2",
                "1+1/2",
                "1+1/2*2",
                "1+1/2*2-1",//5
                "1+1*2+2",
                "2^2+2/2*2",
                "2/2+2/2^2",
                "2*2+2/2^2",
                //basic
                "200*70",//10
                "20*700",
                "200/12.5",
                "12.5/500",
                //par
                "(1+1)/2",
                "1+1/(2+2)",//15
                "(1+1)/(2+2)",
                "(1+1)/((2+2)+4)",
                "(12345)+(12345)",
                //par nested

            },scriptGlobals);

            for (int i = 0; i < TokenResult.Count; i++) {
                int index = TokenResult.Count;
                switch (i)
                {
                    case 0:
                        Assert.AreEqual(double.Parse("1"), double.Parse(TokenResult[i]),"Error::"+i);
                        break;
                    case 1:
                        Assert.AreEqual(double.Parse("-2"), double.Parse(TokenResult[i]), "Error::" + i);
                        break;
                    case 2:
                        Assert.AreEqual(double.Parse("3"), double.Parse(TokenResult[i]), "Error::" + i);
                        break;
                    case 3:
                        Assert.AreEqual(double.Parse("1.5"), double.Parse(TokenResult[i]), "Error::" + i);
                        break;
                    case 4:
                        Assert.AreEqual(double.Parse("2"), double.Parse(TokenResult[i]), "Error::" + i);
                        break;
                    case 5:
                        Assert.AreEqual(double.Parse("1"), double.Parse(TokenResult[i]), "Error::" + i);
                        break;
                    case 6:
                        Assert.AreEqual(double.Parse("5"), double.Parse(TokenResult[i]), "Error::" + i);
                        break;
                    case 7:
                        Assert.AreEqual(double.Parse("6"), double.Parse(TokenResult[i]), "Error::" + i);
                        break;
                    case 8:
                        Assert.AreEqual(double.Parse("1.5"), double.Parse(TokenResult[i]), "Error::" + i);
                        break;
                    case 9:
                        Assert.AreEqual(double.Parse("4.5"), double.Parse(TokenResult[i]), "Error::" + i);
                        break;
                    case 10:
                        Assert.AreEqual(double.Parse("14000"), double.Parse(TokenResult[i]), "Error::" + i);
                        break;
                    case 11:
                        Assert.AreEqual(double.Parse("14000"), double.Parse(TokenResult[i]), "Error::" + i);
                        break;
                    case 12:
                        Assert.AreEqual(double.Parse("16"), double.Parse(TokenResult[i]), "Error::" + i);
                        break;
                    case 13:
                        Assert.AreEqual(double.Parse("0.025"), double.Parse(TokenResult[i]), "Error::" + i);
                        break;
                    case 14:
                        Assert.AreEqual(double.Parse("1"), double.Parse(TokenResult[i]), "Error::" + i);
                        break;
                    case 15:
                        Assert.AreEqual(double.Parse("1.25"), double.Parse(TokenResult[i]), "Error::" + i);
                        break;
                    case 16:
                        Assert.AreEqual(double.Parse("0.5"), double.Parse(TokenResult[i]), "Error::" + i);
                        break;
                    case 17:
                        Assert.AreEqual(double.Parse("0.25"), double.Parse(TokenResult[i]), "Error::" + i);
                        break;
                    case 18:
                        Assert.AreEqual(double.Parse("24690"), double.Parse(TokenResult[i]), "Error::" + i);
                        break;

                    default:
                        break;
                }


            }



            //BASIC

            //with infinity





            //with par


        }

        //[TestMethod]
        //public void testLex_test_GetPar_Utility_And_parseOrderOfOperation()
        //{

        //    //Lexer_Math mathLex = new Lexer_Math();
        //    int index = 0;
        //    string testStr = "1+(1+1)+1";
        //    List<Token> result = Lexer_Math.lex(testStr, index);

        //    //Utilities_Parser_Math utilities_ParserMath = new Utilities_Parser_Math();
        //    List<(string functionName, int startIndex_ParFunc, int endIndex_ParFunc, List<Token> result)> resultPar = Utilities_Parser_Math.getParentParFunc(result);

        //    List<Token> comp_Par = new List<Token>() {
        //        //{new Token(){ value="(", tokenType=TokenType.LPAREN} },
        //        {new Token(){ value="1", tokenType=TokenType.NUMBER} },
        //        {new Token(){ value="+", tokenType=TokenType.PLUS} },
        //        {new Token(){ value="1", tokenType=TokenType.NUMBER} },
        //        //{new Token(){ value=")", tokenType=TokenType.RPAREN} }
        //    };

        //    List<Token> comp_Res = new List<Token>() {
        //        {new Token(){ value="1", tokenType=TokenType.NUMBER} },
        //        {new Token(){ value="+", tokenType=TokenType.PLUS} },
        //        {new Token(){ value="+", tokenType=TokenType.PLUS} },
        //        {new Token(){ value="1", tokenType=TokenType.NUMBER} },
        //    };

        //    Assert.AreEqual(2, resultPar[0].startIndex_ParFunc);
        //    Assert.AreEqual(comp_Par[0].value, resultPar[0].result[0].value);
        //    Assert.AreEqual(comp_Par[0].tokenType, resultPar[0].result[0].tokenType);

        //    Assert.AreEqual(comp_Par[1].value, resultPar[0].result[1].value);
        //    Assert.AreEqual(comp_Par[1].tokenType, resultPar[0].result[1].tokenType);

        //    Assert.AreEqual(comp_Par[2].value, resultPar[0].result[2].value);
        //    Assert.AreEqual(comp_Par[2].tokenType, resultPar[0].result[2].tokenType);

        //    //Assert.AreEqual(comp_Par[3].value, resultPar[3].value);
        //    //Assert.AreEqual(comp_Par[3].tokenType, resultPar[3].tokenType);

        //    //Assert.AreEqual(comp_Par[4].value, resultPar[4].value);
        //    //Assert.AreEqual(comp_Par[4].tokenType, resultPar[4].tokenType);


        //    //===
        //    Assert.AreEqual(comp_Res[0].value, result[0].value);
        //    Assert.AreEqual(comp_Res[0].tokenType, result[0].tokenType);

        //    Assert.AreEqual(comp_Res[1].value, result[1].value);
        //    Assert.AreEqual(comp_Res[1].tokenType, result[1].tokenType);

        //    Assert.AreEqual(comp_Res[2].value, result[2].value);
        //    Assert.AreEqual(comp_Res[2].tokenType, result[2].tokenType);

        //    Assert.AreEqual(comp_Res[3].value, result[3].value);
        //    Assert.AreEqual(comp_Res[3].tokenType, result[3].tokenType);

        //    int index_Order = 0;
        //    Assert.AreEqual("2", Utilities_Parser_Math.basicParse(comp_Par, ref index_Order).value);
        //}


        [TestMethod]
        public void testLex_AbleToGet_AllMathTokens()
        {
            //Lexer_Math mathLex = new Lexer_Math();
            int index = 0;
            string testStr = "1+asd-asd(()/*^123...456+123.123";
            List<Token> result = Lexer_Math.lex(testStr, index);

            int i = 0;
            while (result.Count > 0)
            {

                Token token = result[0];
                result.RemoveAt(0);
                switch (i)
                {
                    case 0:
                        Assert.AreEqual("1", token.value);
                        Assert.AreEqual(TokenType.NUMBER, token.tokenType);
                        break;
                    case 1:
                        Assert.AreEqual("+", token.value);
                        Assert.AreEqual(TokenType.PLUS, token.tokenType);
                        break;
                    case 2:
                        Assert.AreEqual("asd", token.value);
                        Assert.AreEqual(TokenType.STRING, token.tokenType);
                        break;
                    case 3:
                        Assert.AreEqual("-", token.value);
                        Assert.AreEqual(TokenType.MINUS, token.tokenType);
                        break;
                    case 4:
                        Assert.AreEqual("asd(", token.value);
                        Assert.AreEqual(TokenType.FUNCTION, token.tokenType);
                        break;



                    case 5:
                        Assert.AreEqual("(", token.value);
                        Assert.AreEqual(TokenType.LPAREN, token.tokenType);
                        break;

                    case 6:
                        Assert.AreEqual(")", token.value);
                        Assert.AreEqual(TokenType.RPAREN, token.tokenType);
                        break;

                    case 7:
                        Assert.AreEqual("/", token.value);
                        Assert.AreEqual(TokenType.DIVIDE, token.tokenType);
                        break;

                    case 8:
                        Assert.AreEqual("*", token.value);
                        Assert.AreEqual(TokenType.MULTIPLY, token.tokenType);
                        break;

                    case 9:
                        Assert.AreEqual("^", token.value);
                        Assert.AreEqual(TokenType.POWER, token.tokenType);
                        break;

                    case 10:
                        Assert.AreEqual("123...456", token.value);
                        Assert.AreEqual(TokenType.STRING, token.tokenType);
                        break;
                    case 11:
                        Assert.AreEqual("+", token.value);
                        Assert.AreEqual(TokenType.PLUS, token.tokenType);
                        break;
                    case 12:
                        Assert.AreEqual("123.123", token.value);
                        Assert.AreEqual(TokenType.NUMBER, token.tokenType);
                        break;
                    default:
                        break;
                }
                i++;
            }

        }
    }
}
