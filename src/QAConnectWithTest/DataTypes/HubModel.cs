using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTypes
{
    /// <summary>
    /// User detail of connected user
    /// </summary>
    public class UserDetail
    {
        /// <summary>
        /// Connection ID of connected user
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// User id of connected user
        /// </summary>
        public long UserID { get; set; }
    }
}