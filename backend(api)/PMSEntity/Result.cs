using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSEntity
{
   public class Result : Entity
    {
        
        public int ResultId { get; set; }
        public int EvntId { get; set; }
        public string StudentUserName { get; set; }
        public double Mark { get; set; }
        public string CourseName { get; set; }
        }
}
