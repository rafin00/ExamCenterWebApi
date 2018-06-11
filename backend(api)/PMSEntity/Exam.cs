using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSEntity
{
   public class Exam : Entity
    {
        
        public int ExamId { get; set; }
        public int EvntId { get; set; }
        public int QuestionId { get; set; }
        public string CourseName { get; set; }
    }
}
