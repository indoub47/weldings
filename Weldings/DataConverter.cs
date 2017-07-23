using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weldings
{
    internal static class DataConverter
    {
        // iš Google Sheets data formato List<ILIst<Object>> padaro List<WeldingInspection>
        internal static List<WeldingInspection> ConvertPirmieji(List<IList<Object>> data, string[] mapping, string operatorius)
        {
            List<WeldingInspection> tikrinimaiList = new List<WeldingInspection>();
            string ifas = Properties.Settings.Default.Ifas;
            if (data == null || data.Count == 0) return tikrinimaiList;
            foreach (var row in data)
            {
                object value;
                string linija, salKodas, aparatas, suvirino;
                DateTime tikrinimoData;
                int kelias, km;
                int? pk, m, siule;

                string kur = "Operatorius " + operatorius + ", pirmieji tikrinimai: ";

                value = row[Array.IndexOf(mapping, "Linia")];
                if (value == null || value.ToString().Trim() == string.Empty)
                    throw new Exception(kur + "neįrašytas laukas XX.yyyy.yy.yy.y.");
                else
                    linija = value.ToString().Trim();

                value = row[Array.IndexOf(mapping, "Kel")];
                try
                {
                    kelias = Convert.ToInt32(value);
                }
                catch
                {
                    throw new Exception(kur + "neįrašytas arba įrašytas ne skaičiais laukas yy.Xyyy.yy.yy.y.");
                }

                value = row[Array.IndexOf(mapping, "kilomrtras")];
                try
                {
                    km = Convert.ToInt32(value);
                }
                catch
                {
                    throw new Exception(kur + "neįrašytas arba įrašytas ne skaičiais laukas yy.yXXX.yy.yy.y.");
                }

                value = row[Array.IndexOf(mapping, "piket")];
                if (value == null || value.ToString().Trim() == string.Empty)
                    pk = null;
                else
                    try
                    {
                        pk = Convert.ToInt32(value);
                    }
                    catch
                    {
                        throw new Exception(kur + "ne skaičiais įrašytas laukas yy.yyyy.XX.yy.y.");
                    }

                value = row[Array.IndexOf(mapping, "metras")];
                if (value == null || value.ToString().Trim() == string.Empty)
                    m = null;
                else
                    try
                    {
                        m = Convert.ToInt32(value);
                    }
                    catch
                    {
                        throw new Exception(kur + "ne skaičiais įrašytas laukas yy.yyyy.yy.XX.y.");
                    }

                value = row[Array.IndexOf(mapping, "siule")];
                if (value == null || value.ToString().Trim() == string.Empty)
                    siule = null;
                else
                    try
                    {
                        siule = Convert.ToInt32(value);
                    }
                    catch
                    {
                        throw new Exception(kur + "ne skaičiais įrašytas laukas yy.yyyy.yy.yy.X.");
                    }

                value = row[Array.IndexOf(mapping, "saliginis kodas")];
                if (value == null || value.ToString().Trim() == string.Empty)
                    throw new Exception(kur + "nenurodytas salyginis kodas.");
                else
                    salKodas = value.ToString().Trim();

                value = row[Array.IndexOf(mapping, "I_pat_aparat")];
                if (value == null || value.ToString().Trim() == string.Empty)
                    throw new Exception(kur + "nenurodytas defektoskopo kodas.");
                else
                    aparatas = value.ToString().Trim();

                value = row[Array.IndexOf(mapping, "suvirino")];
                if (value == null || value.ToString().Trim() == string.Empty)
                    throw new Exception(kur + "nenurodyta, kas suvirino.");
                else
                    suvirino = value.ToString().Trim();

                value = row[Array.IndexOf(mapping, "I_pat_data")];
                try
                {
                    tikrinimoData = Convert.ToDateTime(value);
                }
                catch
                {
                    throw new Exception(kur + "nenurodyta arba nesuprantama data.");
                }

                WeldingInspection wi = new WeldingInspection(
                        linija, // linija
                        kelias, // kelias
                        km, // km
                        pk, // pk
                        m, // m
                        siule, // siule
                        salKodas, // sąlyginis kodas
                        operatorius, // operatorius
                        aparatas, // aparatas
                        tikrinimoData.Date, // data
                        suvirino, // suvirino
                        ifas, // if
                        false, "", null); // panaikintas, pastaba, defektoId
                tikrinimaiList.Add(wi);
            }
            return tikrinimaiList;
        }

        internal static List<WeldingInspection> ConvertNepirmieji(List<IList<Object>> data, string[] mapping, string operatorius)
        {
            List<WeldingInspection> tikrinimaiList = new List<WeldingInspection>();
            if (data == null || data.Count == 0) return tikrinimaiList;
            foreach (var row in data)
            {
                object value;
                long id;
                string linija, salKodas, aparatas;
                int kelias, km;
                int? pk, m, siule;
                DateTime tikrinimoData;
                Kelintas kelintasTikrinimas;

                string kur = "Operatorius " + operatorius + ", nepirmieji tikrinimai: ";

                value = row[Array.IndexOf(mapping, "number")];
                try
                {
                    id = Convert.ToInt64(value);
                }
                catch
                {
                    throw new Exception(kur + "neįrašytas arba įrašytas ne skaičiais id.");
                }

                value = row[Array.IndexOf(mapping, "Linia")];
                if (value == null || value.ToString().Trim() == string.Empty)
                    throw new Exception(kur + "neįrašytas laukas XX.yyyy.yy.yy.y.");
                else
                    linija = value.ToString().Trim();

                value = row[Array.IndexOf(mapping, "Kel")];
                try
                {
                    kelias = Convert.ToInt32(value);
                }
                catch
                {
                    throw new Exception(kur + "neįrašytas arba įrašytas ne skaičiais laukas yy.Xyyy.yy.yy.y.");
                }

                value = row[Array.IndexOf(mapping, "kilomrtras")];
                try
                {
                    km = Convert.ToInt32(value);
                }
                catch
                {
                    throw new Exception(kur + "neįrašytas arba įrašytas ne skaičiais laukas yy.yXXX.yy.yy.y.");
                }

                value = row[Array.IndexOf(mapping, "piket")];
                if (value == null || value.ToString().Trim() == string.Empty)
                    pk = null;
                else
                    try
                    {
                        pk = Convert.ToInt32(value);
                    }
                    catch
                    {
                        throw new Exception(kur + "ne skaičiais įrašytas laukas yy.yyyy.XX.yy.y.");
                    }

                value = row[Array.IndexOf(mapping, "metras")];
                if (value == null || value.ToString().Trim() == string.Empty)
                    m = null;
                else
                    try
                    {
                        m = Convert.ToInt32(value);
                    }
                    catch
                    {
                        throw new Exception(kur + "ne skaičiais įrašytas laukas yy.yyyy.yy.XX.y.");
                    }

                value = row[Array.IndexOf(mapping, "siule")];
                if (value == null || value.ToString().Trim() == string.Empty)
                    siule = null;
                else
                    try
                    {
                        siule = Convert.ToInt32(value);
                    }
                    catch
                    {
                        throw new Exception(kur + "ne skaičiais įrašytas laukas yy.yyyy.yy.yy.X.");
                    }

                value = row[Array.IndexOf(mapping, "saliginis kodas")];
                if (value == null || value.ToString().Trim() == string.Empty)
                    throw new Exception(kur + "nenurodytas salyginis kodas.");
                else
                    salKodas = value.ToString().Trim();

                value = row[Array.IndexOf(mapping, "tikrinimo_aparatas")];
                if (value == null || value.ToString().Trim() == string.Empty)
                    throw new Exception(kur + "nenurodytas defektoskopo kodas.");
                else
                    aparatas = value.ToString().Trim();

                value = row[Array.IndexOf(mapping, "tikrinimo_data")];
                try
                {
                    tikrinimoData = Convert.ToDateTime(value);
                }
                catch
                {
                    throw new Exception(kur + "nenurodyta arba nesuprantama data.");
                }

                value = row[Array.IndexOf(mapping, "kelintas_tikrinimas")];
                if (value == null || value.ToString().Trim() == string.Empty)
                    throw new Exception(kur + "nenurodyta, kelintas tikrinimas.");
                else
                    kelintasTikrinimas = (Kelintas)Enum.Parse(typeof(Kelintas), value.ToString().Trim());

                WeldingInspection wi = new WeldingInspection(
                        id, // number, id
                        linija, // linija
                        kelias, // kelias
                        km, // km
                        pk, // pk
                        m, // m
                        siule, // siule
                        salKodas, // sąlyginis kodas
                        operatorius, // operatorius
                        aparatas, // aparatas
                        tikrinimoData.Date, // data
                        false, "", null, // panaikintas, pastaba, defektoId
                        kelintasTikrinimas); // kelintas tikrinimas
                tikrinimaiList.Add(wi);
            }
            return tikrinimaiList;
        }

        internal static List<WeldingInspection> ReadPirmiejiCsv(string CsvPath, string[] delims)
        {
            List<WeldingInspection> tikrinimaiList = new List<WeldingInspection>();
            using (var reader = new StreamReader(CsvPath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(delims,StringSplitOptions.None);
                    int kelias, km;
                    int? pk, m, siule;

                    if (values[1] == null || values[1].ToString().Trim() == string.Empty)
                        throw new Exception("Nenurodyta linija pirmuosiuose tikrinimuose");

                    try
                    {
                        kelias = Convert.ToInt32(values[2]);
                        km = Convert.ToInt32(values[3]);
                    }
                    catch
                    {
                        throw new Exception("Pirmuosiuose tikrinimuose kelias arba km neišreikšti skaičiais");
                    }

                    if (values[4] == null || values[4].ToString() == string.Empty) pk = null;
                    else pk = Convert.ToInt32(values[4]);

                    if (values[5] == null || values[5].ToString() == string.Empty) m = null;
                    else m = Convert.ToInt32(values[5]);

                    if (values[6] == null || values[6].ToString() == string.Empty) siule = null;
                    else siule = Convert.ToInt32(values[6]);

                    WeldingInspection wi = new WeldingInspection(
                        values[1], // linija
                        kelias, // kelias
                        km, // km
                        pk, // pk
                        m, // m
                        siule, // siule
                        values[7], // sąlyginis kodas
                        values[15], // operatorius
                        values[14], // aparatas
                        Convert.ToDateTime(values[13]).Date, // data
                        values[9], // suvirino
                        Properties.Settings.Default.Ifas, // if
                        false, "", null); // panaikintas, pastaba, defektoId
                    tikrinimaiList.Add(wi);
                }
            }
            return tikrinimaiList;            
        }

        internal static List<WeldingInspection> ReadNepirmiejiCsv(string CsvPath, string[] delims)
        {
            List<WeldingInspection> tikrinimaiList = new List<WeldingInspection>();
            using (var reader = new StreamReader(CsvPath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(delims, StringSplitOptions.None);
                    int kelias, km;
                    int? pk, m, siule;

                    if (values[1] == null || values[1].ToString().Trim() == string.Empty)
                        throw new Exception("Nenurodyta linija nepirmuosiuose tikrinimuose");

                    try
                    {
                        kelias = Convert.ToInt32(values[2]);
                        km = Convert.ToInt32(values[3]);
                    }
                    catch
                    {
                        throw new Exception("Nepirmuosiuose tikrinimuose kelias arba km neišreikšti skaičiais");
                    }

                    if (values[4] == null || values[4].ToString() == string.Empty) pk = null;
                    else pk = Convert.ToInt32(values[4]);

                    if (values[5] == null || values[5].ToString() == string.Empty) m = null;
                    else m = Convert.ToInt32(values[5]);

                    if (values[6] == null || values[6].ToString() == string.Empty) siule = null;
                    else siule = Convert.ToInt32(values[6]);

                    WeldingInspection wi = new WeldingInspection(
                       Convert.ToInt64(values[0]),
                       values[1], // linija
                       kelias, // kelias
                       km, // km
                       pk, // pk
                       m, // m
                       siule, // siule
                       values[7], // sąlyginis kodas
                       values[14], // operatorius
                       values[13], // aparatas
                       Convert.ToDateTime(values[12]).Date, // data
                       false, "", null, // panaikintas, pastaba, defektoId
                       (Kelintas)Enum.Parse(typeof(Kelintas), values[15])); // kelintas
                    tikrinimaiList.Add(wi);
                }
            }
            return tikrinimaiList;
        }
    }
}
