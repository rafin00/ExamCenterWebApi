using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMSInterface;
using PMSEntity;
namespace PMSRepository
{
  public  class UserRepository : Repository<User>, IUserRepository
    {
      PMSDBContext context = new PMSDBContext();
        public List<User> Search(string key)
      {
         
            throw new NotImplementedException();
        }

       public List<User> GetAllByType(string type)
        {
           return context.Users.Where(us=>us.UserType==type).ToList();
        }
    }
}
