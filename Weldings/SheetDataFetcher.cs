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
    public static class SheetDataFetcher
    {
        public static List<IList<Object>> Fetch(string spreadsheetId, string range, int filterFieldIndex, SheetsService service)
        {

            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);
 
            ValueRange response = request.Execute();
            IList<IList<Object>> allRecords = response.Values;

            if (allRecords == null)
                throw new Exception("Duomenų parsisiuntimo iš Google Sheets rezultatas - null.");

            // nufiltruojami tie, kurių įvedimo į db data nelygi null
            List<IList<Object>> list = allRecords.ToList<IList<Object>>();
            list.RemoveAt(0);
            return list.Where(x => x.Count <= filterFieldIndex || x[filterFieldIndex] == null).ToList<IList<Object>>();
        }
    }
}
