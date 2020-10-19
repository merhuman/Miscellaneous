﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;
using System.Data.SQLite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;


namespace Miscellaneous
{
    class ProgramLibs
    {
        internal ProgramLibs()
        {

        }

        /* class properties */
        

        /* enums */
        internal enum StringType { Normal, With_0x, Empty };
        internal enum FileType { Excel, SSParam, DBC, Json, Undefined };

        #region UnderDevelopment
        public static List<string> GenerateUnknownDID(List<string> validDID)
        {
            List<string> l_res = new List<string>();

            return l_res;
        }
        #endregion UnderDevelopment

        #region GeneralFunction
        internal static StringType CheckTheInputFormat(string input)
        {
            /*
             * This Check function will be updated later to identify more precisely
             * Such as:
             * whether the input string is truly a With_0x type or not? If it starts with 0x
             * but the following sub string does not contain other 0x then this means the input
             * string is not a With_0x type
             * And so on...
             * */
            if (string.IsNullOrEmpty(input))
            {
                return StringType.Empty;
            }
            else if (input.StartsWith("0x"))
            {
                return StringType.With_0x;
            }
            
            return StringType.Normal;
        }

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

        internal static string RemoveWhiteSpace(string input)
        {
            return input.ToCharArray()
                    .Where(c => !Char.IsWhiteSpace(c))
                    .Select(c => c.ToString())
                    .Aggregate((a, b) => a + b);
        }

        internal static int CountNumberOfChunks(string inputString)
        {
            /*
             * This function counts the number of bytes by counting the "0x" combinations in string.
             * */
            int l_dataLength = 0;
            StringType l_stringType;
            string l_string = inputString.Trim();
            l_stringType = CheckTheInputFormat(l_string);
            
            switch (l_stringType)
            {
                case StringType.With_0x:
                    l_dataLength = l_string.Split(',').Count();
                    break;

                case StringType.Normal:
                    l_dataLength = l_string.Aggregate(0, (total, next) => char.IsWhiteSpace(next) ? total = total + 1 : total);
                    l_dataLength++; // Real number of bytes.
                    break;

                default:
                    break;
            }

            return l_dataLength;
        }

        internal static int CountNumberOfSpace(string inputString)
        {
            /* 
             * This function counts the number of spaces
             * */
            int l_numberOfSpaces = 0;
            l_numberOfSpaces = inputString.Aggregate(0, (total, next) => char.IsWhiteSpace(next) ? total = total + 1 : total);
            return l_numberOfSpaces;
        }

        #endregion GeneralFunction

        #region Conversion
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

        #region OpenFile
        internal static bool OpenFile(FileType fileType, string inputFilePath, string fileName, ref List<string> nameList, Nullable<bool> includeTitle)
        {
            string l_sourcePath = "./Configuration";
            Directory.CreateDirectory(l_sourcePath);
            l_sourcePath = Path.Combine(l_sourcePath, fileName);
            if (!string.IsNullOrEmpty(inputFilePath))
            {
                switch (fileType)
                {
                    case FileType.Excel:
                        // Might add try catch structure here later.
                        File.Copy(inputFilePath, l_sourcePath, true);                       
                        if (includeTitle == null || includeTitle == false)
                            ReadAllSignalNamesFromExcel(l_sourcePath, ref nameList, includeTitle: false);
                        else
                            ReadAllSignalNamesFromExcel(l_sourcePath, ref nameList, includeTitle: true);
                        break;

                    case FileType.DBC:
                        l_sourcePath = Path.ChangeExtension(l_sourcePath, ".txt");  // convert dbc file to txt file for more convenient purpose
                        File.Copy(inputFilePath, l_sourcePath, true);
                        GetNodesFromDBCFile(l_sourcePath, ref nameList);
                        
                        break;

                    case FileType.Json:
                        File.Copy(inputFilePath, l_sourcePath, true);
                        break;

                    default:
                        return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Load SS Parameter File and convert to txt file.
        /// </summary>
        /// <param name="inputFilePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal static bool OpenSSParamFile(string inputFilePath, string fileName)
        {
            string l_sourcePath = "./Configuration";
            Directory.CreateDirectory(l_sourcePath);
            l_sourcePath = Path.Combine(l_sourcePath, fileName);

            string l_outputString = String.Empty;

            if (!string.IsNullOrEmpty(inputFilePath) && File.Exists(inputFilePath))
            {
                List<string> l_signalNameList = new List<string>();
                byte[] bin = File.ReadAllBytes(inputFilePath);

                using (MemoryStream stream = new MemoryStream(bin))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {

                    foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                    {
                        //int l_startRow = (includeTitle == true) ? (worksheet.Dimension.Start.Row + 1) : worksheet.Dimension.Start.Row;
                        //for (int row = l_startRow; row <= worksheet.Dimension.End.Row; row++)
                        //{
                        //    if (worksheet.Cells.Value != null)
                        //    {
                        //        l_signalNameList.Add(worksheet.Cells[row, 1].Value.ToString());
                        //    }
                        //}

                    }
                    ExcelWorksheet l_workSheet = excelPackage.Workbook.Worksheets["Origin"];
                    // Calculation using method from imported dll file.
                }

            }
            return false;
        }

        public class ProjectInfo
        {
            private string _name = String.Empty;
            private List<float> _release;
            private List<string> _variant;
            
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }
            
            public bool AddRelease(float release)
            {
                _release.Add(release);
                if (!(_release?.Any() ?? false))
                {
                    _release.Add(release);
                    return true;
                }
                return false;
            }
            
            public int NumberOfRelease()
            {
                if (!(_release?.Any() ?? false))
                {
                    return _release.Count();
                }
                return 0;
            }

            public bool DropRelease(float release)
            {
                if (!(_release?.Any() ?? false))
                {
                    _release.Remove(release);
                    return true;
                }
                return false;
            }

            public bool DropRelease(int index)
            {
                if (!(_release?.Any() ?? false))
                {
                    _release.RemoveAt(index);
                    return true;
                }
                return false;
            }

            public bool AddVariant(string variant)
            {
                if (!(_release?.Any() ?? false))
                {
                    _variant.Add(variant);
                    return true;
                }
                return false;
            }

            public bool DropVariant(string variant)
            {
                if (!(_release?.Any() ?? false))
                {
                    _variant.Remove(variant);
                    return true;
                }
                return false;
            }

            public bool DropVariant(int index)
            {
                if (!(_release?.Any() ?? false))
                {
                    _variant.RemoveAt(index);
                    return true;
                }
                return false;
            }
        }

        internal static bool ConvertToJson(FileType fileType, string inputFilePath, string fileName)
        {
            if (File.Exists(inputFilePath))
            {

            }
            return false;
        }

        internal static bool GetNodesFromDBCFile(string filePath, ref List<string> nodeList)
        {
            string[] l_nodeNameList;
            if (File.Exists(filePath))
            {
                foreach (var line in File.ReadAllLines(filePath))
                {
                    if (line.StartsWith("BU_:"))
                    {
                        l_nodeNameList = line.Split(' ');
                        nodeList = l_nodeNameList.ToList();
                        nodeList.Remove("BU_:");
                        return true;
                    }
                }
            }
            return false;
        }

        internal static bool GetMessageAndSignal(string filePath)
        {
            return false;
        }

        internal static bool ReadAllSignalNamesFromExcel(string filePath, ref List<string> signalNameList, bool includeTitle)
        {
            List<string> l_signalNameList = new List<string>();
            byte[] bin = File.ReadAllBytes(filePath);

            using (MemoryStream stream = new MemoryStream(bin))
            using (ExcelPackage excelPackage = new ExcelPackage(stream))
            {
                
                foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                {
                    int l_startRow = (includeTitle == true) ? (worksheet.Dimension.Start.Row + 1) : worksheet.Dimension.Start.Row;
                    for (int row = l_startRow; row <= worksheet.Dimension.End.Row; row++)
                    {
                        if (worksheet.Cells.Value != null)
                        {
                            l_signalNameList.Add(worksheet.Cells[row, 1].Value.ToString());
                        }  
                    }
                }
            }
            signalNameList = l_signalNameList;
            return true;
        }
        #endregion OpenFile

        #region Search
        internal static bool SearchString(string inputString)
        {

            return false;
        }

        internal static int[] FindAllIndexOf (int[] values, int val)
        {
            return values.Select((b, i) => object.Equals(b, val) ? i : -1).Where(i => i != -1).ToArray();
        }

        #endregion Search

        #region TextEdit
        internal static string GetRequestResponse(string inputString)
        {
            string l_pattern = @"\[.*?\]";
            string l_replacement = "";
            string l_result = "";

            try
            {
                l_result = Regex.Replace(inputString, l_pattern, l_replacement, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(0.5));

            }
            catch
            {

            }

            return l_result;
        }

        internal static List<string> GetCounterName(string filePath)
        {
            List<string> l_result = new List<string>();
            return l_result;
        }

        /// <summary>
        /// Identify single frame and multi frame request/response
        /// </summary>
        /// <param name="inputString">Input hex string</param>
        /// <returns>
        /// 0: Multi frame request/response
        /// 1: Single frame request/response
        /// -1: Not a request/response
        /// </returns>
        public static int DetectSingleFrame(string inputString)
        {
            string l_input;

            l_input = inputString.Replace(" ", "");
            if (0 == 0)
            {
                if (l_input.Substring(0, 2) == "10")
                {
                    return 0;
                }
            }
            else
            {
                return -1;
            }
            
            return 1;
        }

        #endregion TextEdit

        #region TestedMethods
        public static string Add0x(string hexString)
        {
            string l_res = String.Empty;
            int l_strLength = hexString.Length;
            if (hexString.Length % 2 == 0)
            {
                for (int idx = 0; idx < l_strLength; idx++)
                {
                    if (idx % 2 == 0)
                    {
                        l_res += "0x";
                    }
                    l_res += hexString[idx];

                    if (idx % 2 != 0 && idx < l_strLength - 1)
                    {
                        l_res += ",";
                    }
                }
            }

            return l_res;
        }

        public static double ConvertInputSignal(int convertType, double inputValue, double resolution, double offset)
        {
            double l_result;
            if (convertType == 0)
            {
                l_result = (inputValue * resolution) + offset;
            }
            else
            {
                l_result = (inputValue - offset) / resolution;
            }
            return l_result;
        }

        

        #endregion TestedMethods
    }
}