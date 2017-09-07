using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public partial class GroupModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Wallpaper { get; set; }
        public Nullable<long> Owner1 { get; set; }
        public Nullable<long> Owner2 { get; set; }
        public Nullable<byte> PrivacyID { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<long> UserID { get; set; }
    }
}
