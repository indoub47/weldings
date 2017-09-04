using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weldings
{
    public enum Kelintas { I, II, III, IV, papildomas }
    // TODO: ką daryti su Pak_suv_data
    // TODO: galimus operatorius ir defektoskopus (ir patikrinimus?) surašyti į Settings kaip comma separated things ir parsinti prieš patikrinant
    public class WeldingInspection
    {
        public long? Id { private set; get; }
        public string Linija { private set; get; }
        public int Kelias { private set; get; }
        public int Km { private set; get; }
        public int? Pk { private set; get; }
        public int M { private set; get; }
        public int? Siule { private set; get; }
        public int? Nr { private set; get; }
        public string SalygKodas { private set; get; }
        public string Operatorius { private set; get; }
        public string Aparatas { private set; get; }
        public DateTime TikrinimoData { private set; get; }
        public string Suvirino { private set; get; }
        public Kelintas KelintasTikrinimas { private set; get; }
        public string Pastaba { private set; get; }

        public WeldingInspection(
            long? id,
            string linija, int kelias, int km, int? pk, int m, int? siule, string salygKodas,
            string operatorius, string aparatas, DateTime data, string suvirino,
            Kelintas kelintasTikrinimas, string pastaba = "")
        {
            this.Id = id;
            this.Linija = linija;
            this.Kelias = kelias;
            this.Km = km;
            this.Pk = pk;
            this.M = m;
            this.Siule = siule;
            if (this.Kelias == 8 || this.Kelias == 9)
            {
                this.Nr = this.M;
            }
            else
            {
                this.Nr = null;
            }
            this.SalygKodas = salygKodas;
            this.Operatorius = operatorius;
            this.Aparatas = aparatas;
            this.TikrinimoData = data;
            this.Suvirino = suvirino;
            this.KelintasTikrinimas = kelintasTikrinimas;
            this.Pastaba = pastaba;
        }

        public string VietosKodas
        {
            get
            {
                return string.Format("{0}.{1:0}{2:000}.{3:#00}.{4:#00}.{5:##0}", Linija, Kelias, Km, Pk, M, Siule);
            }
        }

        public bool SameVietaAs(WeldingInspection other)
        {
            return Linija == other.Linija &&
                Kelias == other.Kelias &&
                Km == other.Km &&
                Pk == other.Pk &&
                M == other.M &&
                Siule == other.Siule;
        }
    }


}
