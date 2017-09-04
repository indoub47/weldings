using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Weldings
{
    [Serializable]
    public class BadData:ISerializable
    {
        public string Operatorius { get; set; }
        public SheetType Sheet { get; set; }
        public string Zhyme { get; set; }
        public string Message { get; set; }  

        public BadData()
        {

        }

        public BadData(string operatorius, SheetType sheet, string zhyme = null, string message = null)
        {
            Operatorius = operatorius;
            Sheet = sheet;
            Zhyme = zhyme;
            Message = message;
        }

        protected BadData(SerializationInfo sinfo, StreamingContext ctx)
        {
            Operatorius = sinfo.GetString("o");
            Sheet = (SheetType)sinfo.GetInt32("s");
            Zhyme = sinfo.GetString("z");
            Message = sinfo.GetString("m");
        }

        public virtual void GetObjectData(SerializationInfo sinfo, StreamingContext ctx)
        {
            sinfo.AddValue("o", this.Operatorius);
            sinfo.AddValue("s", (Int32)this.Sheet);
            sinfo.AddValue("z", this.Zhyme);
            sinfo.AddValue("m", this.Message);
        }
    }
}
