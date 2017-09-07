using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataTypes
{
    public class FriendRequestModel : BaseModel
    {
        public long ID { get; set; }
        [Required]
        public long UserID { get; set; }
        [Required]
        public long PageID { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public Enums.PageType PageType { get; set; }
        public Enums.FriendshipStatus PageRelationStatus { get; set; }

        public string UserInfo { get; set; }

        public bool? IsRead { get; set; }
    }
}
