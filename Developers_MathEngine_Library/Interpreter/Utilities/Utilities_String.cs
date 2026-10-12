using Developers_MathEngine_Library.Interpreter.Token;
using Developers_MathEngine_Library.Interpreter.Utilities;
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

namespace Developers_MathEngine_Library.Interpreter.Utilities
{
    public class Utilities_String
    {

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
                else
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

            string remainder = "0";
            string result = "";
            Stack<char> count_List = new Stack<char>();
            Stack<char> refStrValue_List = new Stack<char>(refStrValue);
            //Stack<char> incrementBy_List = new Stack<char>(incrementBy);
            //count_List.TryPop(out char count_PopResult);
            //do {

            if (refStrValue_List.TryPop(out char refStrValue_List_Pop))
            {
                result = result.Insert(0, addNumber(refStrValue_List_Pop.ToString(), incrementBy_SingleChar.ToString(), remainder).value);
                while (!remainder.Equals("0"))
                {
                    if (refStrValue_List.TryPop(out refStrValue_List_Pop))
                    {
                        result = result.Insert(0, addNumber(refStrValue_List_Pop.ToString(), incrementBy_SingleChar.ToString(), remainder).value);
                    }
                    else
                    {
                        result = result.Insert(0, remainder);
                        remainder = "0";
                    }
                }
                while (refStrValue_List.TryPop(out refStrValue_List_Pop))
                {
                    result = result.Insert(0, refStrValue_List_Pop.ToString());
                }
            }
            //}while () ;
            return result;

        }

        public static string decrement(string refStrValue, char decrementBy_SingleChar = '1')
        {

            string remainder = "0";
            string result = "";
            Stack<char> count_List = new Stack<char>();
            Stack<char> refStrValue_List = new Stack<char>(refStrValue);
            Stack<char> decrementBy_List = new Stack<char>(decrementBy_SingleChar);
            //count_List.TryPop(out char count_PopResult);
            //do {

            if (refStrValue_List.TryPop(out char refStrValue_List_Pop))
            {

                string refStr = refStrValue_List_Pop.ToString();
                if ((int)refStrValue_List_Pop < (int)decrementBy_SingleChar)
                {
                    refStr = refStr.Insert(0, "1");
                }

                result = result.Insert(0, subtractNumber(refStr, decrementBy_SingleChar.ToString(), remainder).value);
                while (!remainder.Equals("0"))
                {
                    if (refStrValue_List.TryPop(out refStrValue_List_Pop))
                    {
                        if ((int)refStrValue_List_Pop < (int)decrementBy_SingleChar)
                        {
                            refStr = refStr.Insert(0, "1");
                        }
                        result = result.Insert(0, subtractNumber(refStr, decrementBy_SingleChar.ToString(), remainder).value);
                    }
                    else
                    {
                        result = result.Insert(0, subtractNumber("10", decrementBy_SingleChar.ToString(), remainder).value);
                        remainder = "0";
                    }
                }
                while (refStrValue_List.TryPop(out refStrValue_List_Pop))
                {
                    result = result.Insert(0, refStrValue_List_Pop.ToString());
                }
            }
            return result;

        }

        public static string indexOfStr(string strToChk, char charToChk, bool addOne = false)
        {
            List<char> countList = new List<char>();
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
            return "-1";
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
                try
                {
                    str1 = str1.Remove(0, 1);
                }
                catch (ArgumentOutOfRangeException)
                {
                    finished1 = true;
                }
                try
                {
                    str2 = str2.Remove(0, 1);
                }
                catch (ArgumentOutOfRangeException)
                {

                    finished2 = true;
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

                        if (isLStringNeg && isRStringNeg)
                        {
                            return false;
                        }
                        return true;
                    }
                    else if (double.Parse(getCharAtIndex(startToIndexL, count)) <
                    double.Parse(getCharAtIndex(startToIndexR, count)))
                    {

                        if (isLStringNeg && isRStringNeg)
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
                startToIndexL = removeString(lString, "0", lIndex + 1); ;
            }
            else
            {
                startToIndexL = removeString(lString, "0"); ;
            }

            if (rIndex != "-1")
            {
                startToIndexR = removeString(rString, "0", rIndex + 1); ;
            }
            else
            {
                startToIndexR = removeString(rString, "0"); ;
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

                        if (isLStringNeg && isRStringNeg)
                        {
                            return false;
                        }
                        return true;
                    }
                    else if (double.Parse(getCharAtIndex(startToIndexL, count)) <
                    double.Parse(getCharAtIndex(startToIndexR, count)))
                    {

                        if (isLStringNeg && isRStringNeg)
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
                startToIndexL = removeString(lString, "0", lIndex + 1); ;
            }
            else
            {
                startToIndexL = removeString(lString, "0"); ;
            }

            if (rIndex != "-1")
            {
                startToIndexR = removeString(rString, "0", rIndex + 1); ;
            }
            else
            {
                startToIndexR = removeString(rString, "0"); ;
            }


            if (isTrimEqual && startToIndexL.TrimEnd('.').TrimEnd('0')
                .Equals(startToIndexR.TrimEnd('.').TrimEnd('0')))
            {
                return false;
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

        public static (string value, string remainder) subtractNumber(string character1, string character2, string remainder)
        {
            //if (character1.Equals('.')) character1 = "0";
            //if (character2.Equals('.')) character2 = "0";
            int result = int.Parse(character1) - int.Parse(character2) - int.Parse(remainder);
            return (result < 0 ? (result * -1).ToString() : result.ToString(), result < 0 || character1.Length > 1 ? 1.ToString() : 0.ToString());
        }

        public static (string value, string remainder) multiplyNumber(string character1, string character2, string remainder)
        {
            int result = int.Parse(character1) * int.Parse(character2) + int.Parse(remainder);
            return ((result % 10).ToString(), (result / 10).ToString());
        }


        public static (string value, string remainder) divideString(string str1, string str2)
        {
            //str1 = removeString(str1, indexOfStr(str1, '.'));
            //str2 = removeString(str2, indexOfStr(str2, '.'));

            List<char> str1_Manip = new List<char>(str1);
            string result = "";
            string calcResult = "";
            StringBuilder lCalcStr = new StringBuilder();
            //lCalcStr.Append(str1[0]);
            string count = "";
            while (!countStr<char>(str1_Manip).Equals("0"))
            {
                lCalcStr.Append(str1_Manip[0]);
                str1_Manip.RemoveAt(0);

                if (!string.IsNullOrEmpty(count))
                {
                    count = "0";
                }

                while (isLeftStr_LargerThenEqualToRightStr_Number(lCalcStr.ToString(), str2))
                {
                    if (string.IsNullOrEmpty(count))
                    {
                        count = "0";
                    }
                    //TODO:::::::::::::::::::::::::::::::
                    count = increment(count, '1');

                    calcResult = Utilities_StringMath.evaluate_BasicMath(
                        (TokenType.NUMBER, lCalcStr.ToString()),
                        (TokenType.MINUS, "-"),
                        (TokenType.NUMBER, str2)
                        );
                    lCalcStr.Clear();
                    lCalcStr.Append(calcResult);
                }

                result += count;



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
                        (TokenType.NUMBER, result)
                        );

            string add = Utilities_StringMath.evaluate_BasicMath(
                        (TokenType.NUMBER, str1),
                        (TokenType.MINUS, "-"),
                        (TokenType.NUMBER, mult)
                        );


            string addTrim = add.ToString().Trim('0');

            if (string.IsNullOrEmpty(addTrim))
            {
                add = "0";
            }

            if (addTrim.StartsWith('.'))
            {
                add.Insert(0, "0");
            }


            return (result, add);

            //return (result, int.Parse(str1) - int.Parse(str2) * result);
        }
    }
}
