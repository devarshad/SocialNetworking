using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public partial class NotificationModel : BaseModel
    {
        public long ID { get; set; }
        public Nullable<byte> TypeID { get; set; }
        public long UserID { get; set; }
        public long ItemID { get; set; }
        public string Message { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> IsRead { get; set; }
        public Nullable<bool> Active { get; set; }
    }
}
