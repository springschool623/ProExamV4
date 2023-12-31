//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProExam.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Teacher
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Teacher()
        {
            this.Subjects_Teacher = new HashSet<Subjects_Teacher>();
        }

        public string TeacherCode { get; set; }
        public string Tea_FirstName { get; set; }
        public string Tea_LastName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Tea_DOB { get; set; }
        public string Tea_Gender { get; set; }
        public string Tea_Email { get; set; }
        public string Tea_PhoneNumber { get; set; }
        public string Tea_Address { get; set; }
        public string Tea_Password { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Subjects_Teacher> Subjects_Teacher { get; set; }
    }
}
