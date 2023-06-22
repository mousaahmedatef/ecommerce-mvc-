using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class GenericRepository<T>:IGenericRepository<T> where T: class
    {
        // where T: class-->  struct or enum or interface دا كلاس مش T دى مش بتاخد غير كلاس ف انا بعرفها ان ال Set<T>() دى انا كاتبها عشان ال
        //بتطلع ايرور Set<T>() عشان لو مكتبهتاش ال
        private readonly MVCAppDbContext context;

        public GenericRepository(MVCAppDbContext context)
        {
            this.context = context;
        }
        public async Task<int> Add(T Item)
        {
            context.Set<T>().Add(Item);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Delete(T Item)
        {
            context.Set<T>().Remove(Item);
            return await context.SaveChangesAsync();
        }

        public async Task<T> Get(int? id)
            => await context.Set<T>().FindAsync(id);


        public async Task<IEnumerable<T>> GetAll()
        {
            if (typeof(T)==typeof(Employee))
            {
                return  (IEnumerable<T>)await context.Set<Employee>().Include(E => E.Department).ToListAsync();
            }
            return await context.Set<T>().ToListAsync();
        }

        public async Task<int> Update(T Item)
        {
            context.Set<T>().Update(Item);
            return await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> SearchByName(string name)
        {
            if (typeof(T) == typeof(Employee))
            {
                var data = (IEnumerable<T>)await context.Employees.Where(a => a.Name.Contains(name)).ToListAsync();
                return data;

            }
            else
            {
                var data = (IEnumerable<T>)await context.Departments.Where(a => a.Name.Contains(name)).ToListAsync();
                return data;
            }
            

        }
    }
}
