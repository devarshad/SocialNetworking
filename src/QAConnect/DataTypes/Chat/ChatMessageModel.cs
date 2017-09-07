using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataTypes
{
    public class ChatMessageModel : BaseModel
    {
        [Required]
        public long FromID { get; set; }
        public string FromName { get; set; }
        [Required]
        public long ToID { get; set; }
        public string ToName { get; set; }
        public string UserToPictureUrl { get; set; }
        public string UserToName { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        public string ClientGuid { get; set; }
        [Required]
        public Enums.PageType PageType { get; set; }
        public int RoomID { get; set; }
        public long ConversationID { get; set; }
    }
}
