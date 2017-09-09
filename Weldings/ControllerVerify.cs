using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Sheets.v4;
using System.Windows.Forms;
using Weldings.Properties;
using System.ComponentModel;

namespace Weldings
{
    public static class ControllerVerify
    {
        private delegate List<WeldingInspection> ConvertDataToWIMethod(List<IList<Object>> data, string[] mapping, string operId);
        private delegate List<BadData> VerifyObjectsMethod(List<WeldingInspection> inspectionList, string operId); 

        public static EventHandler<ProgressChangedEventArgs> ProgressUpdated;

        public static void OnProgressUpdated(int progressCount, string progressInfo)
        {
            ProgressUpdated?.Invoke(
                "Weldings.ControllerVerify", 
                new ProgressChangedEventArgs(progressCount, progressInfo));
        }

        // kaupiami visi patikrinimai tam, kad būtų galima pagauti, jeigu tą patį suvirinimą tikrina du kartus
        private static IEnumerable<WeldingInspection> allInspections;

        /// <summary>
        /// 1. Fetches Google Sheets data
        /// 2. Converts Google Sheets data to WeldingInspection objects
        /// 3. Checks if the WeldingInspection objects are valid for database
        /// </summary>
        /// <param name="service">Google Sheets API v4 service to connect to Google Sheets</param>
        /// <param name="operators">All operators data from spreadsheets.json config file</param>
        /// <param name="allRanges">All range data from spreadsheets.json config file</param>
        /// <param name="sb">StringBuilder workflow output</param>
        public static void DoControll(
            SheetsService service, 
            Spreadsheets.Operator[] operators, 
            Spreadsheets.SheetsRanges allRanges,
            StringBuilder sb)
        {
            allInspections = new List<WeldingInspection>();

            foreach (Spreadsheets.Operator oper in operators)
            {
                sb.AppendLine();

                OnProgressUpdated(0,
                    $"verifying data: operator - {oper.OperatorId}, sheet - \"{allRanges.Pirmieji.SheetName}\"...");

                DoControllSingleSheet(
                    service,
                    oper,
                    allRanges.Pirmieji,
                    DataConverter.ConvertPirmieji,
                    ObjectVerifier.VerifyPirmieji,
                    sb);

                OnProgressUpdated(0,
                    $"verifying data: operator - {oper.OperatorId}, sheet - \"{allRanges.Nepirmieji.SheetName}\"...");

                DoControllSingleSheet(
                    service,
                    oper,
                    allRanges.Nepirmieji,
                    DataConverter.ConvertNepirmieji,
                    ObjectVerifier.VerifyNepirmieji,
                    sb);
            }

            StringBuilder repSb = RepeatFinder.FindRepeats(allInspections.ToList());
            sb.AppendLine(Messages.Default.RepeatedRecordsHeader);
            sb.Append(repSb);
        }


        /// <summary>
        /// Single Google Sheets sheet:
        /// 1. fetches data from the sheet
        /// 2. converts it to WeldingInspection objects
        /// 3. verifies if the objects are valid for updating DB
        /// </summary>
        /// <param name="service">Google Sheets service</param>
        /// <param name="operatorData">which operator's spreadsheet</param>
        /// <param name="rangeData">which sheet of the spreadsheet</param>
        /// <param name="convertMethod">data converting method, specific to this sheet type</param>
        /// <param name="verifyMethod">data verifying method specific to this sheet type</param>
        /// <param name="sb">reference to the string builder to output workflow information</param>
        /// <returns>success or failure</returns>
        private static bool DoControllSingleSheet(
            SheetsService service,
            Spreadsheets.Operator operatorData, // lemia, katro operatoriaus spreadsheet
            Spreadsheets.SheetsRanges.SheetRangeData rangeData, // lemia, katrą to operatoriaus lentelę 
            ConvertDataToWIMethod convertMethod, // tam lentelės tipui specifinis convert metodas 
            VerifyObjectsMethod verifyMethod, // tam lentelės tipui specifinis verify metodas
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
                LogWriter.Log(ex);
                OnProgressUpdated(0, errorText);
                return false;
            }

            // convert row data to WeldingInspection objects
            List<WeldingInspection> wiList;
            try
            {
                wiList = convertMethod(dataRows, rangeData.FieldMappings, operatorData.OperatorId);
            }
            catch (BadDataException bdEx)
            {
                string errorText = operSheet + Messages.Default.BadGSData;
                List<BadData> bdl = bdEx.BadDataList.ToList();
                string bdListString = BadDataReportCreator.CreateString(bdl).ToString();
                sb.AppendLine(errorText);
                sb.AppendLine(bdListString);
                OnProgressUpdated(0, errorText);
                return false;
            }
            catch (Exception ex)
            {
                string errorText = operSheet + Messages.Default.GSDataConvertFail;
                sb.AppendLine(errorText);
                sb.AppendLine(Messages.Default.FailureInfo + ex.Message);
                LogWriter.Log(ex);
                OnProgressUpdated(0, errorText);
                return false;
            }
            allInspections = allInspections.Concat(wiList);

            // verify WeldingInspection objects
            List<BadData> bdList;
            
            bdList = verifyMethod(wiList, operatorData.OperatorId);
            if (bdList.Count > 0)
            {
                string strBadDataList = BadDataReportCreator.CreateString(bdList.ToList()).ToString();
                sb.AppendLine(operSheet + Messages.Default.GSDataVerifyResult).AppendLine(strBadDataList);
            }
            else
            {
                sb.AppendLine(operSheet + Messages.Default.GSDataVerifyOK);
            }

            return true;
        }
    }
}