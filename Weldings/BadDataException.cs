using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Weldings
{
    [Serializable]
    class BadDataException:Exception
    {
        private readonly IList<BadData> badDataList;

        public BadDataException()
        {
        }

        public BadDataException(string message)
            : base(message)
        {
        }

        public BadDataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public BadDataException(string message, IList<BadData> lsBadData)
            : base(message)
        {
            this.badDataList = lsBadData;
        }

        public BadDataException(string message, IList<BadData> lsBadData, Exception innerException)
            : base(message, innerException)
        {
            this.badDataList = lsBadData;
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        // Constructor should be protected for unsealed classes, private for sealed classes.
        // (The Serializer invokes this constructor through reflection, so it can be private)
        protected BadDataException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.badDataList = (IList<BadData>)info.GetValue("bdl", typeof(IList<BadData>));
        }

        public IList<BadData> BadDataList
        {
            get { return this.badDataList; }
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            // Note: if "List<T>" isn't serializable you may need to work out another
            //       method of adding your list, this is just for show...
            info.AddValue("bdl", this.BadDataList, typeof(IList<BadData>));

            // MUST call through to the base class to let it save its own state
            base.GetObjectData(info, context);
        }
    }
}