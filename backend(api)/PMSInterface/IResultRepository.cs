using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMSEntity;

namespace PMSInterface
{
    public interface IResultRepository : IRepository<Result>
    {
        List<Result> Search(string key);
        List<Result> GetStudentAverageMarksByCourse(string StudentUserName);
        List<Result> GetTeacherAverageMarksByCourse(string StudentUserName);
        }
}
