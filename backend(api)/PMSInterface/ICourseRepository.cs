using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMSEntity;

namespace PMSInterface
{
    public interface ICourseRepository : IRepository<Course>
    {
        List<Course> Search(string key);
    }
}
