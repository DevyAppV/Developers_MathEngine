using Developers_MathEngine_Library.Interpreter;
using Developers_MathEngine_Library.Interpreter.Token;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Developers_MathEngine_Library.Interpreter.Utilities
{
    public class Utilities_StringMath
    {


        public static string? evaluate_BasicMath2(
            (TokenType tokenType, string StrVal)? LResult,
            (TokenType tokenType, string StrVal)? Operator,
            (TokenType tokenType, string StrVal)? RResult)
        {
            string? result = "";

            bool minus_AddNeg = false;
            int LResult_ZeroEndAtIndex = -1;

            if (LResult != null && RResult != null)
            {
                if (Operator?.tokenType == TokenType.MINUS ||
                    Operator?.tokenType == TokenType.MULTIPLY)
                {

                    if (
                        !Utilities_String.isLeftStr_LargerThenRightStr_Number(LResult.Value.StrVal, RResult.Value.StrVal))
                    {
                        (TokenType tokenType, string StrVal) temp = LResult.Value;
                        LResult = RResult.Value;
                        RResult = temp;
                        minus_AddNeg = true;
                    }

                    if (LResult != null)
                        LResult_ZeroEndAtIndex = Utilities_String.isStartStr_Zero(LResult.Value.StrVal);
                }
                else if (Operator?.tokenType == TokenType.DIVIDE)
                {
                    string lResultStr = LResult.Value.StrVal;
                    string rResultStr = RResult.Value.StrVal;

                    Utilities_String.setEvenNumLength_With0(ref lResultStr, ref rResultStr);

                    LResult = (TokenType.NUMBER, lResultStr);
                    RResult = (TokenType.NUMBER, rResultStr);
                }
            }

            int? lDecimalIndex = LResult?.StrVal.IndexOf('.');
            int? rDecimalIndex = RResult?.StrVal.IndexOf('.');

            int? lSubString_FrmDecimal_Length = 0;
            int? rSubString_FrmDecimal_Length = 0;

            int? lSubString_ToDecimal_Length = 0;
            int? rSubString_ToDecimal_Length = 0;

            if (lDecimalIndex != null && lDecimalIndex >= 0)
            {
                lSubString_FrmDecimal_Length = LResult?.StrVal.Substring(lDecimalIndex.Value).Length;
                lSubString_ToDecimal_Length = LResult?.StrVal.Substring(0, lDecimalIndex.Value + 1).Length;
            }

            if (rDecimalIndex != null && rDecimalIndex >= 0)
            {
                rSubString_FrmDecimal_Length = RResult?.StrVal.Substring(rDecimalIndex.Value).Length;
                rSubString_ToDecimal_Length = RResult?.StrVal.Substring(0, rDecimalIndex.Value + 1).Length;
            }

            bool isLFromDecimal_LargerThanR = lSubString_FrmDecimal_Length > rSubString_FrmDecimal_Length ? true : false;
            bool isLToDecimal_LargerThanR = lSubString_ToDecimal_Length > rSubString_ToDecimal_Length ? true : false;
            bool isLToDecimal_EqualR = lSubString_ToDecimal_Length == rSubString_ToDecimal_Length ? true : false;

            int? i;

            (string value, string remainder) addResult = ("0", "0");
            string addRemainder = "0";
            int addCount = 0;

            //int RResult_ZeroEndAtIndex;

            //if (RResult != null)
            //    RResult_ZeroEndAtIndex = Utilities_String.isStartStr_Zero(RResult.Value.StrVal);

            switch (Operator?.tokenType)
            {
                case TokenType.PLUS:

                    //ADD FROM DECIMAL TO END
                    if (isLFromDecimal_LargerThanR && lDecimalIndex != -1)
                    {
                        for (i = LResult?.StrVal.Length - 1; i >= 0; i--)
                        {
                            if (LResult != null && LResult.Value.StrVal[i.Value].Equals('.'))
                            {
                                result = result?.Insert(0, ".");
                                //i--;
                                break;
                            }
                            if (lDecimalIndex != -1 && rSubString_FrmDecimal_Length <= i - lDecimalIndex)
                            {
                                if (LResult != null)
                                    result = result?.Insert(0, LResult.Value.StrVal[i.Value].ToString());
                            }
                            else
                            {
                                if (LResult != null && RResult != null)
                                {
                                    string rVal = "0";
                                    if (RResult.Value.StrVal.Count() - 1 - addCount >= 0)
                                    {
                                        rVal = RResult.Value.StrVal[RResult.Value.StrVal.Count() - 1 - addCount].ToString();
                                    }
                                    addResult = Utilities_String.addNumber(LResult.Value.StrVal[i.Value].ToString(), rVal, addRemainder.ToString());
                                    result = result?.Insert(0, addResult.value.ToString());
                                    addRemainder = addResult.remainder;
                                    addCount++;
                                }
                            }
                        }
                    }
                    else if (!isLFromDecimal_LargerThanR && rDecimalIndex != -1)
                    {
                        addCount = 0;
                        for (i = RResult?.StrVal.Length - 1; i >= 0; i--)
                        {
                            if (RResult != null && RResult.Value.StrVal[i.Value].Equals('.'))
                            {
                                result = result?.Insert(0, ".");
                                break;
                            }
                            if (rDecimalIndex != -1 && lSubString_FrmDecimal_Length <= i - rDecimalIndex)
                            {
                                if (RResult != null)
                                    result = result?.Insert(0, RResult.Value.StrVal[i.Value].ToString());
                            }
                            else
                            {
                                if (LResult != null && RResult != null)
                                {
                                    string lVal = "0";
                                    if (LResult.Value.StrVal.Count() - 1 - addCount >= 0)
                                    {
                                        lVal = LResult.Value.StrVal[LResult.Value.StrVal.Count() - 1 - addCount].ToString();
                                    }
                                    addResult = Utilities_String.addNumber(lVal, RResult.Value.StrVal[i.Value].ToString(), addRemainder.ToString());
                                    result = result?.Insert(0, addResult.value.ToString());
                                    addRemainder = addResult.remainder;
                                    addCount++;
                                }
                            }
                        }
                    }

                    //ADD TO DECIMAL FROM START
                    if (isLToDecimal_LargerThanR)
                    {
                        addCount = 0;
                        if (lDecimalIndex == -1)
                        {
                            lDecimalIndex = LResult?.StrVal.Length - 1;
                        }
                        if (rDecimalIndex == -1)
                        {
                            rDecimalIndex = RResult?.StrVal.Length - 1;
                        }
                        for (i = lDecimalIndex; i >= 0; i--)
                        {
                            if (LResult != null && LResult.Value.StrVal[i.Value].Equals('.'))
                            {
                                i--;
                            }
                            if (LResult != null && RResult != null)
                            {
                                int lVal = 0;
                                int rVal = 0;
                                if (rDecimalIndex != null && rDecimalIndex - addCount >= 0)
                                {
                                    if (RResult.Value.StrVal[rDecimalIndex.Value - addCount].Equals('.'))
                                    {
                                        addCount++;
                                    }
                                    if (rDecimalIndex - addCount >= 0)
                                    {
                                        rVal = int.Parse(RResult.Value.StrVal[rDecimalIndex.Value - addCount].ToString());
                                    }
                                }
                                if (i >= 0)
                                {
                                    lVal = int.Parse(LResult.Value.StrVal[i.Value].ToString());
                                }



                                addResult = Utilities_String.addNumber(lVal.ToString(), rVal.ToString(), addRemainder.ToString());

                                result = result?.Insert(0, addResult.value.ToString());
                                addRemainder = addResult.remainder;
                                addCount++;
                            }
                        }
                    }
                    else if (!isLToDecimal_LargerThanR)
                    {
                        addCount = 0;

                        if (lDecimalIndex == -1)
                        {
                            lDecimalIndex = LResult?.StrVal.Length - 1;
                        }
                        if (rDecimalIndex == -1)
                        {
                            rDecimalIndex = RResult?.StrVal.Length - 1;
                        }
                        for (i = rDecimalIndex; i >= 0; i--)
                        {
                            if (RResult != null && RResult.Value.StrVal[i.Value].Equals('.'))
                            {
                                //addCount++;
                                //continue;
                                i--;
                            }
                            if (LResult != null && RResult != null)
                            {

                                int lVal = 0;
                                int rVal = 0;
                                if (lDecimalIndex != null && lDecimalIndex - addCount >= 0)
                                {
                                    if (LResult.Value.StrVal[lDecimalIndex.Value - addCount].Equals('.'))
                                    {
                                        addCount++;
                                    }
                                    if (lDecimalIndex - addCount >= 0)
                                    {
                                        lVal = int.Parse(LResult.Value.StrVal[lDecimalIndex.Value - addCount].ToString());
                                    }
                                }
                                if (i >= 0)
                                {
                                    rVal = int.Parse(RResult.Value.StrVal[i.Value].ToString());
                                }

                                addResult = Utilities_String.addNumber(lVal.ToString(), rVal.ToString(), addRemainder.ToString());
                                result = result?.Insert(0, addResult.value.ToString());
                                addRemainder = addResult.remainder;
                                addCount++;
                            }
                        }
                    }

                    break;
                case TokenType.MINUS:

                    //TODO: FIX SUBTRACTION

                    if (LResult != null && LResult.Value.StrVal.StartsWith('-'))
                    {
                        LResult = (LResult.Value.tokenType, LResult.Value.StrVal.Remove(0, 1));
                        Operator = (TokenType.PLUS, "+");
                        result = "-" + evaluate_BasicMath(LResult, Operator, RResult);
                    }
                    else
                    {

                        if (isLFromDecimal_LargerThanR && lDecimalIndex != -1)
                        {
                            for (i = LResult?.StrVal.Length - 1; i >= 0; i--)
                            {
                                if (LResult != null &&
                                    //i.Value - addCount < LResult.Value.StrVal.Count() &&
                                    LResult.Value.StrVal[i.Value /*- addCount*/].Equals('.'))
                                {
                                    result = result?.Insert(0, ".");
                                    //i--;
                                    continue;
                                }
                                if (lDecimalIndex != -1 && rSubString_FrmDecimal_Length <= i - lDecimalIndex)
                                {
                                    //if (LResult != null)
                                    //    result = result?.Insert(0, LResult.Value.StrVal[i.Value].ToString());

                                    if (LResult != null && RResult != null)
                                    {
                                        string rVal = "0";
                                        string lVal = LResult.Value.StrVal[i.Value].ToString();

                                        if (!Utilities_String.isLeftStr_LargerThenRightStr_Number(lVal, rVal))
                                        {
                                            lVal = lVal.Insert(0, "1");
                                        }
                                        //if (LResult.Value.StrVal.Count() - 1 - addCount >= 0)
                                        //{
                                        //    lVal = LResult.Value.StrVal[LResult.Value.StrVal.Count() - 1 - addCount].ToString();
                                        //}
                                        if (i > LResult_ZeroEndAtIndex || LResult_ZeroEndAtIndex == -1)
                                        {
                                            addResult = Utilities_String.subtractNumber(lVal, rVal, addRemainder.ToString());

                                            addRemainder = addResult.remainder;
                                        }
                                        else
                                            addResult = Utilities_String.subtractNumber(lVal, rVal, 0.ToString());
                                        result = result?.Insert(0, addResult.value.ToString());

                                        //addCount++;
                                    }
                                }
                                else
                                {
                                    if (LResult != null && RResult != null)
                                    {
                                        string rVal = "0";
                                        if (RResult.Value.StrVal.Count() - 1 - addCount >= 0)
                                        {
                                            rVal = RResult.Value.StrVal[RResult.Value.StrVal.Count() - 1 - addCount].ToString();
                                        }


                                        if (i > LResult_ZeroEndAtIndex || LResult_ZeroEndAtIndex == -1)
                                        {
                                            addResult = Utilities_String.subtractNumber(LResult.Value.StrVal[i.Value].ToString(), rVal, addRemainder.ToString());

                                            addRemainder = addResult.remainder;
                                        }
                                        else
                                            addResult = Utilities_String.subtractNumber(LResult.Value.StrVal[i.Value].ToString(), rVal, 0.ToString());
                                        result = result?.Insert(0, addResult.value.ToString());

                                        addCount++;
                                    }
                                }
                            }
                        }
                        else if (!isLFromDecimal_LargerThanR && rDecimalIndex != -1)
                        {
                            addCount = 0;
                            for (i = RResult?.StrVal.Length - 1; i >= 0; i--)
                            {
                                if (RResult != null &&
                                    //i.Value - addCount < RResult.Value.StrVal.Count() &&
                                    RResult.Value.StrVal[i.Value /*- addCount*/].Equals('.'))
                                {
                                    result = result?.Insert(0, ".");
                                    break;
                                }
                                if (rDecimalIndex != -1 && lSubString_FrmDecimal_Length <= i - rDecimalIndex)
                                {
                                    //if (RResult != null)
                                    //{
                                    //    //string rVal = result?.Insert(0, RResult.Value.StrVal[i.Value].ToString());
                                    //    string rVal = RResult.Value.StrVal[i.Value].ToString();
                                    //}
                                    if (LResult != null && RResult != null)
                                    {
                                        string lVal = "0";
                                        string rVal = RResult.Value.StrVal[i.Value].ToString();

                                        if (!Utilities_String.isLeftStr_LargerThenRightStr_Number(lVal, rVal))
                                        {
                                            lVal = lVal.Insert(0, "1");
                                        }
                                        //if (LResult.Value.StrVal.Count() - 1 - addCount >= 0)
                                        //{
                                        //    lVal = LResult.Value.StrVal[LResult.Value.StrVal.Count() - 1 - addCount].ToString();
                                        //}
                                        if (i > LResult_ZeroEndAtIndex || LResult_ZeroEndAtIndex == -1)
                                        {
                                            addResult = Utilities_String.subtractNumber(lVal, rVal, addRemainder.ToString());

                                            addRemainder = addResult.remainder;
                                        }
                                        else
                                            addResult = Utilities_String.subtractNumber(lVal, rVal, 0.ToString());
                                        result = result?.Insert(0, addResult.value.ToString());

                                        //addCount++;
                                    }
                                }
                                else
                                {
                                    if (LResult != null && RResult != null)
                                    {
                                        string lVal = "0";
                                        if (LResult.Value.StrVal.Count() - 1 - addCount >= 0)
                                        {
                                            lVal = LResult.Value.StrVal[LResult.Value.StrVal.Count() - 1 - addCount].ToString();
                                        }

                                        string rVal = RResult.Value.StrVal[i.Value].ToString();

                                        if (!Utilities_String.isLeftStr_LargerThenRightStr_Number(lVal, rVal))
                                        {
                                            lVal = lVal.Insert(0, "1");
                                        }

                                        if (i > LResult_ZeroEndAtIndex || LResult_ZeroEndAtIndex == -1)
                                        {
                                            addResult = Utilities_String.subtractNumber(lVal, rVal, addRemainder.ToString());

                                            addRemainder = addResult.remainder;
                                        }
                                        else
                                            addResult = Utilities_String.subtractNumber(lVal, rVal, 0.ToString());
                                        result = result?.Insert(0, addResult.value.ToString());

                                        addCount++;
                                    }
                                }
                            }
                        }

                        //ADD TO DECIMAL FROM START
                        if (isLToDecimal_LargerThanR && !isLToDecimal_EqualR)
                        {
                            addCount = 0;
                            if (lDecimalIndex == -1)
                            {
                                lDecimalIndex = LResult?.StrVal.Length - 1;
                            }
                            if (rDecimalIndex == -1)
                            {
                                rDecimalIndex = RResult?.StrVal.Length - 1;
                            }
                            for (i = lDecimalIndex; i >= 0; i--)
                            {
                                if (LResult != null && LResult.Value.StrVal[i.Value].Equals('.'))
                                {
                                    i--;
                                }
                                if (LResult != null && RResult != null)
                                {
                                    int lVal = 0;
                                    int rVal = 0;
                                    if (rDecimalIndex != null && rDecimalIndex - addCount >= 0)
                                    {
                                        if (RResult.Value.StrVal[rDecimalIndex.Value - addCount].Equals('.'))
                                        {
                                            addCount++;
                                        }
                                        if (rDecimalIndex - addCount >= 0)
                                        {
                                            if (!RResult.Value.StrVal[rDecimalIndex.Value - addCount].Equals('-'))
                                            {
                                                rVal = int.Parse(RResult.Value.StrVal[rDecimalIndex.Value - addCount].ToString());
                                            }
                                        }
                                    }
                                    if (i >= 0)
                                    {
                                        lVal = int.Parse(LResult.Value.StrVal[i.Value].ToString());
                                    }



                                    if (i > LResult_ZeroEndAtIndex || LResult_ZeroEndAtIndex == -1)
                                    {
                                        //TODO ERROR: if lval not parse == 10, lval parsed = 0, rval ==2, result should be 8 instead of -2
                                        //addResult = Utilities_String.subtractNumber(lVal.ToString(), rVal.ToString(), addRemainder.ToString());
                                        if (Utilities_String.isLeftStr_LargerThenRightStr_Number(lVal.ToString(), rVal.ToString()))
                                        {
                                            addResult = Utilities_String.subtractNumber(lVal.ToString(), rVal.ToString(), addRemainder.ToString());
                                        }
                                        else
                                        {
                                            string lTest = "";
                                            string rTest = "";

                                            if (LResult.Value.StrVal.StartsWith('-'))
                                            {
                                                lTest = LResult.Value.StrVal.Remove(0, 1);
                                            }

                                            if (RResult.Value.StrVal.StartsWith('-'))
                                            {
                                                rTest = RResult.Value.StrVal.Remove(0, 1);
                                            }
                                            if (Utilities_String.isLeftStr_LargerThenRightStr_Number(lTest, rTest))
                                            {
                                                if (!Utilities_String.isLeftStr_LargerThenRightStr_Number(lVal.ToString(), rVal.ToString()))
                                                {
                                                    addResult = Utilities_String.subtractNumber((10 + lVal).ToString(), rVal.ToString(), addRemainder.ToString());

                                                }
                                                else
                                                {
                                                    addResult = Utilities_String.subtractNumber(lVal.ToString(), rVal.ToString(), addRemainder.ToString());
                                                }
                                            }
                                            else
                                            {
                                                addResult = Utilities_String.subtractNumber(lVal.ToString(), rVal.ToString(), addRemainder.ToString());
                                            }

                                        }
                                        addRemainder = addResult.remainder;
                                    }
                                    else
                                        addResult = Utilities_String.subtractNumber(lVal.ToString(), rVal.ToString(), 0.ToString());

                                    //result = result.Remove(0,1);
                                    result = result?.Insert(0, addResult.value.ToString());
                                    addCount++;
                                }
                            }
                        }
                        else if (!isLToDecimal_LargerThanR && !isLToDecimal_EqualR)
                        {
                            addCount = 0;

                            if (lDecimalIndex == -1)
                            {
                                lDecimalIndex = LResult?.StrVal.Length - 1;
                            }
                            if (rDecimalIndex == -1)
                            {
                                rDecimalIndex = RResult?.StrVal.Length - 1;
                            }
                            for (i = rDecimalIndex; i >= 0; i--)
                            {
                                if (RResult != null && RResult.Value.StrVal[i.Value].Equals('.'))
                                {
                                    //addCount++;
                                    //continue;
                                    i--;
                                }
                                if (LResult != null && RResult != null)
                                {

                                    int lVal = 0;
                                    int rVal = 0;
                                    if (lDecimalIndex != null && lDecimalIndex - addCount >= 0)
                                    {
                                        if (LResult.Value.StrVal[lDecimalIndex.Value - addCount].Equals('.'))
                                        {
                                            addCount++;
                                        }
                                        if (lDecimalIndex - addCount >= 0)
                                        {
                                            if (!LResult.Value.StrVal[lDecimalIndex.Value - addCount].Equals('-'))
                                            {
                                                lVal = int.Parse(LResult.Value.StrVal[lDecimalIndex.Value - addCount].ToString());
                                            }
                                        }
                                    }
                                    if (i >= 0)
                                    {
                                        rVal = int.Parse(RResult.Value.StrVal[i.Value].ToString());
                                    }

                                    if (i > LResult_ZeroEndAtIndex || LResult_ZeroEndAtIndex == -1)
                                    {
                                        if (Utilities_String.isLeftStr_LargerThenRightStr_Number(lVal.ToString(), rVal.ToString()))
                                        {
                                            addResult = Utilities_String.subtractNumber(lVal.ToString(), rVal.ToString(), addRemainder.ToString());
                                        }
                                        else
                                        {
                                            string lTest = "";
                                            string rTest = "";

                                            if (LResult.Value.StrVal.StartsWith('-'))
                                            {
                                                lTest = LResult.Value.StrVal.Remove(0, 1);
                                            }

                                            if (RResult.Value.StrVal.StartsWith('-'))
                                            {
                                                rTest = RResult.Value.StrVal.Remove(0, 1);
                                            }
                                            if (Utilities_String.isLeftStr_LargerThenRightStr_Number(lTest, rTest))
                                            {
                                                if (!Utilities_String.isLeftStr_LargerThenRightStr_Number(lVal.ToString(), rVal.ToString()))
                                                {
                                                    addResult = Utilities_String.subtractNumber((10 + lVal).ToString(), rVal.ToString(), addRemainder.ToString());

                                                }
                                                else
                                                {
                                                    addResult = Utilities_String.subtractNumber(lVal.ToString(), rVal.ToString(), addRemainder.ToString());
                                                }
                                            }
                                            else
                                            {
                                                addResult = Utilities_String.subtractNumber(lVal.ToString(), rVal.ToString(), addRemainder.ToString());
                                            }

                                        }
                                        addRemainder = addResult.remainder;
                                    }
                                    else
                                        addResult = Utilities_String.subtractNumber(lVal.ToString(), rVal.ToString(), 0.ToString());

                                    //result = result.Remove(0, 1);
                                    result = result?.Insert(0, addResult.value.ToString());
                                    addCount++;
                                }
                            }
                        }
                        else if (string.IsNullOrEmpty(result))
                        {
                            result = "0";
                        }

                        if (double.Parse(addRemainder) > 0 || minus_AddNeg)
                        {
                            //addRemainder = 0;
                            result = result?.Insert(0, "-");
                        }


                    }
                    break;
                case TokenType.MULTIPLY:

                    if (LResult != null && RResult != null)
                    {
                        int lDecimalPlace = 0;
                        int rDecimalPlace = 0;
                        int lDecimalPlaceCount = 0;
                        int rDecimalPlaceCount = 0;
                        List<string> multiplyResult_List = new List<string>();
                        int count = LResult.Value.StrVal.Count() > RResult.Value.StrVal.Count() ? LResult.Value.StrVal.Count() : RResult.Value.StrVal.Count();
                        if (LResult.Value.StrVal.Replace(".", "").Replace("-", "").Count() >= RResult.Value.StrVal.Replace(".", "").Replace("-", "").Count())
                        {
                            int addedZeros = 0;
                            for (i = count - 1; i >= 0; i--)
                            {
                                if (i < RResult.Value.StrVal.Count())
                                {
                                    if (RResult.Value.StrVal[i.Value].Equals('.') || RResult.Value.StrVal[i.Value].Equals('-'))
                                    {
                                        if (RResult.Value.StrVal[i.Value].Equals('.') && rDecimalPlace == 0)
                                            rDecimalPlace = rDecimalPlaceCount;
                                        continue;
                                    }
                                    else
                                    {

                                        result = "";

                                        for (int q = count - 1; q >= 0; q--)
                                        {
                                            if (q < LResult.Value.StrVal.Count())
                                            {
                                                if (LResult.Value.StrVal[q].Equals('.') || LResult.Value.StrVal[q].Equals('-'))
                                                {
                                                    if (LResult.Value.StrVal[q].Equals('.') && lDecimalPlace == 0)
                                                        lDecimalPlace = lDecimalPlaceCount;
                                                    continue;
                                                }
                                                else
                                                {
                                                    addResult = Utilities_String.multiplyNumber(LResult.Value.StrVal[q].ToString(), RResult.Value.StrVal[i.Value].ToString(), addRemainder.ToString());
                                                    addRemainder = addResult.remainder;
                                                    result = result.Insert(0, addResult.value.ToString());
                                                }
                                            }
                                            lDecimalPlaceCount++;
                                        }

                                        int zeroCount = addedZeros;
                                        while (zeroCount > 0)
                                        {
                                            result += "0";
                                            zeroCount--;
                                        }
                                        if (double.Parse(addRemainder) != 0)
                                        {
                                            result = result.Insert(0, addRemainder.ToString());
                                            addRemainder = "0";
                                        }

                                        multiplyResult_List.Add(result);

                                        addedZeros++;

                                    }
                                    rDecimalPlaceCount++;
                                }
                            }
                        }
                        else
                        {
                            int addedZeros = 0;
                            for (i = count - 1; i >= 0; i--)
                            {
                                if (i < LResult.Value.StrVal.Count())
                                {
                                    if (LResult.Value.StrVal[i.Value].Equals('.') || LResult.Value.StrVal[i.Value].Equals('-'))
                                    {
                                        if (LResult.Value.StrVal[i.Value].Equals('.') && lDecimalPlace == 0)
                                            lDecimalPlace = lDecimalPlaceCount;
                                        continue;
                                    }
                                    else
                                    {

                                        result = "";

                                        for (int q = count - 1; q >= 0; q--)
                                        {
                                            if (q < RResult.Value.StrVal.Count())
                                            {
                                                if (RResult.Value.StrVal[q].Equals('.') || RResult.Value.StrVal[q].Equals('-'))
                                                {
                                                    if (RResult.Value.StrVal[q].Equals('.') && rDecimalPlace == 0)
                                                        rDecimalPlace = rDecimalPlaceCount;
                                                    continue;
                                                }
                                                else
                                                {
                                                    addResult = Utilities_String.multiplyNumber(LResult.Value.StrVal[i.Value].ToString(), RResult.Value.StrVal[q].ToString(), addRemainder.ToString());
                                                    addRemainder = addResult.remainder;
                                                    result = result.Insert(0, addResult.value.ToString());
                                                }
                                            }
                                            rDecimalPlaceCount++;
                                        }

                                        int zeroCount = addedZeros;
                                        while (zeroCount > 0)
                                        {
                                            result += "0";
                                            zeroCount--;
                                        }

                                        if (double.Parse(addRemainder) != 0)
                                        {
                                            result = result.Insert(0, addRemainder.ToString());
                                            addRemainder = "0";
                                        }

                                        multiplyResult_List.Add(result);
                                        addedZeros++;

                                    }
                                    lDecimalPlaceCount++;
                                }

                            }
                        }


                        if (multiplyResult_List.Count() > 1)
                        {
                            (TokenType tokenType, string strToCmp) l = (TokenType.NONE, "");
                            (TokenType tokenType, string strToCmp) r = (TokenType.NONE, "");
                            for (i = 0; i < multiplyResult_List.Count(); i++)
                            {
                                if (i == 0)
                                {
                                    l = (TokenType.NUMBER, multiplyResult_List[i.Value]);
                                    if ((++i) >= multiplyResult_List.Count())
                                        throw new Exception("Uneven number of operands");
                                    r = (TokenType.NUMBER, multiplyResult_List[i.Value]);
                                }
                                else
                                {
                                    //l = (TokenType.Math_Number, multiplyResult_List[i.Value]);
                                    r = (TokenType.NUMBER, multiplyResult_List[i.Value]);
                                }
                                (TokenType tokenType, string strToCmp) o = (TokenType.PLUS, "+");



                                if (i == 1)
                                {
                                    result = evaluate_BasicMath(l, o, r);
                                }
                                else if (result != null)
                                {
                                    result = evaluate_BasicMath((TokenType.NUMBER, result), o, r);
                                }
                            }
                        }




                        int decimalPlaces = lDecimalPlace + rDecimalPlace;
                        if (result != null && decimalPlaces > 0)
                        {
                            while (result.Count() <= decimalPlaces)
                            {
                                result = result.Insert(0, "0");
                            }
                            result = result.Insert(1, ".");
                        }
                    }


                    //result = (double.Parse(LResult.Value.StrVal) * double.Parse(RResult.Value.StrVal)).ToString();
                    break;
                case TokenType.DIVIDE:

                    if (LResult != null && RResult != null)
                    {
                        if (string.IsNullOrEmpty(RResult.Value.StrVal.Trim('0').Replace(".", "")))
                        {
                            throw new Exception("Division by zero");
                        }

                        string lVal = LResult.Value.StrVal.Replace("-", "").Replace(".", "").TrimStart('0');
                        string rVal = RResult.Value.StrVal.Replace("-", "").Replace(".", "").TrimStart('0');

                        if (string.IsNullOrEmpty(lVal)) lVal = "0";
                        if (string.IsNullOrEmpty(rVal)) rVal = "0";

                        int countSmallest = lVal.Count() < rVal.Count() ? lVal.Count() : rVal.Count();
                        int countLargest = lVal.Count() < rVal.Count() ? lVal.Count() : rVal.Count();
                        bool isLValLargerThanEqRVal = lVal.Count() >= rVal.Count() ? true : false;

                        int divideLFromIndex = 0;

                        string calcLVal = "";
                        string calcRVal = "";
                        addRemainder = "-1";
                        while (double.Parse(addRemainder) != 0)
                        {
                            if (isLValLargerThanEqRVal)
                            {
                                if (divideLFromIndex == 0)
                                {
                                    calcLVal = lVal.Substring(divideLFromIndex, countSmallest);
                                    divideLFromIndex += countSmallest;
                                }
                                else
                                {
                                    calcLVal = addRemainder.ToString();
                                    addRemainder = "0";
                                    divideLFromIndex += 1;
                                }
                                calcRVal = rVal.Substring(0, countSmallest);

                                if (divideLFromIndex != 0)
                                {
                                    while (calcLVal.Count() < divideLFromIndex)
                                    {
                                        if (divideLFromIndex < lVal.Count())
                                        {
                                            calcLVal += lVal.Substring(divideLFromIndex, 1);
                                        }
                                        else
                                        {
                                            calcLVal += "0";
                                        }
                                    }
                                }
                            }
                            else
                            {

                                if (divideLFromIndex == 0)
                                {
                                    calcLVal += lVal.Substring(divideLFromIndex, countSmallest);
                                    divideLFromIndex += countSmallest;
                                    while (calcLVal.Count() < divideLFromIndex)
                                    {
                                        if (divideLFromIndex < lVal.Count())
                                        {
                                            calcLVal += lVal.Substring(divideLFromIndex, 1);
                                            divideLFromIndex += 0;
                                        }
                                        else
                                        {
                                            calcLVal += "0";
                                        }

                                    }
                                }
                                else
                                {
                                    calcLVal = addRemainder.ToString();
                                    addRemainder = "0";
                                }
                                calcRVal = rVal.Substring(0, countLargest);
                                divideLFromIndex += countLargest;

                                if (divideLFromIndex != 0)
                                {
                                    while (calcLVal.Count() < divideLFromIndex)
                                    {
                                        if (divideLFromIndex < lVal.Count())
                                        {
                                            calcLVal += lVal.Substring(divideLFromIndex, 1);
                                        }
                                        else
                                        {
                                            calcLVal += "0";
                                        }
                                    }
                                }
                            }

                            addResult = Utilities_String.divideString(calcLVal, calcRVal);
                            addRemainder = addResult.remainder;
                            result += addResult.value.ToString();
                            if (string.IsNullOrEmpty(result.Trim('0')))
                            {
                                result = "";
                            }

                        }

                        if (lDecimalIndex != null && rDecimalIndex != null)
                        {
                            bool add0 = false;

                            //result = result.Trim('0');
                            if (lDecimalIndex.Value < 0)
                            {
                                lDecimalIndex = LResult.Value.StrVal.Count();
                            }
                            else if (lDecimalIndex.Value == 0)
                            {
                                add0 = true;
                                result = result.Insert(0, "0");
                                lDecimalIndex = 1;
                            }

                            if (rDecimalIndex.Value < 0)
                            {
                                rDecimalIndex = RResult.Value.StrVal.Count();
                            }
                            else if (rDecimalIndex.Value == 0)
                            {
                                if (!add0)
                                {
                                    result = result.Insert(0, "0");
                                }
                                rDecimalIndex = 1;
                            }


                            int zeroCount = lDecimalIndex.Value - rDecimalIndex.Value;


                            if (zeroCount > 0)
                            {

                                zeroCount++;
                                while (zeroCount > 0)
                                {
                                    result = result.Insert(0, "0");
                                    zeroCount--;
                                }

                                result = result.Insert(0, ".").TrimStart('0');
                            }
                            else if (zeroCount < 0)
                            {

                                zeroCount--;
                                while (zeroCount < 0)
                                {
                                    result += "0";
                                    zeroCount++;
                                }
                                result += ".".Trim('0');
                            }
                            else if (zeroCount == 0 && lDecimalIndex.Value < result.Length)
                            {
                                result = result.Insert(lDecimalIndex.Value, ".");
                            }




                            //result = result.Insert(lDecimalIndex.Value / rDecimalIndex.Value, ".").TrimStart('0');
                            result = result.Insert(0, result.StartsWith(".") ? "0" : "");

                            if (LResult.Value.StrVal.StartsWith("-") && !RResult.Value.StrVal.StartsWith("-") ||
                                !LResult.Value.StrVal.StartsWith("-") && RResult.Value.StrVal.StartsWith("-"))
                            {
                                if (lDecimalIndex != null && rDecimalIndex != null)
                                {
                                    result = result.Insert(0, "-");
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(result))
                        {
                            result = "0";
                        }
                    }

                    //result = (double.Parse(LResult.Value.StrVal) / double.Parse(RResult.Value.StrVal)).ToString();

                    break;
                case TokenType.POWER:

                    //result = Math.Pow(double.Parse(LResult.Value.StrVal), double.Parse(RResult.Value.StrVal)).ToString();

                    result = LResult.Value.StrVal;
                    for (string pow = "1";
                        pow != RResult.Value.StrVal;
                        pow = evaluate_BasicMath((TokenType.NUMBER, pow), (TokenType.PLUS, "+"), (TokenType.NUMBER, "1")))
                    {
                        result = evaluate_BasicMath((TokenType.NUMBER, result), (TokenType.MULTIPLY, "*"), (TokenType.NUMBER, result));
                    }
                    if (RResult.Value.StrVal.Equals("0"))
                    {
                        result = "1";
                    }

                    break;
                    //case TokenType.Math_Modulus://first - acts as function parenthesis

                    //    result = (double.Parse(LResult.Value.StrVal) % double.Parse(RResult.Value.StrVal)).ToString();
                    //    break;
                    //case TokenType.Math_Sqrt:
                    //    result = Math.Sqrt(double.Parse(RResult.Value.StrVal)).ToString();
                    //    break;
                    //    //case TokenType.Math_Number:

                    //    break;
            }

            return result;

        }

        public static string? evaluate_BasicMath(
            (TokenType tokenType, string StrVal)? LResult,
            (TokenType tokenType, string StrVal)? Operator,
            (TokenType tokenType, string StrVal)? RResult)
        {

            string operand1LNum = "";
            string operand1RNum = "";

            string operand2LNum = "";
            string operand2RNum = "";

            string lResDecIndex = Utilities_String.indexOfStr(LResult.Value.StrVal, '.');
            if (!lResDecIndex.Equals("-1"))
            {
                operand1LNum = Utilities_String.removeString(LResult.Value.StrVal, Utilities_String.indexOfStr(LResult.Value.StrVal, '.'));
                operand1RNum = Utilities_String.removeString(LResult.Value.StrVal, "0", Utilities_String.indexOfStr(LResult.Value.StrVal, '.'));
            }
            else
            {
                operand1LNum = LResult.Value.StrVal;
            }
            string rResDecIndex = Utilities_String.indexOfStr(RResult.Value.StrVal, '.');
            if (!rResDecIndex.Equals("-1"))
            {
                operand2LNum = Utilities_String.removeString(RResult.Value.StrVal, Utilities_String.indexOfStr(RResult.Value.StrVal, '.'));
                operand2RNum = Utilities_String.removeString(RResult.Value.StrVal, "0", Utilities_String.indexOfStr(RResult.Value.StrVal, '.'));
            }
            else
            {
                operand2LNum = RResult.Value.StrVal;
            }




            bool isOperand1Neg = false;
            bool isOperand2Neg = false;
            if (operand1LNum.StartsWith('-'))
            {
                isOperand1Neg = true;
                operand1LNum = operand1LNum.Remove(0, 1);
            }
            if (operand2LNum.StartsWith('-'))
            {
                isOperand2Neg = true;
                operand2LNum = operand2LNum.Remove(0, 1);
            }


            string operandNum_WO_Neg_L = operand1LNum + "." + operand1RNum;
            string operandNum_WO_Neg_R = operand2LNum + "." + operand2RNum;




            string operand1NumCombined = operand1LNum + operand1RNum;
            string operand2NumCombined = operand2LNum + operand2RNum;

            Stack<char> operand1NumCombined_Stack = new Stack<char>(operand1NumCombined);
            Stack<char> operand2NumCombined_Stack = new Stack<char>(operand2NumCombined);


            //string operand1NumCombined_EqLen = operand1LNum + operand1RNum;
            //string operand2NumCombined_EqLen = operand2LNum + operand2RNum;

            //Utilities_String.setEvenNumLength_With0(ref operand1NumCombined_EqLen, ref operand2NumCombined_EqLen, false);

            //Stack<char> operand1NumCombined_EqLen_Stack = new Stack<char>(operand1NumCombined_EqLen);
            //Stack<char> operand2NumCombined_EqLen_Stack = new Stack<char>(operand2NumCombined_EqLen);

            string operand1LNumEqLen = operand1LNum;
            string operand1RNumEqLen = operand1RNum;

            string operand2LNumEqLen = operand2LNum;
            string operand2RNumEqLen = operand2RNum;


            Utilities_String.setEvenNumLength_With0(ref operand1LNumEqLen, ref operand2LNumEqLen, true);
            Utilities_String.setEvenNumLength_With0(ref operand1RNumEqLen, ref operand2RNumEqLen, false);

            Stack<char> operand1NumCombined_EqLen_Stack = new Stack<char>(operand1LNumEqLen + operand1RNumEqLen);
            Stack<char> operand2NumCombined_EqLen_Stack = new Stack<char>(operand2LNumEqLen + operand2RNumEqLen);


            Stack<char> operand1LNumEqLen_Stack = new Stack<char>(operand1LNum);
            Stack<char> operand1RNumEqLen_Stack = new Stack<char>(operand1RNum);

            Stack<char> operand2LNumEqLen_Stack = new Stack<char>(operand2LNum);
            Stack<char> operand2RNumEqLen_Stack = new Stack<char>(operand2RNum);

            string remainder = "0";

            (string value, string remainder) calcResult = ("0", "0");
            (string value, string remainder) result = ("", "0");

            switch (Operator.Value.tokenType)
            {
                case TokenType.PLUS:

                    if ((isOperand1Neg && isOperand2Neg) || (!isOperand1Neg && !isOperand2Neg))
                    {
                        //string pop1 = "";
                        //string pop2 = "";
                        //rightCalc
                        while (Utilities_String.isCountStr_GreaterThan(operand1RNumEqLen_Stack, "0") ||
                            Utilities_String.isCountStr_GreaterThan(operand2RNumEqLen_Stack, "0"))
                        {
                            //bool isPop1 = operand1RNumEqLen_Stack.TryPop(out char pop1);
                            //bool isPop2 = operand2RNumEqLen_Stack.TryPop(out char pop2);

                            bool isPop1 = operand1RNumEqLen_Stack.TryPop(out char pop1);
                            bool isPop2 = operand2RNumEqLen_Stack.TryPop(out char pop2);
                            if (isPop1 && isPop2)
                            {
                                calcResult = Utilities.Utilities_String.addNumber(pop1.ToString(), pop2.ToString(), remainder);

                                result.value = result.value.Insert(0, calcResult.value);
                            }
                            else if (isPop1)
                            {
                                calcResult = Utilities.Utilities_String.addNumber("0", pop1.ToString(), remainder);
                                result.value = result.value.Insert(0, calcResult.value);
                                while (operand1RNumEqLen_Stack.TryPop(out pop1))
                                {
                                    calcResult = Utilities.Utilities_String.addNumber("0", pop1.ToString(), remainder);

                                    result.value = result.value.Insert(0, calcResult.value);
                                }
                            }
                            else if (isPop2)
                            {
                                calcResult = Utilities.Utilities_String.addNumber("0", pop2.ToString(), remainder);
                                result.value = result.value.Insert(0, calcResult.value);
                                while (operand2RNumEqLen_Stack.TryPop(out pop2))
                                {
                                    calcResult = Utilities.Utilities_String.addNumber("0", pop2.ToString(), remainder);

                                    result.value = result.value.Insert(0, calcResult.value);
                                }
                            }
                        }

                        result.value = result.value.Insert(0, ".");

                        //leftCalc
                        while (Utilities_String.isCountStr_GreaterThan(operand1LNumEqLen_Stack, "0") ||
                            Utilities_String.isCountStr_GreaterThan(operand2LNumEqLen_Stack, "0"))
                        {
                            //calcResult = Utilities.Utilities_String.addNumber(operand1LNumEqLen_Stack.Pop().ToString(), operand2LNumEqLen_Stack.Pop().ToString(), remainder);

                            //result.value = result.value.Insert(0, calcResult.value);


                            bool isPop1 = operand1LNumEqLen_Stack.TryPop(out char pop1);
                            bool isPop2 = operand2LNumEqLen_Stack.TryPop(out char pop2);
                            if (isPop1 && isPop2)
                            {
                                calcResult = Utilities.Utilities_String.addNumber(pop1.ToString(), pop2.ToString(), remainder);

                                result.value = result.value.Insert(0, calcResult.value);
                            }
                            else if (isPop1)
                            {
                                calcResult = Utilities.Utilities_String.addNumber("0", pop1.ToString(), remainder);
                                result.value = result.value.Insert(0, calcResult.value);
                                while (operand1LNumEqLen_Stack.TryPop(out pop1))
                                {
                                    calcResult = Utilities.Utilities_String.addNumber("0", pop1.ToString(), remainder);

                                    result.value = result.value.Insert(0, calcResult.value);
                                }
                            }
                            else if (isPop2)
                            {
                                calcResult = Utilities.Utilities_String.addNumber("0", pop2.ToString(), remainder);
                                result.value = result.value.Insert(0, calcResult.value);
                                while (operand2LNumEqLen_Stack.TryPop(out pop2))
                                {
                                    calcResult = Utilities.Utilities_String.addNumber("0", pop2.ToString(), remainder);

                                    result.value = result.value.Insert(0, calcResult.value);
                                }
                            }
                        }

                        if (!remainder.Equals("0"))
                        {
                            result.value = result.value.Insert(0, "0");
                            remainder = "0";
                        }

                        if (isOperand1Neg && isOperand2Neg)
                        {
                            result.value = result.value.Insert(0, "-");
                        }
                    }
                    else if (!isOperand1Neg && isOperand2Neg)
                    {
                        result.value = evaluate_BasicMath(
                            (TokenType.NUMBER, operandNum_WO_Neg_R),
                            (TokenType.MINUS, "-"),
                            (TokenType.NUMBER, operandNum_WO_Neg_L)
                            );
                    }
                    else if (isOperand1Neg && !isOperand2Neg)
                    {
                        result.value = evaluate_BasicMath(
                            (TokenType.NUMBER, operandNum_WO_Neg_L),
                            (TokenType.MINUS, "-"),
                            (TokenType.NUMBER, operandNum_WO_Neg_R)
                            );
                    }

                    break;
                case TokenType.MINUS:
                    //---=-+
                    //--+=--
                    //+-+=-+
                    //+--=--

                    if ((isOperand1Neg && isOperand2Neg) || (!isOperand1Neg && !isOperand2Neg))
                    {
                        //operand1NumCombined_Stack

                        string pop1 = "";
                        string pop2 = "";
                        bool isPop1 = operand1NumCombined_EqLen_Stack.TryPop(out char pop1c);
                        bool isPop2 = operand2NumCombined_EqLen_Stack.TryPop(out char pop2c);

                        if (isPop1 && isPop2)
                        {
                            if (Utilities_String.isLeftStr_LargerThenEqualToRightStr_Number(operand1LNum, operand2LNum))
                            {
                                do
                                {
                                    pop1 = pop1c.ToString();
                                    pop2 = pop2c.ToString();

                                    if (!Utilities_String.isLeftStr_LargerThenEqualToRightStr_Number(pop1.ToString(), pop2.ToString()))
                                    {
                                        pop1 = pop1.Insert(0, "1");
                                    }

                                    calcResult = Utilities.Utilities_String.subtractNumber(pop1.ToString(), pop2.ToString(), remainder);
                                    remainder = calcResult.remainder;

                                    result.value = result.value.Insert(0, calcResult.value);

                                    isPop1 = operand1NumCombined_EqLen_Stack.TryPop(out pop1c);
                                    isPop2 = operand2NumCombined_EqLen_Stack.TryPop(out pop2c);
                                } while (isPop1 && isPop2);

                                while (isPop1)
                                {
                                    pop1 = pop1c.ToString();
                                    calcResult = Utilities.Utilities_String.subtractNumber(pop1.ToString(), "0", remainder);
                                    remainder = calcResult.remainder;

                                    result.value = result.value.Insert(0, calcResult.value);


                                    isPop1 = operand1NumCombined_EqLen_Stack.TryPop(out pop1c);

                                }
                                if (isOperand1Neg && isOperand2Neg)
                                {
                                    result.value = result.value.Insert(0, "-");
                                }

                                result.value = Utilities_String.insertAt_FromLast(result.value, Utilities_String.countStr(operand1RNumEqLen), ".");

                            }
                            else
                            {

                                do
                                {
                                    pop1 = pop1c.ToString();
                                    pop2 = pop2c.ToString();

                                    if (!Utilities_String.isLeftStr_LargerThenEqualToRightStr_Number(pop2.ToString(), pop1.ToString()))
                                    {
                                        pop2 = pop2.Insert(0, "1");
                                    }

                                    calcResult = Utilities.Utilities_String.subtractNumber(pop2.ToString(), pop1.ToString(), remainder);
                                    remainder = calcResult.remainder;

                                    result.value = result.value.Insert(0, calcResult.value);

                                    isPop1 = operand1NumCombined_EqLen_Stack.TryPop(out pop1c);
                                    isPop2 = operand2NumCombined_EqLen_Stack.TryPop(out pop2c);
                                } while (isPop1 && isPop2);

                                while (isPop2)
                                {
                                    pop2 = pop2c.ToString();
                                    calcResult = Utilities.Utilities_String.subtractNumber(pop2.ToString(), "0", remainder);
                                    remainder = calcResult.remainder;

                                    result.value = result.value.Insert(0, calcResult.value);


                                    isPop2 = operand1NumCombined_EqLen_Stack.TryPop(out pop2c);

                                }
                                if (!isOperand1Neg && !isOperand2Neg)
                                {
                                    result.value = result.value.Insert(0, "-");
                                }

                                result.value = Utilities_String.insertAt_FromLast(result.value, Utilities_String.countStr(operand1RNumEqLen), ".");

                            }
                        }





                    }
                    else if (!isOperand1Neg && isOperand2Neg)
                    {
                        result.value = evaluate_BasicMath(
                            (TokenType.NUMBER, operandNum_WO_Neg_L),
                            (TokenType.PLUS, "+"),
                            (TokenType.NUMBER, operandNum_WO_Neg_R)
                            );

                    }
                    else if (isOperand1Neg && !isOperand2Neg)
                    {
                        result.value = evaluate_BasicMath(
                            (TokenType.NUMBER, "-" + operandNum_WO_Neg_L),
                            (TokenType.PLUS, "+"),
                            (TokenType.NUMBER, "-" + operandNum_WO_Neg_R)
                            );

                    }



                    break;
                case TokenType.MULTIPLY:


                    string zerosToAdd = "0";
                    Queue<string> multValueList = new Queue<string>();
                    while (Utilities_String.isCountStr_GreaterThan(operand1NumCombined_Stack, "0"))
                    {
                        string multValue = "";

                        char operandL = operand1NumCombined_Stack.Pop();


                        while (Utilities_String.isCountStr_GreaterThan(operand2NumCombined_Stack, "0"))
                        {
                            char operandR = operand2NumCombined_Stack.Pop();
                            calcResult = Utilities_String.multiplyNumber(operandL.ToString(), operandR.ToString(), remainder);



                            multValue = multValue.Insert(0, calcResult.value);

                        }

                        if (calcResult.remainder != "0")
                        {
                            calcResult = Utilities_String.addNumber("0", "0", calcResult.remainder);
                            calcResult.remainder = "0";
                            multValue = multValue.Insert(0, calcResult.value);
                        }


                        multValue = Utilities_String.addZeroStoString(multValue, zerosToAdd, true);
                        multValueList.Enqueue(multValue);
                        //zerosToAdd = evaluate_BasicMath(
                        //    (TokenType.NUMBER, zerosToAdd),
                        //    (TokenType.PLUS, "+"),
                        //    (TokenType.NUMBER, "1")
                        //    );
                        zerosToAdd = Utilities_String.increment(zerosToAdd, '1');
                        operand2NumCombined_Stack = new Stack<char>(operand2NumCombined);
                    }


                    string multVal1 = "0";
                    string multVal2 = "0";
                    while (Utilities_String.isCountStr_GreaterThan<string>(multValueList, "0"))
                    {
                        multVal1 = multValueList.Dequeue();

                        multVal2 = evaluate_BasicMath(
                            (TokenType.NUMBER, multVal1),
                            (TokenType.PLUS, "+"),
                            (TokenType.NUMBER, multVal2)
                            );
                    }
                    result.value = multVal2;


                    if ((!isOperand1Neg && isOperand2Neg) || (isOperand1Neg && !isOperand2Neg))
                    {
                        result.value = result.value.Insert(0, "-");
                    }

                    string decimalFromLast = evaluate_BasicMath(
                            (TokenType.NUMBER, Utilities_String.countStr(operand1RNum)),
                            (TokenType.PLUS, "+"),
                            (TokenType.NUMBER, Utilities_String.countStr(operand2RNum))
                            );

                    result.value = Utilities_String.insertAt_FromLast(result.value, decimalFromLast, ".");

                    break;
                case TokenType.DIVIDE:


                    calcResult = Utilities_String.divideString(operand1NumCombined, operand2NumCombined);


                    if (Utilities_String.isCountStr_GreaterThan(operand2LNum, Utilities_String.countStr(operand1LNum)))
                    {
                        result.value = Utilities_String.addZeroStoString(
                            calcResult.value,

                            evaluate_BasicMath(
                            (TokenType.NUMBER, Utilities_String.countStr(operand2LNum)),
                            (TokenType.MINUS, "-"),
                            (TokenType.NUMBER, Utilities_String.countStr(operand1LNum))
                            ), false);

                        //if (!Utilities_String.isLeftStr_LargerThenEqualToRightStr_Number(operand2LNumEqLen + operand2RNumEqLen, operand1LNumEqLen + operand1RNumEqLen))
                        //{
                            result.value = Utilities_String.insertAt(result.value, "0", "0.");
                        //}
                        //else
                        //{
                        //    result.value = Utilities_String.insertAt(result.value, "0", ".");
                        //}

                    }
                    else if (!Utilities_String.isCountStr_GreaterThan(operand2LNum, Utilities_String.countStr(operand1LNum)))
                    {
                        if (!Utilities_String.isLeftStr_LargerThenRightStr_Number(operand2LNum, operand1LNum))
                        {
                            result.value = Utilities_String.insertAt(calcResult.value,
                                Utilities_String.increment(evaluate_BasicMath(
                                (TokenType.NUMBER, Utilities_String.countStr(operand1LNum)),
                                (TokenType.MINUS, "-"),
                                (TokenType.NUMBER, Utilities_String.countStr(operand2LNum))
                                ), '1'), "."
                                );
                        }
                        else
                        {
                            result.value = Utilities_String.insertAt(calcResult.value,
                                evaluate_BasicMath(
                                (TokenType.NUMBER, Utilities_String.countStr(operand1LNum)),
                                (TokenType.MINUS, "-"),
                                (TokenType.NUMBER, Utilities_String.countStr(operand2LNum))), "."
                                );
                        }
                    }

                    if ((!isOperand1Neg && isOperand2Neg) || (isOperand1Neg && !isOperand2Neg))
                    {
                        result.value = result.value.Insert(0, "-");
                    }

                    break;
                case TokenType.POWER:

                    result.value = LResult.Value.StrVal;
                    for (string pow = "1";
                        pow != RResult.Value.StrVal;
                        pow = evaluate_BasicMath((TokenType.NUMBER, pow), (TokenType.PLUS, "+"), (TokenType.NUMBER, "1")))
                    {
                        result.value = evaluate_BasicMath(
                            (TokenType.NUMBER, result.value),
                            (TokenType.MULTIPLY, "*"),
                            (TokenType.NUMBER, result.value));
                    }
                    if (RResult.Value.StrVal.Equals("0"))
                    {
                        result.value = "1";
                    }



                    break;
            }


            if (!string.IsNullOrEmpty(result.value) && 
                (result.value.Equals("-0") ||
               result.value.Equals("-0.")))
            {
                return "0";
            }
            if (!string.IsNullOrEmpty(result.value) && result.value.Last().Equals('.'))
            {
                result.value = Utilities_String.removeString(result.value, Utilities_String.indexOfStr(result.value, result.value.Last()));
            }
            return result.value;
        }
    }
}
