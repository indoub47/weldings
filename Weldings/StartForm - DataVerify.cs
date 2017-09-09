
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
        private void doDataVerifyWork(BackgroundWorker worker, DoWorkEventArgs e)
        {
            int pc = 1; // progress count
            SheetsService service = null;
            string outputFileName = string.Empty;
            bool preWorkResult = preWork(ref service, ref outputFileName, worker, e, ref pc);
            if (!preWorkResult) return;

            Spreadsheets.Operator[] operators = SpreadsheetsData.Operators;
            Spreadsheets.SheetsRanges allRanges = SpreadsheetsData.AllRanges;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(StartFormMessages.Default.DataVerifyOutputHeaderFormat,
                DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
            sb.AppendLine();

            // do the main job and write processing info file

            if (worker.CancellationPending) e.Cancel = true;
            worker.ReportProgress(pc++, "Starting the data verify work:");

            try
            {
                // the main work
                ControllerVerify.DoControll(service, operators, allRanges, sb);
            }
            catch (Exception ex)
            {
                // all exceptions are caught and logged into sb
                // this is for some unexpected exception
                worker.ReportProgress(0, StartFormMessages.Default.UnexpectedExceptionHeader + ex.Message);
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
                        worker.ReportProgress(0, string.Format(
                                StartFormMessages.Default.UnableWriteVerifyInfoFile,
                                outputFileName));
                        LogWriter.Log(wEx);
                    }
                }
            }

            e.Result = string.Format(StartFormMessages.Default.VerifyDoneMessage, outputFileName);

        }
    }
}
