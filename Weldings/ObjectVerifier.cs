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
    internal static class ObjectVerifier
    {

        internal static StringBuilder VerifyPirmieji(List<WeldingInspection> wiList)
        {
            // StringBuilder errors = new StringBuilder(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
            // errors.AppendLine().AppendLine("Suvirinimų pirmųjų patikrinimų problemos").AppendLine();

            // antrašte pasirūpina klientas, nes kitaip gaunasi labai užšikta ataskaita
            StringBuilder errors = new StringBuilder();
            using (OleDbConnection conn = new OleDbConnection(string.Format(Settings.Default.OleDbConnectionString, Settings.Default.AccessDbPath)))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                //try
                //{
                    conn.Open();
                    foreach (WeldingInspection wi in wiList)
                    {
                        List<long> samePlaceIds = VerifyByVieta(wi, cmd);
                        if (samePlaceIds.Count > 0)
                        {
                            errors.AppendFormat("{0}.{1}{2:000}.{3}.{4}.{5} - tą patį vietos kodą turi DB įrašai {6}",
                                wi.Linija,
                                wi.Kelias,
                                wi.Km,
                                (wi.Pk == null || wi.Pk.ToString().Trim() == string.Empty) ? "  " : Convert.ToInt32(wi.Pk.ToString()).ToString("#00"),
                                (wi.M == null || wi.M.ToString().Trim() == string.Empty) ? "  " : Convert.ToInt32(wi.M.ToString()).ToString("#00"),
                                (wi.Siule == null || wi.Siule.ToString().Trim() == string.Empty) ? " " : Convert.ToInt32(wi.Siule.ToString()).ToString("##0"),
                                String.Join(", ", samePlaceIds));
                            errors.AppendLine().AppendLine();
                        }
                    }
                //}
               // catch
                //{
                    // do nothing for debugging
                //}
                return errors;
            }
        }

        internal static StringBuilder VerifyNepirmieji(List<WeldingInspection> wiList)
        {
            //StringBuilder errors = new StringBuilder(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
            //errors.AppendLine().AppendLine("Suvirinimų nepirmųjų patikrinimų problemos").AppendLine();

            // antrašte pasirūpina klientas, nes kitaip gaunasi labai užšikta ataskaita
            StringBuilder errors = new StringBuilder();
            List <string> problems = new List<string>();
            using (OleDbConnection conn = new OleDbConnection(string.Format(Settings.Default.OleDbConnectionString, Settings.Default.AccessDbPath)))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                //try
                //{
                    conn.Open();
                    foreach (WeldingInspection wi in wiList)
                    {
                        problems = VerifyById(wi, cmd);
                        if (problems.Count > 0)
                        {
                            errors.Append(String.Join(Environment.NewLine, problems));
                            errors.AppendLine().AppendLine();
                        }
                    }
                //}
               // catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}
                return errors;
            }
        }

        private static List<long> VerifyByVieta(WeldingInspection wi, OleDbCommand cmd)
        {
            // prieš insertinant pirmuosius suvirinimus, tikrina, ar yra duomenų bazėje su tokiu vietos kodu;
            // grąžina esamų suvirinimų su tokiu vietos kodu id sąrašą
            cmd.CommandText = "SELECT number FROM ssd WHERE Linia = @linija AND Kel = @kelias AND kilomrtras = @km AND piket = @pk AND metras = @m AND siule = @siule AND [saliginis kodas] IN ('06.4', '06.3');";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Linia", wi.Linija);
            cmd.Parameters.AddWithValue("@Kel", wi.Kelias);
            cmd.Parameters.AddWithValue("@kilomrtras", wi.Km);
            DBUpdater.AddNullableParam(cmd.Parameters, wi.Pk, "@piket");
            DBUpdater.AddNullableParam(cmd.Parameters, wi.M, "@metras");
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

        private static List<string> VerifyById(WeldingInspection wi, OleDbCommand cmd)
        {
            // prieš updateinant ne pirmuosius patikrinimus,
            // patikrina ar operatoriaus įrašytas suvirinimas yra duomenų bazėje ir ar duomenys teisingi;
            // grąžina neatitikimų sąrašą.
            cmd.CommandText = "SELECT * FROM ssd WHERE number = @id";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@id", wi.Id);
            List<string> errors = new List<string>();
            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                verifyRecord(wi, errors, reader);
            }
            return errors;
        }


        // reader - IReadable, IIndexable
        private static void verifyRecord(WeldingInspection wi, List<string> errors, OleDbDataReader reader)
        {
            if (reader.Read())
            {
                if (reader["Linia"].ToString() != wi.Linija)
                {
                    errors.Add(string.Format("{0} - neatitinka vietos kodas (Linia).", wi.Id));
                }

                if (Convert.ToInt32(reader["Kel"]) != wi.Kelias)
                {
                    errors.Add(string.Format("{0} - neatitinka vietos kodas (Kel).", wi.Id));
                }

                if (Convert.ToInt32(reader["kilomrtras"]) != wi.Km)
                {
                    errors.Add(string.Format("{0} - neatitinka vietos kodas (kilomrtras).", wi.Id));
                }

                if (intFieldAndPropNotEq(reader["piket"], wi.Pk))
                {
                    errors.Add(string.Format("{0} - neatitinka vietos kodas (piket).", wi.Id));
                }

                if (intFieldAndPropNotEq(reader["metras"], wi.M))
                {
                    errors.Add(string.Format("{0} - neatitinka vietos kodas (metras).", wi.Id));
                }

                if (intFieldAndPropNotEq(reader["siule"], wi.Siule))
                {
                    errors.Add(string.Format("{0} - neatitinka vietos kodas (siule).", wi.Id));
                }

                if (reader["saliginis kodas"].ToString() != wi.SalygKodas) // nereikia tikrinti null,  nes db yra privalomas
                {
                    errors.Add(string.Format("{0} - DB esantis sąlyginis kodas {1} neatitinka siūlomo sąlyginio kodo {2}.",
                        wi.Id, reader["saliginis kodas"], wi.SalygKodas));
                }

                if (wi.KelintasTikrinimas != Kelintas.papildomas)
                {
                    string patDataField = "", formerPatDataField = "";
                    switch (wi.KelintasTikrinimas)
                    {
                        case Kelintas.II:
                            patDataField = "II_pat_data";
                            formerPatDataField = "I_pat_data";
                            break;
                        case Kelintas.III:
                            patDataField = "III_pat_data";
                            formerPatDataField = "II_pat_data";
                            break;
                        case Kelintas.IV:
                            patDataField = "IV_pat_data";
                            formerPatDataField = "III_pat_data";
                            break;
                    }

                    // patDataField turi būti tuščias
                    if (reader[patDataField] != null && reader[patDataField].ToString() != string.Empty)
                    {
                        errors.Add(string.Format("{0} - {1} tikrinimas jau atliktas.", wi.Id, wi.KelintasTikrinimas));
                    }
                    // formerPatDataField turi būti netuščias
                    else if (reader[formerPatDataField] == null || reader[formerPatDataField].ToString() == string.Empty)
                    {
                        errors.Add(string.Format("{0} - neatliktas ankstesnis patikrinimas {1}", wi.Id, formerPatDataField));
                    }
                    // formerPatDataField turi būti ankstesnis už wi.Data
                    else if (Convert.ToDateTime(reader[formerPatDataField]) > wi.TikrinimoData)
                    {
                        errors.Add(string.Format("{0} - ankstesnis patikrinimas {1} atliktas vėliau ({2:d}) negu dabar siūlomas {3} ({4:d}).",
                            wi.Id, formerPatDataField, reader[formerPatDataField], patDataField, wi.TikrinimoData));
                    }
                }
            }
            else
            {
                errors.Add(string.Format("{0} - įrašas nerastas DB", wi.Id));
            }
        }

        private static bool intFieldAndPropNotEq (object field, int? propertyValue)
        {
            // tikrina ar iš duomenų bazės partempto reader laukas ir WeldingInspection objekto property yra nelygūs
            if (field == DBNull.Value) return (propertyValue != null);
            return (Convert.ToInt32(field) != propertyValue);
        } 
    }
}
