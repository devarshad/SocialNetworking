using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public partial class CommentModel : BaseModel
    {
        public long CommentId { get; set; }
        public long PostId { get; set; }

        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }
        public long UserID { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<bool> Active { get; set; }
        public bool Liked { get; set; }
        public int TotalLikes { get; set; }
    }
}
