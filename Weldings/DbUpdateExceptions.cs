using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;

namespace Weldings
{
    [Serializable]
    public class DbUpdateException : Exception
    {
        private readonly bool rollbackSuccess;
        private readonly SheetType sheetType;

        public DbUpdateException(): 
            base()
        {
        }

        public DbUpdateException(string message): 
            base(message)
        {
        }

        public DbUpdateException(string message, bool rollbackSuccess, SheetType sheetType) :
            base(message)
        {
            this.rollbackSuccess = rollbackSuccess;
            this.sheetType = sheetType;
        }

        public DbUpdateException(string message, Exception innerException): 
            base(message, innerException)
        {
        }

        public DbUpdateException(string message, Exception innerException, bool rollbackSuccess, SheetType sheetType) :
            base(message, innerException)
        {
            this.rollbackSuccess = rollbackSuccess;
            this.sheetType = sheetType;
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        // Constructor should be protected for unsealed classes, private for sealed classes.
        // (The Serializer invokes this constructor through reflection, so it can be private)
        protected DbUpdateException(SerializationInfo info, StreamingContext context): 
            base(info, context)
        {
            this.rollbackSuccess = info.GetBoolean("rbs");
            this.sheetType = (SheetType)info.GetInt32("st");
        }

        public bool RollbackSuccess
        {
            get { return this.rollbackSuccess; }
        }

        public SheetType SheetType
        {
            get { return this.sheetType; }
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
            info.AddValue("rbs", this.RollbackSuccess, typeof(bool));
            info.AddValue("st", (Int32)this.SheetType, typeof(Int32));

            // MUST call through to the base class to let it save its own state
            base.GetObjectData(info, context);
        }
    }
}
