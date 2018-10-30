using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMSInterface;
using PMSEntity;
namespace PMSRepository
{
  public  class EvntRepository : Repository<Evnt>, IEvntRepository
    {
      PMSDBContext context = new PMSDBContext();
        public List<Evnt> Search(string key)
      {
         
            throw new NotImplementedException();
        }

        public Exam GetCourse(int EvntId)
        {
            return context.Exams.Find(EvntId);
        }

        public int GetMaxId()
        {
            return context.Evnts.Max(q => q.EvntId);
        }
        public List<Evnt> GetAllByTeacher(string ThrUserName)
        {
            return context.Evnts.Where(ev => ev.TeacherUserName == ThrUserName).ToList();
        }

        public List<Question> GetQuestionsByEvntByCourse(int evntid,string courseName)
        {
            List<Exam> e= context.Exams.ToList().FindAll(ex => ex.CourseName == courseName && ex.EvntId == evntid);
           List<Question> ques = new List<Question>();
           foreach (Exam ex in e)
           {
               Question q = context.Questions.ToList().Find(que=>que.QuestionId==ex.QuestionId);
               ques.Add(q);
           }
           return ques;
     }

        public List<Evnt> GetEvntsByStudent(string UserName)
        {
            List<Registration> regs = new List<Registration>();
            
                regs = context.Registrations.ToList().FindAll(reg => reg.StudentUserName == UserName && reg.Result == (-1));
           
               List<Evnt> evnts = new List<Evnt>();

            foreach (Registration reg in regs)
            {
               
                    Evnt evnt = context.Evnts.SingleOrDefault(ev => ev.EvntId == reg.EvntId && ev.EvntEdt > DateTime.Now);
                    if (evnt != null)
                    {
                        evnts.Add(evnt);
                    }
                
               
            }        
            return evnts;
        }

        public List<Evnt> GetFinishedEvntsByStudent(string UserName)
        {
            List<Registration> regs = new List<Registration>();
            
                regs = context.Registrations.ToList().FindAll(reg => reg.StudentUserName == UserName && reg.Result != (-1));
         
            List<Evnt> evnts = new List<Evnt>();

            foreach (Registration reg in regs)
            {
               
                    Evnt evnt = context.Evnts.SingleOrDefault(ev => ev.EvntId == reg.EvntId && ev.EvntEdt < DateTime.Now);
                    if (evnt != null)
                    {
                        evnts.Add(evnt);
                    }
              
            }
            return evnts;
        }
        public List<Evnt> GetAllByTeacherName(string username)
        {
  return           context.Evnts.Where(ev => ev.TeacherUserName == username).ToList();
        }
        public Evnt GetEvntByTeacher(string username, int evntid)
        {
           return context.Evnts.SingleOrDefault(ev => ev.EvntId == evntid && ev.TeacherUserName == username);
        }
        public void DeleteTotalEvnt(int evntid)
        {
            context.Results.RemoveRange(context.Results.ToList().FindAll(res => res.EvntId == evntid));
            context.SaveChanges();
            context.Registrations.RemoveRange(context.Registrations.ToList().FindAll(res => res.EvntId == evntid));
            context.SaveChanges();
            context.Exams.RemoveRange(context.Exams.ToList().FindAll(res => res.EvntId == evntid));
            context.SaveChanges();
            context.Answers.RemoveRange(context.Answers.ToList().FindAll(res => res.EvntId == evntid));
            context.SaveChanges();
            context.Evnts.RemoveRange(context.Evnts.ToList().FindAll(res => res.EvntId == evntid));
            context.SaveChanges();


        }
        public bool CheckEvnt(Evnt ev, string username)
        {
            if (context.Registrations.SingleOrDefault(reg => reg.EvntId == ev.EvntId && reg.StudentUserName == username && reg.Result==(-1)) != null)
            {
                return true;
            }
            return
                false;
        }
        public bool CheckEvntVR(Evnt ev, string username)
        {
            if (context.Registrations.SingleOrDefault(reg => reg.EvntId == ev.EvntId && reg.StudentUserName == username && reg.Result != (-1)) != null)
            {
                return true;
            }
            return
                false;
        }
        public bool CheckEvntTeacher(Evnt ev, string username)
        {
            if (context.Registrations.SingleOrDefault(reg => reg.EvntId == ev.EvntId &&  reg.Result != (-1)) != null)
            {
                return true;
            }
            return
                false;
        }
        public List<Evnt> GetFinishedEvntsByTeacher(string username)
        {
            List<Evnt> evnt = context.Evnts.ToList().FindAll(ev => ev.TeacherUserName == username  && ev.EvntEdt<DateTime.Now);
            return evnt;    
        }

        public void test() { }

    }
}
