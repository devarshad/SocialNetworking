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
    
    public partial class ConnectedUser
    {
        public long UserID { get; set; }
        public string ConnectionID { get; set; }
        public Nullable<byte> ConnectedBy { get; set; }
        public Nullable<byte> StatusID { get; set; }
        public Nullable<System.DateTime> ConnectedOn { get; set; }
    
        public virtual UserStatu UserStatu { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
    }
}