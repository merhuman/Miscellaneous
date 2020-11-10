using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using System.Data;
using System.Data.SQLite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        internal enum FileType { Excel, SSParam, DBC, Json, A2L, HTML, Undefined };
        public static string g_nodePrefix = "BU_: ";
        public static string g_mesPrefix = "BO_ ";
        public static string g_sigPrefix = " SG_ ";
        public static string g_valTabPrefix = "VAL_ ";

        public static string g_vsmRegex = @"(VSM_)[ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz1234567890]*";
        public static string g_fusRegex = @"(FUS_)[ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz1234567890]*";

        #region UnderDevelopment
        /// <summary>
        /// Deduce invalid DIDs from valid DIDs in decimal form.
        /// Then write them to excel file under hexadecimal form.
        /// </summary>
        /// <param name="validDID"></param>
        /// <returns></returns>
        public static List<string> GenerateUnknownDID(string[] validDID)
        {
            List<string> l_res = new List<string>();
            int l_upperThres = 65535; // or 0xFFFF
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveFileDialog1.Title = "Save Excel As";
            saveFileDialog1.InitialDirectory = @"E:\Work";
            saveFileDialog1.ShowDialog();

            string l_savedPath = saveFileDialog1.FileName;

            FileInfo l_excelFile = new FileInfo(l_savedPath);
            if (l_savedPath != "")
            {
                using (ExcelPackage l_excelPackage = new ExcelPackage(l_excelFile))
                {
                    int l_startDID = 4096; // change this to suit used case.

                    if (l_excelPackage.Workbook.Worksheets.Any(sheet => sheet.Name == "Unused DIDs"))
                    {
                        l_excelPackage.Workbook.Worksheets.Delete("Unused DIDs");
                    }

                    ExcelWorksheet l_worksheet = l_excelPackage.Workbook.Worksheets.Add("Unused DIDs");

                    for (int rowIdx = 1; rowIdx <= 256; rowIdx++)
                    {
                        for (int colIdx = 1; colIdx <= 256; colIdx++)
                        {
                            string l_counterStr = String.Format("{0:x4}", l_startDID);
                            if (!validDID.Any(l_did => l_did == l_counterStr))
                            {
                                l_worksheet.Cells[rowIdx, colIdx].Value = l_counterStr;
                            }
                            
                            l_startDID++;
                            if (l_startDID > l_upperThres) goto LoopEnd;
                        }

                    }

                LoopEnd:
                    Console.WriteLine("Finish");
                    l_excelPackage.Save();
                }
            }
            
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
        /*
         * For now there are 2 versions of OpenFile method. This should be refined later for clearer structure. 
         */
        internal static bool OpenFile(FileType fileType, string inputFilePath, string fileName, bool? includeTitle)
        {
            string l_sourcePath = "./Configuration";
            Directory.CreateDirectory(l_sourcePath);
            l_sourcePath = Path.Combine(l_sourcePath, fileName);

            List<string> l_nameList = new List<string>();

            if (!string.IsNullOrEmpty(inputFilePath))
            {
                // Might add try catch structure here later.
                File.Copy(inputFilePath, l_sourcePath, true);
                if (includeTitle == null || includeTitle == false)
                    ReadAllSignalNamesFromExcel(l_sourcePath, ref l_nameList, includeTitle: false);
                else
                    ReadAllSignalNamesFromExcel(l_sourcePath, ref l_nameList, includeTitle: true);
            }
            return false;
        }

        internal static bool OpenFile(FileType fileType, string inputFilePath, string fileName)
        {
            string l_sourcePath = "./Configuration";
            Directory.CreateDirectory(l_sourcePath);
            l_sourcePath = Path.Combine(l_sourcePath, fileName);
            string l_outputPath = String.Empty;
            List<string> l_nodeList = new List<string>();
            List<string[]> l_mesList = new List<string[]>();
            List<string[]> l_sigList = new List<string[]>();
            List<string[]> l_valTabList = new List<string[]>();
            List<string> l_fusList = new List<string>();
            List<string> l_vsmList = new List<string>();
            List<string> l_nameList = new List<string>();

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            if (!string.IsNullOrEmpty(inputFilePath))
            {
                switch (fileType)
                {
                    case FileType.Excel:
                        // Might add try catch structure here later.
                        File.Copy(inputFilePath, l_sourcePath, true);                       
                        ReadAllSignalNamesFromExcel(l_sourcePath, ref l_nameList, includeTitle: true);
                        break;

                    case FileType.DBC:
                        l_sourcePath = Path.ChangeExtension(l_sourcePath, ".txt");  // convert dbc file to txt file for more convenient purposes
                        File.Copy(inputFilePath, l_sourcePath, true);
                        GetNodesFromDBCFile(l_sourcePath, ref l_nodeList);
                        GetMessageAndSignalFromDBCFile(l_sourcePath, ref l_mesList, ref l_sigList);
                        GetValTablesFromDBCFile(l_sourcePath, ref l_valTabList);

                        // temporary solution -- excel -- gen mes and sig into corresponding sheets
                        saveFileDialog1.Filter = "Excel (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                        saveFileDialog1.Title = "Save Excel As";
                        saveFileDialog1.InitialDirectory = @"E:\Work";
                        saveFileDialog1.ShowDialog();

                        string l_savedPath = saveFileDialog1.FileName;

                        FileInfo l_excelFile = new FileInfo(l_savedPath);

                        if (l_savedPath != "")
                        {
                            using (ExcelPackage l_excelPackage = new ExcelPackage(l_excelFile))
                            {
                                DataTable l_dataTable = new DataTable();

                                // Add mes
                                l_dataTable.Columns.Add("No", typeof(string));
                                l_dataTable.Columns.Add("MesID(hex)", typeof(string));
                                l_dataTable.Columns.Add("MesID(dex)", typeof(string));
                                l_dataTable.Columns.Add("MesName", typeof(string));
                                l_dataTable.Columns.Add("DLC", typeof(string));
                                l_dataTable.Columns.Add("Transmitter", typeof(string));

                                for(int idx = 0; idx < l_mesList.Count(); idx++)
                                {
                                    Int64 _mesId = Convert.ToInt64(l_mesList[idx][0]);
                                    if (0 <= _mesId && _mesId <= 0x7ff) // for common ID number
                                    {
                                        l_dataTable.Rows.Add(idx + 1,
                                            String.Format("{0:x3}", _mesId),
                                            l_mesList[idx][0],
                                            l_mesList[idx][1],
                                            l_mesList[idx][2],
                                            l_mesList[idx][3]);
                                    }
                                    else if (0x80000000 <= _mesId && _mesId <= 0x8fffffff) // for unusual ID number.
                                    {
                                        _mesId -= 0x80000000;
                                        l_dataTable.Rows.Add(idx + 1,
                                            String.Format("{0:x}", _mesId),
                                            String.Format("{0}", _mesId),
                                            l_mesList[idx][1],
                                            l_mesList[idx][2],
                                            l_mesList[idx][3]);
                                    }
                                }

                                if (l_excelPackage.Workbook.Worksheets.Any(sheet => sheet.Name == "MessageList"))
                                {
                                    l_excelPackage.Workbook.Worksheets.Delete("MessageList");
                                }
                                ExcelWorksheet l_worksheet = l_excelPackage.Workbook.Worksheets.Add("MessageList");

                                l_worksheet.Cells["A1"].LoadFromDataTable(l_dataTable, true);

                                // Add sig
                                l_dataTable.Columns.Clear();
                                l_dataTable.Rows.Clear();
                                l_dataTable.Columns.Add("No", typeof(string));
                                l_dataTable.Columns.Add("MesName", typeof(string));
                                l_dataTable.Columns.Add("SigName", typeof(string));
                                l_dataTable.Columns.Add("FacRes", typeof(string));
                                l_dataTable.Columns.Add("MinMax", typeof(string));
                                l_dataTable.Columns.Add("Unit", typeof(string));
                                l_dataTable.Columns.Add("Receiver", typeof(string));

                                for (int idx = 0; idx < l_sigList.Count(); idx++)
                                {
                                    l_dataTable.Rows.Add(idx + 1, 
                                        l_sigList[idx][0],
                                        l_sigList[idx][1],
                                        l_sigList[idx][3],
                                        l_sigList[idx][4],
                                        l_sigList[idx][5],
                                        l_sigList[idx][6]);
                                }

                                if (l_excelPackage.Workbook.Worksheets.Any(sheet => sheet.Name == "SignalList"))
                                {
                                    l_excelPackage.Workbook.Worksheets.Delete("SignalList");
                                }
                                l_worksheet = l_excelPackage.Workbook.Worksheets.Add("SignalList");

                                l_worksheet.Cells["A1"].LoadFromDataTable(l_dataTable, true);

                                // Add Value Table
                                l_dataTable.Columns.Clear();
                                l_dataTable.Rows.Clear();
                                l_dataTable.Columns.Add("No", typeof(string));
                                l_dataTable.Columns.Add("MesID(hex)", typeof(string));
                                l_dataTable.Columns.Add("MesID(dec)", typeof(string));
                                l_dataTable.Columns.Add("SigName", typeof(string));
                                l_dataTable.Columns.Add("ValTable", typeof(string));

                                for (int idx = 0; idx < l_valTabList.Count(); idx++)
                                {
                                    Int64 _mesId = Convert.ToInt64(l_valTabList[idx][0]);
                                    if (0 <= _mesId && _mesId <= 0x7ff) // for common ID number
                                    {
                                        l_dataTable.Rows.Add(idx + 1,
                                            String.Format("{0:x3}", _mesId),
                                            l_valTabList[idx][0],
                                            l_valTabList[idx][1],
                                            l_valTabList[idx][2]);
                                    }
                                    else if (0x80000000 <= _mesId && _mesId <= 0x8fffffff) // for unusual ID number.
                                    {
                                        _mesId -= 0x80000000;
                                        l_dataTable.Rows.Add(idx + 1,
                                            String.Format("{0:x}", _mesId),
                                            String.Format("{0}", _mesId),
                                            l_valTabList[idx][1],
                                            l_valTabList[idx][2]);
                                    }
                                }

                                if (l_excelPackage.Workbook.Worksheets.Any(sheet => sheet.Name == "ValueTableList"))
                                {
                                    l_excelPackage.Workbook.Worksheets.Delete("ValueTableList");
                                }
                                l_worksheet = l_excelPackage.Workbook.Worksheets.Add("ValueTableList");

                                l_worksheet.Cells["A1"].LoadFromDataTable(l_dataTable, true);

                                l_excelPackage.Save(); // need to add checking open excel file method before this line.
                                ;
                            }
                            
                        }
                        else
                        {

                        }

                        //ConvertToSQLite();
                        // This has to wait
                        //saveFileDialog1.Filter = "Json (*.json)|*.json|All files (*.*)|*.*";
                        //saveFileDialog1.Title = "Save Json As";
                        //saveFileDialog1.ShowDialog();
                        //if (saveFileDialog1.FileName != "")
                        //{
                        //    switch (saveFileDialog1.FilterIndex)
                        //    {
                        //        case 1:

                        //            break;

                        //        default:
                        //            // not defined filter type or "All File" type.
                        //            break;
                        //    }
                        //}
                        
                        //ConvertToJson(FileType.DBC);
                        break;

                    case FileType.Json:
                        File.Copy(inputFilePath, l_sourcePath, true);
                        break;

                    case FileType.A2L:
                        l_sourcePath = Path.ChangeExtension(l_sourcePath, ".txt");  // convert a2l file to txt file for more convenient purposes
                        File.Copy(inputFilePath, l_sourcePath, true);
                        GetVSMnFUSFromFile(l_sourcePath, ref l_vsmList, ref l_fusList);
                        break;

                    case FileType.HTML: // for now let's just keep it same as A2L file type.
                        l_sourcePath = Path.ChangeExtension(l_sourcePath, ".txt");  // convert a2l file to txt file for more convenient purposes
                        File.Copy(inputFilePath, l_sourcePath, true);
                        GetVSMnFUSFromFile(l_sourcePath, ref l_vsmList, ref l_fusList);
                        break;

                    default:
                        return false;
                }
                ;
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
                get => _name;
                set => _name = value;
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

        internal static bool ConvertToJson(FileType fileType, string filePath, string fileName)
        {
            if (File.Exists(filePath))
            {
                
            }
            else
            {
                //System.IO.FileStream fs = File.Open(filePath, );

                //fs.Close();
                // This structure is bulky, not suitable for enquiry
                //JObject rss1 =
                //    new JObject(
                //        new JProperty("ProjectInfo",
                //            new JProperty("Name", ""),
                //            new JProperty("Release", new JArray { }),
                //            new JProperty("Variant", new JArray { }),
                //        new JProperty("DBC", 
                //            new JProperty("Node", new JArray { }),
                //            new JProperty("Message",
                //                new JProperty("RandomID", 
                //                    new JProperty("Name", "TestSignalName"),
                //                    new JProperty("Transmitter", new JArray { }),
                //                    new JProperty("Signal",
                //                        new JProperty("TestSignalName", 
                //                            new JProperty("EndBit", 7),
                //                            new JProperty("BitLength", 8),
                //                            new JProperty("Factor", 1),
                //                            new JProperty("Offset", 0),
                //                            new JProperty("Minimum", 0),
                //                            new JProperty("Maximum", 255),
                //                            new JProperty("Unit", "Not defined"),
                //                            new JProperty("Receiver", new JArray { }),
                //                            new JProperty("InitValueRaw", 0),
                //                            new JProperty("ValueTable"), 
                //                            new JProperty("InvalidValue", new JArray { }),
                //                            new JProperty("UsedInRelease", new JArray { }),
                //                            new JProperty("VSMName", ""),
                //                            new JProperty("ADIName", ""))))))));
                
            }
            
            return false;
        }

        internal static bool ConvertToSQLite(FileType fileType, string inputFilePath, string fileName)
        {
            if (File.Exists(inputFilePath))
            {

            }
            return false;
        }

        internal static bool GetNodesFromDBCFile(string filePath, ref List<string> nodeList)
        {
            string[] l_nodeNameList = new string[] { } ; 
            if (File.Exists(filePath))
            {
                foreach (var line in File.ReadAllLines(filePath))
                {
                    if (line.StartsWith(g_nodePrefix))
                    {
                        l_nodeNameList = line.Split(' ');
                        nodeList = l_nodeNameList.ToList();
                        nodeList.Remove(g_nodePrefix);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Get names of messages and signals
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="messageList"></param>
        /// <param name="signalList"></param>
        /// <returns>
        /// false: if there is no message
        /// true: if there is at least one message
        /// </returns>
        internal static bool GetMessageAndSignalFromDBCFile(string filePath, ref List<string[]> messageList, ref List<string[]> signalList)
        {
            string[] l_messageNameList = new string[] { };
            string[] l_signalNameList = new string[] { };
            //string[] l_res = new string[] { };
            string[] l_res = Enumerable.Repeat(String.Empty, 4).ToArray();
            string[] l_res2 = Enumerable.Repeat(String.Empty, 8).ToArray();
            string l_previousMes = String.Empty;

            foreach (var line in File.ReadAllLines(filePath))
            {
                if (line.StartsWith(g_mesPrefix))
                {
                    //l_messageNameList = line.Split(' ');
                    l_messageNameList = Regex.Split(line, @"\s+");
                    //messageList = l_messageNameList.ToList();
                    //messageList.Remove(g_mesPrefix);
                    l_messageNameList[2] = l_messageNameList[2].Replace(":", "");
                    Array.Copy(l_messageNameList, 1, l_res, 0, 4);  // this line gets error if the string array is not defined clearly.
                    l_previousMes = l_res[1]; // careful with this one, it might cause issue
                    messageList.Add((string[])l_res.Clone());
                }
                else if (line.StartsWith(g_sigPrefix))
                {
                    // problem with signal split. Need to use regex.
                    //l_signalNameList = line.Split(' ');
                    l_signalNameList = Regex.Split(line.Replace(":",""), @"\s+");
                    l_res2[0] = l_previousMes;
                    Array.Copy(l_signalNameList, 2, l_res2, 1, 6);  // this line gets error if the string array is not defined clearly.
                    signalList.Add((string[])l_res2.Clone());
                }
            }
            if (messageList.Count() != 0) return true;
            return false;
        }

        /// <summary>
        /// Get value tables
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="valTableList"></param>
        /// <returns>
        /// false: if there is no value table
        /// true: if there is at least one value table
        /// </returns>
        internal static bool GetValTablesFromDBCFile(string filePath, ref List<string[]> valTableList)
        {
            List <string> l_valTableList = new List<string>();
            bool l_longValTabFlag = false;
            string[] l_res = Enumerable.Repeat(String.Empty, 3).ToArray();
            foreach (var line in File.ReadAllLines(filePath))
            {
                if (line.StartsWith(g_valTabPrefix) && l_longValTabFlag == false)
                {
                    l_valTableList = Regex.Split(line, @"\s+").ToList();
                    l_res[0] = l_valTableList[1]; // exclude prefix
                    l_res[1] = l_valTableList[2];
                    l_res[2] = l_valTableList.Where(x => x != l_valTableList[0] && x != l_valTableList[1] && x != l_valTableList[2]).Aggregate((i, j) => i + j);

                    if (!l_res[2].EndsWith(";"))
                    {
                        l_longValTabFlag = true; // Value table has more than 1 line.
                        continue;
                    }
                    else
                    {
                        l_longValTabFlag = false; // reset the flag
                    }
                }
                else if (l_longValTabFlag == true && !Regex.IsMatch(line, @"^\s+$")) //current line is not an empty line
                {
                    l_res[2] = String.Concat(l_res[2], line);
                }
                else continue; // current line is empty -> skip it.
                valTableList.Add((string[])l_res.Clone());
                l_longValTabFlag = false;
            }
            if (valTableList.Count() != 0) return true;
            return false;
        }

        internal static bool GetVSMnFUSFromFile(string filePath, ref List<string> vsmList, ref List<string> fusList)
        {
            HashSet<string> l_vsmList = new HashSet<string>();  // using HashSet helps to boost the process speed.
            HashSet<string> l_adiList = new HashSet<string>();
            Regex l_vsmPattern = new Regex(g_vsmRegex);
            Regex l_fusPattern = new Regex(g_fusRegex);
            string l_content = String.Empty;

            l_content = File.ReadAllText(filePath);
            MatchCollection l_rawVSMMatchCollection = l_vsmPattern.Matches(l_content);
            MatchCollection l_rawFUSMatchCollection = l_fusPattern.Matches(l_content);
            
            if (l_rawVSMMatchCollection.Count != 0)
            {
                foreach (Match match in l_rawVSMMatchCollection)
                {
                    l_vsmList.Add(match.Value);
                }
            }

            // error code. unclear cause
            if (l_rawFUSMatchCollection.Count != 0)
            {
                foreach (Match match in l_rawFUSMatchCollection)
                {
                    l_adiList.Add(match.Value);
                }
            }

            vsmList = l_vsmList.Distinct().ToList();
            fusList = l_adiList.Distinct().ToList();
            if (vsmList.Count() != 0 || fusList.Count() != 0) return true;
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
        internal static bool SearchString(string filePath, ref List<string[]> messageList, ref List<string[]> signalList)
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

        #region ConvertMethods
        public static bool ConvertExcel2Param(string filePath, string sheetName, bool titleInclude)
        {
            List<string> l_excelData = new List<string>();
            byte[] l_bin = File.ReadAllBytes(filePath);
            
            using (ExcelPackage l_excelPackage = new ExcelPackage(new MemoryStream(l_bin)))
            {
                ExcelWorksheet l_worksheet = l_excelPackage.Workbook.Worksheets[sheetName];
                for (int rowIdx = (titleInclude == false)? l_worksheet.Dimension.Start.Row : l_worksheet.Dimension.Start.Row + 1; rowIdx <= l_worksheet.Dimension.End.Row; rowIdx++)
                {
                    for (int colIdx = l_worksheet.Dimension.Start.Column; colIdx <= l_worksheet.Dimension.End.Column; colIdx++)
                    {
                        if (l_worksheet.Cells[rowIdx, colIdx].Value != null)
                        {
                            l_excelData.Add(l_worksheet.Cells[rowIdx, colIdx].Value.ToString());
                        }
                    }
                }
            }
            //if (l_excelData.Count() != 0) return true;
            return false;
        }

        public static bool ConvertParam2Excel()
        {

            return false;
        }
        #endregion ConvertMethods

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
