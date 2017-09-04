using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;


namespace Weldings
{
    public static class Controller1
    {
        // Fetch data from Google Sheets
        // Try to convert it to WeldingInspections
        // Verify
        // Inform user about problems

        private delegate List<WeldingInspection> ConvertDataToListMethod(List<IList<Object>> data, string[] mapping, string operId);
        private delegate List<BadData> VerifyObjectsMethod(List<WeldingInspection> inspectionList, string operId);
        private delegate int DbUpdateMethod(List<WeldingInspection> inspectionList);

        // kaupiami visi patikrinimai tam, kad b8t7 galima pagauti, jeigu tą patį suvirinimą tikrina du kartus
        private static List<WeldingInspection> allInspections = new List<WeldingInspection>();

        // vieno operatoriaus vienos GS lentelės duomenis - fetchina, paverčia WeldingInspection ir patikrina
        private static StringBuilder fetchConvertVerifySheet(SheetsService service, 
            Spreadsheets.Operator operatorData, // lemia, katro operatoriaus spreadsheet
            Spreadsheets.SheetsRanges.SheetRangeData rangeData, // lemia, katrą to operatoriaus lentelę 
            ConvertDataToListMethod convertMethod, // -- 
            VerifyObjectsMethod verifyMethod) // --
        {
            List<IList<Object>> data = SheetDataFetcher.Fetch(
                operatorData.SpreadsheetId, 
                rangeData.RangeAddress, 
                rangeData.FilterColumn.Index, 
                service).ToList();
            List<WeldingInspection> inspectionList = convertMethod(data, rangeData.FieldMappings, operatorData.OperatorId);
            allInspections.Concat(inspectionList); // vėlesniam pasikartojimų tikrinimui
            List<BadData> bdList = verifyMethod(inspectionList, operatorData.OperatorId);
            return BadDataReportCreator.CreatePlainTxt(bdList);
        }

        private static StringBuilder fetchConvertVerifySpreadsheet(
            SheetsService service, 
            Spreadsheets.Operator operatorData, 
            Spreadsheets.SheetsRanges allRanges)
        {
            StringBuilder sb = new StringBuilder().AppendLine(operatorData.OperatorId);

            StringBuilder pirmiejiProblems = fetchConvertVerifySheet(
                service, 
                operatorData, 
                allRanges.Pirmieji, 
                DataConverter.ConvertPirmieji, 
                ObjectVerifier.VerifyPirmieji);

            StringBuilder nepirmiejiProblems = fetchConvertVerifySheet(
                service, 
                operatorData, 
                allRanges.Nepirmieji, 
                DataConverter.ConvertNepirmieji, 
                ObjectVerifier.VerifyNepirmieji);

            return sb.AppendLine("Pirmieji").Append(pirmiejiProblems).AppendLine("Nepirmieji").Append(nepirmiejiProblems);
        }

        public static StringBuilder FetchConvertVerify(SheetsService service, Spreadsheets.Operator[] operators, Spreadsheets.SheetsRanges allRanges)
        {
            StringBuilder sb = new StringBuilder(DateTime.Now.ToShortDateString()).Append(", ").AppendLine(DateTime.Now.ToShortTimeString());

            foreach (Spreadsheets.Operator operatorData in operators)
            {
                StringBuilder operatorProblems = fetchConvertVerifySpreadsheet(service, operatorData, allRanges);
                sb.Append(operatorProblems);
                sb.AppendLine();
            }

            StringBuilder repSb = RepeatFinder.FindRepeats(allInspections);
            sb.AppendLine().AppendLine("Pasikartojantys suvirinimai:");
            sb.Append(repSb);
            return sb;
        }
    }
}
