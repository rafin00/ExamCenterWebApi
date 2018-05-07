using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMSInterface;
using PMSEntity;
namespace PMSRepository
{
    public class OptionRepository : Repository<Option>, IOptionRepository
    {
      PMSDBContext context = new PMSDBContext();
      public List<Option> Search(string key)
      {
         
            throw new NotImplementedException();
        }

      public List<Option> GetOptionsByQuestionId(int id)
      {
          return context.Options.ToList().FindAll(op => op.QuestionId == id).ToList();
      }
        
    }
}
