using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using System.Threading;
using Weldings.Properties;

namespace Weldings
{
    public partial class StartForm : Form
    {
        private void loadSettings()
        {
            txDataVerifyFNFormat.Text = Settings.Default.DataVerifyFileNameFormat;
            txDbBackupFNFormat.Text = Settings.Default.DBBackupFilenameFormat;
            txDbUpdateInfoFNFormat.Text = Settings.Default.DbUpdateFileNameFormat;
            txDbUpdateReportFNFormat.Text = Settings.Default.UpdateReportFileNameFormat;

            chbVerifyDate.Checked = Settings.Default.CheckDateIfReal;
            nudDaysBefore.Value = Convert.ToDecimal(Settings.Default.AllowedDayCount);

            txIFValue.Text = Settings.Default.Ifas;
            txJSONPath.Text = Settings.Default.SpreadsheetsJsonPath;

            txbDbPath.Text = Settings.Default.AccessDbPath;
            txbOutputFolder.Text = Settings.Default.OutputPath;

            chbAbortFailedRollback.Checked = Settings.Default.AbortOnFailedRollback;
            chbBackupDB.Checked = Settings.Default.CreateDBBackup;
        }

        private void btnSelectDb_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "MDB files (*.mdb)|*.mdb|ACCDB files (*.accdb)|*.accdb";
            openFileDialog.Multiselect = false;
            openFileDialog.Title = StartFormMessages.Default.OpenFileDialogTitle;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.AccessDbPath = openFileDialog.FileName;
                Settings.Default.Save();
                txbDbPath.Text = Settings.Default.AccessDbPath;
            }
        }

        private void btnSelectOutputFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.Description = StartFormMessages.Default.SelectFolderDialogTitle;
            folderBrowserDialog.ShowNewFolderButton = true;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.OutputPath = folderBrowserDialog.SelectedPath;
                Settings.Default.Save();
                txbOutputFolder.Text = Settings.Default.OutputPath;
            }
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            Settings.Default.DataVerifyFileNameFormat = txDataVerifyFNFormat.Text.Trim();
            Settings.Default.DBBackupFilenameFormat = txDbBackupFNFormat.Text.Trim();
            Settings.Default.DbUpdateFileNameFormat = txDbUpdateInfoFNFormat.Text.Trim();
            Settings.Default.UpdateReportFileNameFormat = txDbUpdateReportFNFormat.Text.Trim();

            Settings.Default.CheckDateIfReal = chbVerifyDate.Checked;
            Settings.Default.AllowedDayCount = Convert.ToInt32(nudDaysBefore.Value);

            Settings.Default.Ifas = txIFValue.Text.Trim();

            Settings.Default.AbortOnFailedRollback = chbAbortFailedRollback.Checked;
            Settings.Default.CreateDBBackup = chbBackupDB.Checked;

            Settings.Default.Save();
            MessageBox.Show(
                StartFormMessages.Default.SettingsSavedMsg,
                StartFormMessages.Default.SettingsSavedBoxTitle,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
