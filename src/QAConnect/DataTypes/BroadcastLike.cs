using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public class BroadcastLike
    {
        public int TotalLikes { get; set; }
        public LikeModel Like { get; set; }
        public NotificationModel Notification { get; set; }
    }
}
