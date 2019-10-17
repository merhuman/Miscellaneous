using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;

namespace Bosch
{
    class ProgramLibs
    {
        internal ProgramLibs()
        {

        }

        #region UnderDevelopment
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
        internal enum FileType { Excel, DBC, Undefined };
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

        #region OpenFile
        internal static bool OpenFile(FileType fileType, string inputFilePath, string fileName, ref List<string> nameList)
        {
            string l_sourcePath = "./Configuration";
            Directory.CreateDirectory(l_sourcePath);
            l_sourcePath = Path.Combine(l_sourcePath, fileName);

            switch (fileType)
            {
                case FileType.Excel:
                    File.Copy(inputFilePath, l_sourcePath, true);
                    ReadAllSignalNames(l_sourcePath, ref nameList);
                    break;

                case FileType.DBC:
                    l_sourcePath = Path.ChangeExtension(l_sourcePath, ".txt");  // convert dbc file to txt file for more convenient purpose
                    File.Copy(inputFilePath, l_sourcePath, true);
                    GetNodesFromDBCFile(l_sourcePath, ref nameList);
                    break;

                default:
                    return false;
            }
            return true;
        }

        internal static bool ReadAllSignalNames(string filePath, ref List<string> signalNameList)
        {
            List<string> l_signalNameList = new List<string>();
            byte[] bin = File.ReadAllBytes(filePath);

            using (MemoryStream stream = new MemoryStream(bin))
            using (ExcelPackage excelPackage = new ExcelPackage(stream))
            {
                foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                {
                    for (int row = worksheet.Dimension.Start.Row; row <= worksheet.Dimension.End.Row; row++)
                    {
                        for (int column = worksheet.Dimension.Start.Column; column <= worksheet.Dimension.End.Column; column++)
                        {
                            if (worksheet.Cells.Value != null)
                            {
                                l_signalNameList.Add(worksheet.Cells[row, column].Value.ToString());
                            }
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

        internal static bool GetNodesFromDBCFile(string filePath, ref List<string> nodeList)
        {
            string[] l_nodeNameList;
            foreach (var line in File.ReadAllLines(filePath))
            {
                if (line.StartsWith("BU_:"))
                {
                    l_nodeNameList = line.Split(' ');
                    nodeList = l_nodeNameList.ToList<string>();
                    return true;
                }
            }
            return false;
        }
        #endregion Search
    }

    class BloggingContext : DbContext
    {
    }
}
