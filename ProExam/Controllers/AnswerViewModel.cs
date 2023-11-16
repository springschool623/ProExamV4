using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProExam.Controllers
{
    public class AnswerViewModel
    {
        public int QuestionId { get; set; }
        public string SelectedAnswer { get; set; }
    }
}