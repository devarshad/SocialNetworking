using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public class FollwersList
    {
        public long UserID { get; set; }
        public string ConnectionID { get; set; }
        public Nullable<byte> ConnectedBy { get; set; }
        public Nullable<byte> StatusID { get; set; }
    }
}
