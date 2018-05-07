using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMSEntity;

namespace PMSInterface
{
    public interface IRegistrationRepository : IRepository<Registration>
    {
        List<Registration> Search(string key);
        bool CheckRegistration(int evntid, string username);
        void AddRemoveRegistration(int evntid, string username);
        void calculateResult(int evntid, string username);
        List<Registration> GetResult(string username);

       List< Registration> GetRegistrationsByEvnts(int id);
       Registration GetEvntResult(string username, int evntid);
       void AssignAll(int EvntId);
       void UnAssignAll(int EvntId);
    }
}
