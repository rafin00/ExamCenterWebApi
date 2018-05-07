using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMSInterface;
using PMSEntity;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PMSRepository
{
    public class RegistrationRepository : Repository<Registration>, IRegistrationRepository
    {
        string connStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
      PMSDBContext context = new PMSDBContext();
      public List<Registration> Search(string key)
      {
         
            throw new NotImplementedException();
        }
      public void AssignmentAllStudents(int evntid)
      {
          
      }
      public bool CheckRegistration(int evntid, string username)
      {
          List<Registration> regis =context.Registrations.Where(reg => reg.StudentUserName == username && reg.EvntId == evntid).ToList();

          if (regis.Count == 0)
              return false;
           else return true;
      }
      public void AddRemoveRegistration(int evntid, string username)
      {
          List<Registration> regis = context.Registrations.Where(reg => reg.StudentUserName == username && reg.EvntId == evntid).ToList();
          if (regis.Count == 0)
          {
              Registration reg = new Registration();
              reg.EvntId = evntid;
              reg.Result = -1;
              reg.StudentUserName = username;
              context.Registrations.Add(reg);
              context.SaveChanges();
          }
          else
          {
              context.Registrations.Remove(regis[0]);
              context.SaveChanges();
          }
      }

      public void calculateResult(int evntid, string username)
      {

          List<Answer> ans = context.Answers.Where(an => an.EvntId == evntid && an.StudentUserName == username).ToList();
          int result = 0, total = ans.Count; float per = 0;
          IQuestionRepository qrepo = new QuestionRepository();
          foreach (Answer an in ans)
          {
              if(qrepo.Get(an.QuestionId).Answer==an.MyAnswer)
              result++;
              
          }

          Registration res = new Registration();

          res = context.Registrations.Where(reg => reg.StudentUserName == username && reg.EvntId == evntid).ToList()[0];
          res.Result = result;
         
          context.SaveChanges();


          List<Exam> exm = context.Exams.Where(ex => ex.EvntId == evntid).ToList();
          List<Exam> totcourse = new List<Exam>();

          foreach (Exam ex in exm)
          {
              if (totcourse.SingleOrDefault(re => re.CourseName == ex.CourseName) == null)
                  totcourse.Add(ex);
          }
          foreach (Exam ex in totcourse)
          {
              List<Answer> answer = ans.FindAll(a=>a.CourseName==ex.CourseName);
              double totalques = answer.Count; double corr = 0;
              foreach (Answer oans in answer)
              {
                  if (qrepo.Get(oans.QuestionId).Answer == oans.MyAnswer)
                      corr++;
              }
              Result resta = new Result();
              resta.CourseName = ex.CourseName; resta.EvntId = ex.EvntId; resta.Mark = (corr / totalques) * 100; resta.StudentUserName = username;
              context.Results.Add(resta);
              context.SaveChanges();
          }

        
      }

      public List<Registration> GetResult(string username)
      {
          return context.Registrations.Where(reg => reg.StudentUserName == username && reg.Result != -1).ToList();
      }
      public List<Registration> GetRegistrationsByEvnts(int evntid)
      {
         return context.Registrations.Where(reg => reg.EvntId == evntid && reg.Result!=(-1)).ToList();
      }
      public Registration GetEvntResult(string username, int evntid)
      {
          return context.Registrations.SingleOrDefault(reg=>reg.StudentUserName==username && reg.EvntId==evntid && reg.Result != -1);
      }
      public void AssignAll(int EvntId)
      {
          List<User> students = context.Users.ToList().FindAll(us => us.UserType == "Student");
          foreach (User std in students)
          {
              if (context.Registrations.SingleOrDefault(reg => reg.EvntId == EvntId && reg.StudentUserName == std.UserName) == null)
              {
                  Registration reg= new Registration();
                  reg.EvntId=EvntId; reg.Result=-1; reg.StudentUserName=std.UserName;
                  context.Set<Registration>().Add(reg);
                 
                  context.SaveChanges(); reg = new Registration();
              }
          }
      }
      public void UnAssignAll(int EvntId)
      {
          List<User> students = context.Users.ToList().FindAll(us => us.UserType == "Student");
          foreach (User std in students)
          {
              Registration regis =context.Registrations.SingleOrDefault(reg => reg.EvntId == EvntId && reg.StudentUserName == std.UserName);
              if (regis == null)
              {
                  
              }
              else
              {
                  context.Set<Registration>().Remove(regis);
                  context.SaveChanges();
              }
          }
      }
    }
}
