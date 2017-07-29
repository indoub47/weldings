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
    internal static class Controller1
    {
        // Fetch data from Google Sheets
        // Try to convert it to WeldingInspections
        // Verify
        // Inform user about problems

        private delegate List<WeldingInspection> ConvertDataToListMethod(List<IList<Object>> data, string[] mapping, string operId);
        private delegate StringBuilder VerifyObjectsMethod(List<WeldingInspection> inspectionList);
        private delegate int DbUpdateMethod(List<WeldingInspection> inspectionList);

        // kaupiami visi patikrinimai tam, kad b8t7 galima pagauti, jeigu tą patį suvirinimą tikrina du kartus
        private static List<WeldingInspection> allInspections = new List<WeldingInspection>();

        // vieno operatoriaus vienos GS lentelės duomenis - fetchina, paverčia WeldingInspection ir patikrina
        private static StringBuilder fetchConvertVerifySheet(SheetsService service, 
            Spreadsheets.Operator operatorData, // lemia, katro operatoriaus spreadsheet
            Spreadsheets.SheetsRanges.SheetRangeData rangeData, ConvertDataToListMethod convertMethod, VerifyObjectsMethod verifyMethod) // lemia, katrą to operatoriaus lentelę
        {
            List<IList<Object>> data = SheetDataFetcher.Fetch(operatorData.SpreadsheetId, rangeData.RangeAddress, rangeData.FilterColumn.Index, service).ToList();
            List<WeldingInspection> inspectionList = convertMethod(data, rangeData.FieldMappings, operatorData.OperatorId);
            allInspections.Concat(inspectionList); // vėlesniam pasikartojimų tikrinimui
            return verifyMethod(inspectionList); // returns a StringBuilder
        }

        private static StringBuilder fetchConvertVerifySpreadsheet(SheetsService service, Spreadsheets.Operator operatorData, Spreadsheets.SheetsRanges allRanges)
        {
            StringBuilder sb = new StringBuilder().AppendLine(operatorData.OperatorId);
            StringBuilder pirmiejiProblems = fetchConvertVerifySheet(service, operatorData, allRanges.Pirmieji, DataConverter.ConvertPirmieji, ObjectVerifier.VerifyPirmieji);
            StringBuilder nepirmiejiProblems = fetchConvertVerifySheet(service, operatorData, allRanges.Nepirmieji, DataConverter.ConvertNepirmieji, ObjectVerifier.VerifyNepirmieji);
            return sb.AppendLine("Pirmieji").Append(pirmiejiProblems).AppendLine("Nepirmieji").Append(nepirmiejiProblems);
        }

        internal static StringBuilder FetchConvertVerify(SheetsService service, Spreadsheets.Operator[] operators, Spreadsheets.SheetsRanges allRanges)
        {
            StringBuilder sb = new StringBuilder(DateTime.Now.ToShortDateString()).Append(", ").AppendLine(DateTime.Now.ToShortTimeString());

            foreach (Spreadsheets.Operator operatorData in operators)
            {
                StringBuilder operatorProblems = fetchConvertVerifySpreadsheet(service, operatorData, allRanges);
                sb.Append(operatorProblems);
                sb.AppendLine();
            }

            List<RepeatFinder.Repeats> repeatsList = RepeatFinder.FindRepeats(allInspections);
            foreach (RepeatFinder.Repeats repeats in repeatsList)
            {
                sb.AppendLine("Kartojasi suvirinimas " + getRepeatsLine(repeats));
            }

            return sb;
        }

        private static string getRepeatsLine(RepeatFinder.Repeats repeats)
        {
            string line = repeats.VietosKodas + ": ";
            foreach (RepeatFinder.Repeats.Rep rep in repeats.RepList)
            {
                line += string.Format("{0}-{1}", rep.OperatorId, rep.Times);
                if (rep != repeats.RepList.Last())
                {
                    line += ", ";
                }
            }
            return line;
        }
    }
}
