using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataTypes
{
    public partial class PostModel : BaseModel
    {
        public long PostId { get; set; }

        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }
        public string UserName { get; set; }
        public long PostedBy { get; set; }
        public System.DateTime PostedDate { get; set; }
        public Nullable<int> Privacy { get; set; }
        public string Link { get; set; }
        public string LinkIcon { get; set; }
        public string LinkHeader { get; set; }
        public string LinkDescription { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<byte> PostType { get; set; }
        public string PostDescription { get; set; }
        public bool Liked { get; set; }
        public int TotalLikes { get; set; }
        public List<CommentModel> PostComments { get; set; }
    }
}
