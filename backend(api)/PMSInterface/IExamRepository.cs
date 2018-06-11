using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMSEntity;

namespace PMSInterface
{
    public interface IExamRepository : IRepository<Exam>
    {
        List<Exam> Search(string key);
        List<Exam> GetCourses(int evntid);
        void DeleteEvntQuestion(int evntid, int questionid);
        void DeleteEvntCourse(int evntid, string coursename);
        Exam GetEvntQuestion(int evntid, int questionid);
      void  DeleteExamQuestion(Exam ex);
      bool CheckIdenticalQuestion(Question ques, int evntid); List<Exam> GetByEvnt(string username, int evntid);
    }
}
