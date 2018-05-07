using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace PMSEntity
{
   public class Course : Entity
    {
        [Key]
        public string CourseName { get; set; }
        
        public string TeacherUserName { get; set; }
        
    }
}
