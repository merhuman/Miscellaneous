using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OfficeOpenXml;

namespace Bosch
{
    class ProgramLibs
    {
        internal ProgramLibs()
        {

        }

        #region UnderDevelopment
        static void ReadExcelFile()
        {
            
        }
        #endregion UnderDevelopment

        #region GeneralFunction
        internal string AddSpacesBetweenDTCString(string input)
        {
            /* 
             * This function is not complete yet. 
             * Currently, it can only add 0x without
             * checking whether space is already there or not 
             */

            var list = Enumerable
                .Range(0, input.Length / 2)
                .Select(i => input.Substring(i * 2, 2))
                .ToList();
            var res = string.Join(",", list);
            return res;
        }

        internal bool CheckPattern(string pattern)
        {

            return true;
        }
 
        #endregion GeneralFunction

        #region Conversion
        /* enums */
        internal enum StringType { Normal, With_0x  };
        internal static StringType CheckTheInputFormat(string input)
        {
            if (input.StartsWith("0x"))
            {
                return StringType.With_0x;
            }
            return StringType.Normal;
        }

        internal static bool CheckAndAddSpace(string input)
        {
            /*
             * This will be the most important function for checking input string.
             * */

            int l_lenOfInputString = input.Length;
            string[] l_inputString = new string[l_lenOfInputString];

            /* Check even length of input string */
            if (l_lenOfInputString % 2 != 0)
            {
                return false;
            }

            /* this part here need to be updated */
            /**/

            return true;
        }

        internal static string RemoveWhiteSpace(string input)
        {
            return input.ToCharArray()
                    .Where(c => !Char.IsWhiteSpace(c))
                    .Select(c => c.ToString())
                    .Aggregate((a, b) => a + b);
        }

        internal static string HexStringToString(string hexString)
        {
            /*
             * This function will convert hex string to ascii string.
             * */
            var l_outputString = new StringBuilder();
            string l_modifiedHexString;

            // Drop all space in hex string
            l_modifiedHexString = RemoveWhiteSpace(hexString);

            // Condition to make sure the length of hex string must be even and greater than 0
            if (l_modifiedHexString == null || (l_modifiedHexString.Length & 1) == 1)
            {
                throw new ArgumentException();
            }
            
            for (var i = 0; i < l_modifiedHexString.Length; i += 2)
            {
                var hexChar = l_modifiedHexString.Substring(i, 2);
                l_outputString.Append((char)Convert.ToByte(hexChar, 16));
            }

            return Convert.ToString(l_outputString);
        }

        internal static string Add0xToHexString(string hexString)
        {
            /* 
             * Add 0x to input Hex String
             * */
            
            string l_outputString;
            if (hexString.Contains("0x"))
                {
                    hexString.Replace("0x", " ").TrimStart();
                    l_outputString = "0x" + hexString.Replace(" ", ",0x");
                }
                else
                {
                    l_outputString = "0x" + hexString.Replace(" ", ",0x");
                }
            return l_outputString;
        }

        internal static string Drop0xInHexString(string hexString)
        {
            /* 
             * Drop 0x from input Hex String
             * */
             
            string l_outputString;
            l_outputString = hexString.Replace(",0x", " ").Substring(2);
            return l_outputString;
        }

        #endregion Conversion

        #region Search
        internal bool SearchString()
        {

            return false;
        }
        #endregion Search

        #region OpenFile
        internal static bool OpenExcelFile()
        {
            
            return true;
        }
        #endregion OpenFile
    }
}
