using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;


namespace Weldings
{
    internal static class Controller2
    {
        // Fetch data from Google Sheets
        // Try to convert it to WeldingInspections
        // Update database
        // Update Google Sheets

        private delegate List<WeldingInspection> ConvertDataToListMethod(List<IList<Object>> data, string[] mapping, string operId);
        private delegate StringBuilder VerifyObjectsMethod(List<WeldingInspection> inspectionList);
        private delegate int DbUpdateMethod(List<WeldingInspection> inspectionList);

        // kaupiami visi patikrinimai reportui
        private static IEnumerable<WeldingInspection> allInspections = new List<WeldingInspection>();
        private static StringBuilder results = new StringBuilder(
            string.Format("{0}, {1}", 
                DateTime.Now.ToShortDateString(), 
                DateTime.Now.ToShortTimeString()))
            .AppendLine();

        internal static List<WeldingInspection> GetAllProcessedInspections()
        {
            return allInspections.ToList();
        }

        internal static StringBuilder GetProcessingResultInfo()
        {
            return results;
        }

        // vieno operatoriaus vienos GS lentelės duomenis - fetchina, paverčia WeldingInspection ir updateinaDB bei GS
        private static void fetchConvertUpdateSheet(SheetsService service,
            Spreadsheets.Operator operatorData, // lemia, katro operatoriaus spreadsheet
            Spreadsheets.SheetsRanges.SheetRangeData rangeData, // lemia, katrą to operatoriaus lentelę 
            ConvertDataToListMethod convertMethod, // --
            DbUpdateMethod dbUpdateMethod) // --
        {
            try
            {
                List<IList<Object>> data = SheetDataFetcher.Fetch(
                    operatorData.SpreadsheetId, 
                    rangeData.RangeAddress, 
                    rangeData.FilterColumn.Index, 
                    service).ToList();
                List<WeldingInspection> inspectionList = convertMethod(data, rangeData.FieldMappings, operatorData.OperatorId);
                dbUpdateMethod(inspectionList);
                results.AppendFormat("{0} {1} db update OK", operatorData.OperatorId, rangeData.SheetName).AppendLine();
                allInspections = allInspections.Concat(inspectionList);
            }
            catch (Exception ex)
            {
                results.AppendFormat("{0} {1} db update FAIL.", operatorData.OperatorId, rangeData.SheetName).AppendLine();
                Exception e;
                if (ex.InnerException != null)
                    e = ex.InnerException;
                else
                    e = ex;
                results.AppendFormat("Exception message: {0}", e.Message).AppendLine();
                results.AppendLine("Google Sheets lentelėje duomenys nepažymimi kaip įvesti į DB ir šitie duomenys nebus įtraukti į ataskaitą.");
                results.AppendLine();
                return;
            }

            // updateinama Google Sheets lentelė.
            // jeigu GS updateinimas nutrūktų, informuoja, kad tokio operatoriaus tokie patikrinimai į dB sukišti, bet lentelė neatnaujinta.
            try
            {
                BatchUpdateValuesResponse response = GSheetsUpdater.BatchUpdateSheet(operatorData.SpreadsheetId, rangeData, service);
                results.AppendFormat("{0} {1} Google Sheets update OK", operatorData.OperatorId, rangeData.SheetName).AppendLine();
                results.AppendLine();
            }
            catch (Exception ex)
            {
                results.AppendFormat("{0} {1} Google Sheets update FAIL.", operatorData.OperatorId, rangeData.SheetName).AppendLine();
                results.AppendFormat("Failure info: {0}", ex.Message).AppendLine();
                results.Append("DĖMESIO! Kadangi duomenys į duomenų bazę sukelti, tie duomenys bus įtraukti į ataskaitą, kaip sutvarkyti. ");
                results.AppendLine("Google Sheets lentelę reikėtų sutvarkyti rankiniu būdu - prisijungti prie Google Sheets, atsidaryti lentelę ir joje pažymėti, kurie duomenys yra sukelti į DB.");
                results.AppendLine();
            }
        }

        private static void fetchConvertUpdateSpreadsheet(SheetsService service, Spreadsheets.Operator operatorData, Spreadsheets.SheetsRanges allRanges)
        {
            fetchConvertUpdateSheet(service, operatorData, allRanges.Pirmieji, DataConverter.ConvertPirmieji, DBUpdater.DoPirmieji);
            fetchConvertUpdateSheet(service, operatorData, allRanges.Nepirmieji, DataConverter.ConvertNepirmieji, DBUpdater.DoNepirmieji);
        }

        internal static void fetchConvertUpdate(SheetsService service, Spreadsheets.Operator[] operators, Spreadsheets.SheetsRanges allRanges)
        {
            foreach (Spreadsheets.Operator operatorData in operators)
            {
                fetchConvertUpdateSpreadsheet(service, operatorData, allRanges);
            }
            allInspections.ToList();
        }

    }
}
