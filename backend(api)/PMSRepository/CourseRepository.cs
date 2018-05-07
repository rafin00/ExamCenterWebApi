using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMSInterface;
using PMSEntity;
namespace PMSRepository
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
      PMSDBContext context = new PMSDBContext();
      public List<Course> Search(string key)
      {
         
            throw new NotImplementedException();
        }
        
    }
}
