﻿namespace Weldings
{
    partial class StartForm
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
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabMore = new System.Windows.Forms.TabPage();
            this.chbBackupDB = new System.Windows.Forms.CheckBox();
            this.chbAbortFailedRollback = new System.Windows.Forms.CheckBox();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chbVerifyDate = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nudDaysBefore = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txDbUpdateReportFNFormat = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txDbUpdateInfoFNFormat = new System.Windows.Forms.TextBox();
            this.txDbBackupFNFormat = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txDataVerifyFNFormat = new System.Windows.Forms.TextBox();
            this.txJSONPath = new System.Windows.Forms.TextBox();
            this.txIFValue = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txbProgress = new System.Windows.Forms.TextBox();
            this.txbDbPath = new System.Windows.Forms.TextBox();
            this.txbOutputFolder = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSelectDb = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSelectOutputFolder = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDBUpdate = new System.Windows.Forms.Button();
            this.btnVerifyData = new System.Windows.Forms.Button();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.bgw = new System.ComponentModel.BackgroundWorker();
            this.tabMore.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDaysBefore)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // tabMore
            // 
            this.tabMore.Controls.Add(this.chbBackupDB);
            this.tabMore.Controls.Add(this.chbAbortFailedRollback);
            this.tabMore.Controls.Add(this.btnSaveSettings);
            this.tabMore.Controls.Add(this.groupBox2);
            this.tabMore.Controls.Add(this.groupBox1);
            this.tabMore.Controls.Add(this.txJSONPath);
            this.tabMore.Controls.Add(this.txIFValue);
            this.tabMore.Controls.Add(this.label10);
            this.tabMore.Controls.Add(this.label5);
            this.tabMore.Location = new System.Drawing.Point(4, 22);
            this.tabMore.Name = "tabMore";
            this.tabMore.Padding = new System.Windows.Forms.Padding(3);
            this.tabMore.Size = new System.Drawing.Size(452, 398);
            this.tabMore.TabIndex = 1;
            this.tabMore.Text = "More Options";
            this.tabMore.UseVisualStyleBackColor = true;
            // 
            // chbBackupDB
            // 
            this.chbBackupDB.AutoSize = true;
            this.chbBackupDB.Location = new System.Drawing.Point(12, 328);
            this.chbBackupDB.Name = "chbBackupDB";
            this.chbBackupDB.Size = new System.Drawing.Size(99, 17);
            this.chbBackupDB.TabIndex = 21;
            this.chbBackupDB.Text = "Backup the DB";
            this.chbBackupDB.UseVisualStyleBackColor = true;
            // 
            // chbAbortFailedRollback
            // 
            this.chbAbortFailedRollback.AutoSize = true;
            this.chbAbortFailedRollback.Location = new System.Drawing.Point(12, 305);
            this.chbAbortFailedRollback.Name = "chbAbortFailedRollback";
            this.chbAbortFailedRollback.Size = new System.Drawing.Size(189, 17);
            this.chbAbortFailedRollback.TabIndex = 20;
            this.chbAbortFailedRollback.Text = "Abort on failed transaction rollback";
            this.chbAbortFailedRollback.UseVisualStyleBackColor = true;
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(345, 365);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(97, 23);
            this.btnSaveSettings.TabIndex = 19;
            this.btnSaveSettings.Text = "Save settings";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chbVerifyDate);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.nudDaysBefore);
            this.groupBox2.Location = new System.Drawing.Point(6, 162);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(436, 66);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Inspection date verification";
            // 
            // chbVerifyDate
            // 
            this.chbVerifyDate.AutoSize = true;
            this.chbVerifyDate.Checked = true;
            this.chbVerifyDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbVerifyDate.Location = new System.Drawing.Point(6, 19);
            this.chbVerifyDate.Name = "chbVerifyDate";
            this.chbVerifyDate.Size = new System.Drawing.Size(188, 17);
            this.chbVerifyDate.TabIndex = 0;
            this.chbVerifyDate.Text = "Check if inspection date is  eligible";
            this.chbVerifyDate.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(247, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "days before today";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(186, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "The earliest inspection date permitted:";
            // 
            // nudDaysBefore
            // 
            this.nudDaysBefore.Location = new System.Drawing.Point(191, 38);
            this.nudDaysBefore.Maximum = new decimal(new int[] {
            366,
            0,
            0,
            0});
            this.nudDaysBefore.Name = "nudDaysBefore";
            this.nudDaysBefore.Size = new System.Drawing.Size(50, 20);
            this.nudDaysBefore.TabIndex = 2;
            this.nudDaysBefore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudDaysBefore.Value = new decimal(new int[] {
            14,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txDbUpdateReportFNFormat);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txDbUpdateInfoFNFormat);
            this.groupBox1.Controls.Add(this.txDbBackupFNFormat);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txDataVerifyFNFormat);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(436, 150);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "File name formats";
            // 
            // txDbUpdateReportFNFormat
            // 
            this.txDbUpdateReportFNFormat.Location = new System.Drawing.Point(145, 71);
            this.txDbUpdateReportFNFormat.Name = "txDbUpdateReportFNFormat";
            this.txDbUpdateReportFNFormat.Size = new System.Drawing.Size(285, 20);
            this.txDbUpdateReportFNFormat.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 74);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "DB update report";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(66, 126);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(302, 13);
            this.label11.TabIndex = 12;
            this.label11.Text = "yyyy-year, MM-month, dd-day, H-hour, mm-minutes, ss-seconds";
            // 
            // txDbUpdateInfoFNFormat
            // 
            this.txDbUpdateInfoFNFormat.Location = new System.Drawing.Point(145, 97);
            this.txDbUpdateInfoFNFormat.Name = "txDbUpdateInfoFNFormat";
            this.txDbUpdateInfoFNFormat.Size = new System.Drawing.Size(285, 20);
            this.txDbUpdateInfoFNFormat.TabIndex = 11;
            // 
            // txDbBackupFNFormat
            // 
            this.txDbBackupFNFormat.Location = new System.Drawing.Point(145, 45);
            this.txDbBackupFNFormat.Name = "txDbBackupFNFormat";
            this.txDbBackupFNFormat.Size = new System.Drawing.Size(285, 20);
            this.txDbBackupFNFormat.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 100);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "DB update report";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "DB backup";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Data verify result";
            // 
            // txDataVerifyFNFormat
            // 
            this.txDataVerifyFNFormat.Location = new System.Drawing.Point(145, 19);
            this.txDataVerifyFNFormat.Name = "txDataVerifyFNFormat";
            this.txDataVerifyFNFormat.Size = new System.Drawing.Size(285, 20);
            this.txDataVerifyFNFormat.TabIndex = 7;
            // 
            // txJSONPath
            // 
            this.txJSONPath.Location = new System.Drawing.Point(125, 269);
            this.txJSONPath.Name = "txJSONPath";
            this.txJSONPath.ReadOnly = true;
            this.txJSONPath.Size = new System.Drawing.Size(214, 20);
            this.txJSONPath.TabIndex = 15;
            // 
            // txIFValue
            // 
            this.txIFValue.Location = new System.Drawing.Point(125, 243);
            this.txIFValue.Name = "txIFValue";
            this.txIFValue.Size = new System.Drawing.Size(214, 20);
            this.txIFValue.TabIndex = 4;
            this.txIFValue.Text = "4";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(28, 272);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(91, 13);
            this.label10.TabIndex = 14;
            this.label10.Text = "JSON file location";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 246);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Value for the field \"IF\"";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txbProgress);
            this.tabPage1.Controls.Add(this.txbDbPath);
            this.tabPage1.Controls.Add(this.txbOutputFolder);
            this.tabPage1.Controls.Add(this.btnCancel);
            this.tabPage1.Controls.Add(this.btnSelectDb);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.btnSelectOutputFolder);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.btnDBUpdate);
            this.tabPage1.Controls.Add(this.btnVerifyData);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(452, 398);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main";
            // 
            // txbProgress
            // 
            this.txbProgress.Location = new System.Drawing.Point(8, 169);
            this.txbProgress.Multiline = true;
            this.txbProgress.Name = "txbProgress";
            this.txbProgress.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbProgress.Size = new System.Drawing.Size(435, 228);
            this.txbProgress.TabIndex = 17;
            // 
            // txbDbPath
            // 
            this.txbDbPath.Location = new System.Drawing.Point(8, 41);
            this.txbDbPath.Name = "txbDbPath";
            this.txbDbPath.ReadOnly = true;
            this.txbDbPath.Size = new System.Drawing.Size(379, 20);
            this.txbDbPath.TabIndex = 13;
            // 
            // txbOutputFolder
            // 
            this.txbOutputFolder.Location = new System.Drawing.Point(8, 93);
            this.txbOutputFolder.Name = "txbOutputFolder";
            this.txbOutputFolder.ReadOnly = true;
            this.txbOutputFolder.Size = new System.Drawing.Size(379, 20);
            this.txbOutputFolder.TabIndex = 10;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(355, 140);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSelectDb
            // 
            this.btnSelectDb.Location = new System.Drawing.Point(393, 39);
            this.btnSelectDb.Name = "btnSelectDb";
            this.btnSelectDb.Size = new System.Drawing.Size(51, 23);
            this.btnSelectDb.TabIndex = 15;
            this.btnSelectDb.Text = "Select";
            this.btnSelectDb.UseVisualStyleBackColor = true;
            this.btnSelectDb.Click += new System.EventHandler(this.btnSelectDb_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Database File";
            // 
            // btnSelectOutputFolder
            // 
            this.btnSelectOutputFolder.Location = new System.Drawing.Point(393, 91);
            this.btnSelectOutputFolder.Name = "btnSelectOutputFolder";
            this.btnSelectOutputFolder.Size = new System.Drawing.Size(51, 23);
            this.btnSelectOutputFolder.TabIndex = 12;
            this.btnSelectOutputFolder.Text = "Select";
            this.btnSelectOutputFolder.UseVisualStyleBackColor = true;
            this.btnSelectOutputFolder.Click += new System.EventHandler(this.btnSelectOutputFolder_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Output Folder";
            // 
            // btnWriteData
            // 
            this.btnDBUpdate.Enabled = false;
            this.btnDBUpdate.Location = new System.Drawing.Point(127, 140);
            this.btnDBUpdate.Name = "btnWriteData";
            this.btnDBUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnDBUpdate.TabIndex = 9;
            this.btnDBUpdate.Text = "Update DB";
            this.btnDBUpdate.UseVisualStyleBackColor = true;
            this.btnDBUpdate.Click += new System.EventHandler(this.btnDBUpdate_Click);
            // 
            // btnVerifyData
            // 
            this.btnVerifyData.Location = new System.Drawing.Point(34, 140);
            this.btnVerifyData.Name = "btnVerifyData";
            this.btnVerifyData.Size = new System.Drawing.Size(75, 23);
            this.btnVerifyData.TabIndex = 8;
            this.btnVerifyData.Text = "Verify";
            this.btnVerifyData.UseVisualStyleBackColor = true;
            this.btnVerifyData.Click += new System.EventHandler(this.btnVerifyData_Click);
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabPage1);
            this.tabMain.Controls.Add(this.tabMore);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(460, 424);
            this.tabMain.TabIndex = 8;
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 424);
            this.Controls.Add(this.tabMain);
            this.Name = "StartForm";
            this.Text = "Process Welding Data";
            this.Load += new System.EventHandler(this.StartForm_Load);
            this.tabMore.ResumeLayout(false);
            this.tabMore.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDaysBefore)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TabPage tabMore;
        private System.Windows.Forms.CheckBox chbBackupDB;
        private System.Windows.Forms.CheckBox chbAbortFailedRollback;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chbVerifyDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudDaysBefore;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txDbUpdateReportFNFormat;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txDbUpdateInfoFNFormat;
        private System.Windows.Forms.TextBox txDbBackupFNFormat;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txDataVerifyFNFormat;
        private System.Windows.Forms.TextBox txJSONPath;
        private System.Windows.Forms.TextBox txIFValue;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox txbDbPath;
        private System.Windows.Forms.TextBox txbOutputFolder;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSelectDb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSelectOutputFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDBUpdate;
        private System.Windows.Forms.Button btnVerifyData;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TextBox txbProgress;
        private System.ComponentModel.BackgroundWorker bgw;
    }
}

