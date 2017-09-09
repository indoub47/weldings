
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
        private bool preWork(ref SheetsService service, ref string outputFileName, BackgroundWorker worker, DoWorkEventArgs e, ref int pc)
        {
            if (worker.CancellationPending) e.Cancel = true;
            worker.ReportProgress(pc++, "Checking if DB file exists...");

            if (!File.Exists(Settings.Default.AccessDbPath))
            {
                string errorText = string.Format("DB file {0} not found!",
                    Settings.Default.AccessDbPath);
                worker.ReportProgress(0, errorText);
                LogWriter.Log(errorText);
                return false;
            }

            if (worker.CancellationPending) e.Cancel = true;
            worker.ReportProgress(pc++, "Checking if output folder exists (if not, trying to create one)...");

            try
            {
                checkOutputFolder();
            }
            catch (Exception ex)
            {
                worker.ReportProgress(0,
                    string.Format(
                        StartFormMessages.Default.CannotCreateOutputFolder,
                        Settings.Default.OutputPath));
                LogWriter.Log(ex);
                return false;
            }

            if (worker.CancellationPending) e.Cancel = true;
            worker.ReportProgress(pc++, "Checking if output folder is writable...");

            if (!dirIsWritable(Settings.Default.OutputPath))
            {
                string errorText = string.Format(
                    "Output directory {0} is not writable",
                    Settings.Default.OutputPath);
                worker.ReportProgress(0, errorText);
                LogWriter.Log(errorText);
                return false;
            }

            if (worker.CancellationPending) e.Cancel = true;
            worker.ReportProgress(pc++, "Creating a file in the output folder for the data verify progress logging...");

            outputFileName = createFileName(
                Settings.Default.DataVerifyFileNameFormat,
                "DataVerifyFileNameFormat", ".txt");
            if (outputFileName == string.Empty)
            {
                return false;
            }

            if (worker.CancellationPending) e.Cancel = true;
            worker.ReportProgress(pc++, "Attempting to authorize online and create the GoogleSheets service...");

            // create service
            try
            {
                service = GSheetsConnector.Connect();
            }
            catch (Exception ex)
            {
                worker.ReportProgress(0, "Cannot create Sheets Service");
                LogWriter.Log(ex);
                return false;
            }

            return true;
        }
    }
}
