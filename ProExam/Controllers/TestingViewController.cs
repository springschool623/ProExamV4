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
    public class TestingViewController : Controller
    {
        private ProExamDBEntities7 db = new ProExamDBEntities7();

        // GET: TestingView
        public ActionResult Index(string subjectId)
        {
            // Kiểm tra xem subjectId có giá trị không
            if (!string.IsNullOrEmpty(subjectId))
            {
                // Lọc danh sách môn học chỉ hiển thị môn có ID được chọn
                var selectedQuestion = db.Questions.Where(s => s.Subject_id == subjectId).ToList();


                return View(selectedQuestion);
            }

            // Nếu subjectId không có giá trị, trả về toàn bộ danh sách môn học
            return View(db.Questions.ToList());
        }

        [HttpPost]
        public ActionResult SubmitAnswers(FormCollection form)
        {
            return RedirectToAction("Index", "TestingView"); // hoặc chuyển hướng đến một view khác nếu cần
        }
    }
}
