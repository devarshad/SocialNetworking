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
    
    public partial class FriendshipStatu
    {
        public FriendshipStatu()
        {
            this.EventUsers = new HashSet<EventUser>();
            this.GroupUsers = new HashSet<GroupUser>();
            this.Friendships = new HashSet<Friendship>();
        }
    
        public byte ID { get; set; }
        public string Name { get; set; }
        public Nullable<bool> Active { get; set; }
    
        public virtual ICollection<EventUser> EventUsers { get; set; }
        public virtual ICollection<GroupUser> GroupUsers { get; set; }
        public virtual ICollection<Friendship> Friendships { get; set; }
    }
}
