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

            // check if output folder exists
            // check if dbfile exists
        }

        private void btnVerifyData_Click(object sender, EventArgs e)
        {
            SheetsService service = GSheetsConnector.Connect();
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
                    sw.Write(Controller1.FetchConvertVerify(service, operators, allRanges));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    service.Dispose();
                }
            }
            MessageBox.Show("Done.\nDabar bus atidarytas failas su išvardintomis problemomis.");
            Process.Start(outputFileName);           
        }

        private void btnWriteData_Click(object sender, EventArgs e)
        {
            this.btnWriteData.Enabled = false;
            // create db backup
            // perform update
            // create report
            // open report
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
                string.Format(format, DateTime.Now));
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
    }
}
