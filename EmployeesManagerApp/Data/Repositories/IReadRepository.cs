using EmployeesManagerApp.Data.Entities;

namespace EmployeesManagerApp.Data.Repositories
{
    public interface IReadRepository<out T> where T : class, IEntity
    {
        IEnumerable<T> GetAll();
        T PobierzPracownikaPoId(int id);
    }
}