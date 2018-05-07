using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMSInterface;
using PMSEntity;
namespace PMSRepository
{
    public class ResultRepository : Repository<Result>, IResultRepository
    {
      PMSDBContext context = new PMSDBContext();
      public List<Result> Search(string key)
      {
         
            throw new NotImplementedException();
        }
      public List<Result> GetStudentAverageMarksByCourse(string StudentUserName)
      {
          List<Result> totresults = context.Results.Where(res => res.StudentUserName == StudentUserName).ToList();
          List<Result> crsresult = new List<Result>();
          foreach (Result res in totresults)
          {
              if (crsresult.SingleOrDefault(r => r.CourseName == res.CourseName) == null)
              {
                  crsresult.Add(res);

              }
          }
          List<Result> tosend = new List<Result>();
          foreach (Result res in crsresult)
          {
              List<Result> tres = totresults.FindAll(r => r.CourseName == res.CourseName);
              double total = tres.Count; double corr = 0;
              foreach (Result resta in tres)
              {
                  corr += resta.Mark;
              }
              res.Mark = corr / total;
              tosend.Add(res);
          }
          return tosend;
      }

      public List<Result> GetTeacherAverageMarksByCourse(string TeacherUserName)
      {
          List<Evnt> totevnts = context.Evnts.ToList().FindAll(ev => ev.TeacherUserName == TeacherUserName && ev.EvntEdt < DateTime.Now);
          List<Result> totresults = new List<Result>();
          foreach (Evnt ev in totevnts)
          {
              totresults.AddRange(context.Results.ToList().FindAll(res => res.EvntId == ev.EvntId));
          }

          List<Result> crsresult = new List<Result>();
          foreach (Result res in totresults)
          {
              if (crsresult.SingleOrDefault(r => r.CourseName == res.CourseName) == null)
              {
                  crsresult.Add(res);

              }
          }
          List<Result> tosend = new List<Result>();
          foreach (Result res in crsresult)
          {
              List<Result> tres = totresults.FindAll(r => r.CourseName == res.CourseName);
              double total = tres.Count; double corr = 0;
              foreach (Result resta in tres)
              {
                  corr += resta.Mark;
              }
              res.Mark = corr / total;
              tosend.Add(res);
          }
          return tosend;
      }
    }
}
