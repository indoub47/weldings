using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weldings
{
    internal static class ReportCreator
    {
        internal static StringBuilder CreateTxt(List<WeldingInspection>wiListPirmieji, List<WeldingInspection>wiListNepirmieji)
        {
            IEnumerable<WeldingInspection> wiList = wiListPirmieji.Concat(wiListNepirmieji);
            StringBuilder sb = new StringBuilder(DateTime.Now.ToShortDateString() + ", " + DateTime.Now.ToShortTimeString()).AppendLine();

            var groupped = wiList.OrderBy(x1 => x1.TikrinimoData).GroupBy(xx1 => xx1.TikrinimoData.Date, (key1, group1) => new
            {
                TikrinimoData = key1.Date,
                GrByTikrinimoData = group1.OrderBy(x2 => x2.Aparatas).GroupBy(xx2 => xx2.Aparatas, (key2, group2) => new
                {
                    Aparatas = key2,
                    GrByAparatas = group2.OrderBy(x3 => x3.Operatorius).GroupBy(xx3 => xx3.Operatorius, (key3, group3) => new
                    {
                        Operatorius = key3,
                        GrByOperatorius = group3.OrderBy(x4 => x4.SalygKodas).GroupBy(xx4 => xx4.SalygKodas, (key4, group4) => new
                        {
                            SalygKodas = key4,
                            GrBySalygKodas = group4.OrderBy(x5 => x5.KelintasTikrinimas).GroupBy(xx5 => xx5.KelintasTikrinimas, (key5, group5) => new
                            {
                                KelintasTikrinimas = key5,
                                GrByKelintasTikrinimas = group5.OrderBy(x6 => x6.VietosKodas)
                            })
                        })
                    })
                })
            });

            foreach (var a1 in groupped)
            {
                sb.AppendLine(a1.TikrinimoData.ToShortDateString());
                foreach (var a2 in a1.GrByTikrinimoData)
                {
                    sb.AppendLine("\t" + a2.Aparatas);
                    foreach (var a3 in a2.GrByAparatas)
                    {
                        sb.AppendLine("\t\t" + a3.Operatorius);
                        foreach (var a4 in a3.GrByOperatorius)
                        { 
                            foreach (var a5 in a4.GrBySalygKodas)
                            {
                                foreach (var a6 in a5.GrByKelintasTikrinimas)
                                {
                                    sb.AppendLine("\t\t\t" + a6.VietosKodas + " - " + a4.SalygKodas + " - " + a5.KelintasTikrinimas);
                                }
                            }
                        }
                    }
                }
                sb.AppendLine();
            }
            return sb;
        }
    }
}
