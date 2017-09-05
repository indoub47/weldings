using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Sheets.v4;
using System.Windows.Forms;
using Weldings.Properties;

namespace Weldings
{
    public static class ControllerUpdate
    {
        private delegate List<WeldingInspection> ConvertDataToWIMethod(List<IList<Object>> data, string[] mapping, string operId);
        private delegate int DbUpdateMethod(List<WeldingInspection> inspectionList);

        // kaupiami visi patikrinimai reportui į žurnalą
        private static IEnumerable<WeldingInspection> allInspections;

        public static List<WeldingInspection> GetInspections()
        {
            return allInspections.ToList();
        }

        /// <summary>
        /// 1. Fetches Google Sheets data
        /// 2. Converts Google Sheets data to WeldingInspection objects
        /// 3. Checks if the WeldingInspection objects are valid for database
        /// </summary>
        /// <param name="service">Google Sheets API v4 service to connect to Google Sheets</param>
        /// <param name="operators">All operators data from spreadsheets.json config file</param>
        /// <param name="allRanges">All range data from spreadsheets.json config file</param>
        /// <param name="sb">StringBuilder workflow output</param>
        /// <exception cref="Weldings.DbUpdateException">is thrownon on failed db update and failed rollback if Settings.Default.AbortOnFailedRollback is set to true</exception>
        public static void DoControll(
            SheetsService service, 
            Spreadsheets.Operator[] operators, 
            Spreadsheets.SheetsRanges allRanges,
            StringBuilder sb)
        {
            allInspections = new List<WeldingInspection>();

            foreach (Spreadsheets.Operator oper in operators)
            {
                DoControllSingleSheet(
                    service,
                    oper,
                    allRanges.Pirmieji,
                    DataConverter.ConvertPirmieji,
                    DBUpdater.DoPirmieji,
                    sb);

                DoControllSingleSheet(
                    service,
                    oper,
                    allRanges.Nepirmieji,
                    DataConverter.ConvertNepirmieji,
                    DBUpdater.DoNepirmieji,
                    sb);
            }            
        }


        /// <summary>
        /// Single Google Sheets sheet:
        /// 1. fetches data from the sheet
        /// 2. converts it to WeldingInspection objects
        /// 3. updates database
        /// 4. in GoogleSheets marks those rows which have been put into database
        /// </summary>
        /// <param name="service">Google Sheets service</param>
        /// <param name="operatorData">which operator's spreadsheet</param>
        /// <param name="rangeData">which sheet of the spreadsheet</param>
        /// <param name="convertMethod">data converting method, specific to this sheet type</param>
        /// <param name="dbUpdateMethod">db updating method specific to this sheet type</param>
        /// <param name="sb">reference to the string builder to output workflow information</param>
        /// <returns>success or failure</returns>
        /// <exception cref="Weldings.DbUpdateException">is thrownon on failed db update and failed rollback if Settings.Default.AbortOnFailedRollback is set to true</exception>
        private static bool DoControllSingleSheet(
            SheetsService service,
            Spreadsheets.Operator operatorData, // lemia, katro operatoriaus spreadsheet
            Spreadsheets.SheetsRanges.SheetRangeData rangeData, // lemia, katrą to operatoriaus lentelę 
            ConvertDataToWIMethod convertMethod, // tam lentelės tipui specifinis convert metodas 
            DbUpdateMethod dbUpdateMethod, // tam lentelės tipui specifinis update metodas
            StringBuilder sb)
        {
            string operSheet = string.Format(Messages.Default.OperSheetFrmt,
                    operatorData.OperatorId, rangeData.SheetName);
            
            // fetch data from GoogleSheets
            List<IList<Object>> dataRows;
            try
            {
                dataRows = SheetDataFetcher.Fetch(
                    operatorData.SpreadsheetId,
                    rangeData.RangeAddress,
                    rangeData.FilterColumn.Index,
                    service).ToList();
            }
            catch (Exception ex)
            {
                string errorText = Messages.Default.GSDataFetchFail;
                sb.AppendLine(errorText);
                sb.AppendLine(Messages.Default.FailureInfo + ex.Message);
                ErrorMessage(errorText);
                LogWriter.Log(ex);
                return false;
            }

            // convert row data to WeldingInspection objects
            List<WeldingInspection> wiList;
            try
            {
                wiList = convertMethod(dataRows, rangeData.FieldMappings, operatorData.OperatorId);
                // sb.AppendLine(operSheet + Messages.Default.GSDataConvertOK);
            }
            catch (BadDataException bdEx)
            {
                string errorText = operSheet + Messages.Default.BadGSData;
                string bdListString = BadDataReportCreator.CreateString(bdEx.BadDataList.ToList()).ToString();
                sb.AppendLine(errorText);
                sb.AppendLine(bdListString);
                ErrorMessage(errorText);
                return false;
            }
            catch (Exception ex)
            {
                string errorText = operSheet + Messages.Default.GSDataConvertFail;
                sb.AppendLine(errorText);
                sb.AppendLine(Messages.Default.FailureInfo + ex.Message);
                ErrorMessage(errorText);
                LogWriter.Log(ex);
                return false;
            }

            // update database
            try
            {
                dbUpdateMethod(wiList);
                sb.AppendLine(operSheet + Messages.Default.DBUpdateOK);
            }
            catch (DbUpdateException dbuEx) when (dbuEx.RollbackSuccess)
            {
                string errorText = operSheet + dbuEx.Message;
                sb.AppendLine(errorText);
                sb.AppendLine(Messages.Default.FailureInfo + dbuEx.InnerException.Message);
                ErrorMessage(errorText);
                LogWriter.Log(dbuEx);
                return false;
            }
            catch (DbUpdateException dbuEx) when (!dbuEx.RollbackSuccess)
            {
                string errorText = operSheet + dbuEx.Message;
                sb.AppendLine(errorText);
                sb.AppendLine(Messages.Default.FailureInfo + dbuEx.InnerException.Message);
                ErrorMessage(errorText);
                if (Settings.Default.AbortOnFailedRollback)
                {
                    throw dbuEx;
                }
                return false;
            }

            allInspections = allInspections.Concat(wiList);

            // update Google Sheets
            try
            {
                GSheetsUpdater.BatchUpdateSheet(operatorData.SpreadsheetId, rangeData, service);
                sb.AppendLine(operSheet + Messages.Default.GSUpdateOK);
            }
            catch (Exception ex)
            {
                string errorText = operSheet + Messages.Default.GSUpdateFail;
                sb.AppendLine(errorText);
                sb.AppendLine(Messages.Default.FailureInfo + ex.Message);
                sb.AppendLine(Messages.Default.GSUpdateFailInstruction);
                ErrorMessage(errorText);
                LogWriter.Log(ex);
                return true; // pagrindinis darbas - db updateinimas - pavyko, o sheet updateinimas yra tik detalės
            }

            return true;
        }

        private static void ErrorMessage(string message)
        {
            if (Settings.Default.ShowErrorMessages)
            {
                MessageBox.Show(message,
                  Messages.Default.UpdateOperationErrorMsgTitle,
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Error);
            }
        }


    }
}