using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weldings
{
    internal static class RepeatFinder
    {

        internal class Repeats
        {
            internal class Rep
            {
                internal string OperatorId;
                internal int Times;

                internal Rep(string operatorId, int times)
                {
                    OperatorId = operatorId;
                    Times = times;
                }
            }

            internal string VietosKodas;
            internal List<Rep> RepList;

            internal Repeats (string vietosKodas, Rep rep)
            {
                VietosKodas = vietosKodas;
                RepList = new List<Rep>();
                RepList.Add(rep);
            }
        }

        internal static List<Repeats> FindRepeats(List<WeldingInspection> wiList)
        {
            List<WeldingInspection> testedList = new List<WeldingInspection>();
            List<Repeats> foundRepeats = new List<Repeats>();
            for (int i = 0; i < wiList.Count; i++)
            {
                WeldingInspection wi = wiList[i];
                if (!wasTested(wi, testedList))
                {
                    testedList.Add(wi);
                    Repeats repeats = getRepeats(wi, wiList, i + 1);
                    if (repeats.RepList.Count > 0)
                    {
                        foundRepeats.Add(repeats);
                    }
                }
            }
            return foundRepeats;
        }

        private static Repeats getRepeats(WeldingInspection wi, List<WeldingInspection> wiList, int fromIndex)
        {
            Repeats repeats = new Repeats(wi.VietosKodas, new Repeats.Rep(wi.Operatorius, 1));
            for (int i = fromIndex; i < wiList.Count; i++)
            {
                WeldingInspection currWi = wiList[i];
                if (wi.SameVietaAs(currWi))
                {
                    int ind = repeats.RepList.FindIndex(item => item.OperatorId == currWi.Operatorius);
                    if (ind < 0)
                    {
                        repeats.RepList.Add(new Repeats.Rep(currWi.Operatorius, 1));
                    }
                    else
                    {
                        repeats.RepList[ind].Times++;
                    }
                }
            }

            if (repeats.RepList.Count == 1 && repeats.RepList[0].Times == 1)
            {
                repeats.RepList = new List<Repeats.Rep>();
            }
            return repeats;
        }

        private static bool wasTested(WeldingInspection wi, List<WeldingInspection> testedList)
        {
            foreach (WeldingInspection t in testedList)
            {
                if (wi.SameVietaAs(t)) return true;
            }
            return false;
        }
    }
}
