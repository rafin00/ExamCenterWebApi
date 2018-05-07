using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMSEntity;

namespace PMSInterface
{
    public interface IQuestionRepository : IRepository<Question>
    {
        List<Question> Search(string key);
         int GetMaxId();
         List<Question> GetEvntCourseQuestions(int evntid,string coursename);
         List<Question> GetCourseQuestions(string coursename, string username);
         int CheckIdenticalQuestion(Question ques, List<Option> options);
         bool CheckValidQuestion(Question question, List<Option> options);
         List<Question> GetQuestionsforevnt(int evntid, string coursename, string username);
         List<Question> GetQuestionsforevntTV(int evntid, string coursename, string username);
    }
}
