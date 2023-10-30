using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProExam.ViewModels
{
    public class StudentSubjectTeacherViewModel
    {
        public IEnumerable<ProExam.Models.Subjects_Student> SubjectsStudents { get; set; }
        public IEnumerable<ProExam.Models.Subjects_Teacher> SubjectsTeachers { get; set; }
    }
}