namespace Bosch
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tab_FunctionControl = new System.Windows.Forms.TabControl();
            this.tab_Convert = new System.Windows.Forms.TabPage();
            this.btn_Clear = new System.Windows.Forms.Button();
            this.btn_Compare = new System.Windows.Forms.Button();
            this.btn_AddDrop = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btn_Convert = new System.Windows.Forms.Button();
            this.btn_Save = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tb_Output = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tb_Input = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_SID = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_ID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_DID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cl_Options = new System.Windows.Forms.CheckedListBox();
            this.tb_NumberOfData = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_Mode = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_Padding = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btn_OpenFileExcel_Misc = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btn_TestButton2 = new System.Windows.Forms.Button();
            this.btn_ArrangeMisc = new System.Windows.Forms.Button();
            this.sts_Strip = new System.Windows.Forms.StatusStrip();
            this.ts_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.ofd_OpenfileMisc = new System.Windows.Forms.OpenFileDialog();
            this.btn_OpenFileDBC_Misc = new System.Windows.Forms.Button();
            this.tab_FunctionControl.SuspendLayout();
            this.tab_Convert.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.sts_Strip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab_FunctionControl
            // 
            this.tab_FunctionControl.Controls.Add(this.tab_Convert);
            this.tab_FunctionControl.Controls.Add(this.tabPage2);
            this.tab_FunctionControl.Location = new System.Drawing.Point(3, 3);
            this.tab_FunctionControl.Name = "tab_FunctionControl";
            this.tab_FunctionControl.SelectedIndex = 0;
            this.tab_FunctionControl.Size = new System.Drawing.Size(788, 311);
            this.tab_FunctionControl.TabIndex = 2;
            // 
            // tab_Convert
            // 
            this.tab_Convert.Controls.Add(this.btn_Clear);
            this.tab_Convert.Controls.Add(this.btn_Compare);
            this.tab_Convert.Controls.Add(this.btn_AddDrop);
            this.tab_Convert.Controls.Add(this.button4);
            this.tab_Convert.Controls.Add(this.btn_Convert);
            this.tab_Convert.Controls.Add(this.btn_Save);
            this.tab_Convert.Controls.Add(this.groupBox3);
            this.tab_Convert.Controls.Add(this.groupBox2);
            this.tab_Convert.Controls.Add(this.groupBox1);
            this.tab_Convert.Location = new System.Drawing.Point(4, 22);
            this.tab_Convert.Name = "tab_Convert";
            this.tab_Convert.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Convert.Size = new System.Drawing.Size(780, 285);
            this.tab_Convert.TabIndex = 0;
            this.tab_Convert.Text = "Convert";
            this.tab_Convert.UseVisualStyleBackColor = true;
            // 
            // btn_Clear
            // 
            this.btn_Clear.Location = new System.Drawing.Point(417, 248);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(70, 25);
            this.btn_Clear.TabIndex = 9;
            this.btn_Clear.Text = "Clear";
            this.btn_Clear.UseVisualStyleBackColor = true;
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // btn_Compare
            // 
            this.btn_Compare.Location = new System.Drawing.Point(519, 242);
            this.btn_Compare.Name = "btn_Compare";
            this.btn_Compare.Size = new System.Drawing.Size(66, 37);
            this.btn_Compare.TabIndex = 8;
            this.btn_Compare.Text = "Compare";
            this.btn_Compare.UseVisualStyleBackColor = true;
            this.btn_Compare.Click += new System.EventHandler(this.btn_Compare_Click);
            // 
            // btn_AddDrop
            // 
            this.btn_AddDrop.Location = new System.Drawing.Point(601, 199);
            this.btn_AddDrop.Name = "btn_AddDrop";
            this.btn_AddDrop.Size = new System.Drawing.Size(70, 37);
            this.btn_AddDrop.TabIndex = 7;
            this.btn_AddDrop.Text = "Add 0x";
            this.btn_AddDrop.UseVisualStyleBackColor = true;
            this.btn_AddDrop.Click += new System.EventHandler(this.btn_AddDrop_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(682, 242);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 6;
            this.button4.Text = "test button";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // btn_Convert
            // 
            this.btn_Convert.Location = new System.Drawing.Point(687, 199);
            this.btn_Convert.Name = "btn_Convert";
            this.btn_Convert.Size = new System.Drawing.Size(75, 37);
            this.btn_Convert.TabIndex = 1;
            this.btn_Convert.Text = "Convert";
            this.btn_Convert.UseVisualStyleBackColor = true;
            this.btn_Convert.Click += new System.EventHandler(this.btn_Convert_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(519, 199);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(66, 37);
            this.btn_Save.TabIndex = 3;
            this.btn_Save.Text = "Save";
            this.btn_Save.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tb_Output);
            this.groupBox3.Location = new System.Drawing.Point(6, 127);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(481, 118);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Output";
            // 
            // tb_Output
            // 
            this.tb_Output.Location = new System.Drawing.Point(6, 19);
            this.tb_Output.Multiline = true;
            this.tb_Output.Name = "tb_Output";
            this.tb_Output.Size = new System.Drawing.Size(469, 83);
            this.tb_Output.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tb_Input);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(481, 115);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Input";
            // 
            // tb_Input
            // 
            this.tb_Input.Location = new System.Drawing.Point(6, 19);
            this.tb_Input.Multiline = true;
            this.tb_Input.Name = "tb_Input";
            this.tb_Input.Size = new System.Drawing.Size(469, 79);
            this.tb_Input.TabIndex = 0;
            this.tb_Input.TextChanged += new System.EventHandler(this.tb_Input_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_SID);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.tb_ID);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tb_DID);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cl_Options);
            this.groupBox1.Controls.Add(this.tb_NumberOfData);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cb_Mode);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cb_Padding);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(493, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(269, 187);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Setting";
            // 
            // cb_SID
            // 
            this.cb_SID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_SID.FormattingEnabled = true;
            this.cb_SID.Items.AddRange(new object[] {
            "0x10",
            "0x11",
            "0x14",
            "0x22",
            "0x28",
            "0x85",
            "0x3E",
            "0x2E",
            "0x19",
            "0x31",
            "0x27"});
            this.cb_SID.Location = new System.Drawing.Point(52, 106);
            this.cb_SID.Name = "cb_SID";
            this.cb_SID.Size = new System.Drawing.Size(66, 21);
            this.cb_SID.TabIndex = 12;
            this.cb_SID.SelectedIndexChanged += new System.EventHandler(this.cb_SID_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 162);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(18, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "ID";
            // 
            // tb_ID
            // 
            this.tb_ID.Location = new System.Drawing.Point(52, 159);
            this.tb_ID.Name = "tb_ID";
            this.tb_ID.Size = new System.Drawing.Size(66, 20);
            this.tb_ID.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "DID:";
            // 
            // tb_DID
            // 
            this.tb_DID.Location = new System.Drawing.Point(52, 133);
            this.tb_DID.Name = "tb_DID";
            this.tb_DID.Size = new System.Drawing.Size(66, 20);
            this.tb_DID.TabIndex = 8;
            this.tb_DID.TextChanged += new System.EventHandler(this.tb_DID_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "SID:";
            // 
            // cl_Options
            // 
            this.cl_Options.FormattingEnabled = true;
            this.cl_Options.Items.AddRange(new object[] {
            "SID",
            "DID",
            "Identifier"});
            this.cl_Options.Location = new System.Drawing.Point(124, 107);
            this.cl_Options.Name = "cl_Options";
            this.cl_Options.Size = new System.Drawing.Size(121, 49);
            this.cl_Options.TabIndex = 4;
            // 
            // tb_NumberOfData
            // 
            this.tb_NumberOfData.Location = new System.Drawing.Point(124, 78);
            this.tb_NumberOfData.Name = "tb_NumberOfData";
            this.tb_NumberOfData.Size = new System.Drawing.Size(121, 20);
            this.tb_NumberOfData.TabIndex = 5;
            this.tb_NumberOfData.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_NumberOfData_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "No of data byte:";
            // 
            // cb_Mode
            // 
            this.cb_Mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Mode.FormattingEnabled = true;
            this.cb_Mode.Items.AddRange(new object[] {
            "String to Hex",
            "Hex to String",
            "Add 0x",
            "Drop 0x"});
            this.cb_Mode.Location = new System.Drawing.Point(124, 21);
            this.cb_Mode.Name = "cb_Mode";
            this.cb_Mode.Size = new System.Drawing.Size(121, 21);
            this.cb_Mode.TabIndex = 3;
            this.cb_Mode.SelectedIndexChanged += new System.EventHandler(this.cb_Mode_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Mode:";
            // 
            // cb_Padding
            // 
            this.cb_Padding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Padding.FormattingEnabled = true;
            this.cb_Padding.Items.AddRange(new object[] {
            "0x00",
            "0x20",
            "0x30"});
            this.cb_Padding.Location = new System.Drawing.Point(124, 48);
            this.cb_Padding.Name = "cb_Padding";
            this.cb_Padding.Size = new System.Drawing.Size(121, 21);
            this.cb_Padding.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Padding:";
            // 
            // tabPage2
            // 
            this.tabPage2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(780, 285);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Miscellaneous";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btn_OpenFileDBC_Misc);
            this.groupBox5.Controls.Add(this.btn_OpenFileExcel_Misc);
            this.groupBox5.Location = new System.Drawing.Point(6, 131);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(388, 139);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Input Signal";
            // 
            // btn_OpenFileExcel_Misc
            // 
            this.btn_OpenFileExcel_Misc.Location = new System.Drawing.Point(22, 29);
            this.btn_OpenFileExcel_Misc.Name = "btn_OpenFileExcel_Misc";
            this.btn_OpenFileExcel_Misc.Size = new System.Drawing.Size(94, 26);
            this.btn_OpenFileExcel_Misc.TabIndex = 0;
            this.btn_OpenFileExcel_Misc.Text = "Open Excel File";
            this.btn_OpenFileExcel_Misc.UseVisualStyleBackColor = true;
            this.btn_OpenFileExcel_Misc.Click += new System.EventHandler(this.btn_OpenFile_Misc_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.btn_TestButton2);
            this.groupBox4.Controls.Add(this.btn_ArrangeMisc);
            this.groupBox4.Location = new System.Drawing.Point(6, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(388, 119);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Re-Arrange";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(267, 67);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "New function";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btn_TestButton2
            // 
            this.btn_TestButton2.Location = new System.Drawing.Point(22, 34);
            this.btn_TestButton2.Name = "btn_TestButton2";
            this.btn_TestButton2.Size = new System.Drawing.Size(75, 23);
            this.btn_TestButton2.TabIndex = 3;
            this.btn_TestButton2.Text = "Arrange";
            this.btn_TestButton2.UseVisualStyleBackColor = true;
            this.btn_TestButton2.Click += new System.EventHandler(this.btn_TestButton2_Click);
            // 
            // btn_ArrangeMisc
            // 
            this.btn_ArrangeMisc.Location = new System.Drawing.Point(183, 32);
            this.btn_ArrangeMisc.Name = "btn_ArrangeMisc";
            this.btn_ArrangeMisc.Size = new System.Drawing.Size(75, 25);
            this.btn_ArrangeMisc.TabIndex = 2;
            this.btn_ArrangeMisc.Text = "Obsolete";
            this.btn_ArrangeMisc.UseVisualStyleBackColor = false;
            this.btn_ArrangeMisc.Click += new System.EventHandler(this.btn_AddMisc_Click);
            // 
            // sts_Strip
            // 
            this.sts_Strip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ts_Status});
            this.sts_Strip.Location = new System.Drawing.Point(0, 317);
            this.sts_Strip.Name = "sts_Strip";
            this.sts_Strip.Size = new System.Drawing.Size(791, 22);
            this.sts_Strip.TabIndex = 4;
            this.sts_Strip.Text = "statusStrip1";
            // 
            // ts_Status
            // 
            this.ts_Status.Name = "ts_Status";
            this.ts_Status.Size = new System.Drawing.Size(0, 17);
            // 
            // ofd_OpenfileMisc
            // 
            this.ofd_OpenfileMisc.FileName = "openFileDialog1";
            // 
            // btn_OpenFileDBC_Misc
            // 
            this.btn_OpenFileDBC_Misc.Location = new System.Drawing.Point(22, 61);
            this.btn_OpenFileDBC_Misc.Name = "btn_OpenFileDBC_Misc";
            this.btn_OpenFileDBC_Misc.Size = new System.Drawing.Size(94, 23);
            this.btn_OpenFileDBC_Misc.TabIndex = 1;
            this.btn_OpenFileDBC_Misc.Text = "Open DBC File";
            this.btn_OpenFileDBC_Misc.UseVisualStyleBackColor = true;
            this.btn_OpenFileDBC_Misc.Click += new System.EventHandler(this.btn_OpenFileDBC_Misc_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 339);
            this.Controls.Add(this.sts_Strip);
            this.Controls.Add(this.tab_FunctionControl);
            this.Name = "Form1";
            this.Text = "Supportive Application";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tab_FunctionControl.ResumeLayout(false);
            this.tab_Convert.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.sts_Strip.ResumeLayout(false);
            this.sts_Strip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabControl tab_FunctionControl;
        private System.Windows.Forms.TabPage tab_Convert;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tb_Input;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cb_Padding;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckedListBox cl_Options;
        private System.Windows.Forms.TextBox tb_Output;
        private System.Windows.Forms.ComboBox cb_Mode;
        private System.Windows.Forms.TextBox tb_NumberOfData;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_Convert;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.StatusStrip sts_Strip;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_DID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_ID;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ToolStripStatusLabel ts_Status;
        private System.Windows.Forms.ComboBox cb_SID;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btn_ArrangeMisc;
        private System.Windows.Forms.Button btn_AddDrop;
        private System.Windows.Forms.Button btn_Compare;
        private System.Windows.Forms.OpenFileDialog ofd_OpenfileMisc;
        private System.Windows.Forms.Button btn_TestButton2;
        private System.Windows.Forms.Button btn_Clear;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btn_OpenFileExcel_Misc;
        private System.Windows.Forms.Button btn_OpenFileDBC_Misc;
    }
}

