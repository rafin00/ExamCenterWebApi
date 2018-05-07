using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMSEntity;

namespace PMSInterface
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        List<Answer> Search(string key);
        void setAnsTable(string UserName, int evntid);
        void answerupdate(Answer an);
        bool checkexist(string username, int evntid);
        Answer GetMyAnswer(Answer an);
    }
}
