using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace PMSEntity
{
    public class User : Entity
    {
        [Key]
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string UserType { get; set; }
        public String Password { get; set; }
    }
}