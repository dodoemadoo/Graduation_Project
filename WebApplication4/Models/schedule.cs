//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication4.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class schedule
    {
        public int SS_ID { get; set; }
        public string week_Day { get; set; }
        public Nullable<int> slot_ID { get; set; }
        public Nullable<int> class_ID { get; set; }
        public Nullable<int> teacher_subject_ID { get; set; }
        public string semester { get; set; }
    
        public virtual Slot Slot { get; set; }
        public virtual T_S T_S { get; set; }
    }
}
