using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weldings
{
    // TODO: sužinoti, kaip nustatyti grąžinamą tipą iš linq ir atskirti grupavimą-filtravimą nuo prezentacijos.
    // Prezentaciją atiduoti klientui (Controller1)
    internal static class RepeatFinder
    {
        internal static StringBuilder FindRepeats(List<WeldingInspection> wiList)
        {
            var gr = wiList.OrderBy(x => x.VietosKodas).GroupBy(x => x.VietosKodas, (key, group) => new
            {
                VietosKodas = key,
                GrByVietosKodas = group.OrderBy(y => y.TikrinimoData).ThenBy(y => y.KelintasTikrinimas)
            }).Where(z => z.GrByVietosKodas.Count() > 1).ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var x in gr)
            {
                sb.AppendLine(x.VietosKodas + ":");
                foreach (var y in x.GrByVietosKodas)
                {
                    sb.AppendLine("\t" + y.TikrinimoData.ToShortDateString() + " - " + y.KelintasTikrinimas.ToString() + " - " + y.Operatorius);
                }
            }

            return sb;
        }
    }
}
