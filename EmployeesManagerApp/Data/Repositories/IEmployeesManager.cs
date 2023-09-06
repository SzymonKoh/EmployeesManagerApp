using EmployeesManagerApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesManagerApp.Data.Repositories
{
    public interface IEmployeesManager<T> : IReadRepository<T>, IWriteRepository<T> where T : class, IEntity
    {
        event EventHandler<T>? PracownikDodany;
        event EventHandler<T>? PracownikUsuniety;
    }
}
