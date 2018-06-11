using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMSInterface;
using PMSEntity;
namespace PMSRepository
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
      PMSDBContext context = new PMSDBContext();
      public List<Question> Search(string key)
      {
         
            throw new NotImplementedException();
        }

      public int GetMaxId()
        {
            return context.Questions.Max(q => q.QuestionId);
        }
      public List<Question> GetEvntCourseQuestions(int evntid,string coursename)
      {
          List<Exam> exms=  context.Exams.ToList().FindAll(ex => ex.EvntId == evntid && ex.CourseName == coursename);
          List<Question> questions = new List<Question>();
          Question ques = new Question();
          foreach (Exam ex in exms)
          {
              ques = context.Questions.SingleOrDefault(qu => qu.QuestionId == ex.QuestionId);
              questions.Add(ques);
              ques = new Question();
          }
          return questions;
      }
      public List<Question> GetCourseQuestions(string coursename,string username)
      {
          return context.Questions.Where(ques=>ques.CourseName==coursename && ques.TeacherUserName==username).ToList();
      }

      public int CheckIdenticalQuestion(Question ques, List<Option> options)
      {
          bool exists = true; int cnt = 0;
          Question q = context.Questions.SingleOrDefault(qu => qu.Answer == ques.Answer && ques.TeacherUserName==qu.TeacherUserName && qu.CourseName == ques.CourseName && qu.QuestionText == ques.QuestionText);
          if (q != null)
          {
              List<Option> optn = context.Options.ToList().FindAll(op => op.QuestionId == q.QuestionId);
              if (optn.Count == options.Count)
              {
                  for (int i = 0; i < options.Count; i++)
                  {
                      if (optn[i].OptionText == options[i].OptionText)
                      {
                          cnt++;
                      }
                  }
              }
          }
          if (cnt == options.Count)
          {
              return q.QuestionId;
          }
          else
          {
              return -1;
          }
      }

      public bool CheckValidQuestion(Question question, List<Option> options)
      {
          bool valid = false;
          foreach (Option op in options)
          {
              if (op.OptionText == question.Answer) { valid = true; }
          }
          return valid;
      }
      public List<Question> GetQuestionsforevnt(int evntid,string coursename, string username)
      {
          List<Answer> answers = context.Answers.ToList().FindAll(ans => ans.EvntId == evntid && ans.StudentUserName == username && ans.CourseName==coursename);
          List<Question> questions = new List<Question>();
          Question ques = new Question();
          foreach (Answer ans in answers)
          {
              questions.Add(context.Questions.SingleOrDefault(qu => qu.QuestionId == ans.QuestionId));
          }
          return questions;
      }
    
    public List<Question> GetQuestionsforevntTV(int evntid,string coursename, string username)
      {
          List<Exam> exam = context.Exams.ToList().FindAll(ans => ans.EvntId == evntid && ans.CourseName==coursename);
          List<Question> questions = new List<Question>();
          Question ques = new Question();
          foreach (Exam ex in exam)
          {
              questions.Add(context.Questions.SingleOrDefault(qu => qu.QuestionId == ex.QuestionId));
          }
          return questions;
      }
    }
}
