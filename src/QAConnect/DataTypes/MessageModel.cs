using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public partial class MessageModel
    {
        public long ID { get; set; }
        public long SenderID { get; set; }
        public long RecieverID { get; set; }
        public string Text { get; set; }
        public Nullable<bool> IsRead { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> Active { get; set; }
    }
}
