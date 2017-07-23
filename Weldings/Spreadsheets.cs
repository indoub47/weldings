using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace Weldings
{
    public class Spreadsheets
    {
        public class Operator
        {
            public string OperatorId;
            public string SpreadsheetId;
            public string[] SheetGids;
        }

        public class SheetsRanges
        {
            public class SheetRangeData
            {
                public class FilterColumnData
                {
                    public int Index;
                    public string CellAddressFormat;
                }

                public string SheetName;
                public string RangeAddress;
                public int StartRow;
                public FilterColumnData FilterColumn;
                public string[] FieldMappings;
            }

            public SheetRangeData Pirmieji;
            public SheetRangeData Nepirmieji;
            public SheetRangeData Defektai;
        }

        public Operator[] Operators { get; set; }
        public SheetsRanges AllRanges { get; set; }
    }

    public static class SpreadsheetsData
    {
        private static Spreadsheets instance;

        private static Spreadsheets getSPInstance()
        {
            if (instance == null)
            {
                string jsonString = File.ReadAllText(Properties.Settings.Default.SpreadsheetsJsonFilePath);
                instance = JsonConvert.DeserializeObject<Spreadsheets>(jsonString);
            }
            return instance;
        }

        public static Spreadsheets.SheetsRanges AllRanges
        {
            get
            {
                return getSPInstance().AllRanges;
            }
        }

        public static Spreadsheets.Operator[] Operators
        {
            get
            {
                return getSPInstance().Operators;
            }
        }

        public static string[] OperatorCodes
        {
            get
            {
                return Operators.Select(x => x.OperatorId).ToArray();
            }
        }

        public static Spreadsheets.Operator GetOperatorById(string operatorId)
        {
            return Operators.Where(x => x.OperatorId == operatorId).First();
        }
    }
}
