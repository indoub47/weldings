using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weldings
{
    internal enum Kelintas { I, II, III, IV, papildomas }
    // TODO: ką daryti su Pak_suv_data
    // TODO: galimus operatorius ir defektoskopus (ir patikrinimus?) surašyti į Settings kaip comma separated things ir parsinti prieš patikrinant
    internal class WeldingInspection
    {
        internal long? Id { private set; get; }
        internal string Linija { private set; get; }
        internal int Kelias { private set; get; }
        internal int Km { private set; get; }
        internal int? Pk { private set; get; }
        internal int? M { private set; get; }
        internal int? Siule { private set; get; }
        internal int? Nr { private set; get; }
        internal string SalygKodas { private set; get; }
        internal string Operatorius { private set; get; }
        internal string Aparatas { private set; get; }
        internal DateTime TikrinimoData { private set; get; }
        internal string Suvirino { private set; get; }
        internal string Ifas { private set; get; }
        internal string Pastaba { private set; get; }
        internal long? DefektoId { private set; get; }
        internal Kelintas KelintasTikrinimas { private set; get; }

        internal WeldingInspection()
        {
            // startinis konstruktorius, įrašantis default reikšmes
            this.Pastaba = "";
        }

        internal WeldingInspection(
            string linija, int kelias, int km, int? pk, int? m, int? siule, string salygKodas,
            string operatorius, string aparatas, DateTime data, 
            string pastaba, long? defektoId,
            Kelintas kelintasTikrinimas):this()
        {
            // minimalus konstruktorius
            this.Id = null;
            this.Linija = linija;
            this.Kelias = kelias;
            this.Km = km;
            this.Pk = pk;
            this.M = m;
            if (this.Kelias == 8 || this.Kelias == 9)
                if (this.M != null)
                    if (this.Siule != null)
                        throw new Exception("Suvirinimas iešme, bet nurodyta siūlė.");
                    else
                    {
                        this.Nr = this.M;
                        this.Siule = null;
                    }
                else
                    throw new Exception("Suvirinimas iešme, bet metrų pozicijoje neįrašytas suvirinimo numeris.");
            else
            {
                this.Nr = null;
                this.Siule = siule;
            }
            this.SalygKodas = salygKodas;
            this.Operatorius = operatorius;
            this.Aparatas = aparatas;
            this.TikrinimoData = data;
            this.Suvirino = null;
            this.Ifas = null;
            this.Pastaba = pastaba;
            this.DefektoId = defektoId;
            this.KelintasTikrinimas = kelintasTikrinimas;
        }

        internal WeldingInspection(
            long id,
            string linija, int kelias, int km, int? pk, int? m, int? siule, string salygKodas, 
            string operatorius, string aparatas, DateTime data,
            bool panaikintas, string pastaba, long? defektoId,
            Kelintas kelintasTikrinimas)
            : this(linija, kelias, km, pk, m, siule, salygKodas, operatorius, aparatas, data, pastaba, defektoId, kelintasTikrinimas)
        {
            // ne pirmas tikrinimas
            if (kelintasTikrinimas == Kelintas.I)
            {
                throw new Exception("Pirmasis tikrinimas įvedamas su ne pirmojo tikrinimo konstruktoriumi.");
            }
            this.Id = id;
            if (kelintasTikrinimas == Kelintas.papildomas)
            {
                this.Pastaba += string.Format(Properties.Settings.Default.PapildomoPastaba, operatorius, aparatas, data);
            }
        }

        internal WeldingInspection(
            string linija, int kelias, int km, int? pk, int? m, int? siule, string salygKodas, 
            string operatorius, string aparatas, DateTime data, 
            string suvirino, string ifas,
            bool panaikintas, string pastaba, long? defektoId)
            : this(linija, kelias, km, pk, m, siule, salygKodas, operatorius, aparatas, data, pastaba, defektoId, Kelintas.I)
        {
            // pirmas
            this.Suvirino = suvirino;
            this.Ifas = ifas;
        }

        internal string VietosKodas
        {
            get
            {
                return string.Format("{0}.{1:0}{2:000}.{3:#00}.{4:#00}.{5:##0}", Linija, Kelias, Km, Pk, M, Siule);
            }
        }
    }


}
