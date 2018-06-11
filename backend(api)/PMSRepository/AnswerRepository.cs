using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMSInterface;
using PMSEntity;
namespace PMSRepository
{

    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        PMSDBContext context = new PMSDBContext();
      
      public List<Answer> Search(string key)
      {
         
            throw new NotImplementedException();
        }

      
      public void setAnsTable(string UserName, int evntid)
      {

          IQuestionRepository quesrepo = new QuestionRepository();
          IAnswerRepository ansrepo = new AnswerRepository();

          List<Exam> exms = context.Exams.Where(ex => ex.EvntId == evntid).ToList();
          List<Question> ques = new List<Question>();
          List<Answer> answers = new List<Answer>();
          foreach(Exam ex in exms)
          {
              Question qu = quesrepo.Get(ex.QuestionId);
              Answer ans = new Answer();
              ans.CourseName = qu.CourseName;
              ans.EvntId = evntid;
              ans.MyAnswer = "NA";
              ans.QuestionId = qu.QuestionId;
              ans.StudentUserName = UserName;
              answers.Add(ans);
          }

          //

          int n = answers.Count;
          Random rnd = new Random();
          while (n > 1)
          {
              int k = (rnd.Next(0, n) % n);
              n--;
              Answer value = answers[k];
              answers[k] = answers[n];
              answers[n] = value;
          }
          //

         
         foreach (Answer ans in answers)
         {      
             ansrepo.Insert(ans);

         }

      }
        public void answerupdate(Answer an)
        {
            Answer ans = context.Answers.Where(anse=>anse.QuestionId==an.QuestionId && anse.EvntId==an.EvntId && anse.StudentUserName==an.StudentUserName).ToList()[0];
            ans.MyAnswer=an.MyAnswer;
            context.SaveChanges();
        }
        public Answer GetMyAnswer(Answer an)
        {
            Answer ans = context.Answers.Where(anse => anse.QuestionId == an.QuestionId && anse.EvntId == an.EvntId && anse.StudentUserName == an.StudentUserName).ToList()[0];
            return ans;
        }
        public bool checkexist(string username, int evntid)
        {
            List<Answer> answers = context.Answers.ToList().FindAll(ans=>ans.EvntId==evntid && ans.StudentUserName==username);
            if (answers.Count == 0)
            {
                return false;
            }
            return true;
        }
    
    }
}
