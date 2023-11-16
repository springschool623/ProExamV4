using ProExam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProExam.Controllers
{
    public class TestingViewModel
    {
        public List<Question> Questions { get; set; }
        public int Score { get; set; }
    }
}