using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataTypes
{
    public abstract class BaseModel
    {
        public string PostedByName { get; set; }
        public string PostedByAvatar { get; set; }
        public string UserInfo { get; set; }
        public Enums.BroadCastType broadCastType;
    }
}
