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
    internal static class Controller
    {
        private delegate List<WeldingInspection> ConvertDataToListMethod(List<IList<Object>> data, string[] mapping, string operId);
        private delegate StringBuilder VerifyObjectsMethod(List<WeldingInspection> inspectionList);
        private delegate int DbUpdateMethod(List<WeldingInspection> inspectionList);

        // kaupiami visi patikrinimai reportui
        private static List<WeldingInspection> allInspections = new List<WeldingInspection>();

        // vieno operatoriaus vienos GS lentelės duomenis - fetchina, paverčia WeldingInspection ir patikrina
        private static StringBuilder fetchConvertVerifySheet(SheetsService service, 
            Spreadsheets.Operator operatorData, // lemia, katro operatoriaus spreadsheet
            Spreadsheets.SheetsRanges.SheetRangeData rangeData, ConvertDataToListMethod convertMethod, VerifyObjectsMethod verifyMethod) // lemia, katrą to operatoriaus lentelę
        {
            List<IList<Object>> data = SheetDataFetcher.Fetch(operatorData.SpreadsheetId, rangeData.RangeAddress, rangeData.FilterColumn.Index, service).ToList();
            List<WeldingInspection> inspectionList = convertMethod(data, rangeData.FieldMappings, operatorData.OperatorId);
            return verifyMethod(inspectionList);
        }

        private static StringBuilder fetchConvertVerifySpreadsheet(SheetsService service, Spreadsheets.Operator operatorData, Spreadsheets.SheetsRanges allRanges)
        {
            StringBuilder sb = new StringBuilder().AppendLine(operatorData.OperatorId);
            StringBuilder pirmiejiProblems = fetchConvertVerifySheet(service, operatorData, allRanges.Pirmieji, DataConverter.ConvertPirmieji, ObjectVerifier.VerifyPirmieji);
            StringBuilder nepirmiejiProblems = fetchConvertVerifySheet(service, operatorData, allRanges.Nepirmieji, DataConverter.ConvertNepirmieji, ObjectVerifier.VerifyNepirmieji);
            return sb.AppendLine("Pirmieji").Append(pirmiejiProblems).AppendLine("Nepirmieji").Append(nepirmiejiProblems);
        }

        internal static StringBuilder fetchConvertVerify(SheetsService service, Spreadsheets.Operator[] operators, Spreadsheets.SheetsRanges allRanges)
        {
            StringBuilder sb = new StringBuilder(DateTime.Now.ToShortDateString()).Append(", ").AppendLine(DateTime.Now.ToShortTimeString());
            foreach (Spreadsheets.Operator operatorData in operators)
            {
                StringBuilder operatorProblems = fetchConvertVerifySpreadsheet(service, operatorData, allRanges);
                sb.Append(operatorProblems);
                sb.AppendLine();
            }
            return sb;
        }


        // vieno operatoriaus GS lentelės duomenis - updateina DB
        private static int updateDb(List<WeldingInspection> inspections, DbUpdateMethod updateMethod)
        {
            int updateCount = 0;
            try
            {
                updateCount = updateMethod(inspections);
                allInspections.Concat(inspections);
            }
            catch
            {
                return 0;
            }
            return updateCount;
        }

        


    }
}
