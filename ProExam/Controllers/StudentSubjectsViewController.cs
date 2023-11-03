using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProExam.Models;

namespace ProExam.Controllers
{
    public class StudentSubjectsViewController : Controller
    {
        private ProExamDBEntities7 db = new ProExamDBEntities7();

        // GET: StudentSubjectsView
        public ActionResult Index(string subjectId)
        {
            // Kiểm tra xem subjectId có giá trị không
            if (!string.IsNullOrEmpty(subjectId))
            {
                // Lọc danh sách môn học chỉ hiển thị môn có ID được chọn
                var selectedSubject = db.Subjects.Where(s => s.Subject_ID == subjectId).ToList();

                // Lấy thông tin giáo viên dạy môn học đã chọn
                var teacherInfo = db.Subjects_Teacher
                                    .Where(st => st.Subject_ID == subjectId)
                                    .Join(db.Teachers, st => st.TeacherCode, t => t.TeacherCode, (st, t) => new { TeacherCode = t.TeacherCode, TeacherName = t.Tea_FirstName + t.Tea_LastName })
                                    .FirstOrDefault();

                // Kiểm tra xem giáo viên đã chọn có tồn tại không
                if (teacherInfo != null)
                {
                    // Lưu mã giáo viên vào ViewBag để sử dụng trong View
                    ViewBag.TeacherName = teacherInfo.TeacherName;

                    return View(selectedSubject);
                }
                else
                {
                    // Lưu mã giáo viên vào ViewBag để sử dụng trong View
                    ViewBag.TeacherName = "Chưa có";

                    return View(selectedSubject);
                }
            }

            // Nếu subjectId không có giá trị, trả về toàn bộ danh sách môn học
            return View(db.Subjects.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
