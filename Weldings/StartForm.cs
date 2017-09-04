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
        public StartForm()
        {
            InitializeComponent();
        }

        private void StartForm_Load(object sender, EventArgs e)
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
            chbVerbose.Checked = Settings.Default.ShowErrorMessages;
        }

        private void btnVerifyData_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Settings.Default.AccessDbPath))
            {
                string errorText = string.Format("DB file {0} not found!",
                    Settings.Default.AccessDbPath);
                MessageBox.Show(
                    errorText,
                    StartFormMessages.Default.OperationAbortedTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogWriter.Log(errorText);
                return;
            }

            try
            {
                checkOutputFolder();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(
                        StartFormMessages.Default.CannotCreateOutputFolder, Settings.Default.OutputPath),
                    StartFormMessages.Default.CreateFolderErrorTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogWriter.Log(ex);
                return;
            }

            if (!dirIsWritable(Settings.Default.OutputPath))
            {
                string errorText = string.Format(
                    "Output directory {0} is not writable",
                    Settings.Default.OutputPath);
                MessageBox.Show(
                    errorText,
                    StartFormMessages.Default.OperationAbortedTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogWriter.Log(errorText);
                return;
            }

            Spreadsheets.Operator[] operators = SpreadsheetsData.Operators;
            Spreadsheets.SheetsRanges allRanges = SpreadsheetsData.AllRanges;

            string outputFileName = createFileName(
                Settings.Default.DataVerifyFileNameFormat,
                "DataVerifyFileNameFormat", ".txt");
            if (outputFileName == string.Empty)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(StartFormMessages.Default.DataVerifyOutputHeaderFormat,
                DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
            sb.AppendLine();

            // create service
            SheetsService service = null;
            try
            {
                service = GSheetsConnector.Connect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Cannot create Sheets Service",
                    StartFormMessages.Default.OperationAbortedTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogWriter.Log(ex);
                return;
            }

            // do the main job and write processing info file
            try
            {
                // the main work
                ControllerVerify.DoControll(service, operators, allRanges, sb);
            }
            catch (Exception ex)
            {
                // all exceptions are caught and logged into sb
                // this is for some unexpected exception
                MessageBox.Show(
                    StartFormMessages.Default.UnexpectedExceptionHeader + ex.Message,
                    StartFormMessages.Default.DataVerifyErrorTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogWriter.Log(ex);
                return;
            }
            finally
            {
                service.Dispose();
                using (StreamWriter sw = new StreamWriter(outputFileName))
                {
                    try
                    {
                        sw.Write(sb);
                    }
                    catch (Exception wEx)
                    {
                        MessageBox.Show(
                            string.Format(
                                StartFormMessages.Default.UnableWriteVerifyInfoFile,
                                outputFileName),
                            StartFormMessages.Default.FileWriteErrorTitle,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LogWriter.Log(wEx);
                    }
                }
            }

            MessageBox.Show(
                StartFormMessages.Default.VerifyDoneMessage,
                StartFormMessages.Default.TaskAccomplishedTitle,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            Process.Start(outputFileName);

            btnWriteData.Enabled = true;
        }

        private void btnWriteData_Click(object sender, EventArgs e)
        {
            this.btnWriteData.Enabled = false;


            if (!File.Exists(Settings.Default.AccessDbPath))
            {
                string errorText = string.Format("DB file {0} not found!",
                    Settings.Default.AccessDbPath);
                MessageBox.Show(
                    errorText,
                    StartFormMessages.Default.OperationAbortedTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogWriter.Log(errorText);
                return;
            }

            try
            {
                checkOutputFolder();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(
                        StartFormMessages.Default.CannotCreateOutputFolder, Settings.Default.OutputPath),
                    StartFormMessages.Default.CreateFolderErrorTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogWriter.Log(ex);
                return;
            }

            if (!dirIsWritable(Settings.Default.OutputPath))
            {
                string errorText = string.Format(
                    "Output directory {0} is not writable",
                    Settings.Default.OutputPath);
                MessageBox.Show(
                    errorText,
                    StartFormMessages.Default.OperationAbortedTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogWriter.Log(errorText);
                return;
            }

            // create db backup
            if (Settings.Default.CreateDBBackup)
            {
                string dbBackupFileName = createFileName(
                    Settings.Default.DBBackupFilenameFormat,
                    "DBBackupFilenameFormat",
                    Path.GetExtension(Path.GetFileName(Settings.Default.AccessDbPath)));

                if (dbBackupFileName == string.Empty)
                {
                    return;
                }

                try
                {
                    File.Copy(Settings.Default.AccessDbPath, dbBackupFileName, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        string.Format(
                            StartFormMessages.Default.UnableCreateDBCopy,
                            Settings.Default.AccessDbPath,
                            dbBackupFileName) + ex.Message,
                        StartFormMessages.Default.FileCopyErrorTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogWriter.Log(ex);
                    return;
                }
            }

            Spreadsheets.Operator[] operators = SpreadsheetsData.Operators;
            Spreadsheets.SheetsRanges allRanges = SpreadsheetsData.AllRanges;

            string outputFileName = createFileName(
                Settings.Default.DbUpdateFileNameFormat,
                "DbUpdateFileNameFormat", ".txt");
            if (outputFileName == string.Empty)
            {
                return;
            }

            string reportFileName = createFileName(
                Settings.Default.UpdateReportFileNameFormat,
                "UpdateReportFileNameFormat", ".txt");
            if (reportFileName == string.Empty)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(
                StartFormMessages.Default.DataUpdateOutputHeaderFormat,
                DateTime.Now.ToShortDateString(),
                DateTime.Now.ToShortTimeString()).AppendLine();

            // create service
            SheetsService service = null;
            try
            {
                service = GSheetsConnector.Connect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.InnerException.Message,
                    ex.Message,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogWriter.Log(ex);
                return;
            }

            // do the main work and write processing info to file
            try
            {
                ControllerUpdate.DoControll(service, operators, allRanges, sb);
            }
            catch (DbUpdateException dbuEx)
            {
                MessageBox.Show(
                    StartFormMessages.Default.RollbackFailureError,
                    StartFormMessages.Default.OperationAbortedTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogWriter.Log(dbuEx);
            }
            catch (Exception ex)
            {
                string errorMessage = StartFormMessages.Default.UnexpectedExceptionHeader + ex.Message;
                MessageBox.Show(
                    errorMessage,
                    StartFormMessages.Default.UpdateErrorTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                sb.AppendLine(errorMessage);
                LogWriter.Log(ex);
            }
            finally
            {
                service.Dispose();
                using (StreamWriter sw = new StreamWriter(outputFileName))
                {
                    try
                    {
                        sw.Write(sb);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            string.Format(
                                StartFormMessages.Default.UnableWriteUpdateInfoFile,
                                outputFileName)
                                + ex.Message,
                            StartFormMessages.Default.FileWriteErrorTitle,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LogWriter.Log(ex);
                    }
                }
            }

            // create report
            StringBuilder sbReport = ReportCreator.CreateTxt(
                ControllerUpdate.GetInspections());
            using (StreamWriter sw = new StreamWriter(reportFileName))
            {
                try
                {
                    sw.Write(sbReport);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        string.Format(
                            StartFormMessages.Default.UnableWriteUpdateReportFile,
                            reportFileName)
                            + ex.Message,
                        StartFormMessages.Default.FileWriteErrorTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogWriter.Log(ex);
                    return;
                }
            }

            // open processing info file name
            Process.Start(outputFileName);
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

        private string createFileName(string format, string whatFile, string extension = "")
        {
            string fn = string.Empty;
            if (extension == "")
            {
                extension = StartFormMessages.Default.OutputFileExtension;
            }

            try
            {
                fn = Path.Combine(
                    Settings.Default.OutputPath,
                    string.Format(format, DateTime.Now, DateTime.Now) + extension
                    );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(
                        StartFormMessages.Default.BadFileNameFormatMessage,
                        whatFile, format) + ex.Message,
                    StartFormMessages.Default.BadStringFormatErrorTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                LogWriter.Log(ex);
                fn = string.Empty;
            }
            return fn;
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
            Settings.Default.ShowErrorMessages = chbVerbose.Checked;

            Settings.Default.Save();
            MessageBox.Show(
                StartFormMessages.Default.SettingsSavedMsg,
                StartFormMessages.Default.SettingsSavedBoxTitle,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void checkOutputFolder()
        {
            if (!Directory.Exists(Settings.Default.OutputPath))
            {
                Directory.CreateDirectory(Settings.Default.OutputPath);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        /// <summary>
        /// Checks the ability to create and write to a file in the supplied directory.
        /// </summary>
        /// <param name="directory">String representing the directory path to check.</param>
        /// <returns>True if successful; otherwise false.</returns>
        private static bool dirIsWritable(string directory)
        {
            const string TEMP_FILE = "tempFile.tmp";
            bool success = false;
            string fullPath = Path.Combine(directory, TEMP_FILE);

            if (Directory.Exists(directory))
            {
                try
                {
                    using (FileStream fs = new FileStream(fullPath, FileMode.Create,
                                                                    FileAccess.Write))
                    {
                        fs.WriteByte(0xff);
                    }

                    if (File.Exists(fullPath))
                    {                        
                        success = true;
                        try
                        {
                            File.Delete(fullPath);
                        }
                        catch
                        {
                            // neleido ištrinti tmp, tai ir chuj s nim
                        }
                    }                    
                }
                catch (Exception)
                {
                    success = false;
                }
            }

            return success;
        }
    }
}
