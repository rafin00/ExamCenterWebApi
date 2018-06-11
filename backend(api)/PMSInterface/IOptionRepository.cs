using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMSEntity;

namespace PMSInterface
{
    public interface IOptionRepository : IRepository<Option>
    {
        List<Option> Search(string key);
      List<Option> GetOptionsByQuestionId(int id);
    }
}
