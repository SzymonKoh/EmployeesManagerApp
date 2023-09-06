using EmployeesManagerApp.Data.Entities;

namespace EmployeesManagerApp.Data.Repositories
{
    public interface IEmployeesManager<T> : IReadRepository<T>, IWriteRepository<T> where T : class, IEntity
    {
        event EventHandler<T>? PracownikDodany;
        event EventHandler<T>? PracownikUsuniety;
    }
}
