using System;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weldings.Properties;
using System.Windows.Forms;

namespace Weldings
{
    public static class ObjectVerifier
    {
        public static List<BadData> VerifyPirmieji(List<WeldingInspection> wiList, string operatorius)
        {
            List<BadData> badDataList = new List<BadData>();
            using (OleDbConnection conn = new OleDbConnection(string.Format(Settings.Default.OleDbConnectionString, Settings.Default.AccessDbPath)))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                try
                {
                    conn.Open();

                    foreach (WeldingInspection wi in wiList)
                    {
                        BadData bd = new BadData(operatorius, SheetType.pirmieji);
                        bd.Zhyme = string.Format("{0}.{1}{2:000}.{3}.{4:#00}.{5}",
                            wi.Linija, wi.Kelias, wi.Km,
                            (wi.Pk == null || wi.Pk.ToString().Trim() == string.Empty) ? " " : Convert.ToInt32(wi.Pk).ToString("#00"),
                            wi.M,
                            (wi.Siule == null || wi.Siule.ToString().Trim() == string.Empty) ? " " : Convert.ToInt32(wi.Siule).ToString("##0"));
                        try
                        {
                            List<long> samePlaceIds = VerifyByVieta(wi, cmd);

                            if (samePlaceIds.Count > 0)
                            {                                
                                bd.Message = string.Format("tą patį vietos kodą turi DB įrašas (-ai) {0}", String.Join(", ", samePlaceIds));
                                badDataList.Add(bd);
                            }
                        }
                        catch (Exception ex1) // jeigu exception tikrinant vieną
                        {
                            bd.Message = string.Format("įrašas nepatikrintas: " + ex1.Message);
                            badDataList.Add(bd);
                            LogWriter.Log(ex1);
                        }
                    }
                }
                catch (Exception ex) // jeigu connection exception
                {
                    badDataList.Add(new BadData(operatorius, SheetType.pirmieji, "visi įrašai", 
                        "nepatikrinti: " + ex.Message));
                    LogWriter.Log(ex);
                }
            }
            return badDataList;
        }

        private static List<long> VerifyByVieta(WeldingInspection wi, OleDbCommand cmd)
        {
            // prieš insertinant pirmuosius suvirinimus, tikrina, ar yra duomenų bazėje su tokiu vietos kodu;
            // grąžina esamų suvirinimų su tokiu vietos kodu id sąrašą
            cmd.CommandText = "SELECT number FROM ssd WHERE Linia = @linija AND Kel = @kelias AND kilomrtras = @km AND piket = @pk AND metras = @m AND siule = @siule AND [saliginis kodas] IN ('06.4', '06.3');";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@linija", wi.Linija);
            cmd.Parameters.AddWithValue("@kelias", wi.Kelias);
            cmd.Parameters.AddWithValue("@km", wi.Km);
            DBUpdater.AddNullableParam(cmd.Parameters, wi.Pk, "@pk");
            DBUpdater.AddNullableParam(cmd.Parameters, wi.M, "@m");
            DBUpdater.AddNullableParam(cmd.Parameters, wi.Siule, "@siule");
            List<long> ids = new List<long>();
            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ids.Add(Convert.ToInt64(reader[0]));
                }
            }
            return ids;
        }

        public static List<BadData> VerifyNepirmieji(List<WeldingInspection> wiList, string operatorius)
        {
            List<BadData> badDataList = new List<BadData>();

            using (OleDbConnection conn = new OleDbConnection(string.Format(Settings.Default.OleDbConnectionString, Settings.Default.AccessDbPath)))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                try
                {
                    conn.Open();
                    foreach (WeldingInspection wi in wiList)
                    {
                        string zhyme = wi.Id.ToString();
                        try
                        {
                            List<string> problems = VerifyById(wi, cmd);

                            foreach (string problem in problems)
                            {
                                badDataList.Add(new BadData(operatorius, SheetType.nepirmieji, zhyme, problem));
                            }
                        }
                        catch (Exception ex1)
                        {
                            badDataList.Add(new BadData(operatorius, SheetType.nepirmieji, zhyme, "įrašas nepatikrintas: " + ex1.Message));
                            LogWriter.Log(ex1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    badDataList.Add(new BadData(operatorius, SheetType.nepirmieji, "visi įrašai",
                        "nepatikrinti: " + ex.Message));
                    LogWriter.Log(ex);
                }
            }
            return badDataList;
        }

        private static List<string> VerifyById(WeldingInspection wi, OleDbCommand cmd)
        {
            // prieš updateinant ne pirmuosius patikrinimus,
            // patikrina ar operatoriaus įrašytas suvirinimas yra duomenų bazėje ir ar duomenys teisingi;
            // grąžina neatitikimų sąrašą.
            cmd.CommandText = "SELECT * FROM ssd WHERE number = @id";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@id", wi.Id);
            List<string> problems;
            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                problems = verifyRecord(wi, reader);
            }
            return problems;
        }


        /// <summary>
        /// Tikrina operatoriaus siūlomo WeldingInspection ir duomenų bazėje esančio įrašo laukus -
        /// lygina, ar vieni laukai sutampa ir ar kitų laukų reikšmės realios
        /// </summary>
        /// <param name="wi">operatoriaus siūlomo WeldingInspection</param>
        /// <param name="reader">iš duomenų bazės gautas įrašas</param>
        /// <returns></returns>
        private static List<string> verifyRecord(WeldingInspection wi, OleDbDataReader reader)
        {
            List<string> problems = new List<string>();
            if (reader.Read())
            {
                if (reader["Linia"].ToString() != wi.Linija)
                {
                    problems.Add("neatitinka vietos kodas (Linia)");
                }

                if (Convert.ToInt32(reader["Kel"]) != wi.Kelias)
                {
                    problems.Add("neatitinka vietos kodas (Kel)");
                }

                if (Convert.ToInt32(reader["kilomrtras"]) != wi.Km)
                {
                    problems.Add("neatitinka vietos kodas (kilomrtras)");
                }

                if (intFieldNotEqProp(reader["piket"], wi.Pk))
                {
                    problems.Add("neatitinka vietos kodas (piket)");
                }

                if (intFieldNotEqProp(reader["metras"], wi.M))
                {
                    problems.Add("neatitinka vietos kodas (metras)");
                }

                if (intFieldNotEqProp(reader["siule"], wi.Siule))
                {
                    problems.Add("neatitinka vietos kodas (siule)");
                }

                if (reader["saliginis kodas"].ToString() != wi.SalygKodas) // nereikia tikrinti null,  nes db yra privalomas
                {
                    problems.Add(string.Format("DB esantis sąlyginis kodas {0} neatitinka siūlomo sąlyginio kodo {1}",
                        reader["saliginis kodas"], wi.SalygKodas));
                }

                if (wi.KelintasTikrinimas != Kelintas.papildomas)
                {
                    string patDataField = "", formerPatDataField = "", nextPatDataField = "";
                    switch (wi.KelintasTikrinimas)
                    {
                        case Kelintas.II:
                            patDataField = "II_pat_data";
                            formerPatDataField = "I_pat_data";
                            nextPatDataField = "III_pat_data";
                            break;
                        case Kelintas.III:
                            patDataField = "III_pat_data";
                            formerPatDataField = "II_pat_data";
                            nextPatDataField = "IV_pat_data";
                            break;
                        case Kelintas.IV:
                            patDataField = "IV_pat_data";
                            formerPatDataField = "III_pat_data";
                            break;
                    }

                    // patDataField turi būti tuščias
                    if (reader[patDataField] != null && reader[patDataField].ToString() != string.Empty)
                    {
                        problems.Add(string.Format("{0} tikrinimas jau atliktas.", wi.KelintasTikrinimas));
                    }
                    // formerPatDataField turi būti netuščias
                    else if (reader[formerPatDataField] == null || reader[formerPatDataField].ToString() == string.Empty)
                    {
                        problems.Add(string.Format("neatliktas ankstesnis patikrinimas ({0} tuščias)", formerPatDataField));
                    }
                    // nextPatDataField turi būti tuščias.
                    // Čia būtų arba db duomenų klaida - rodytų, kad šitas tikrinimas dar neatliktas, bet atliktas vėlesnis,
                    // arba įrašas būtų pažymėtas kaip panaikintas (tuomet surašomos visų tikrinimų fake datos), bet nepakeistas sąlyginis kodas į x.6?
                    else if (nextPatDataField != "" && reader[nextPatDataField] != null && reader[nextPatDataField].ToString() != string.Empty)
                    {
                        problems.Add(string.Format("įrašyta, kad jau atliktas vėlesnis patikrinimas ({0} netuščias)", nextPatDataField));
                    }
                    // formerPatDataField turi būti ankstesnis už wi.Data
                    else if (Convert.ToDateTime(reader[formerPatDataField]) > wi.TikrinimoData)
                    {
                        problems.Add(string.Format("ankstesnis patikrinimas {0} atliktas vėliau ({1:d}) negu dabar siūlomas {2} ({3:d}).",
                            formerPatDataField, reader[formerPatDataField], patDataField, wi.TikrinimoData));
                    }
                }
            }
            else
            {
                problems.Add("įrašas nerastas DB");
            }

            return problems;
        }

        /// <summary>
        /// lygina iš duomenų bazės gauto įrašo lauko reikšmę su operatoriaus siūlomo
        /// įrašo lauko reikšme
        /// </summary>
        /// <param name="field">iš duomenų bazės gauto įrašo lauko reikšmė</param>
        /// <param name="propertyValue">WeldingInspection objekto lauko reikšmė</param>
        /// <returns>ar reikšmės yra nelygios yra nelygios</returns>
        private static bool intFieldNotEqProp (object field, int? propertyValue)
        {
            if (field == DBNull.Value) return (propertyValue != null);
            return (Convert.ToInt32(field) != propertyValue);
        } 
    }
}
