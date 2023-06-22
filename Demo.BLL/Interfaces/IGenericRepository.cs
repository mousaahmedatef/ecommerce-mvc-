using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IGenericRepository<T>
    {
        Task<int> Add(T Item);
        Task<T> Get(int? id);
        Task<IEnumerable<T>> GetAll();
        Task<int> Update(T T);
        Task<int> Delete(T T);
        Task<IEnumerable<T>> SearchByName(string name);
    }
}
