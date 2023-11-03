using ProExam.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ProExam.Controllers
{
    public class HomeController : Controller
    {
        private ProExamDBEntities7 db = new ProExamDBEntities7();

        // GET: Home
        public ActionResult Home()
        {
            // Lấy mã sinh viên từ Session
            string sessionUserCode = Session["UserCode"] as string;

                // Truy vấn cơ sở dữ liệu để kiểm tra xem có tồn tại Subject_ID cho mã sinh viên này hay không
                var studentSubjects = db.Subjects_Student.Where(s => s.StudentCode == sessionUserCode).Select(s => s.Subject_ID).ToList();
                // Lưu danh sách Subject_ID vào Session
                Session["UserSubjects"] = studentSubjects;


                return View(db.Subjects.ToList());
        }

        public ActionResult AboutPage()
        {
            return View();
        }

        public ActionResult FacilityPage()
        {
            return View();
        }

        public ActionResult ContactPage()
        {
            return View();
        }

        public ActionResult Personal()
        {
            // Get the user code from the session variable
            string userCode = Session["UserCode"] as string;

            // Use the user code to retrieve the user's information from the database
            var users = db.Students.Where(u => u.StudentCode == userCode).ToList();

            return View(users);
        }

        public ActionResult TestSchedulesView(string subjectId)
        {
            // Get the user code from the session variable
            string userCode = Session["UserCode"] as string;

            if (userCode == null)
            {
                return View(db.TestSchedules.ToList());
            }
            else
            {
                if (subjectId == null)
                {
                    // Get the subjects for the current user
                    var subjects = db.Subjects_Student.Where(s => s.StudentCode == userCode).Select(s => s.Subject_ID);

                    // Get the test schedules for the subjects that the user has registered for
                    var testSchedules = db.TestSchedules.Where(ts => subjects.Contains(ts.Subject_ID)).ToList();

                    // Pass the testSchedules to the view
                    return View(testSchedules);
                }

                // Lọc danh sách môn học chỉ hiển thị môn có ID được chọn
                var selectedSubject = db.Subjects.SingleOrDefault(s => s.Subject_ID == subjectId);

                if (selectedSubject != null)
                {
                    // Get the test schedules for the selected subject
                    var testSchedules = db.TestSchedules.Where(ts => ts.Subject_ID == subjectId).ToList();

                    // Pass the testSchedules to the view
                    return View(testSchedules);
                }
            }
            return RedirectToAction("Home", "Home");

        }

        public ActionResult LoginSection()
        {
            return View();
        }

        public ActionResult US_Subject()
        {
            // Get the user code from the session variable
            string userCode = Session["UserCode"] as string;

            // If the user code is null, redirect to the login page
            if (userCode == null)
            {
                return RedirectToAction("Home", "Home");
            }

            // Get the subjects for the current user
            var subjects = db.Subjects_Student.Where(s => s.StudentCode == userCode).ToList();

            // Return the view
            return View(subjects);
        }

        [HttpPost]
        public ActionResult Enroll(string subjectId)
        {
            // Lấy mã sinh viên từ Session
            string sessionUserCode = Session["UserCode"] as string;

            // Kiểm tra xem sessionUserCode và subjectId có giá trị không
            if (!string.IsNullOrEmpty(sessionUserCode) )
            {
                if(!string.IsNullOrEmpty(subjectId))
                {
                    // Lọc danh sách môn học chỉ hiển thị môn có ID được chọn
                    var selectedSubject = db.Subjects.SingleOrDefault(s => s.Subject_ID == subjectId);

                    if (selectedSubject.Stu_Quantity != selectedSubject.Sub_Max_Quantity)
                    {
                        // Tìm giá trị lớn nhất hiện tại của Sub_Stu_No trong bảng
                        int maxSubStuNo = db.Subjects_Student.Max(s => (int?)s.Subs_Stu_No) ?? 0;

                        // Tạo đối tượng Subjects_Student với Sub_Stu_No tăng dần
                        Subjects_Student enrollment = new Subjects_Student
                        {
                            Subs_Stu_No = maxSubStuNo + 1, // Tăng giá trị lớn nhất hiện tại
                            StudentCode = sessionUserCode,
                            Subject_ID = subjectId
                        };

                        // Thực hiện thêm vào cơ sở dữ liệu (sử dụng db.SaveChanges())
                        db.Subjects_Student.Add(enrollment);
                        db.SaveChanges();

                        // Call the CalculateStuQuantity method after saving the student
                        DatabaseHelper dbHelper = new DatabaseHelper();
                        dbHelper.CalculateStuQuantity();

                        return RedirectToAction("Home", "Home");
                    }
                }

            }

            // Chuyển hướng hoặc trả về view tùy thuộc vào logic của bạn
            return RedirectToAction("LoginSection", "Home");
        }

        // GET: Students/Edit/5
        public ActionResult PersonalInfoEdit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersonalInfoEdit([Bind(Include = "StudentCode,Stu_FirstName,Stu_LastName,Stu_DOB,Stu_Gender,Stu_Email,Stu_PhoneNumber,Stu_Address,Stu_Password")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Personal");
            }
            return View(student);
        }


        [HttpPost]
        public ActionResult Login(LoginInfo loginInfo)
        {
            // Check if the loginInfo is for a Student
            var isStudent = db.Students.Any(u => u.StudentCode == loginInfo.UserCode && u.Stu_Password == loginInfo.Password);

            // Check if the loginInfo is for a Teacher
            var isTeacher = db.Teachers.Any(u => u.TeacherCode == loginInfo.UserCode && u.Tea_Password == loginInfo.Password);


            if (isStudent)
            {
                // Set the "UserCode" session variable to the user code
                Session["UserCode"] = loginInfo.UserCode;

                var student = db.Students.FirstOrDefault(u => u.StudentCode == loginInfo.UserCode);
                Session["UserName"] =  student.Stu_LastName;

                return RedirectToAction("Home", "Home");
            }
            else if (isTeacher)
            {
                // Set the "UserCode" session variable to the user code
                Session["UserCode"] = loginInfo.UserCode;

                var teacher = db.Teachers.FirstOrDefault(u => u.TeacherCode == loginInfo.UserCode);
                Session["UserName"] =  teacher.Tea_LastName;

                return RedirectToAction("Home", "Home");
            }
            else if (loginInfo.UserCode == "admin" && loginInfo.Password == "admin12345")
            {
                // Set the "UserCode" session variable to "admin" for admin
                Session["UserCode"] = "admin";

                return RedirectToAction("ADHomePage", "AdminDashboard");
            }
            else
            {
                // Invalid login, return to the login page with an error message
                ModelState.AddModelError("", "Invalid student ID or password");

                // Return the loginInfo object to repopulate the form with user-entered values
                return View("Home", loginInfo);
            }
        }


        public ActionResult Logout()
        {
            // Clear session
            Session.Clear();

            // Redirect to the login page
            return RedirectToAction("Home", "Home");
        }
    }
}