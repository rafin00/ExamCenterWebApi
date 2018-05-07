using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMSInterface;
using PMSEntity;
namespace PMSRepository
{
    public class ExamRepository : Repository<Exam>, IExamRepository
    {
      PMSDBContext context = new PMSDBContext();
      ICourseRepository crsrepo = new CourseRepository();
      public List<Exam> Search(string key)
      {
         
            throw new NotImplementedException();
        }
      public List<Exam> GetCourses(int evntid)
      {
          List<Exam> exm = context.Exams.Where(ex => ex.EvntId == evntid).ToList();
          List<Exam> res = new List<Exam>();
          
          foreach (Exam ex in exm)
          {
              if (res.SingleOrDefault(re => re.CourseName == ex.CourseName) == null)
                  res.Add(ex);
          }
          return res;
      }
      public void DeleteEvntQuestion(int evntid, int questionid)
      {
          context.Exams.Remove(context.Exams.SingleOrDefault(ex => ex.QuestionId == questionid && ex.EvntId == evntid));
          context.SaveChanges();
          context.Answers.RemoveRange(context.Answers.ToList().FindAll(ex => ex.QuestionId == questionid && ex.EvntId == evntid));
          context.SaveChanges();
      }
      public void DeleteEvntCourse(int evntid, string coursename)
      {
          context.Exams.RemoveRange(context.Exams.ToList().FindAll(ex => ex.CourseName == coursename && ex.EvntId == evntid));
          context.SaveChanges();
          context.Answers.RemoveRange(context.Answers.ToList().FindAll(ex => ex.CourseName == coursename && ex.EvntId == evntid));
          context.SaveChanges();
          
      }
      public Exam GetEvntQuestion(int evntid, int questionid)
      {
          Exam exm = context.Exams.SingleOrDefault(ex => ex.EvntId == evntid && ex.QuestionId == questionid);
          if (exm == null)
          {
              return exm;
          }
          return exm;
      }
      public void DeleteExamQuestion(Exam ex)
      {
          context.Exams.Remove(ex);
          context.SaveChanges();
      }
      public bool CheckIdenticalQuestion(Question ques,int evntid)
      {
          if (context.Exams.SingleOrDefault(ex => ex.CourseName == ques.CourseName && ex.EvntId == evntid && ex.QuestionId == ques.QuestionId) == null)
              return false;
          else return true;
        }

      public List<Exam> GetByEvnt(string username, int evntid)
      {

          return context.Exams.ToList().FindAll(ex => ex.EvntId == evntid);
      }
    }
}
