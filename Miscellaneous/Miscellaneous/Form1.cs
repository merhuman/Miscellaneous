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
using System.Text.RegularExpressions;

// update lib
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;
using System.Data.SQLite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Miscellaneous
{
    public partial class Form1 : Form
    {
        /* enums */
        /** Conversion part **/
        enum Test { on = 6, off = 9};
        enum ConvertMode { StringToHex, HexToString, Add0x, Drop0x };
        string g_SID = string.Empty;
        string g_DID = string.Empty;
        
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
            ProgramLibs.TF_ReadEnvironmentalInformation_CRCCounter("F1B287", 1, 0, 4, 12.1, 20, 0, 7);
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
            g_odf_Misc.InitialDirectory = @"D:\TH\csharp\Miscellaneous\Miscellaneous\Miscellaneous\Configuration";

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
            ProgramLibs.ConvertToJson(ProgramLibs.FileType.DBC, g_filePath, g_fileName);
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
            g_odf_Misc.InitialDirectory = @"D:\TH\csharp\Miscellaneous\Miscellaneous\Miscellaneous\Configuration";

            
        }

        private void btn_Input2VSM_Click(object sender, EventArgs e)
        {
            double l_inputValue;
            double l_resolutionValue;
            double l_offSetValue;
            double l_rawValue;
            double l_vsmValue;
            double.TryParse(tb_InputValue.Text, out l_inputValue);
            double.TryParse(tb_ResolutionValue.Text, out l_resolutionValue);
            double.TryParse(tb_OffsetValue.Text, out l_offSetValue);
            l_rawValue = Math.Round((l_inputValue - l_offSetValue) / l_resolutionValue);
            l_vsmValue = l_rawValue * l_resolutionValue + l_offSetValue;
            tb_VSMValue.Text = l_vsmValue.ToString();
            tb_RawValue.Text = l_rawValue.ToString();
        }

        private void btn_VSM2Input_Click(object sender, EventArgs e)
        {
            double l_inputValue;
            double l_resolutionValue;
            double l_offSetValue;
            double l_rawValue;
            double l_vsmValue;
            double.TryParse(tb_VSMValue.Text, out l_vsmValue);
            double.TryParse(tb_ResolutionValue.Text, out l_resolutionValue);
            double.TryParse(tb_OffsetValue.Text, out l_offSetValue);
            l_rawValue = Math.Round((l_vsmValue - l_offSetValue) / l_resolutionValue);
            l_inputValue = l_rawValue * l_resolutionValue + l_offSetValue;
            tb_InputValue.Text = l_inputValue.ToString();
            tb_RawValue.Text = l_rawValue.ToString();
        }

        private void btn_Raw2InputVSM_Click(object sender, EventArgs e)
        {
            double l_inputValue;
            double l_resolutionValue;
            double l_offSetValue;
            double l_rawValue;
            double l_vsmValue;
            double.TryParse(tb_RawValue.Text, out l_rawValue);
            double.TryParse(tb_ResolutionValue.Text, out l_resolutionValue);
            double.TryParse(tb_OffsetValue.Text, out l_offSetValue);
            l_inputValue = l_rawValue * l_resolutionValue + l_offSetValue;
            l_vsmValue = l_rawValue * l_resolutionValue + l_offSetValue;
            tb_InputValue.Text = l_inputValue.ToString();
            tb_VSMValue.Text = l_vsmValue.ToString();
        }

        private void btn_GenParam_Click(object sender, EventArgs e)
        {
            string path = @"D:\TH\csharp\Miscellaneous\Miscellaneous\Miscellaneous\Configuration";
            ParamLibs.ParamFile paramStructure = new ParamLibs.ParamFile();
            paramStructure.GenerateParam(path);
            
        }

        private void Btn_GetRequestorReponse_Click(object sender, EventArgs e)
        {

        }

        private void Btn_LoadJsonFile_ParamGen_Click(object sender, EventArgs e)
        {
            g_odf_Misc.Filter = "Excel Workbook (*.xlsx)|*.xlsx|Excel Macro-Enabled Workbook (*.xlsm)|*.xlsm|Excel 97-2003 (*.xls)|*.xls|All files (*.*)|*.*";
            List<string> excelData = new List<string>();
            g_odf_Misc.InitialDirectory = @"D:\TH\csharp\Miscellaneous\Miscellaneous\Miscellaneous\Configuration";

            if (g_odf_Misc.ShowDialog() == DialogResult.OK)
            {
                g_fileName = g_odf_Misc.SafeFileName;
                g_filePath = g_odf_Misc.FileName;

                JObject rss = JObject.Parse(File.ReadAllText(g_filePath));
                //logFile.Name = (string)rss["Name"];
                //logFile.FilePath = (string)rss["FilePath"];
                //logFile.LogTime = (string)rss["LogTime"];
            }
        }

        private void Btn_LoadSensorParamFile(object sender, EventArgs e)
        {
            g_odf_Misc.Filter = "Excel Workbook (*.xlsx)|*.xlsx|Excel Macro-Enabled Workbook (*.xlsm)|*.xlsm|Excel 97-2003 (*.xls)|*.xls|All files (*.*)|*.*";
            List<string> excelData = new List<string>();
            g_odf_Misc.InitialDirectory = @"D:\TH\csharp\Miscellaneous\Miscellaneous\Miscellaneous\Configuration";

            if (g_odf_Misc.ShowDialog() == DialogResult.OK)
            {
                g_fileName = g_odf_Misc.SafeFileName;
                g_filePath = g_odf_Misc.FileName;
                
            }
        }

        private void Btn_CalculateSSParam_Click(object sender, EventArgs e)
        {

        }

        private void Btn_Input2Raw_Click(object sender, EventArgs e)
        {
            double l_inputValue;
            double l_resolutionValue;
            double l_offSetValue;
            double l_rawValue;

            double.TryParse(tb_InputValue.Text, out l_inputValue);
            double.TryParse(tb_ResolutionValue.Text, out l_resolutionValue);
            double.TryParse(tb_OffsetValue.Text, out l_offSetValue);

            l_rawValue = (l_inputValue - l_offSetValue) / l_resolutionValue;
            tb_RawValue.Text = l_rawValue.ToString();
        }

        private void btn_TestButton3_Click(object sender, EventArgs e)
        {

        }
    }
}
