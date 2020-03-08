using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using NPOI.XSSF.UserModel;  // remove later, use OfficeOpenXml instead
using NPOI.SS.UserModel;    // remove later, use OfficeOpenXml instead
using System.Text.RegularExpressions;

// update lib
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;
using System.Data.SQLite;


namespace Bosch
{
    public partial class Form1 : Form
    {
        /* enums */
        /** Conversion part **/
        enum Test { on = 6, off = 9};
        enum ConvertMode { StringToHex, HexToString, Add0x, Drop0x };
        string g_SID;
        string g_DID;
        
        /** Miscellaneous part **/

        /* Global variables */
        /** Conversion part **/
        ConvertMode g_convertMode = ConvertMode.StringToHex;

        /** Miscellaneous part **/
        OpenFileDialog g_odf_Misc = new OpenFileDialog();
        string g_filePath = string.Empty;
        string g_fileName = string.Empty;
        string g_fileContent = string.Empty;

        List<string> g_signalNameList = new List<string>();
        List<string> g_nodeNameList = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /* This will be used later */
            
        }
         
        private void button1_Click(object sender, EventArgs e)
        {
            XSSFWorkbook wb;
            using (FileStream file = new FileStream(@"C:\Users\DELL\source\test.xlsx", FileMode.Open, FileAccess.ReadWrite))
            {
                wb = new XSSFWorkbook(file);

            }
            
            ISheet sheet = wb.GetSheet("Sheet1");

            var rowIndex = 0;
            var row = sheet.CreateRow(rowIndex);
            row.CreateCell(0).SetCellValue("Username");
            using (FileStream file2 = new FileStream(@"C:\Users\DELL\source\test.xlsx", FileMode.Create, FileAccess.ReadWrite))
            {
                wb.Write(file2);
                file2.Close();

            }
        }
      
        private void button2_Click(object sender, EventArgs e)
        {
            Form2 testform = new Form2();
            testform.ShowDialog();
        }

        private void Btn_Convert_Click(object sender, EventArgs e)
        {
            int l_numberOfData;
            int l_accessoryByte;
            string l_paddingByte; // This is default padding byte.
            //string l_SID;   // This has not been implemented yet
            //string l_DID;   // This has not been implemented yet
            string l_outputResult1 = "";
            string l_outputResult2 = "";

            if (g_convertMode == ConvertMode.StringToHex)
            {
                if (!string.IsNullOrEmpty(tb_Input.Text))
                {
                    int.TryParse(tb_NumberOfData.Text, out l_numberOfData);
                    if (l_numberOfData >= tb_Input.Text.Length || l_numberOfData == 0)
                    {
                        l_accessoryByte = l_numberOfData - tb_Input.Text.Length;
                        l_paddingByte = (cb_Padding.Text == "") ? "0x00" : cb_Padding.Text;
                        string testString = cb_Padding.Text;

                        /* Change to normal hex string with space */
                        foreach (char letter in tb_Input.Text)
                        {
                            l_outputResult1 += string.Format(" {0:x2}", Convert.ToUInt16(letter)).ToUpper();
                        }
                        l_outputResult1 = l_outputResult1.TrimStart();

                        /* Change to hex string with 0x */
                        foreach (char letter in tb_Input.Text)
                        {
                            l_outputResult2 += ",0x" + string.Format("{0:x2}", Convert.ToUInt16(letter)).ToUpper();
                        }
                        l_outputResult2 = l_outputResult2.TrimStart(',');

                        /* Add the padding bytes */
                        if (l_numberOfData > 0)
                        {
                            l_paddingByte = l_paddingByte.Substring(2); // drop the 0x part of padding byte.
                            l_outputResult1 += string.Concat(Enumerable.Repeat(" " + l_paddingByte, l_accessoryByte));
                            l_outputResult2 += string.Concat(Enumerable.Repeat(",0x" + l_paddingByte, l_accessoryByte));
                        }

                        /* Add SID and DID */
                        if (cl_Options.GetItemChecked(0) && !cl_Options.GetItemChecked(1))
                        {
                            l_outputResult1 = ProgramLibs.Drop0xInHexString(g_SID) + " " + l_outputResult1;
                            l_outputResult2 = g_SID + "," + l_outputResult2;
                        }
                        else if (!cl_Options.GetItemChecked(0) && cl_Options.GetItemChecked(1))
                        {
                            l_outputResult1 = ProgramLibs.Drop0xInHexString(g_DID) + " " + l_outputResult1;
                            l_outputResult2 = g_DID + "," + l_outputResult2;
                        }
                        else if (cl_Options.GetItemChecked(0) && cl_Options.GetItemChecked(1))
                        {
                            l_outputResult1 = ProgramLibs.Drop0xInHexString(g_SID) + " " + ProgramLibs.Drop0xInHexString(g_DID) + " " + l_outputResult1;
                            l_outputResult2 = g_SID + "," + g_DID + "," + l_outputResult2;
                        }

                        tb_Output.Text = l_outputResult1 + Environment.NewLine + l_outputResult2;
                    }
                    else
                    {
                        ts_Status.Text = "Please let the box empty or input a \">= data_len\" number";
                        /* When the box is left empty, make sure that the value get from that box equal to 0 */
                    }
                }
                else
                {
                    int.TryParse(tb_NumberOfData.Text, out l_numberOfData);
                    l_paddingByte = (cb_Padding.Text == "") ? "0x00" : cb_Padding.Text;
                    l_paddingByte = l_paddingByte.Substring(2); // drop the 0x part of padding byte.
                    l_outputResult1 += string.Concat(Enumerable.Repeat(" " + l_paddingByte, l_numberOfData));
                    l_outputResult2 += string.Concat(Enumerable.Repeat(",0x" + l_paddingByte, l_numberOfData));
                    tb_Output.Text = l_outputResult1.Trim() + Environment.NewLine + l_outputResult2.Substring(1);
                }
            }
            else if (g_convertMode == ConvertMode.HexToString)
            {
                /* Input must be the hex sequence without 0x */
                ProgramLibs.StringType l_inputStringType = ProgramLibs.StringType.Normal;

                if (!string.IsNullOrEmpty(tb_Input.Text))
                {
                    /* Check input string type */
                    l_inputStringType = ProgramLibs.CheckTheInputFormat(tb_Input.Text);
                    if (l_inputStringType == ProgramLibs.StringType.Normal)
                    {
                        tb_Output.Text = ProgramLibs.HexStringToString(tb_Input.Text);
                    }
                    else
                    {
                        l_outputResult1 = tb_Input.Text.Replace("0x", "").Replace(",", "");
                        tb_Output.Text = ProgramLibs.HexStringToString(l_outputResult1);
                    }
                }
            }

        }

        private void Btn_Test_Click(object sender, EventArgs e)
        {
            string[] name = { "Tom", "Ron", "Jane" };
            
        }

        private void Tb_NumberOfData_KeyPress(object sender, KeyPressEventArgs e)
        {
            /* Prevent user from inputing a not digit value */
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void Btn_AddMisc_Click(object sender, EventArgs e)
        {
            /* Initial definition */
            string[] PAS_WorkCmd = new string[] { "True", "False"};
            string[] FPAS_AutoStgySts = new string[] { "On", "Off" };
            string[] Gear = new string[] { "D", "N", "R", "P" };
            string[] EPB = new string[] { "On", "Off" };
            string[] Speed = new string[] { "16", "9", "0" };
            string[] Obstacle = new string[] { "Yes", "No" };
            string[] ObstacleUnchanged = new string[] { "Yes", "No" };

            /* Read the existing excel file */
            XSSFWorkbook wb;
            using (FileStream file = new FileStream(@"E:\Work\2_Projects Update\GWM_VV5_6_7\GWM_VV\Opentest\PP5\3.Design\Configuration\PASDisplaySound.xlsx", FileMode.Open, FileAccess.Read))
            {
                wb = new XSSFWorkbook(file);
                file.Close();
            }

            ISheet sheet = wb.GetSheet("Sheet1");

            int rowIndex = 0;

            var row = sheet.CreateRow(rowIndex);

            row.CreateCell(0).SetCellValue("PAS_WorkCmd");
            row.CreateCell(1).SetCellValue("FPAS_AutoStgySts");
            row.CreateCell(2).SetCellValue("Gear");
            row.CreateCell(3).SetCellValue("EPB");
            row.CreateCell(4).SetCellValue("Speed");
            row.CreateCell(5).SetCellValue("Obstacle");
            row.CreateCell(6).SetCellValue("Obstacle unchange in 5s");
            row.CreateCell(7).SetCellValue("FPAS_WorkSts");
            row.CreateCell(8).SetCellValue("FPAS_DispCmd");
            row.CreateCell(9).SetCellValue("FPAS_SoundIndcn");
            row.CreateCell(10).SetCellValue("RPAS_WorkSts");
            row.CreateCell(11).SetCellValue("RPAS_SoundIndcn");
            rowIndex++;

            foreach (var pasCmd in PAS_WorkCmd)
            {
                foreach (var autoSts in FPAS_AutoStgySts)
                {
                    foreach (var gear in Gear)
                    {
                        foreach (var epb in EPB)
                        {
                            foreach (var speed in Speed)
                            {
                                foreach (var obstacle in Obstacle)
                                {
                                    foreach (var obstacleUnchanged in ObstacleUnchanged)
                                    {
                                        row = sheet.CreateRow(rowIndex);
                                        row.CreateCell(0).SetCellValue(pasCmd);
                                        row.CreateCell(1).SetCellValue(autoSts);
                                        row.CreateCell(2).SetCellValue(gear);
                                        row.CreateCell(3).SetCellValue(epb);
                                        row.CreateCell(4).SetCellValue(speed);
                                        row.CreateCell(5).SetCellValue(obstacle);
                                        row.CreateCell(6).SetCellValue(obstacleUnchanged);

                                        /* Expected status FPAS_WorkSts */
                                        row.CreateCell(7).SetCellValue((pasCmd == "True" || (pasCmd == "False" && (gear == "D" || gear == "N"))) ? "Active" : "Enable");

                                        /* Expected status FPAS_DispCmd */
                                        row.CreateCell(8).SetCellValue(((speed == "9" && obstacle == "Yes") || (speed == "0" && obstacle == "Yes" && obstacleUnchanged == "No"))? "On" : "Off");

                                        /* Expected status FPAS_SoundIncn */
                                        row.CreateCell(9).SetCellValue((pasCmd == "True" && gear != "P") || (((speed == "9" && obstacle == "Yes") || (speed == "0" && obstacle == "Yes" && obstacleUnchanged == "No"))) ? "On" : "Off");

                                        /* Expected status RPAS_WorkSts */
                                        row.CreateCell(10).SetCellValue((pasCmd == "True") ? "Active" : "Enable");

                                        /* Expected status RPAS_SoundIncn */
                                        row.CreateCell(11).SetCellValue((pasCmd == "True" && gear != "P") ? "On" : "Off");

                                        rowIndex++;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            /* Write new data into excel file */
            using (FileStream file2 = new FileStream(@"E:\Work\2_Projects Update\GWM_VV5_6_7\GWM_VV\Opentest\PP5\3.Design\Configuration\PASDisplaySound.xlsx", FileMode.Create, FileAccess.ReadWrite))
            {
                wb.Write(file2);
                file2.Close();
            }
        }

        private void Cb_Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_Mode.SelectedIndex == 0)
            {
                btn_AddDrop.Text = "Add 0x";
                btn_AddDrop.Enabled = false;
                g_convertMode = ConvertMode.StringToHex;
            }
            else if (cb_Mode.SelectedIndex == 1)
            {
                btn_AddDrop.Text = "Add 0x";
                btn_AddDrop.Enabled = false;
                g_convertMode = ConvertMode.HexToString;
            }
            else if (cb_Mode.SelectedIndex == 2)
            {
                btn_AddDrop.Text = "Add 0x";
                btn_AddDrop.Enabled = true;
                g_convertMode = ConvertMode.Add0x;
            }
            else if (cb_Mode.SelectedIndex == 3)
            {
                btn_AddDrop.Text = "Drop 0x";
                btn_AddDrop.Enabled = true;
                g_convertMode = ConvertMode.Drop0x;
            }
                
        }

        private void Btn_AddDrop_Click(object sender, EventArgs e)
        {
            /*
             * Currently this function is not complete, it's just usable.
             * The input string must follow below pattern
             * Add 0x: XX XX XX
             * Drop 0X: 0xXX,0xXX,0xXX
             */
            string l_input = tb_Input.Text;

            /* Add 0x to the string */
            if (cb_Mode.SelectedIndex == 2)
            {
                tb_Output.Text = ProgramLibs.Add0xToHexString(tb_Input.Text);
            }
            /* Drop 0x from the string */
            else if (cb_Mode.SelectedIndex == 3)
            {
                tb_Output.Text = tb_Input.Text.Replace("0x", " ").Replace(",", "").Replace("  ", " ").Trim();
                /*
                 * This line will replace all ",0x" and take string from 3rd letter to neglect
                 */
            }
        }

        private void Btn_Compare_Click(object sender, EventArgs e)
        {
            if (tb_Input.Text == tb_Output.Text)
            {
                ts_Status.Text = "Equal";
            }
            else
            {
                ts_Status.Text = "Not Equal";
            }
        }

        private void Btn_TestButton2_Click(object sender, EventArgs e)
        {
            /*
             * This function will be modified to be more effective than the current one.
             * */
            int l_numberOfProperties = new int();
            List<string> l_informationNames = new List<string>();
            List<List<string>> l_informationValues = new List<List<string>>();
            
            XSSFWorkbook wb;
            using (FileStream file_read = new FileStream(@"C:\Users\ghy3hc\Desktop\Miscellaneous\Bosch\Bosch\Configuration\Status.xlsx", FileMode.Open, FileAccess.Read))
            {
                wb = new XSSFWorkbook(file_read);
                file_read.Close();
            }

            ISheet sheet = wb.GetSheet("Information");

            // Get number of properties, please set the value to the exact cell.
            l_numberOfProperties = Convert.ToUInt16(sheet.GetRow(1).GetCell(0).ToString());
            int[] l_numberOfValues = new int[l_numberOfProperties]; // Declare an array to store number of values per property

            // Get the names of properties
            for (int column = 0; column < l_numberOfProperties; column++)
            {
                l_informationNames.Add(sheet.GetRow(0).GetCell(column + 1).ToString());
                l_numberOfValues[column] = Convert.ToInt16(sheet.GetRow(1).GetCell(column + 1).ToString());
            }

            // Get the values belonging to each property
            for (int column = 0; column < l_numberOfProperties; column++)
            {
                List<string> l_valueList = new List<string>();
                for (int row = 0; row < l_numberOfValues[column]; row++)
                {
                    l_valueList.Add(sheet.GetRow(row + 2).GetCell(column + 1).ToString());
                }
                l_informationValues.Add(l_valueList);
            }

            int[] l_numberOfDuplicationPerProp = new int[l_numberOfProperties];
            int[] l_numberOfDuplicationPerSet = new int[l_numberOfProperties];

            // Get the duplication times of values of each property
            int l_maximumDuplicationTime = 1;
            for (int i = 0; i < l_numberOfProperties; i++)
            {
                l_maximumDuplicationTime *= l_numberOfValues[i];
            }
            
            // Get the duplication times of value in each set belonging to each property
            for (int i = 0; i < l_numberOfProperties; i++)
            {
                int tempNumber = 1;
                for (int j = i + 1; j < l_numberOfProperties; j++)
                    tempNumber *= l_numberOfValues[j];
                l_numberOfDuplicationPerProp[i] = tempNumber;
            }

            // Get the duplication times of each set belonging to each property
            for (int i = 0; i < l_numberOfProperties; i++)
            {
                l_numberOfDuplicationPerSet[i] = l_maximumDuplicationTime / l_numberOfValues[i];
                l_numberOfDuplicationPerSet[i] = l_numberOfDuplicationPerSet[i] / l_numberOfDuplicationPerProp[i];
            }

            // Write all of values into excel file
            ISheet sheetOutput = wb.GetSheet("Variant Handling");

            // Clear old information in the sheet
            /* This hasnt been defined yet */

            // Create the rows for filling information
            for (int row = 0; row < l_maximumDuplicationTime; row++)
            {
                IRow rowSheet = sheetOutput.CreateRow(row);
            }

            //for (int column = 0; column < l_numberOfProperties - 1; column++)
            //{
            //    for (int row = 0; row < l_numberOfValues[column]; row++)
            //    {
            //        for (int time = 0; time < l_numberOfDuplicationPerProp[column]; time++)
            //        {
            //            IRow rowSheet = sheetOutput.GetRow(time + row*l_numberOfDuplicationPerProp[column]);
            //            rowSheet.CreateCell(column).SetCellValue(l_informationValues[column][row]);
            //        }
            //    }

            //}

            //for (int time = 0; time < l_numberOfDuplicationPerProp[l_numberOfProperties - 1]; time++)
            //{
            //    for (int row = 0; row < l_numberOfValues[l_numberOfProperties - 1]; row++)
            //    {
            //        IRow rowSheet = sheetOutput.GetRow(row + time * l_numberOfValues[l_numberOfProperties - 1]);
            //        rowSheet.CreateCell(l_numberOfProperties - 1).SetCellValue(l_informationValues[l_numberOfProperties - 1][row]);
            //    }
            //}

            for (int column = 0; column < l_numberOfProperties; column++)
            {
                for (int dup = 0; dup < l_numberOfDuplicationPerSet[column]; dup++)
                {
                    for (int row = 0; row < l_numberOfValues[column]; row++)
                    {
                        for (int time = 0; time < l_numberOfDuplicationPerProp[column]; time++)
                        {
                            IRow rowSheet = sheetOutput.GetRow(time + row * l_numberOfDuplicationPerProp[column] + dup * l_numberOfValues[column] * l_numberOfDuplicationPerProp[column]);
                            rowSheet.CreateCell(column).SetCellValue(l_informationValues[column][row]);
                        }
                    }
                }
            }

            using (FileStream file_write = new FileStream(@"C:\Users\ghy3hc\Desktop\Miscellaneous\Bosch\Bosch\Configuration\Status.xlsx", FileMode.Create, FileAccess.Write))
            {
                wb.Write(file_write);
                file_write.Close();
            }
        }

        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            tb_Input.Clear();
            tb_Output.Clear();

            tb_NumberOfChunksInput.Text = tb_NumberOfSpacesInput.Text = tb_NumberOfCharsInput.Text = "0";
            tb_NumberOfChunksOutput.Text = tb_NumberOfSpacesOutput.Text = tb_NumberOfCharsOutput.Text = "0";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            /*
             * This function will be modified to be more effective than the current one.
             * */
            int l_numberOfProperties = new int();
            List<string> l_informationNames = new List<string>();
            List<List<string>> l_informationValues = new List<List<string>>();

            XSSFWorkbook wb;
            using (FileStream file_read = new FileStream(@"C:\Users\ghy3hc\Desktop\Miscellaneous\Bosch\Bosch\Configuration\Status.xlsx", FileMode.Open, FileAccess.Read))
            {
                wb = new XSSFWorkbook(file_read);
                file_read.Close();
            }

            ISheet sheet = wb.GetSheet("Information2");

            // Get number of properties, please set the value to the exact cell.
            l_numberOfProperties = Convert.ToUInt16(sheet.GetRow(1).GetCell(0).ToString());
            int[] l_numberOfValues = new int[l_numberOfProperties]; // Declare an array to store number of values per property

            // Get the names of properties
            for (int column = 0; column < l_numberOfProperties; column++)
            {
                l_informationNames.Add(sheet.GetRow(0).GetCell(column + 1).ToString());
                l_numberOfValues[column] = Convert.ToInt16(sheet.GetRow(1).GetCell(column + 1).ToString());
            }

            // Get the values belonging to each property
            for (int column = 0; column < l_numberOfProperties; column++)
            {
                List<string> l_valueList = new List<string>();
                for (int row = 0; row < l_numberOfValues[column]; row++)
                {
                    l_valueList.Add(sheet.GetRow(row + 2).GetCell(column + 1).ToString());
                }
                l_informationValues.Add(l_valueList);
            }

            int[] l_numberOfDuplicationPerProp = new int[l_numberOfProperties];
            int[] l_numberOfDuplicationPerSet = new int[l_numberOfProperties];

            // Get the duplication times of values of each property
            int l_maximumDuplicationTime = 1;
            for (int i = 0; i < l_numberOfProperties; i++)
            {
                l_maximumDuplicationTime *= l_numberOfValues[i];
            }

            // Get the duplication times of value in each set belonging to each property
            for (int i = 0; i < l_numberOfProperties; i++)
            {
                int tempNumber = 1;
                for (int j = i + 1; j < l_numberOfProperties; j++)
                    tempNumber *= l_numberOfValues[j];
                l_numberOfDuplicationPerProp[i] = tempNumber;
            }

            // Get the duplication times of each set belonging to each property
            for (int i = 0; i < l_numberOfProperties; i++)
            {
                l_numberOfDuplicationPerSet[i] = l_maximumDuplicationTime / l_numberOfValues[i];
                l_numberOfDuplicationPerSet[i] = l_numberOfDuplicationPerSet[i] / l_numberOfDuplicationPerProp[i];
            }

            // Write all of values into excel file
            ISheet sheetOutput = wb.GetSheet("Variant Handling");

            // Clear old information in the sheet
            /* This hasnt been defined yet */

            // Create the rows for filling information
            for (int row = 0; row < l_maximumDuplicationTime; row++)
            {
                IRow rowSheet = sheetOutput.CreateRow(row);
            }

            for (int column = 0; column < l_numberOfProperties; column++)
            {
                for (int dup = 0; dup < l_numberOfDuplicationPerSet[column]; dup++)
                {
                    for (int row = 0; row < l_numberOfValues[column]; row++)
                    {
                        for (int time = 0; time < l_numberOfDuplicationPerProp[column]; time++)
                        {
                            IRow rowSheet = sheetOutput.GetRow(time + row * l_numberOfDuplicationPerProp[column] + dup * l_numberOfValues[column] * l_numberOfDuplicationPerProp[column]);
                            rowSheet.CreateCell(column).SetCellValue(l_informationValues[column][row]);
                        }
                    }
                }
            }

            using (FileStream file_write = new FileStream(@"C:\Users\ghy3hc\Desktop\Miscellaneous\Bosch\Bosch\Configuration\Status.xlsx", FileMode.Create, FileAccess.Write))
            {
                wb.Write(file_write);
                file_write.Close();
            }
        }

        private void Cb_SID_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(cb_SID.SelectedIndex)
            {
                case 0:
                    g_SID = "0x10";
                    break;

                case 1:
                    g_SID = "0x11";
                    break;

                case 2:
                    g_SID = "0x14";
                    break;

                case 3:
                    g_SID = "0x22";
                    break;

                case 4:
                    g_SID = "0x28";
                    break;

                case 5:
                    g_SID = "0x85";
                    break;

                case 6:
                    g_SID = "0x3E";
                    break;

                case 7:
                    g_SID = "0x2E";
                    break;

                case 8:
                    g_SID = "0x19";
                    break;

                case 9:
                    g_SID = "0x31";
                    break;

                case 10:
                    g_SID = "0x27";
                    break;
            }
        }

        private void Tb_DID_TextChanged(object sender, EventArgs e)
        {
            g_DID = tb_DID.Text;
        }

        private void Btn_OpenFile_Misc_Click(object sender, EventArgs e)
        {
            g_odf_Misc.Filter = "Excel Workbook (*.xlsx)|*.xlsx|Excel Macro-Enabled Workbook (*.xlsm)|*.xlsm|Excel 97-2003 (*.xls)|*.xls|All files (*.*)|*.*";
            List<string> excelData = new List<string>();
            g_odf_Misc.InitialDirectory = @"D:\TH\csharp\Miscellaneous\Bosch\Bosch\Configuration";

            if (g_odf_Misc.ShowDialog() == DialogResult.OK)
            {
                g_fileName = g_odf_Misc.SafeFileName;
                g_filePath = g_odf_Misc.FileName;
                ProgramLibs.OpenFile(ProgramLibs.FileType.Excel, g_filePath, g_fileName, ref g_signalNameList, cb_TitleInclude.Checked);
            }
        }

        private void Btn_OpenFileDBC_Misc_Click(object sender, EventArgs e)
        {
            g_odf_Misc.Filter = "CANdb++ Database (.mdc)(*.mdc)|*.mdc|CANdb Network (.dbc)(*.dbc)|*.dbc|CAN Database (.mdc;.dbc)(*.mdc;*.dbc)|*.mdc, *dbc |All files (*.*)|*.*";
            List<string> excelData = new List<string>();
            g_odf_Misc.InitialDirectory = @"D:\TH\csharp";

            string testString = string.Empty;

            if (g_odf_Misc.ShowDialog() == DialogResult.OK)
            {
                g_fileName = g_odf_Misc.SafeFileName;
                g_filePath = g_odf_Misc.FileName;
            }

            ProgramLibs.OpenFile(ProgramLibs.FileType.DBC, g_filePath, g_fileName, ref g_nodeNameList, null);
        }

        private void Btn_ClearInput_Click(object sender, EventArgs e)
        {
            tb_Input.Clear();
            tb_NumberOfChunksInput.Text = tb_NumberOfSpacesInput.Text = tb_NumberOfCharsInput.Text = "0";
        }

        private void Btn_ClearOutput_Click(object sender, EventArgs e)
        {
            tb_Output.Clear();
            tb_NumberOfChunksOutput.Text = tb_NumberOfSpacesOutput.Text = tb_NumberOfCharsOutput.Text = "0";
        }

        private void Btn_countInput_Click(object sender, EventArgs e)
        {
            tb_NumberOfChunksInput.Text = ProgramLibs.CountNumberOfChunks(tb_Input.Text).ToString();
            tb_NumberOfSpacesInput.Text = ProgramLibs.CountNumberOfSpace(tb_Input.Text).ToString();
            tb_NumberOfCharsInput.Text = tb_Input.Text.Count().ToString();
        }

        private void Btn_countOutput_Click(object sender, EventArgs e)
        {
            tb_NumberOfChunksOutput.Text = ProgramLibs.CountNumberOfChunks(tb_Output.Text).ToString();
            tb_NumberOfSpacesOutput.Text = ProgramLibs.CountNumberOfSpace(tb_Output.Text).ToString();
            tb_NumberOfCharsOutput.Text = tb_Output.Text.Count().ToString();
        }

        private void Btn_LoadProject_Click(object sender, EventArgs e)
        {
            g_odf_Misc.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            List<string> excelData = new List<string>();
            g_odf_Misc.InitialDirectory = @"D:\TH\csharp\Miscellaneous\Bosch\Bosch\Configuration";

            
        }

        private void btn_Input2VSM_Click(object sender, EventArgs e)
        {
            
        }

        private void btn_VSM2Input_Click(object sender, EventArgs e)
        {

        }
    }
}
