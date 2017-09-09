using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weldings
{
    public class BGWProgressUpdatedEventArgs:EventArgs
    {
        public int ProgessCount { get; set; }
        public string ProgressInfo { get; set; }
    }
}
