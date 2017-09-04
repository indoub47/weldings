using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weldings
{
    public class BadDataReportCreator
    {
        public static StringBuilder CreatePlainTxt(List<BadData> badDataList)
        {
            StringBuilder sb = new StringBuilder();

            return sb;
        }


        public static StringBuilder CreateString(List<BadData> badDataList)
        {
            IEnumerable<BadData> bdList = badDataList;
            StringBuilder sb = new StringBuilder();

            var groupped = bdList.OrderBy(x1 => x1.Operatorius).GroupBy(xx1 => xx1.Operatorius, (key1, group1) => new
            {
                Operatorius = key1,
                GrByOperatorius = group1.OrderBy(x2 => x2.Sheet).GroupBy(xx2 => xx2.Sheet, (key2, group2) => new
                {
                    Sheet = key2.ToString(),
                    GrBySheet = group2.OrderBy(x3 => x3.Zhyme).GroupBy(xx3 => xx3.Zhyme, (key3, group3) => new
                    {
                        Zhyme = key3,
                        GrByZhyme = group3.OrderBy(x4 => x4.Message)
                    })
                })
            });

            foreach (var a1 in groupped)
            {
                foreach (var a2 in a1.GrByOperatorius)
                {
                    sb.AppendLine("Operatorius: " + a1.Operatorius + ", lentelė: " + a2.Sheet);
                    foreach (var a3 in a2.GrBySheet)
                    {
                        sb.AppendLine("\tįrašas: " + a3.Zhyme);
                        foreach (var a4 in a3.GrByZhyme)
                        {
                            sb.AppendLine("\t\t - " + a4.Message);
                        }
                    }
                }
            }

            return sb;
        }


    }
}
