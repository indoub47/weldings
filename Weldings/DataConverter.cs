using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weldings
{
    public static class DataConverter
    {

        // iš Google Sheets data formato List<ILIst<Object>> padaro List<WeldingInspection>
        //   , o jeigu buvo rasta blogų duomenų, tuomet throw new BadDataException
        public static List<WeldingInspection> ConvertPirmieji(List<IList<Object>> data, string[] mapping, string operatorius)
        {
            List<WeldingInspection> tikrinimaiList = new List<WeldingInspection>();
            List<BadData> badDataList = new List<BadData>();
            string ifas = Properties.Settings.Default.Ifas;
            if (data == null || data.Count == 0) return tikrinimaiList;
            foreach (var row in data)
            {
                object value;
                int bdlCount = badDataList.Count; // kiek buvo BadData šitos row tikrinimo pradžioje

                string linija = null, salKodas = null, aparatas = null, suvirino = null;
                int kelias = -1, km = -1, m = -1;
                int? pk = null, siule = null;
                DateTime tikrinimoData = DateTime.MinValue;

                // žyma, nurodyti, kurioje vietoje problemos
                string vietosKodoSurogatas = dataRowToVietosKodas(row, mapping);

                // tikrinami bendri pirmiems ir nepirmiems
                List<string> messages = patikrintiBendrus(row, mapping,
                    ref linija, ref kelias, ref km, ref pk, ref m, ref siule,
                    ref salKodas, ref aparatas, ref tikrinimoData);

                foreach (string message in messages)
                {
                    badDataList.Add(new BadData(operatorius, SheetType.pirmieji, vietosKodoSurogatas, message));
                }

                // specifinis pirmiesiems
                value = getRowItem("suvirino", mapping, row);
                if (value == null || value.ToString().Trim() == string.Empty)
                {
                    badDataList.Add(new BadData(operatorius, SheetType.pirmieji, vietosKodoSurogatas, "kas suvirino"));
                }
                else
                {
                    suvirino = value.ToString().Trim();
                }

                // specifinis pirmiesiems
                if (kelias != -1 && kelias != 8 && pk == null)
                {
                    badDataList.Add(new BadData(operatorius, SheetType.pirmieji, vietosKodoSurogatas, "pk"));
                }

                // specifinis pirmiesiems
                if ((kelias == 8 || kelias == 9) && siule != null)
                {
                    badDataList.Add(new BadData(operatorius, SheetType.pirmieji, vietosKodoSurogatas, "suvirinimas iešme, o nurodyta siūlė"));
                }

                // jeigu tikrinant šitą row nebuvo aptikta problemų - new WeldingInspection
                if (bdlCount == badDataList.Count)
                {
                    WeldingInspection wi = new WeldingInspection(
                            null, // id
                            linija, kelias, km, pk, m, siule,
                            salKodas,
                            operatorius,
                            aparatas,
                            tikrinimoData.Date,
                            suvirino,
                            Kelintas.I);
                    tikrinimaiList.Add(wi);
                }
            }

            if (badDataList.Count > 0)
            {
                throw new BadDataException("Blogi duomenys lentelėse - nepavyksta perskaityti vietos kodo", badDataList);
            }

            // jeigu visi įrašai geri
            return tikrinimaiList;
        }


        // iš Google Sheets data formato List<ILIst<Object>> padaro List<WeldingInspection>
        //   , o jeigu buvo rasta blogų duomenų, tuomet throw new BadDataException
        public static List<WeldingInspection> ConvertNepirmieji(List<IList<Object>> data, string[] mapping, string operatorius)
        {
            List<WeldingInspection> tikrinimaiList = new List<WeldingInspection>();
            List<BadData> badDataList = new List<BadData>();
            if (data == null || data.Count == 0) return tikrinimaiList;
            foreach (var row in data)
            {
                object value; // row stulpelio value
                int bdlCount = badDataList.Count; // kiek buvo BadData šitos row tikrinimo pradžioje

                long id = 0;
                string linija = null, salKodas = null, aparatas = null;
                int kelias = -1, km = -1, m = -1;
                int? pk = null, siule = null;
                DateTime tikrinimoData = DateTime.MinValue;
                Kelintas kelintas = Kelintas.I;

                // specifinis nepirmiesiems
                value = getRowItem("id", mapping, row);
                string idZyma = value == null ? "no id" : value.ToString(); // žyma, pažymėti, kurioje vietoje problemos
                try
                {
                    id = Convert.ToInt64(value);
                }
                catch
                {
                    badDataList.Add(new BadData(operatorius, SheetType.nepirmieji, idZyma, "suvirinimo id"));
                }

                // tikrinami bendri pirmiems ir nepirmiems
                List<string> messages = patikrintiBendrus(row, mapping,
                    ref linija, ref kelias, ref km, ref pk, ref m, ref siule,
                    ref salKodas, ref aparatas, ref tikrinimoData);

                foreach (string message in messages)
                {
                    badDataList.Add(new BadData(operatorius, SheetType.nepirmieji, idZyma, message));
                }

                // specifinis nepirmiems
                value = getRowItem("kelintas_tikrinimas", mapping, row);
                if (value == null || value.ToString().Trim() == string.Empty || !new[]{"2", "3", "4", "papild"}.Contains(value.ToString().Trim()))
                {
                    badDataList.Add(new BadData(operatorius, SheetType.nepirmieji, idZyma, "kelintas tikrinimas"));
                }
                else
                {
                    string val = value.ToString().Trim();
                    switch (val)
                    {
                        case "2":
                            kelintas = Kelintas.II;
                            break;
                        case "3":
                            kelintas = Kelintas.III;
                            break;
                        case "4":
                            kelintas = Kelintas.IV;
                            break;
                        case "papild":
                            kelintas = Kelintas.papildomas;
                            break;
                    }
                }

                // jeigu tikrinant šitą row nebuvo aptikta problemų - new WeldingInspection
                if (bdlCount == badDataList.Count)
                {
                    WeldingInspection wi = new WeldingInspection(
                        id,
                        linija, kelias, km, pk, m, siule,
                        salKodas,
                        operatorius,
                        aparatas,
                        tikrinimoData.Date,
                        null, // suvirino
                        kelintas);
                    tikrinimaiList.Add(wi);
                }
            }

            if (badDataList.Count > 0)
            {
                throw new BadDataException("Blogi duomenys lentelėje - nepavyksta perskaityti vietos kodo", badDataList);
            }

            // jeigu visi įrašai geri
            return tikrinimaiList;
        }

        // Atlieka duomenų patikrinimą. Atlieka tuos tikrinimus, kurie bendri ir pirmiesiems, ir 
        // nepirmiesiems. Grąžina List<string> - problemų List. Jeigu problemų nėra, grąžina tuščią List
        private static List<string> patikrintiBendrus(IList<object> row, string[] mapping,
            ref string linija, ref int kelias, ref int km, ref int? pk, ref int m, ref int? siule,
            ref string salKodas, ref string aparatas, ref DateTime tikrinimoData)
        {
            object value;
            List<string> messages = new List<string>();

            value = getRowItem("linija", mapping, row);
            if (value == null || value.ToString().Trim() == string.Empty)
            {
                messages.Add("XX.oooo.oo.oo.o");
            }
            else
            {
                linija = value.ToString().Trim();
            }

            value = getRowItem("kelias", mapping, row);
            try
            {
                kelias = Convert.ToInt32(value);
            }
            catch
            {
                messages.Add("oo.Xooo.oo.oo.o");
            }

            value = getRowItem("km", mapping, row);
            try
            {
                km = Convert.ToInt32(value);
            }
            catch
            {
                messages.Add("oo.oXXX.oo.oo.o");
            }

            value = getRowItem("pk", mapping, row);
            if (value == null || value.ToString().Trim() == string.Empty)
            {
                pk = null;
            }
            else
            {
                try
                {
                    pk = Convert.ToInt32(value);
                }
                catch
                {
                    messages.Add("oo.oooo.XX.oo.o");
                }
            }

            value = getRowItem("m", mapping, row);
            try
            {
                m = Convert.ToInt32(value);
            }
            catch
            {
                messages.Add("oo.oooo.oo.XX.o");
            }

            value = getRowItem("siule", mapping, row);
            if (value == null || value.ToString().Trim() == string.Empty)
            {
                siule = null;
            }
            else
            {
                try
                {
                    siule = Convert.ToInt32(value);
                }
                catch
                {
                    messages.Add("oo.oooo.oo.oo.X");
                }
            }

            value = getRowItem("salyginis_kodas", mapping, row);
            if (value == null || value.ToString().Trim() == string.Empty)
            {
                messages.Add("salyginis kodas");
            }
            else
            {
                salKodas = value.ToString().Trim();
            }

            value = getRowItem("aparatas", mapping, row);
            if (value == null || value.ToString().Trim() == string.Empty)
            {
                messages.Add("defektoskopo kodas");
            }
            else
            {
                aparatas = value.ToString().Trim();
            }

            value = getRowItem("tikrinimo_data", mapping, row);
            try
            {
                tikrinimoData = Convert.ToDateTime(value);
                if (Properties.Settings.Default.CheckDateIfReal && isNotReal(tikrinimoData))
                {
                    messages.Add(string.Format("tikrinimo data {0:yyyy-MM-dd} neatrodo reali", tikrinimoData));
                }
            }
            catch
            {
                messages.Add("tikrinimo data");
            }

            return messages;
        }

        // Tikrina, ar patikrinimo data nėra ateityje arba per giliai praeityje
        private static bool isNotReal(DateTime tikrinimoData)
        {
            return (tikrinimoData > DateTime.Now) ||
                (tikrinimoData < DateTime.Now.AddDays(-Properties.Settings.Default.AllowedDayCount));
        }

        // pagamina vietos kodo surogatą iš nepatikrintų duomenų - reikalingas kažkaip 
        // pažymėti, kurioje Google Sheet eilutėje aptiktos problemos
        private static string dataRowToVietosKodas(IList<object> row, string[] mapping)
        {
            return string.Format("{0:00}.{1:0}{2:000}.{3:#00}.{4:#00}.{5:##0}",
                getRowCellObject(getRowItem("linija",mapping, row)),
                getRowCellObject(getRowItem("kelias", mapping, row)),
                getRowCellObject(getRowItem("km", mapping, row)),
                getRowCellObject(getRowItem("pk", mapping, row)),
                getRowCellObject(getRowItem("m", mapping, row)),
                getRowCellObject(getRowItem("siule", mapping, row)));
        }

        // jeigu tuščias row cell, grąžina brūkšnelį. 
        private static object getRowCellObject(object obj)
        {
            if (obj == null || obj.ToString().Trim() == string.Empty)
            {
                return (object)"_";
            }

            try
            {
                return (object)Convert.ToInt32(obj);
            }
            catch
            {
                return obj;
            }

        }

        private static object getRowItem(string colName, string[] mapping, IList<object> row)
        {
            // iš tikrųjų čia apsisaugoti nuo to dalyko, kad row gali būti trumpesnis
            // negu mapping
            int ind = Array.IndexOf(mapping, colName);
            if (row.Count - 1 < ind) return null;
            return row[ind];
        }
    }
}
