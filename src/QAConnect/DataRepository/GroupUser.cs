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
    
    public partial class GroupUser
    {
        public long GroupID { get; set; }
        public Nullable<long> UserID { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<byte> StatusID { get; set; }
    
        public virtual FriendshipStatu FriendshipStatu { get; set; }
        public virtual Group Group { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
    }
}