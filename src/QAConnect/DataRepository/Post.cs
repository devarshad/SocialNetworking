//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataRepository
{
    using System;
    using System.Collections.Generic;
    
    public partial class Post
    {
        public Post()
        {
            this.Comments = new HashSet<Comment>();
        }
    
        public long ID { get; set; }
        public string Text { get; set; }
        public long UserID { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> Privacy { get; set; }
        public string Link { get; set; }
        public string LinkIcon { get; set; }
        public string LinkHeader { get; set; }
        public string LinkDescription { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<byte> TypeID { get; set; }
        public string PostDescription { get; set; }
    
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual PostType PostType { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
