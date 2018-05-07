using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace PMSEntity
{
   public class Answer : Entity
    {
        
        public int AnswerId { get; set; }
        public int EvntId { get; set; }
        public int QuestionId { get; set; }
        public string MyAnswer { get; set; }
        public string StudentUserName { get; set; }
        public string CourseName { get; set; }
    }
}
