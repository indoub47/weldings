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
            txDbUpdateFNFormat.Text = Settings.Default.DbUpdateResultFileNameFormat;

            chbVerifyDate.Checked = Settings.Default.CheckDateIfReal;
            nudDaysBefore.Value = Convert.ToDecimal(Settings.Default.AllowedDayCount);

            txIFValue.Text = Settings.Default.Ifas;
            txJSONPath.Text = Settings.Default.SpreadsheetsJsonPath;

            txbDbPath.Text = Settings.Default.AccessDbPath;
            txbOutputFolder.Text = Settings.Default.OutputPath;
        }

        private void btnVerifyData_Click(object sender, EventArgs e)
        {
            try
            {
                checkOutputFolder();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot create the output folder \"" + Settings.Default.OutputPath + "\". Try to create it manually.\n\n" + ex.Message ,
                    "StartForm.btnVerifyDataClick Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SheetsService service = null;

            try
            {
                service = GSheetsConnector.Connect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("GSheetsConnector.Connect() failed: " + ex.Message, "StartForm.btnVerifyDataClick Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Spreadsheets.Operator[] operators = SpreadsheetsData.Operators;
            Spreadsheets.SheetsRanges allRanges = SpreadsheetsData.AllRanges;

            string outputFileName =  getOutputFileName(Settings.Default.DataVerifyFileNameFormat, "data verify output");
            if (outputFileName == string.Empty)
            {
                service.Dispose();
                return;
            }
            
            using (StreamWriter sw = new StreamWriter(outputFileName))
            {
                try
                {
                    StringBuilder sb = Controller1.FetchConvertVerify(service, operators, allRanges);
                    sw.Write(sb);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "StartForm.btnVerifyDataClick Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    service.Dispose();
                }
            }
            MessageBox.Show("Atlikta.\nDabar bus atidarytas failas su išvardintomis problemomis.", 
                "Task accomplished", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Process.Start(outputFileName);
            btnWriteData.Enabled = true;
        }

        private void btnWriteData_Click(object sender, EventArgs e)
        {
            this.btnWriteData.Enabled = false;

            // check if output folder exists; if not then create it
            try
            {
                checkOutputFolder();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot create the output folder \"" + Settings.Default.OutputPath + "\". Try to create it manually.\n\n" + ex.Message, 
                    "StartForm.btnWriteDataClick Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // create db backup
            string dbBackupFileName = Path.Combine(Settings.Default.OutputPath, string.Format(Settings.Default.DBBackupFilenameFormat, DateTime.Now));
            try
            {
                File.Copy(Settings.Default.AccessDbPath, dbBackupFileName, true);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Unable to create database file copy: \n\tfrom: \"" + Settings.Default.AccessDbPath + "\"\n\tto: \"" + dbBackupFileName + "\"\n\n" + ex.Message,
                    "StartForm.btnWriteDataClick Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // perform update
            /* internal static void fetchConvertUpdate(SheetsService service, Spreadsheets.Operator[] operators, Spreadsheets.SheetsRanges allRanges) */
            SheetsService service = null;
            try
            {
                service = GSheetsConnector.Connect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("GSheetsConnector.Connect() failed: " + ex.Message, "StartForm.btnWriteDataClick Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Spreadsheets.Operator[] operators = SpreadsheetsData.Operators;
            Spreadsheets.SheetsRanges allRanges = SpreadsheetsData.AllRanges;

            Controller2.fetchConvertUpdate(service, operators, allRanges);
            service.Dispose();

            // write all processing to file
            string processingInfoFileName = getOutputFileName(Settings.Default.ProcessingInfoFileNameFormat, "processing info file");
            if (processingInfoFileName == string.Empty) return;
            using (StreamWriter sw = new StreamWriter(processingInfoFileName))
            {
                try
                {
                    sw.Write(Controller2.GetProcessingResultInfo());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Nepavyko įrašyti processing info failo į \"" + processingInfoFileName + "\"\n" + ex.Message,
                        "StartForm.btnWriteDataClick Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                    
                }
            }

            // create report
            StringBuilder sbReport = ReportCreator.CreateTxt(Controller2.GetAllProcessedInspections());
            string reportFileName = getOutputFileName(Settings.Default.DbUpdateResultFileNameFormat, "db update result");
            if (reportFileName == string.Empty) return;
            using (StreamWriter sw = new StreamWriter(reportFileName))
            {
                try
                {
                    sw.Write(sbReport);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Nepavyko įrašyti db update result į \"" + reportFileName + "\"\n" + ex.Message,
                        "StartForm.btnWriteDataClick Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // open processing info file name
            Process.Start(processingInfoFileName);
        }

        private void btnSelectDb_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "MDB files (*.mdb)|*.mdb|ACCDB files (*.accdb)|*.accdb";
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Select database file";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.AccessDbPath = openFileDialog.FileName;
                Settings.Default.Save();
                txbDbPath.Text = Settings.Default.AccessDbPath;
            }
        }

        private void btnSelectOutputFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.Description = "Select output folder";
            folderBrowserDialog.ShowNewFolderButton = true;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.OutputPath = folderBrowserDialog.SelectedPath;
                Settings.Default.Save();
                txbOutputFolder.Text = Settings.Default.OutputPath;
            }
        }

        private string getOutputFileName(string format, string whatFile)
        {
            string fn = string.Empty;
            try
            {
                fn = Path.Combine(Settings.Default.OutputPath,
                string.Format(format, DateTime.Now) + ".txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bad " + whatFile + " file name format " + Settings.Default.DataVerifyFileNameFormat +
                    "\n" + ex.Message,
                    "Bad string format",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Asterisk);
            }
            return fn;
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            Settings.Default.DataVerifyFileNameFormat = txDataVerifyFNFormat.Text.Trim();
            Settings.Default.DBBackupFilenameFormat = txDbBackupFNFormat.Text.Trim();
            Settings.Default.DbUpdateResultFileNameFormat = txDbUpdateFNFormat.Text.Trim();

            Settings.Default.CheckDateIfReal = chbVerifyDate.Checked;
            Settings.Default.AllowedDayCount = Convert.ToInt32(nudDaysBefore.Value);

            Settings.Default.Ifas = txIFValue.Text.Trim();
            Settings.Default.Save();
            MessageBox.Show("Settings saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
