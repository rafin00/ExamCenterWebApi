using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSEntity
{
   public class Question : Entity
    {
        
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string Answer { get; set; }
        public String CourseName { get; set; }
        public String TeacherUserName { get; set; }
    }
}
