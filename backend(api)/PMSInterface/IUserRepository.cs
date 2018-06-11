using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMSEntity;

namespace PMSInterface
{
    public interface IUserRepository : IRepository<User>
    {
        List<User> Search(string key);

        List<User> GetAllByType(string type);
    }
}
