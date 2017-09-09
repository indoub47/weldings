
using Google.Apis.Sheets.v4;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Weldings.Properties;

namespace Weldings
{
    public partial class StartForm : Form
    {
        private void doDBUpdateWork(BackgroundWorker worker, DoWorkEventArgs e)
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

            if (worker.CancellationPending) e.Cancel = true;
            worker.ReportProgress(pc++, "Creating DB update report file...");

            string reportFileName = createFileName(
                Settings.Default.UpdateReportFileNameFormat,
                "UpdateReportFileNameFormat", ".txt");
            if (reportFileName == string.Empty)
            {
                return;
            }

            if (worker.CancellationPending) e.Cancel = true;
            worker.ReportProgress(pc++, "Creating DB file backup...");


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

                    worker.ReportProgress(0, string.Format(
                            StartFormMessages.Default.UnableCreateDBCopy,
                            Settings.Default.AccessDbPath,
                            dbBackupFileName));
                    LogWriter.Log(ex);
                    return;
                }
            }

            // do the main job and write processing info file

            if (worker.CancellationPending) e.Cancel = true;
            worker.ReportProgress(pc++, "Starting the data update work:");

            try
            {
                ControllerUpdate.DoControll(service, operators, allRanges, sb);
            }
            catch (DbUpdateException dbuEx)
            {
                worker.ReportProgress(0, 
                    StartFormMessages.Default.RollbackFailureError);
                LogWriter.Log(dbuEx);
            }
            catch (Exception ex)
            {
                string errorMessage = StartFormMessages.Default.UnexpectedExceptionHeader + ex.Message;
                worker.ReportProgress(0, errorMessage);
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
                        worker.ReportProgress(0,
                            string.Format(
                                StartFormMessages.Default.UnableWriteUpdateInfoFile,
                                outputFileName)
                                + ex.Message);
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
                    worker.ReportProgress(0,
                            string.Format(
                            StartFormMessages.Default.UnableWriteUpdateReportFile,
                            reportFileName)
                            + ex.Message);
                    LogWriter.Log(ex);
                    return;
                }
            }

            e.Result = string.Format(StartFormMessages.Default.UpdateDoneMessage, outputFileName);
        }
    }
}
