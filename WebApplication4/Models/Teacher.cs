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
    
    public partial class Teacher
    {
        public int teacher_id { get; set; }
        public string teacher_Name { get; set; }
        public string teacher_Speciality { get; set; }
        public string teacher_Gender { get; set; }
        public int teacher_Age { get; set; }
        public int natinal_ID { get; set; }
        public int user_id { get; set; }
    
        public virtual User User { get; set; }
    }
}
