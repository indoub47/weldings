using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace Weldings
{
    internal static class GSheetsUpdater
    {
        internal static BatchUpdateValuesResponse BatchUpdateSheet(string spreadsheetId, Spreadsheets.SheetsRanges.SheetRangeData rangeData, SheetsService service)
        {
            // gaunami duomenys iš sheetų
            IList<IList<Object>> sheetValues = getRangeValues(spreadsheetId, rangeData.RangeAddress, service);

            List<ValueRange> requestData = new List<ValueRange>();
            string dateFormat = Properties.Settings.Default.DateFormat;
            object valueToWrite = DateTime.Now.Date.ToString(dateFormat);

            // pridedami duomenys į requestData
            addDataToRequest(sheetValues, rangeData, valueToWrite, requestData);

            // sukuriamas batch update request
            BatchUpdateValuesRequest request = getUpdateValuesRequest(requestData);

            // updateinamas spreadsheet
            return updateSheet(request, spreadsheetId, service);
        }


        private static IList<IList<Object>> getRangeValues(string spreadsheetId, string rangeAddress, SheetsService service)
        {
            // gauna viso range duomenis, tam, kad sužinoti, kuriuos laukus reikia užpildyti
            SpreadsheetsResource.ValuesResource.GetRequest getRequest =
                    service.Spreadsheets.Values.Get(spreadsheetId, rangeAddress);

            ValueRange requestResponse = getRequest.Execute();
            IList<IList<Object>> responseValues = requestResponse.Values;
            return responseValues;
        }

        private static BatchUpdateValuesRequest getUpdateValuesRequest(List<ValueRange> requestData, string valueInputOption = "USER_ENTERED")
        {
            BatchUpdateValuesRequest requestBody = new BatchUpdateValuesRequest();
            requestBody.Data = requestData;
            requestBody.ValueInputOption = valueInputOption;
            return requestBody;
        }

        private static void addDataToRequest(IList<IList<Object>> rangeValues, Spreadsheets.SheetsRanges.SheetRangeData sheetRangeData, object valueToWrite, List<ValueRange> requestData)
        {           
            var updateValue = new List<object>() { valueToWrite };

            // tiems langeliams, kuriuose nėra datos, sukuria ValueRange
            for (int r = 0; r < rangeValues.Count; r++)
            {
                if (rangeValues[r].Count < sheetRangeData.FilterColumn.Index + 1 ||
                    rangeValues[r][sheetRangeData.FilterColumn.Index] == null ||
                    rangeValues[r][sheetRangeData.FilterColumn.Index].ToString().Trim() == string.Empty)
                {
                    ValueRange vr = new ValueRange();
                    vr.Range = string.Format(sheetRangeData.FilterColumn.CellAddressFormat, r + sheetRangeData.StartRow);
                    vr.Values = new List<IList<object>> { updateValue };
                    requestData.Add(vr);
                }
            }
        }

        private static BatchUpdateValuesResponse updateSheet(BatchUpdateValuesRequest request, string spreadsheetId, SheetsService service)
        {
            return service.Spreadsheets.Values.BatchUpdate(request, spreadsheetId).Execute();
        }
    }
}
