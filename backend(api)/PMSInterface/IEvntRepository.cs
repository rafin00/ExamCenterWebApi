using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMSEntity;

namespace PMSInterface
{
    public interface IEvntRepository : IRepository<Evnt>
    {
        List<Evnt> Search(string key);
         Exam GetCourse(int EvntId);
        int GetMaxId();
        List<Evnt> GetAllByTeacher(string ThrUserName);
        List<Question> GetQuestionsByEvntByCourse(int evntid, string courseName);
        List<Evnt> GetEvntsByStudent(string UserName);
        List<Evnt> GetAllByTeacherName(string username);
        Evnt GetEvntByTeacher(string username, int evntid);
        void DeleteTotalEvnt(int evntid);
        bool CheckEvnt(Evnt ev, string username); 
        List<Evnt> GetFinishedEvntsByStudent(string UserName);
        List<Evnt> GetFinishedEvntsByTeacher(string username);
        bool CheckEvntVR(Evnt ev, string username);
    }
}
