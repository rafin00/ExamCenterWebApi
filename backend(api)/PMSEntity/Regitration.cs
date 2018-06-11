using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PMSEntity
{
   public class Registration : Entity
    {
        
        public int RegistrationId { get; set; }
      
       public int EvntId { get; set; }
        public string StudentUserName { get; set; }
        public int Result { get; set; }
      
        }
}
