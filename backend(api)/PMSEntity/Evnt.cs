using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSEntity
{
   public class Evnt : Entity
    {
        
        public int EvntId { get; set; }
        public string EvntName { get; set; }
        public Nullable<System.DateTime> EvntSdt { get; set; }
        public Nullable<System.DateTime> EvntEdt { get; set; }
        public string TeacherUserName { get; set; }

      
    }
}
