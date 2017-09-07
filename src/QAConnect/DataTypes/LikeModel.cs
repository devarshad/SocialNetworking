using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public partial class LikeModel:BaseModel
    {
        public long LikeID { get; set; }
        public long UserID { get; set; }
        public long ItemID { get; set; }
        public byte ItemLevel { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    }
}
