using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department> , IDepartmentRepository
    {
        //constructor هنا ضارب ايرور عشان كدا عملت 
        //لى؟ constructor طيب انا عامل 
        //Empty parameterless constructorعلى ال chaning ف هوا تلقائي بيعمل constructor مفيهوش child انا لما بخلى كلاس يورث من كلاس تانى وال
        //Empty parameterless constructor مفيهوش  GenericRepository لكن للاسف انا ال parent الى موجود ف ال
        // ومش احسن حاجه انى اروح اعملو هناك مخصوص عشان الحته دى
        //parent على الىى موجود حاليا فال chaning لكن انا عايزو يعمل
        //Empty parameterless constructorلل  Create  مش هيعمل clr ف كدا ال user defined constructor ف احنا لما بنكتب بايدينا
        //context لان الى هناك بياخد  context بايدي وابعتلو  constructor فعشان كدا انا هعمل 

        public DepartmentRepository(MVCAppDbContext context):base(context)
        { 
        
        }
        

        
    }
}
