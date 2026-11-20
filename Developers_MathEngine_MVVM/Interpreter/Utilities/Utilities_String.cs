using Developers_MathEngine_MVVM.Interpreter.Token;
using Developers_MathEngine_MVVM.Interpreter.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Pickers.Provider;

namespace Developers_MathEngine_MVVM.Interpreter.Utilities
{
    public class Utilities_String
    {
        public static string getSubString(string refStr, string start_Inclusive, string end_Exclusive, bool isGetToDecimal_Exclusive = false/*, bool fillZero = false*/)
        {
            string count = "0";
            StringBuilder s = new StringBuilder();
            bool isSubStr = false;
            foreach (char c in refStr)
            {
                if (isGetToDecimal_Exclusive && c.Equals('.'))
                {
                    break;
                }
                if (count.Equals(start_Inclusive))
                {
                    isSubStr = true;
                }
                if (isSubStr)
                {
                    s = s.Append(c);
                }
                else if (!end_Exclusive.Equals("infinite"))
                {
                    if (count.Equals(end_Exclusive))
                    {
                        break;
                    }
                    count = increment(count, '1');
                }
                if (!isSubStr && end_Exclusive.Equals("infinite"))
                    count = increment(count, '1');
            }
            //if (fillZero)
            //{
            //    for (; !count.Equals(end_Exclusive); count = increment(count, '1'))
            //    {

            //    }
            //}
            return s.ToString();
        }

        public static bool isStringNotNumber(string refStr, bool trueIfParIsStr)
        {
            if (string.IsNullOrEmpty(refStr)) return true;
            foreach (char c in refStr)
            {
                if (trueIfParIsStr)
                {
                    switch (c)
                    {
                        case '(':
                            return true;
                        case ')':
                            return true;
                        default:
                            break;
                    }
                }

                switch (c)
                {
                    case '.':
                        return true;
                    case '0':
                        return true;
                    case '1':
                        return true;
                    case '2':
                        return true;
                    case '3':
                        return true;
                    case '4':
                        return true;
                    case '5':
                        return true;
                    case '6':
                        return true;
                    case '7':
                        return true;
                    case '8':
                        return true;
                    case '9':
                        break;
                    default:
                        break;
                }
            }
            return false;
        }
        public static string getCharAtIndex(string refStr, string index)
        {



            string count = "0";

            foreach (char c in refStr)
            {
                if (index.Equals(count))
                {
                    return c.ToString();
                }
                count = increment(count, '1');
            }

            return "-1";


        }

        public static string removeString(string strToRem, string startFromIndex, string end = "")
        {
            if (startFromIndex.Equals("0") && string.IsNullOrEmpty(end)) return "";
            if (!string.IsNullOrEmpty(end))
            {
                end = new string(end.TrimStart("0"));
                if (string.IsNullOrEmpty(end))
                {
                    end = "0";
                }
            }


            StringBuilder sb = new StringBuilder();
            string count = "0";
            bool isRemovingSubString = false;
            foreach (char c in strToRem)
            {
                if (count.Equals(startFromIndex))
                {
                    isRemovingSubString = true;
                }
                if (isRemovingSubString && string.IsNullOrEmpty(end))
                {
                    return sb.ToString();
                }
                else if (isRemovingSubString && !string.IsNullOrEmpty(end))
                {
                    if (count.Equals(end))
                    {
                        isRemovingSubString = false;
                    }
                    //pass
                }
                if (!isRemovingSubString)
                {
                    sb.Append(c);
                }
                count = increment(count, '1');
            }
            return sb.ToString();
        }

        public static bool isStringEqual(Stack<char> refStrValue1, Stack<char> refStrValue2)
        {

            while (isCountStr_GreaterThan(refStrValue1, "0") && isCountStr_GreaterThan(refStrValue2, "0"))
            {
                if (!refStrValue1.Pop().Equals(refStrValue2.Pop()))
                {
                    return false;
                }

            }

            if (isCountStr_GreaterThan(refStrValue1, "0") || isCountStr_GreaterThan(refStrValue2, "0"))
            {
                return false;
            }
            return true;
        }

        public static string increment(string refStrValue, char incrementBy_SingleChar = '1')
        {

            bool startsWithNeg = refStrValue.StartsWith('-');
            if (startsWithNeg) refStrValue = refStrValue.Remove(0, 1);
            string remainder = "0";
            string result = "";
            Stack<char> count_List = new Stack<char>();
            Stack<char> refStrValue_List = new Stack<char>(refStrValue);
            //Stack<char> incrementBy_List = new Stack<char>(incrementBy);
            //count_List.TryPop(out char count_PopResult);
            //do {
            if (startsWithNeg)
            {
                result = decrement(refStrValue, incrementBy_SingleChar);
                startsWithNeg = !result.StartsWith('-');
                if (!startsWithNeg) result = result.Remove(0, 1);
            }
            else if (refStrValue_List.TryPop(out char refStrValue_List_Pop))
            {

                (string value, string remainder) addResult = addNumber(refStrValue_List_Pop.ToString(), incrementBy_SingleChar.ToString(), remainder);
                result = result.Insert(0, addResult.value);
                remainder = addResult.remainder;
                while (!remainder.Equals("0"))
                {
                    if (refStrValue_List.TryPop(out refStrValue_List_Pop))
                    {
                        addResult = addNumber(refStrValue_List_Pop.ToString(), "0", remainder);
                        result = result.Insert(0, addResult.value);
                    }
                    else if (!remainder.Equals("0"))
                    {
                        result = result.Insert(0, remainder);
                        remainder = "0";
                        break;
                    }
                    if (!remainder.Equals(addResult.remainder))
                    {
                        remainder = addResult.remainder;
                    }

                }
                while (refStrValue_List.TryPop(out refStrValue_List_Pop))
                {
                    result = result.Insert(0, refStrValue_List_Pop.ToString());
                }
            }
            //}while () ;
            //result = result.TrimStart('0');
            result = (result.Equals(".") || string.IsNullOrEmpty(result)) ? "0" : result;
            result = (result.StartsWith(".")) ? result.Insert(0, "0") : result;
            return (startsWithNeg && !result.StartsWith("-")) ? "-" + result : result;
            //return result;

        }

        public static string decrement(string refStrValue, char decrementBy_SingleChar = '1')
        {
            bool startsWithNeg = refStrValue.StartsWith('-');
            if (startsWithNeg) refStrValue = refStrValue.Remove(0, 1);
            string remainder = "0";
            //string result = "";
            Stack<char> count_List = new Stack<char>();
            string result = "";
            (string value, string remainder) calcResult = ("", "");
            Stack<char> refStrValue_List = new Stack<char>(refStrValue);
            Stack<char> decrementBy_List = new Stack<char>(decrementBy_SingleChar);

            if (startsWithNeg)
            {
                result = increment(refStrValue, decrementBy_SingleChar);
            }
            //count_List.TryPop(out char count_PopResult);
            //do {
            else if (refStrValue_List.TryPop(out char refStrValue_List_Pop))
            {

                if (refStrValue_List.Count > 0 && !refStrValue_List_Pop.Equals('-'))
                {
                    string refStr = refStrValue_List_Pop.ToString();
                    if ((int)refStrValue_List_Pop < (int)decrementBy_SingleChar)
                    {
                        refStr = refStr.Insert(0, "1");
                    }
                    calcResult = subtractNumber(refStr, decrementBy_SingleChar.ToString(), remainder);
                    result = calcResult.value;
                    //result.value = result.value.Insert(0, result.value);
                    if (!calcResult.remainder.Equals(remainder))
                    {
                        remainder = calcResult.remainder;
                    }
                    while (!remainder.Equals("0"))
                    {
                        if (refStrValue_List.TryPop(out refStrValue_List_Pop))
                        {

                            refStr = refStrValue_List_Pop.ToString();
                            //if ((int)refStrValue_List_Pop < (int)decrementBy_SingleChar)
                            //{
                            //    refStr = refStr.ToString().Insert(0, "1");
                            //}
                            //result.value = result.value.Insert(0, subtractNumber(refStr, "0", remainder).value);
                            calcResult = subtractNumber(refStr, "0", remainder);
                            result = result.Insert(0, calcResult.value);
                            //result.value = result.value.Insert(0, result.value);
                            if (!calcResult.remainder.Equals(remainder))
                            {
                                remainder = calcResult.remainder;
                            }


                        }
                        else
                        {
                            //result.value = result.value.Insert(0, subtractNumber("10", decrementBy_SingleChar.ToString(), remainder).value);
                            //remainder = "0";
                            calcResult = subtractNumber(calcResult.value, "0", remainder);
                            result = result.Insert(0, calcResult.value);
                            //result.value = result.value.Insert(0, result.value);
                            if (!calcResult.remainder.Equals(remainder))
                            {
                                remainder = calcResult.remainder;
                            }
                        }
                    }
                    while (refStrValue_List.TryPop(out refStrValue_List_Pop))
                    {
                        //calcResult.value = calcResult.value.Insert(0, refStrValue_List_Pop.ToString());
                        result = result.Insert(0, refStrValue_List_Pop.ToString());
                    }
                }
                else
                {
                    string refStr = refStrValue_List_Pop.ToString();
                    if (refStrValue_List.Count > 0 && (int)refStrValue_List_Pop < (int)decrementBy_SingleChar)
                    {
                        refStr = refStr.Insert(0, "1");
                    }
                    calcResult = subtractNumber(refStr, decrementBy_SingleChar.ToString(), remainder, true);
                    result = result.Insert(0, calcResult.value);
                }
            }
            result = result.TrimStart('0');
            result = (result.Equals(".") || string.IsNullOrEmpty(result)) ? "0" : result;
            result = (result.StartsWith(".")) ? result.Insert(0, "0") : result;
            return (startsWithNeg && !result.StartsWith("-")) ? "-" + result : result;

        }

        public static string indexOfStr(string strToChk, char charToChk, bool addOne = false, bool getMaxCount_NotNeg1 = false)
        {
            //List<char> countList = new List<char>();
            string count = "0";
            //string remainder = "0";
            foreach (char c in strToChk)
            {
                if (c.Equals(charToChk))
                {
                    if (addOne)
                        return increment(count, '1');
                    //return Utilities_StringMath.evaluate_BasicMath(
                    //(TokenType.NUMBER, count),
                    //(TokenType.PLUS, "+"),
                    //(TokenType.NUMBER, "1")
                    //); ;
                    return count;
                }
                count = increment(count, '1');
                //count = Utilities_StringMath.evaluate_BasicMath(
                //    (TokenType.NUMBER, count),
                //    (TokenType.PLUS, "+"),
                //    (TokenType.NUMBER, "1")
                //    );
            }
            if (!getMaxCount_NotNeg1)
                return "-1";
            else
                //if (addOne)
                return count;
            //else
            //    return decrement(count, '1');
        }

        public static string indexOfChar_Last(string strToChk, char charToChk)
        {
            //List<char> countList = new List<char>();
            string count = "0";
            string result = "-1";
            //string remainder = "0";
            foreach (char c in strToChk)
            {
                increment(count, '1');
                if (c.Equals(charToChk))
                {
                    result = count;
                }
            }
            return result;
        }

        public static string indexOfChar_Last(string strToChk, char charToChk, string skip)
        {
            //List<char> countList = new List<char>();
            string count = "0";
            string result = "-1";
            //string remainder = "0";
            Queue<string> que = new Queue<string>();
            foreach (char c in strToChk)
            {
                increment(count, '1');
                if (c.Equals(charToChk))
                {
                    que.Enqueue(count);
                    if (isLeftStr_LargerThenRightStr_Number(count, skip))
                    {
                        result = que.Dequeue();
                    }
                }
            }
            return result;
        }

        public static bool isStringLength_GreaterThan0(string strToChk)
        {
            foreach (char c in strToChk)
            {
                return true;
            }
            return false;
        }

        public static string countStr(string strToCount, bool getAsIndex = false)
        {
            string count = "0";
            foreach (char c in strToCount)
            {
                count = increment(count, '1');
            }
            if (getAsIndex)
                return decrement(count, '1'); ;
            return count;
        }

        public static string countStr(string strToCount, bool getAsIndex, bool excludeDecimal)
        {
            string count = "0";
            foreach (char c in strToCount)
            {
                if (excludeDecimal && c.Equals('.')) continue;
                count = increment(count, '1');
            }
            if (getAsIndex)
                return decrement(count, '1'); ;
            return count;
        }

        public static string countStr(string strToCount, bool toDecimal, bool fromDecimal, bool getAsIndex = false)
        {
            string count = "0";
            bool beginCount = false;
            if (!fromDecimal || toDecimal) beginCount = true;

            foreach (char c in strToCount)
            {
                if (toDecimal && c.Equals('.'))
                {
                    break;
                }
                else if (fromDecimal && c.Equals("."))
                {
                    beginCount = true;
                }
                else if (beginCount)
                {
                    count = increment(count, '1');
                }
            }
            if (getAsIndex)
                return decrement(count, '1'); ;
            return count;
        }
        public static string countStr<T>(List<T> strToCount, bool getAsIndex = false)
        {
            string count = "0";
            foreach (T c in strToCount)
            {
                count = increment(count, '1');
            }
            if (getAsIndex)
                return decrement(count, '1'); ;
            return count;
        }
        public static string insertAt_FromLast(string strToInsert, string index, string value)
        {
            Stack<char> refStr = new Stack<char>(strToInsert);
            string count = "0";
            string result = "";
            //StringBuilder stringBuilder = new StringBuilder();

            bool set = false;
            while (isCountStr_GreaterThan<char>(refStr, "0"))
            {
                result = result.Insert(0, refStr.Pop().ToString());
                if (!set)
                    count = increment(count, '1');
                if (!set && index.Equals(count))
                {
                    set = true;
                    result = result.Insert(0, value);
                }
            }

            return result;
        }
        public static string insertAt(string strToInsert, string index, string value)
        {
            string count = "0";
            StringBuilder stringBuilder = new StringBuilder();
            bool set = false;
            foreach (char c in strToInsert)
            {
                if (!set && index.Equals(count))
                {
                    set = true;
                    stringBuilder.Append(value);
                }
                stringBuilder.Append(c);
                if (!set)
                    count = increment(count, '1');
            }
            if (!set && index.Equals(count))
            {
                set = true;
                stringBuilder.Append(value);
            }

            return stringBuilder.ToString();
        }
        public static bool isCountStr_GreaterThan(string strToCount, string length)
        {
            string count = "0";
            bool greaterThan = false;
            foreach (char c in strToCount)
            {
                if (greaterThan)
                {
                    return true;
                }
                if (length.Equals(count))
                {
                    greaterThan = true;
                }
                count = increment(count, '1');
            }
            if (greaterThan)
            {
                return true;
            }
            return false;
        }
        public static bool isCountStr_GreaterThan<T>(Stack<T> strToCount, string length)
        {
            string count = "0";
            bool greaterThan = false;
            foreach (T c in strToCount)
            {
                if (greaterThan)
                {
                    return true;
                }
                if (length.Equals(count))
                {
                    greaterThan = true;
                }
                count = increment(count, '1');
            }
            if (greaterThan)
            {
                return true;
            }
            return false;
        }
        public static bool isCountStr_GreaterThan<T>(Queue<T> strToCount, string length)
        {
            string count = "0";
            bool greaterThan = false;
            foreach (T c in strToCount)
            {
                if (greaterThan)
                {
                    return true;
                }
                if (length.Equals(count))
                {
                    greaterThan = true;
                }
                count = increment(count, '1');
            }
            if (greaterThan)
            {
                return true;
            }
            return false;
        }
        public static string isCompareStr_LargerLength(string str1, string str2)
        {
            //string sb1 = str1;
            //string sb2 = str2;
            bool finished1 = false;
            bool finished2 = false;
            while (!finished1 && !finished2)
            {
                if (string.IsNullOrEmpty(str1))
                {
                    finished1 = true;
                }
                else
                {
                    str1 = str1.Remove(0, 1);
                }
                if (string.IsNullOrEmpty(str2))
                {
                    finished2 = true;
                }
                else
                {
                    str2 = str2.Remove(0, 1);
                }
            }

            if (finished1 && finished2)
                return "==";

            else if (finished1 && !finished2)
                return "str2";

            else if (!finished1 && finished2)
                return "str1";
            else
                return "";
        }

        public static void setEvenNumLength_With0(ref string lString, ref string rString, bool isAdd0AtBeginning = false)
        {
            string lCount = countStr(lString);
            string rCount = countStr(rString);


            //count = Utilities_StringMath.evaluate_BasicMath(
            //    (TokenType.NUMBER, count),
            //    (TokenType.PLUS, "+"),
            //    (TokenType.NUMBER, "1")
            //    );
            Queue<char> lQueue = new Queue<char>(lString);
            Queue<char> rQueue = new Queue<char>(rString);

            if (!isCountStr_GreaterThan(lString, rCount))
            {
                while (isCountStr_GreaterThan(lQueue, "0"))
                {
                    lQueue.Dequeue();
                    rQueue.Dequeue();
                }
                while (isCountStr_GreaterThan(rQueue, "0"))
                {
                    rQueue.Dequeue();
                    if (!isAdd0AtBeginning)
                    {
                        lString += 0;
                    }
                    else
                    {
                        lString = lString.Insert(0, "0");
                    }
                }
            }
            else
            {
                while (isCountStr_GreaterThan(rQueue, "0"))
                {
                    lQueue.Dequeue();
                    rQueue.Dequeue();
                }
                while (isCountStr_GreaterThan(lQueue, "0"))
                {
                    lQueue.Dequeue();
                    if (!isAdd0AtBeginning)
                    {
                        rString += 0;
                    }
                    else
                    {
                        rString = rString.Insert(0, "0");
                    }
                }
            }

        }


        public static bool isLeftStr_LargerThenEqualToRightStr_Number(string lString, string rString)
        {
            lString = lString.TrimStart('0');
            rString = rString.TrimStart('0');

            string startToIndexL = lString;
            string startToIndexR = rString;

            string lIndex = indexOfStr(lString, '.');
            string rIndex = indexOfStr(rString, '.');



            if (lIndex != "-1")
                startToIndexL = removeString(lString, lIndex);
            if (rIndex != "-1")
                startToIndexR = removeString(rString, rIndex);

            if (string.IsNullOrEmpty(startToIndexL))
            {
                startToIndexL = "0";
            }

            if (string.IsNullOrEmpty(startToIndexR))
            {
                startToIndexR = "0";
            }

            bool isLStringNeg = false;
            bool isRStringNeg = false;
            if (startToIndexL.StartsWith('-'))
            {
                isLStringNeg = true;
                startToIndexL = startToIndexL.Remove(0, 1);
            }

            if (startToIndexR.StartsWith('-'))
            {
                isRStringNeg = true;
                startToIndexR = startToIndexR.Remove(0, 1);
            }


            string largerLength = isCompareStr_LargerLength(startToIndexL, startToIndexR);

            string count = "0";
            if (largerLength.Equals("str1"))
            {
                if (isLStringNeg)
                    return false;
                return true;
            }
            else if (largerLength.Equals("str2"))
            {
                if (isRStringNeg)
                    return true;
                return false;
            }
            else
            {

                if (isLStringNeg && !isRStringNeg)
                {
                    return false;
                }

                if (!isLStringNeg && isRStringNeg)
                {
                    return true;
                }



                foreach (char c in startToIndexL)
                {

                    if (double.Parse(getCharAtIndex(startToIndexL, count)) >
                    double.Parse(getCharAtIndex(startToIndexR, count)))
                    {

                        if (!isLStringNeg && isRStringNeg)
                        {
                            return false;
                        }
                        return true;
                    }
                    else if (double.Parse(getCharAtIndex(startToIndexL, count)) <
                    double.Parse(getCharAtIndex(startToIndexR, count)))
                    {

                        if (isLStringNeg && !isRStringNeg)
                        {
                            return true;
                        }
                        return false;
                    }

                    count = increment(count, '1');


                }
            }



            bool isTrimEqual = false;
            if (startToIndexL.TrimStart('.').TrimStart('0')
                .Equals(startToIndexR.TrimStart('.').TrimStart('0')))
            {
                isTrimEqual = true;
            }


            if (lIndex != "-1")
            {
                startToIndexL = removeString(lString, "0", lIndex + 1);
            }
            else
            {
                startToIndexL = lString;
            }

            if (rIndex != "-1")
            {
                startToIndexR = removeString(rString, "0", rIndex + 1);
            }
            else
            {
                startToIndexR = rString;
            }


            if (isTrimEqual && startToIndexL.TrimEnd('.').TrimEnd('0')
                .Equals(startToIndexR.TrimEnd('.').TrimEnd('0')))
            {
                return true;
            }


            count = "0";
            foreach (char c in startToIndexR)
            {

                string lChar = getCharAtIndex(startToIndexL, count);
                string rChar = getCharAtIndex(startToIndexR, count);

                if (double.Parse(lChar) >
                double.Parse(rChar))
                {
                    if (!isLStringNeg && !isRStringNeg)
                        return true;
                    if (isLStringNeg && isRStringNeg)
                        return false;
                }
                else if (double.Parse(lChar) >
                double.Parse(rChar))
                {
                    if (!isLStringNeg && !isRStringNeg)
                        return false;
                    if (isLStringNeg && isRStringNeg)
                        return true;
                }

                count = increment(count, '1');


            }



            return true;

        }


        public static bool isLeftStr_LargerThenRightStr_Number(string lString, string rString)
        {
            lString = lString.TrimStart('0');
            rString = rString.TrimStart('0');

            string startToIndexL = lString;
            string startToIndexR = rString;

            string lIndex = indexOfStr(lString, '.');
            string rIndex = indexOfStr(rString, '.');



            if (lIndex != "-1")
                startToIndexL = removeString(lString, lIndex);
            if (rIndex != "-1")
                startToIndexR = removeString(rString, rIndex);

            if (string.IsNullOrEmpty(startToIndexL))
            {
                startToIndexL = "0";
            }

            if (string.IsNullOrEmpty(startToIndexR))
            {
                startToIndexR = "0";
            }

            bool isLStringNeg = false;
            bool isRStringNeg = false;
            if (startToIndexL.StartsWith('-'))
            {
                isLStringNeg = true;
                startToIndexL = startToIndexL.Remove(0, 1);
            }

            if (startToIndexR.StartsWith('-'))
            {
                isRStringNeg = true;
                startToIndexR = startToIndexR.Remove(0, 1);
            }


            string largerLength = isCompareStr_LargerLength(startToIndexL, startToIndexR);

            string count = "0";
            if (largerLength.Equals("str1"))
            {
                if (isLStringNeg)
                    return false;
                return true;
            }
            else if (largerLength.Equals("str2"))
            {
                if (isRStringNeg)
                    return true;
                return false;
            }
            else
            {

                if (isLStringNeg && !isRStringNeg)
                {
                    return false;
                }

                if (!isLStringNeg && isRStringNeg)
                {
                    return true;
                }



                foreach (char c in startToIndexL)
                {

                    if (double.Parse(getCharAtIndex(startToIndexL, count)) >
                    double.Parse(getCharAtIndex(startToIndexR, count)))
                    {

                        if (!isLStringNeg && isRStringNeg)
                        {
                            return false;
                        }
                        return true;
                    }
                    else if (double.Parse(getCharAtIndex(startToIndexL, count)) <
                    double.Parse(getCharAtIndex(startToIndexR, count)))
                    {

                        if (isLStringNeg && !isRStringNeg)
                        {
                            return true;
                        }
                        return false;
                    }

                    count = increment(count, '1');


                }
            }



            bool isTrimEqual = false;
            if (startToIndexL.TrimStart('.').TrimStart('0')
                .Equals(startToIndexR.TrimStart('.').TrimStart('0')))
            {
                isTrimEqual = true;
            }


            if (lIndex != "-1")
            {
                startToIndexL = removeString(lString, "0", increment(lIndex, '1'));
            }
            else
            {
                startToIndexL = lString;
            }

            if (rIndex != "-1")
            {
                startToIndexR = removeString(rString, "0", increment(rIndex, '1'));
            }
            else
            {
                startToIndexR = rString;
            }


            if (isTrimEqual && startToIndexL.TrimEnd('.').TrimEnd('0')
                .Equals(startToIndexR.TrimEnd('.').TrimEnd('0')))
            {
                return false;
            }
            if (startToIndexL.Equals(startToIndexR)) return false;

            count = "0";
            foreach (char c in startToIndexR)
            {

                string lChar = getCharAtIndex(startToIndexL, count);
                string rChar = getCharAtIndex(startToIndexR, count);

                if (double.Parse(lChar) >
                double.Parse(rChar))
                {
                    if (!isLStringNeg && !isRStringNeg)
                        return true;
                    if (isLStringNeg && isRStringNeg)
                        return false;
                }
                else if (double.Parse(lChar) <
                double.Parse(rChar))
                {
                    if (!isLStringNeg && !isRStringNeg)
                        return false;
                    if (isLStringNeg && isRStringNeg)
                        return true;
                }

                count = increment(count, '1');


            }



            return true;

        }


        public static string addZeroStoString(string refString, string howMay, bool addToEnd)
        {

            string count = "0";

            while (!howMay.Equals(count))
            {
                if (addToEnd)
                {
                    refString += "0";
                }
                else
                {
                    refString = refString.Insert(0, "0");
                }
                count = increment(count, '1');
            }
            return refString;
        }


        public static int isStartStr_Zero(string strToChk)
        {

            int index = 0;
            if (strToChk.Count() > 0 &&
                strToChk[0] == '-')
                index++;
            while (index < strToChk.Count() && strToChk[index] == '0')
            {
                index++;
                if (index < strToChk.Count() &&
                    strToChk[index] == '.')
                    index++;
            }
            //if (strToChk[index] == '.')
            //    index--;
            return --index;


        }

        //public static bool isNumber(string characterToChk)
        //{
        //    if (characterToChk.Equals("."))
        //    {
        //        return true;
        //    }
        //    else if (int.TryParse(characterToChk, out int result))
        //    {
        //        return true;
        //    }
        //    return false;

        //}
        //public static bool isOperator(string characterToChk)
        //{
        //    if (characterToChk.Equals("+"))
        //    {
        //        return true;
        //    }
        //    else if (characterToChk.Equals("-"))
        //    {
        //        return true;
        //    }
        //    else if (characterToChk.Equals("*"))
        //    {
        //        return true;
        //    }
        //    else if (characterToChk.Equals("/"))
        //    {
        //        return true;
        //    }
        //    else if (characterToChk.Equals("^"))
        //    {
        //        return true;
        //    }
        //    return false;

        //}
        public static bool isParenthesis(string characterToChk)
        {
            if (characterToChk.Equals("("))
            {
                return true;
            }
            else if (characterToChk.Equals(")"))
            {
                return true;
            }
            return false;

        }
        public static bool isBrace(string characterToChk)
        {
            if (characterToChk.Equals("{"))
            {
                return true;
            }
            else if (characterToChk.Equals("}"))
            {
                return true;
            }
            return false;

        }
        public static bool isBracket(string characterToChk)
        {
            if (characterToChk.Equals("["))
            {
                return true;
            }
            else if (characterToChk.Equals("]"))
            {
                return true;
            }
            return false;

        }














        public static (string value, string remainder) addNumber(string character1, string character2, string remainder)
        {
            //if (character1.Equals('.')) character1 = "0";
            //if (character2.Equals('.')) character2 = "0";
            int result = int.Parse(character1) + int.Parse(character2) + int.Parse(remainder);
            return ((result % 10).ToString(), (result / 10).ToString());
        }

        public static (string value, string remainder) subtractNumber(string character1, string character2, string remainder, bool returnNegativeAsIs = false)
        {
            //if (character1.Equals('.')) character1 = "0";
            //if (character2.Equals('.')) character2 = "0";
            int result = int.Parse(character1) - int.Parse(character2) - int.Parse(remainder);
            if (returnNegativeAsIs)
            {
                return (result.ToString(), result < 0 || character1.Length > 1 ? 1.ToString() : 0.ToString());

            }
            //if(result<0)
            //{
            //    return 
            //}
            return (result < 0 ? (result + 10).ToString() : result.ToString(), result < 0 || character1.Length > 1 ? 1.ToString() : 0.ToString());
        }

        public static (string value, string remainder) multiplyNumber(string character1, string character2, string remainder)
        {
            int result = int.Parse(character1) * int.Parse(character2) + int.Parse(remainder);
            return ((result % 10).ToString(), (result / 10).ToString());
        }


        public static string taylorRangeReduction_Power(string x, string n, string precision)
        {

            x = Utilities_StringMath.evaluate_BasicMath(
                               (TokenType.NUMBER, n),
                               (TokenType.MULTIPLY, "*"),
                               (TokenType.NUMBER, Math.Log(double.Parse(x)).ToString()),
                               precision
                               );

            string sum = "1.0";
            string term = "1.0";

            for (int i = 1; i <= 50; i++)
            {
                term = Utilities_StringMath.evaluate_BasicMath(
                               (TokenType.NUMBER, term),
                               (TokenType.MULTIPLY, "*"),
                               (TokenType.NUMBER, x),
                               precision
                               );
                term = Utilities_StringMath.evaluate_BasicMath(
                               (TokenType.NUMBER, term),
                               (TokenType.DIVIDE, "/"),
                               (TokenType.NUMBER, i.ToString()),
                               precision
                               );
                sum = Utilities_StringMath.evaluate_BasicMath(
                               (TokenType.NUMBER, sum),
                               (TokenType.PLUS, "+"),
                               (TokenType.NUMBER, term),
                               precision
                               );

            }

            return sum;

        }

        public static (string value, string remainder) divideString(string str1, string str2, string precision, bool includeDecimal = false)
        {
            bool isNegResult = false;
            if ((str1.StartsWith('-') && !str2.StartsWith('-')) ||
                (!str1.StartsWith('-') && str2.StartsWith('-')))
            {
                isNegResult = true;
            }


            string str1_Original = str1 = str1.TrimStart('0').TrimStart('-');
            string str2_Original = str2 = str2.TrimStart('0').TrimStart('-');
            string str1_Count = Utilities_String.countStr(str1.Replace(".", ""));
            string str2_Count = Utilities_String.countStr(str2.Replace(".", ""));

            string str1_Length_Original = Utilities_String.countStr(Utilities_String.getSubString(str1, "0", "infinite", true));
            string str2_Length_Original = Utilities_String.countStr(Utilities_String.getSubString(str2, "0", "infinite", true));
            //string str1_Length = Utilities_String.countStr(Utilities_String.getSubString(str1, "0", "infinite", true));
            //string str2_Length = Utilities_String.countStr(Utilities_String.getSubString(str2, "0", "infinite", true));

            string str2FromDecimalExcluded = Utilities_String.countStr(Utilities_String.getSubString(str2.Replace(".", ""), str2_Length_Original, "infinite", true), false);
            string str1_NewIndexDecimal = str2FromDecimalExcluded;

            str1_NewIndexDecimal = Utilities_StringMath.evaluate_BasicMath(
                               (TokenType.NUMBER, str1_NewIndexDecimal),
                               (TokenType.PLUS, "+"),
                               (TokenType.NUMBER, indexOfStr(str1, '.', false, true)),
                               precision
                               );
            str1 = str1.Replace(".", "");






            while (isLeftStr_LargerThenRightStr_Number(str2FromDecimalExcluded, "0"))
            //&&
            //isLeftStr_LargerThenRightStr_Number(str2_Count, str1_Count))
            {
                str1 = str1 + "0";
                str2FromDecimalExcluded = decrement(str2FromDecimalExcluded, '1');
            }







            //if (!str1_NewIndexDecimal.Equals("0"))
            str1 = insertAt(str1, str1_NewIndexDecimal, ".");
            //string s = Utilities_String.countStr(Utilities_String.getSubString(str1, "0", "infinite", true),true);
            string decimalPlace = "-1";

            if (str1_Length_Original.Equals(str2_Length_Original))
            {
                if (isLeftStr_LargerThenEqualToRightStr_Number(str1_Original, str2_Original))
                {
                    decimalPlace = str2_Count;
                }
                else
                {
                    decimalPlace = str2_Count;
                    //decimalPlace = Utilities_StringMath.evaluate_BasicMath(
                    //           (TokenType.NUMBER, decimalPlace),
                    //           (TokenType.MINUS, "-"),
                    //           (TokenType.NUMBER, "1"), precision
                    //           );
                }
            }
            else
            {
                string str1QuickCount = Utilities_String.countStr(Utilities_String.getSubString(str1, "0", "infinite", true), false);
                decimalPlace = Utilities_StringMath.evaluate_BasicMath(
                                   (TokenType.NUMBER, str2_Count),
                                   (TokenType.MINUS, "-"),
                                   (TokenType.NUMBER, str1QuickCount), precision
                                   );
                decimalPlace = Utilities_StringMath.evaluate_BasicMath(
                                   (TokenType.NUMBER, str2_Count),
                                   (TokenType.MINUS, "-"),
                                   (TokenType.NUMBER, decimalPlace), precision
                                   );
            }
            //////////else if (!isLeftStr_LargerThenRightStr_Number(str1_Original, str2_Original))
            //////////{
            //////////    string str1QuickCount = Utilities_String.countStr(Utilities_String.getSubString(str1, "0", "infinite", true), false);
            //////////    if (isLeftStr_LargerThenEqualToRightStr_Number(str2_Count, str1QuickCount))
            //////////    {
            //////////        //decimalPlace = str2_Count;
            //////////        decimalPlace = Utilities_StringMath.evaluate_BasicMath(
            //////////                           (TokenType.NUMBER, str2_Count),
            //////////                           (TokenType.MINUS, "-"),
            //////////                           (TokenType.NUMBER, str1QuickCount), precision
            //////////                           );
            //////////        decimalPlace = Utilities_StringMath.evaluate_BasicMath(
            //////////                           (TokenType.NUMBER, str2_Count),
            //////////                           (TokenType.MINUS, "-"),
            //////////                           (TokenType.NUMBER, decimalPlace), precision
            //////////                           );

            //////////    }
            //////////    else
            //////////    {
            //////////        decimalPlace = str2_Count;
            //////////        decimalPlace = Utilities_StringMath.evaluate_BasicMath(
            //////////                           (TokenType.NUMBER, decimalPlace),
            //////////                           (TokenType.PLUS, "+"),
            //////////                           (TokenType.NUMBER, "1"), precision
            //////////                           );
            //////////    }
            //////////    //decimalPlace = Utilities_StringMath.evaluate_BasicMath(
            //////////    //           (TokenType.NUMBER, str2_Count),
            //////////    //           (TokenType.MINUS, "-"),
            //////////    //           (TokenType.NUMBER, str1QuickCount), precision
            //////////    //           );

            //////////    //decimalPlace = Utilities_StringMath.evaluate_BasicMath(
            //////////    //                   (TokenType.NUMBER, decimalPlace),
            //////////    //                   (TokenType.MINUS, "-"),
            //////////    //                   (TokenType.NUMBER, "1"), precision
            //////////    //                   );

            //////////    //}
            //////////}
            //////////else
            //////////{
            //////////    string str1QuickCount = Utilities_String.countStr(Utilities_String.getSubString(str1, "0", "infinite", true), false);
            //////////    if (isLeftStr_LargerThenEqualToRightStr_Number(str2_Count, str1QuickCount))
            //////////    {

            //////////        decimalPlace = Utilities_StringMath.evaluate_BasicMath(
            //////////                           (TokenType.NUMBER, str2_Count),
            //////////                           (TokenType.MINUS, "-"),
            //////////                           (TokenType.NUMBER, str1QuickCount), precision
            //////////                           );
            //////////        decimalPlace = Utilities_StringMath.evaluate_BasicMath(
            //////////                           (TokenType.NUMBER, str2_Count),
            //////////                           (TokenType.MINUS, "-"),
            //////////                           (TokenType.NUMBER, decimalPlace), precision
            //////////                           );

            //////////        //decimalPlace = Utilities_StringMath.evaluate_BasicMath(
            //////////        //                   (TokenType.NUMBER, decimalPlace),
            //////////        //                   (TokenType.MINUS, "-"),
            //////////        //                   (TokenType.NUMBER, "1"), precision
            //////////        //                   );
            //////////    }
            //////////    else
            //////////    {
            //////////        //decimalPlace = str2_Count;

            //////////        decimalPlace = Utilities_StringMath.evaluate_BasicMath(
            //////////                           (TokenType.NUMBER, str2_Count),
            //////////                           (TokenType.MINUS, "-"),
            //////////                           (TokenType.NUMBER, str1QuickCount), precision
            //////////                           );
            //////////        decimalPlace = Utilities_StringMath.evaluate_BasicMath(
            //////////                           (TokenType.NUMBER, str2_Count),
            //////////                           (TokenType.MINUS, "-"),
            //////////                           (TokenType.NUMBER, decimalPlace), precision
            //////////                           );
            //////////        //decimalPlace = Utilities_StringMath.evaluate_BasicMath(
            //////////        //                   (TokenType.NUMBER, str2_Count),
            //////////        //                   (TokenType.PLUS, "+"),
            //////////        //                   (TokenType.NUMBER, str2_Length_Original), precision
            //////////        //                   );
            //////////        //decimalPlace = Utilities_StringMath.evaluate_BasicMath(
            //////////        //                   (TokenType.NUMBER, decimalPlace),
            //////////        //                   (TokenType.MINUS, "-"),
            //////////        //                   (TokenType.NUMBER, "1"), precision
            //////////        //                   );
            //////////    }
            //////////    ////////decimalPlace = Utilities_StringMath.evaluate_BasicMath(
            //////////    ////////               (TokenType.NUMBER, str1QuickCount),
            //////////    ////////               (TokenType.MINUS, "-"),
            //////////    ////////               (TokenType.NUMBER, str2_Count), precision
            //////////    ////////               );
            //////////    ////////    //decimalPlace = Utilities_StringMath.evaluate_BasicMath(
            //////////    ////////    //               (TokenType.NUMBER, Utilities_String.countStr(Utilities_String.getSubString(str1, "0", "infinite", true), false)),
            //////////    ////////    //               (TokenType.PLUS, "+"),
            //////////    ////////    //               (TokenType.NUMBER, str1_Length_Original), precision
            //////////    ////////    //               );
            //////////    ////////    decimalPlace = Utilities_StringMath.evaluate_BasicMath(
            //////////    ////////                       (TokenType.NUMBER, decimalPlace),
            //////////    ////////                       (TokenType.PLUS, "+"),
            //////////    ////////                       (TokenType.NUMBER, "1"), precision
            //////////    ////////                       );
            //////////    //}
            //////////    //decimalPlace = Utilities_StringMath.evaluate_BasicMath(
            //////////    //                   (TokenType.NUMBER, decimalPlace),
            //////////    //                   (TokenType.MINUS, "-"),
            //////////    //                   (TokenType.NUMBER, "1"), precision
            //////////    //                   );
            //////////    //decimalPlace = Utilities_String.countStr(Utilities_String.getSubString(str1, "0", "infinite", true), false);
            //////////}
            ///


            str2 = str2_Original.Replace(".", "");

            if (string.IsNullOrEmpty(str2.Trim('0')) || str2.Trim('0').Equals("."))
            {
                throw new Exception("Error: Divide by zero.");
            }

            decimalPlace = decimalPlace.TrimStart('0');
            if (string.IsNullOrEmpty(decimalPlace)) decimalPlace = "0";
            str1 = str1.Replace(".", "");
            if (string.IsNullOrEmpty(str1.TrimStart('0'))) return ("0", "0");




            List<char> str1_Manip = new List<char>(str1);
            string result = "";
            string calcResult = "";
            StringBuilder lCalcStr = new StringBuilder();
            string precisionRound = increment(precision, '1');
            string precisionCount = "0";
            string count = "0";
            bool beginAddDecimal = false;
            //bool stopDecimalCount = false;
            while (!beginAddDecimal || !countStr<char>(str1_Manip).Equals("0"))
            {
                lCalcStr.Append(str1_Manip[0]);
                str1_Manip.RemoveAt(0);



                if (!string.IsNullOrEmpty(count))
                {
                    count = "0";
                }

                while (isLeftStr_LargerThenEqualToRightStr_Number(lCalcStr.ToString(), str2))
                {
                    if (!beginAddDecimal && includeDecimal)
                        beginAddDecimal = true;


                    if (string.IsNullOrEmpty(count))
                    {
                        count = "0";
                    }

                    count = increment(count, '1');

                    calcResult = Utilities_StringMath.evaluate_BasicMath(
                        (TokenType.NUMBER, lCalcStr.ToString()),
                        (TokenType.MINUS, "-"),
                        (TokenType.NUMBER, str2), precision
                        );
                    lCalcStr.Clear();
                    lCalcStr.Append(calcResult);
                }
                if (beginAddDecimal)
                {
                    precisionCount = increment(precisionCount, '1');
                }
                result += count;
                if (beginAddDecimal && !precision.Equals("infinite"))
                {
                    if (isLeftStr_LargerThenEqualToRightStr_Number(precisionCount, precisionRound))
                    {
                        if (isLeftStr_LargerThenEqualToRightStr_Number(result.Last().ToString(), "5"))
                        {
                            //increment decimal number

                            //result = new string(result.Reverse().ToArray()).Remove(0, 1);
                            //result = new string(result.Reverse().ToArray());
                            result = increment(result, '1');
                            //result = increment(result, '1');
                            //result = new string(result.Reverse().ToArray()).Remove(0, 1);
                            //string roundedNum = increment(result[0].ToString(), '1');
                            //result = result.Remove(0, 1);
                            //result = new string(result.Reverse().ToArray());
                            //result = result + roundedNum;
                        }
                        //else
                        //{
                        //    result = new string(result.Reverse().ToArray()).Remove(0, 1);
                        //    result = new string(result.Reverse().ToArray());
                        //}
                        break;
                    }
                }

                //if (!countStr<char>(str1_Manip).Equals("0"))
                //{
                //    str1_Manip.RemoveAt(0);
                //}

                string calcTrim = lCalcStr.ToString().Trim('0');

                if (countStr<char>(str1_Manip).Equals("0") &&
                    (!calcTrim.Equals("0") &&
                    !calcTrim.Equals(".") &&
                    !string.IsNullOrEmpty(calcTrim))
                    )
                {
                    str1_Manip.Add('0');
                }

            }





            string mult = Utilities_StringMath.evaluate_BasicMath(
                        (TokenType.NUMBER, str2),
                        (TokenType.MULTIPLY, "*"),
                        (TokenType.NUMBER, result), precision
                        );

            string add = Utilities_StringMath.evaluate_BasicMath(
                        (TokenType.NUMBER, str1),
                        (TokenType.MINUS, "-"),
                        (TokenType.NUMBER, mult), precision
                        );


            string addTrim = add.ToString().Trim('0').Replace(".", "");

            if (string.IsNullOrEmpty(addTrim))
            {
                add = "0";
            }

            if (addTrim.StartsWith('.'))
            {
                add.Insert(0, "0");
            }

            while (!isLeftStr_LargerThenEqualToRightStr_Number(decimalPlace, "0"))
            {
                decimalPlace = increment(decimalPlace, '1');
                result = result.Insert(0, "0");
            }

            result = insertAt(result, decimalPlace, ".");

            result = result.TrimStart('0');
            if (result.StartsWith('.') || string.IsNullOrEmpty(result))
            {
                result = result.Insert(0, "0");
            }

            if (isNegResult)
            {
                result = result.Insert(0, "-");
            }
            //result = result.TrimEnd('0');
            //if (result.StartsWith('.') || string.IsNullOrEmpty(result))
            //{
            //    result = result.Insert(0, "0");
            //}
            return (result, add);

            //return (result, int.Parse(str1) - int.Parse(str2) * result);
        }
    }
}
